using System.Diagnostics.CodeAnalysis;

namespace StronglyTypedId.Models
{

	public class Contract
	{
		[SetsRequiredMembers]
		public Contract()
		{
			Key = new ContractKey(default, default);
		}
		private int _id;
		private int _contractNumber;
		public required ContractKey Key { get; init; }


		public ContractBranch Branch { get; set; }
		public ContractState State { get; set; }
		public decimal Amount { get; set; }
		public DateTime SignatureDate { get; set; }
		public ProductType ProductType { get; set; }
		public DateTime DueDate { get; set; }
		public DateTime LastModified { get; }
	}
}
