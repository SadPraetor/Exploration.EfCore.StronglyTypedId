using Microsoft.EntityFrameworkCore.Design;

namespace StronglyTypedId.Data
{
	internal class ContractContextDesignFactory : IDesignTimeDbContextFactory<ContractContext>
	{
		public ContractContext CreateDbContext(string[] args)
		{
			return ContractContext.GetContext("Server=localhost;Database=DEV;User Id=sa;Password=Password!123;TrustServerCertificate=True;");
		}
	}
}
