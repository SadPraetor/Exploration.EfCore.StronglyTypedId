# StronglyTypedId exploration
Purpose of project is to test implementation of strongly typed ids with `EFCore` as ORM. Scenario when your entity has composite key, and you want to group those properties into object. Such objects can be related to entity as `OwnedEntity` or `ComplexEntity`.
<br/>
<br/>
After some testing, only workable solution is with `OwnedEntity`

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
to be continued...