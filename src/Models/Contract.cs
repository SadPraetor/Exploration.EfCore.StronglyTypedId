using Microsoft.EntityFrameworkCore.Infrastructure;

namespace StronglyTypedId.Models
{
	public class Contract
	{
		public Contract()
		{

		}
		public Contract(ILazyLoader lazyLoader)
		{
			LazyLoader = lazyLoader;
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


		private int? _sourceContractId;
		private Contract? _sourceContract;
		public Contract? SourceContract
		{
			get => LazyLoader.Load<Contract>(this, ref _sourceContract);
			set => _sourceContract = value;
		}

		private int? _branchedContractId;
		private Contract? _branchedContract;
		public Contract? BranchedContract
		{
			get => LazyLoader.Load<Contract>(this, ref _branchedContract);
			set => _branchedContract = value;
		}

		private ILazyLoader LazyLoader { get; set; }
	}
}
