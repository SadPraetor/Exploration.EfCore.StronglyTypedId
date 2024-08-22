using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StronglyTypedId.Data;
using StronglyTypedId.Data.Infrastructure;
using Xunit.Abstractions;

namespace StronglyTypedId.Tests
{
	[Collection(nameof(StronglyTypedIdTestCollection))]
	public class TestBase : IDisposable
	{
		protected readonly DbContainerFixture _fixture;
		private readonly ITestOutputHelper _output;
		private readonly ContractContext _dbContext;
		protected SqlConnection GetSqlConnection() => _fixture.GetConnection();
		protected ContractContext CreateDbContext() => new ContractContext(new DbContextOptionsBuilder<ContractContext>()
			.UseSqlServer(_fixture.GetConnection())
			.LogTo(_output.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
			.AddInterceptors(new KeyQueryExpressionVisitor())
				.Options);

		public TestBase(DbContainerFixture fixture, ITestOutputHelper output)
		{
			_fixture = fixture;
			_output = output;
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
