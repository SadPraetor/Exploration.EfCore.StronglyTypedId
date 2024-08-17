using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using StronglyTypedId.Models;
using System.Data;

namespace StronglyTypedId.Data.Services
{
	internal class ContractNumberGenerator : ValueGenerator<int>
	{
		public override bool GeneratesTemporaryValues => false;

		public override int Next(EntityEntry entry)
		{
			if (entry.Entity is not Contract)
			{
				throw new NotImplementedException("Expected entity Contract");
			}

			string sqlCommand = PickSequence(entry);

			var connection = entry.Context.Database.GetDbConnection();

			try
			{
				connection.Open();

				using var command = connection.CreateCommand();

				command.CommandText = sqlCommand;
#pragma warning disable CS8605 // Unboxing a possibly null value.
				return (int)command.ExecuteScalar();
#pragma warning restore CS8605 // Unboxing a possibly null value.
			}
			finally
			{
				connection?.Close();
			}
		}

		private static string PickSequence(EntityEntry entry)
		{
			return ((Contract)entry.Entity).ProductType switch
			{
				ProductType.Stock => "SELECT NEXT VALUE FOR con.Contract_Number_Stock",
				ProductType.Bond => "SELECT NEXT VALUE FOR con.Contract_Number_Bond",
				ProductType.Loan => "SELECT NEXT VALUE FOR con.Contract_Number_Loan",
				ProductType.Insurance => "SELECT NEXT VALUE FOR con.Contract_Number_Insurance",
				_ => throw new InvalidOperationException("Invalid product type")
			};
		}

		public override async ValueTask<int> NextAsync(EntityEntry entry, CancellationToken cancellationToken = default)
		{
			if (entry.Entity is not Contract)
			{
				throw new NotImplementedException("Expected entity Contract");
			}

			string sqlCommand = PickSequence(entry);

			var connection = entry.Context.Database.GetDbConnection();

			try
			{
				await connection.OpenAsync();

				using var command = connection.CreateCommand();

				command.CommandText = sqlCommand;
#pragma warning disable CS8605 // Unboxing a possibly null value.
				return (int)await command.ExecuteScalarAsync();
#pragma warning restore CS8605 // Unboxing a possibly null value.
			}
			finally
			{
				if (connection?.State == ConnectionState.Open)
				{
					await connection.CloseAsync();
				}
			}
		}
	}
}
