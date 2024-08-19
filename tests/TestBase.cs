using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StronglyTypedId.Data;

namespace StronglyTypedId.Tests
{
	[Collection(nameof(StronglyTypedIdTestCollection))]
	public class TestBase : IDisposable
	{
		protected readonly DbContainerFixture _fixture;
		private readonly ContractContext _dbContext;
		protected SqlConnection GetSqlConnection() => _fixture.GetConnection();
		protected ContractContext CreateDbContext() => new ContractContext(new DbContextOptionsBuilder<ContractContext>()
			.UseSqlServer(_fixture.GetConnection())
				.Options);

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
			_dbContext.Dispose();
		}



	}
}
