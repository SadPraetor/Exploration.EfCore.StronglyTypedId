using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StronglyTypedId.Data;
using StronglyTypedId.Models;

namespace StronglyTypedId.Tests
{
	[Collection(nameof(StronglyTypedIdTestCollection))]
	public class TestBase : IDisposable
	{
		protected readonly DbContainerFixture _fixture;
		private readonly ContractContext _dbContext;

		public TestBase(DbContainerFixture fixture)
		{
			_fixture = fixture;

			_dbContext = new ContractContext(new DbContextOptionsBuilder<ContractContext>()
			.UseSqlServer(_fixture.GetConnection())
				.Options);

			_dbContext.Database.Migrate();
		}
		public void Dispose()
		{
			_dbContext.Database.EnsureDeleted();
		}

		[Fact]
		public async Task DbCreationCheck()
		{
			var dbContext = new ContractContext(new DbContextOptionsBuilder<ContractContext>()
			.UseSqlServer(_fixture.GetConnection())
				.Options);

			var contracts = dbContext.Contracts.Count();

			contracts.Should().Be(0);

			var contract = new Contract()
			{
				DueDate = DateTime.Now.AddDays(5),
				SignatureDate = DateTime.Now,
				Amount = 1000,
				Branch = ContractBranch.Master,
				ProductType = ProductType.Loan,
				State = ContractState.Draft
			};

			_dbContext.Contracts.Add(contract);

			_dbContext.SaveChanges();

			dbContext = new ContractContext(new DbContextOptionsBuilder<ContractContext>()
				.UseSqlServer(_fixture.GetConnection())
				.Options);

			var contractsAfter = dbContext.Contracts.Count();

			contractsAfter.Should().Be(1);
		}

		[Fact]
		public async Task DbCreationDoubleCheck()
		{

			var dbContext = new ContractContext(new DbContextOptionsBuilder<ContractContext>()
			.UseSqlServer(_fixture.GetConnection())
				.Options);

			var contracts = dbContext.Contracts.Count();

			contracts.Should().Be(0);

			var contract = new Contract()
			{
				DueDate = DateTime.Now.AddDays(5),
				SignatureDate = DateTime.Now,
				Amount = 1000,
				Branch = ContractBranch.Master,
				ProductType = ProductType.Loan,
				State = ContractState.Draft
			};

			_dbContext.Contracts.Add(contract);

			_dbContext.SaveChanges();

			dbContext = new ContractContext(new DbContextOptionsBuilder<ContractContext>()
				.UseSqlServer(_fixture.GetConnection())
				.Options);

			var contractsAfter = dbContext.Contracts.Count();

			contractsAfter.Should().Be(1);
		}
	}
}
