using FluentAssertions;
using StronglyTypedId.Models;
using Xunit.Abstractions;

namespace StronglyTypedId.Tests
{

	public class ContractPartyRepresentativeTests : TestBase
	{
		public ContractPartyRepresentativeTests(DbContainerFixture fixture, ITestOutputHelper output) : base(fixture, output)
		{
		}

		private Contract BuildContract() => new Contract
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
						Representatives = new List<ContractPartyRepresentative>()
						{
							new ContractPartyRepresentative("John Doe"),
							new ContractPartyRepresentative("Alex Wrath")
						}
					},
					new ContractParty
					{
						Name = "Leas a.s.",
						Representatives = new List < ContractPartyRepresentative >()
						{
							new ContractPartyRepresentative("Jane Doe")
						}
					}
				}
		};

		[Fact]
		public async Task AddContract_WithContractParty_WithRepresentatives_AllRelationshipsGenerated()
		{
			//Arrange
			var contract = BuildContract();

			var context = CreateDbContext();

			//Act

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

			contract.ContractParties
				.Should()
				.AllSatisfy(x =>
				{
					x.Key.ContractPartyId
					.Should()
					.NotBe(default);

					x.Key.ContractNumber
					.Should()
					.NotBe(default)
					.And
					.Be(contract.Key.ContractNumber);

					x.Key.ContractId
					.Should()
					.NotBe(default)
					.And
					.Be(contract.Key.ContractId);
				});

			contract.ContractParties
				.Should()
				.AllSatisfy(x =>
				{
					x.Representatives
					.Should()
					.AllSatisfy(r =>
					{
						r.Key.Id
						.Should()
						.NotBe(default);

						r.Key.ContractPartyId
						.Should()
						.NotBe(default);

						r.Key.ContractNumber
						.Should()
						.NotBe(default)
						.And
						.Be(contract.Key.ContractNumber);

						r.Key.ContractId
						.Should()
						.NotBe(default)
						.And
						.Be(contract.Key.ContractId);
					});
				});
		}

		[Fact]
		public async Task AddAggregate_RetrieveRepresentative_ByKey()
		{
			//Arrange
			var contract = BuildContract();

			var context = CreateDbContext();

			//Act

			context.Contracts.Add(contract);

			await context.SaveChangesAsync();

			var representative = contract.ContractParties[0].Representatives[0];

			var contractPartyRepresentativeKey = new ContractPartyRepresentativeKey(
				representative.Key.Id, representative.Key.ContractPartyId, representative.Key.ContractId, representative.Key.ContractNumber);

			context.ChangeTracker.Clear();
			representative = null;
			//Assert

			representative = context.Set<ContractPartyRepresentative>()
				.FirstOrDefault(x => x.Key == contractPartyRepresentativeKey);

			representative.Should()
				.NotBeNull();

			representative.Name.Should()
				.Be("John Doe");
		}
	}
}
