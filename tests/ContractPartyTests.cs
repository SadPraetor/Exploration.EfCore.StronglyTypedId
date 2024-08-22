using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StronglyTypedId.Models;
using Xunit.Abstractions;

namespace StronglyTypedId.Tests
{
	public class ContractPartyTests : TestBase
	{
		public ContractPartyTests(DbContainerFixture fixture, ITestOutputHelper output) : base(fixture, output)
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

		[Fact]
		public async Task AddByRoot_RetrieveByProperties()
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

			var contractPartyKey = contract.ContractParties[0].Key;

			context.ChangeTracker.Clear();

			var retrieved = await context.Set<ContractParty>()
				.FirstOrDefaultAsync(c => c.Key.ContractPartyId == contractPartyKey.ContractPartyId &&
					c.Key.ContractId == contractPartyKey.ContractId);


			//Assert

			retrieved.Key.ContractId
				.Should()
				.NotBe(default);
		}

		[Fact]
		public async Task AddByRoot_RetrieveByKey()
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

			var contractPartyKey = contract.ContractParties[0].Key;

			context.ChangeTracker.Clear();

			var retrieved = await context.Set<ContractParty>()
				.FirstOrDefaultAsync(c => c.Key == contractPartyKey);


			//Assert

			retrieved.Key.ContractId
				.Should()
				.NotBe(default);
		}

		[Fact]
		public async Task AddByRoot_RetrieveByContractKey()
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

			var contractKey = contract.Key;

			context.ChangeTracker.Clear();

			var retrieved = await context.Set<ContractParty>()
				.FirstOrDefaultAsync(c => c.Key == contractKey);

			//Assert

			retrieved.Key.ContractId
				.Should()
				.NotBe(default);
		}


	}
}
