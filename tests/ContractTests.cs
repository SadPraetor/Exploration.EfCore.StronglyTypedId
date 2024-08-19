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
		public async Task CreateContract()
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
			context = null;
			contract = null;

			//Assert
			context = CreateDbContext();

			contract = await context.Contracts.FindAsync(id, number);

			contract.Should()
				.NotBeNull();

			contract.State.Should()
				.Be(ContractState.Active);

			contract.ProductType.Should()
				.Be(ProductType.Loan);
		}
	}
}
