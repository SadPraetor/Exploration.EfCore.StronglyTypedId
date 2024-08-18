using FluentAssertions;

namespace StronglyTypedId.Tests
{
	[Collection(nameof(StronglyTypedIdTestCollection))]
	public class ContractTest
	{
		private readonly DbContainerFixture _fixture;

		public ContractTest(DbContainerFixture fixture)
		{
			_fixture = fixture;
		}

		[Fact]
		public async Task DbCreationCheck()
		{
			_fixture
				.Should()
				.NotBeNull();
		}
	}
}
