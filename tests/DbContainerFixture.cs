using DotNet.Testcontainers.Builders;
using Testcontainers.MsSql;

namespace StronglyTypedId.Tests
{
	[CollectionDefinition(nameof(StronglyTypedIdTestCollection))]
	public class StronglyTypedIdTestCollection : ICollectionFixture<DbContainerFixture>
	{
	}

	public class DbContainerFixture : IAsyncLifetime
	{
		private MsSqlContainer _container;

		public MsSqlContainer Container => _container;

		public async Task InitializeAsync()
		{
			_container = new MsSqlBuilder()
				.WithAutoRemove(true)
				.WithImage("mcr.microsoft.com/mssql/server:2022-CU10-ubuntu-22.04")
				.WithPassword("Password!123")
				.WithPortBinding(1433)
				.WithName("mssql")
				.WithEnvironment("ACCEPT_EULA", "Y")
				.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
				.WithCleanUp(false)
				.Build();

			await _container.StartAsync();
		}



		public async Task DisposeAsync()
		{
			await _container.StopAsync();
		}

	}
}
