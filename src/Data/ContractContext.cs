using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StronglyTypedId.Data.Infrastructure;
using StronglyTypedId.Models;

namespace StronglyTypedId.Data
{
	public class ContractContext : DbContext
	{
		public ContractContext(DbContextOptions<ContractContext> options) : base(options)
		{


		}

		public static ContractContext GetContext(string cnnString)
		{
			return new ContractContext(
				new DbContextOptionsBuilder<ContractContext>()
				.UseSqlServer(cnnString)
				.Options);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var services = new ServiceCollection();
			services.AddEntityFrameworkSqlServer();

			var descriptor = new ServiceDescriptor(typeof(IQueryCompiler),
				typeof(InterceptingQueryCompiler),
				lifetime: ServiceLifetime.Scoped);
			services.Replace(descriptor);
			var serviceProvider = services.BuildServiceProvider();

			optionsBuilder.UseInternalServiceProvider(serviceProvider);

			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{


			builder.ApplyConfigurationsFromAssembly(typeof(ContractContext).Assembly);
			builder.HasDefaultSchema(schema: "con");
			builder.HasSequence<int>("Contract_Number_Stock", schema: "con")
				.StartsAt(10000000)
				.IncrementsBy(1)
				.HasMax(39999999);
			builder.HasSequence<int>("Contract_Number_Bond", schema: "con")
				.StartsAt(40000000)
				.IncrementsBy(1)
				.HasMax(69999999);
			builder.HasSequence<int>("Contract_Number_Loan", schema: "con")
				.StartsAt(70000000)
				.IncrementsBy(1)
				.HasMax(89999999);
			builder.HasSequence<int>("Contract_Number_Insurance", schema: "con")
				.StartsAt(90000000)
				.IncrementsBy(1)
				.HasMax(99999999);

			builder.HasSequence<int>($"{nameof(ContractSubject)}_Id", schema: "con")
				.StartsAt(1)
				.IncrementsBy(1)
				.HasMax(99999999);

			builder.HasSequence<int>($"{nameof(Insurance)}_Id", schema: "con")
				.StartsAt(1)
				.IncrementsBy(1)
				.HasMax(99999999);

			builder.HasSequence<int>($"{nameof(ContractParty)}_Id", schema: "con")
				.StartsAt(1)
				.IncrementsBy(1)
				.HasMax(99999999);

			builder.HasSequence<int>($"{nameof(ContractPartyRepresentative)}_Id", schema: "con")
				.StartsAt(1)
				.IncrementsBy(1)
				.HasMax(99999999);

		}

		public DbSet<Contract> Contracts { get; set; }

	}
}