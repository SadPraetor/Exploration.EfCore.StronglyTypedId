# StronglyTypedId exploration

> Situation in DDD when you have a combination of aggregate id and entity id. 
Aim is implemenation of strongly typed typed ids with `EFCore` as ORM

Scenario when your entity has composite key, 
and you want to group those properties into object. 
Such objects can be related to entity as `OwnedEntity` or `ComplexEntity`. In ideal scenario 
after setup, it can simplify work with entities for other developers.


## Startup
There is no application to run, only tests. Docker on standard ports is required for tests,
as database instance is started. Tests themseves do not only test C# code, but also how queries
are translated and executed in database.

## Implementation

After some testing, only workable solution is with `OwnedEntity`.

Example of such entity
```csharp
public class Contract
{
	[SetsRequiredMembers]
	public Contract()
	{
		Key = new ContractKey(default, default);
	}
	private int _id;
	private int _contractNumber;
	public required ContractKey Key { get; init; }


	...code omitted for brevity
}
```
```csharp
{
	public record ContractKey(int ContractId, int ContractNumber);
}
```

EF Core setup
```csharp
builder.HasKey("_id", "_contractNumber");

			builder.OwnsOne(
				c => c.Key,
				nb =>
			{
				nb.WithOwner()
				.HasPrincipalKey("_id", "_contractNumber")
				.HasForeignKey(k => new { k.ContractId, k.ContractNumber });

				nb.Property<int>(k => k.ContractId)
					.HasColumnName("Id");

				nb.Property<int>(k => k.ContractNumber)
					.HasColumnName("ContractNumber");

				nb.HasKey(k => new { k.ContractId, k.ContractNumber });
			});

			builder.Property<int>("_id")
				.HasColumnName("Id")
				.UseIdentityColumn(1, 1);

			builder.Property<int>("_contractNumber")
				.HasColumnName("ContractNumber")
				.ValueGeneratedNever()
				.HasValueGenerator<ContractNumberGenerator>();
//code omitted for brevity
```
Owner has backup fields, mapped to same columns as properties on owned entity. Relationship 
is defined (principal/foreign key). 

### KeyExpressionVisitor
Query needs to be adjusted to be able to handle request where you want to retrieve all 
child entity by parent entity key. 
```csharp
var retrieved = await context.Set<ContractParty>()
				.Include(c => c.Contract)
				.Where(c => c.Key == contractKey)
				.ToListAsync();
```
`ContractParty` is retrieved by `Contract.Key`.

Expression visitor must be executed before query starts being compiled. EF Core will extract values of 
`Contract.Key` as paramter, and that cannot be edited in `IQeuryExpressionInterceptor`.

To achieve that `QueryCompiler` must be replaced. 

### Conclusion
Setup is difficult. If you make child entities OwnedEntity, should not be required.
Figuring out where to place interceptor took a lot of effort.
