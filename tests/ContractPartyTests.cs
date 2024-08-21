using FluentAssertions;
using StronglyTypedId.Models;

namespace StronglyTypedId.Tests
{
	public class ContractPartyTests : TestBase
	{
		public ContractPartyTests(DbContainerFixture fixture) : base(fixture)
		{
		}

		[Fact]
		public async Task AddContractWithContractParty_AllRelationshipsGenerated()
		{
			//Arrange
			var contract = new Contract
			{
				Amount = 1000,
				Branch = ContractBranch.Master,
				DueDate = DateTime.Now,
				ProductType = ProductType.Loan,
				SignatureDate = DateTime.Now,
				State = ContractState.Active,
				ContractParties = new List<ContractParty>
				{
					new ContractParty
					{
						Name = "John Doe",
					}
				}
			};


			//Act
			var context = CreateDbContext();

			context.Contracts.Add(contract);

			await context.SaveChangesAsync();

			//Assert

			contract.Key.ContractId
				.Should()
				.NotBe(default);

			contract.Key.ContractNumber
				.Should()
				.NotBe(default)
				.And
				.BeInRange(70000000, 89999999);

			contract.ContractParties[0].Key.ContractPartyId
				.Should()
				.NotBe(default);

			contract.ContractParties[0].Key.ContractId
				.Should()
				.NotBe(default)
				.And
				.Be(contract.Key.ContractId);

			contract.ContractParties[0].Key.ContractNumber
				.Should()
				.NotBe(default)
				.And
				.Be(contract.Key.ContractNumber);
		}
	}
}
