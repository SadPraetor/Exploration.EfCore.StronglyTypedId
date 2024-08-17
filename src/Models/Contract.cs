namespace StronglyTypedId.Models
{
	public class Contract
	{
		public Contract()
		{

		}

		public int Id { get; set; }
		public int ContractNumber { get; private set; }
		public ContractBranch Branch { get; set; }
		public ContractState State { get; set; }
		public decimal Amount { get; set; }
		public DateTime SignatureDate { get; set; }
		public ProductType ProductType { get; set; }
		public DateTime DueDate { get; set; }
		public DateTime LastModified { get; }
	}
}
