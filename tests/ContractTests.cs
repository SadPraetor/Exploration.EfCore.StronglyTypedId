using FluentAssertions;
using StronglyTypedId.Models;

namespace StronglyTypedId.Tests
{
	[Collection(nameof(StronglyTypedIdTestCollection))]
	public class ContractTests : TestBase
	{
		public ContractTests(DbContainerFixture fixture) : base(fixture)
		{
		}

		[Fact]
		public async Task AddContract_GeneratesIdAndNumber()
		{
			//Arrange
			var contract = new Contract
			{
				Amount = 1000,
				Branch = ContractBranch.Master,
				DueDate = DateTime.Now,
				ProductType = ProductType.Loan,
				SignatureDate = DateTime.Now,
				State = ContractState.Active
			};

			//Act
			var context = CreateDbContext();

			context.Contracts.Add(contract);

			await context.SaveChangesAsync();

			//Assert

			contract.Id
				.Should()
				.NotBe(default);

			contract.ContractNumber
				.Should()
				.NotBe(default)
				.And
				.BeInRange(70000000, 89999999);

			var id = contract.Id;
			var number = contract.ContractNumber;
		}



		[Fact]
		public async Task AddContract_RetrieveContract()
		{
			//Arrange
			var contract = new Contract
			{
				Amount = 1000,
				Branch = ContractBranch.Master,
				DueDate = DateTime.Now,
				ProductType = ProductType.Loan,
				SignatureDate = DateTime.Now,
				State = ContractState.Active
			};

			//Act
			var context = CreateDbContext();

			context.Contracts.Add(contract);

			await context.SaveChangesAsync();

			var id = contract.Id;
			var number = contract.ContractNumber;
			context.ChangeTracker.Clear();
			contract = null;
			contract = await context.Contracts.FindAsync(id, number);

			//Assert
			contract.Should()
				.NotBeNull();

			contract!.State.Should()
				.Be(ContractState.Active);

			contract!.ProductType.Should()
				.Be(ProductType.Loan);
		}

		[Fact]
		public async Task AddContract_UnderTransaction_GeneratesIdAndNumber()
		{
			//Arrange
			var contract = new Contract
			{
				Amount = 1000,
				Branch = ContractBranch.Master,
				DueDate = DateTime.Now,
				ProductType = ProductType.Loan,
				SignatureDate = DateTime.Now,
				State = ContractState.Active
			};

			//Act
			var context = CreateDbContext();

			using (var transaction = await context.Database.BeginTransactionAsync())
			{
				context.Contracts.Add(contract);

				await context.SaveChangesAsync();

				//Assert

				contract.Id
					.Should()
					.NotBe(default);

				contract.ContractNumber
					.Should()
					.NotBe(default)
					.And
					.BeInRange(70000000, 89999999);

				transaction.Commit();
			}


			var id = contract.Id;
			var number = contract.ContractNumber;
			context.ChangeTracker.Clear();
			contract = null;


			contract = await context.Contracts.FindAsync(id, number);

			contract.Should()
				.NotBeNull();

			contract!.State.Should()
				.Be(ContractState.Active);

			contract!.ProductType.Should()
				.Be(ProductType.Loan);
		}
	}
}
