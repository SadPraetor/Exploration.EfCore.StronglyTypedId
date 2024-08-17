namespace StronglyTypedId.Models
{
	public record InsuranceKey
	{
		private int _contractSubjectId;
		private int _contractSubjectRank;
		public int Id { get; }
		public ContractSubjectKeyInsurance ContractSubjectKey { get; }
		public InsuranceKey()
		{

		}
		public InsuranceKey(int Id, ContractSubjectKey ContractSubjectKey)
		{
			this.Id = Id;
			_contractSubjectId = ContractSubjectKey.Id;
			_contractSubjectRank = ContractSubjectKey.Rank;
			this.ContractSubjectKey = new ContractSubjectKeyInsurance(ContractSubjectKey.Id, ContractSubjectKey.Rank, Id);
		}


		public record ContractSubjectKeyInsurance : ContractSubjectKey
		{
			public ContractSubjectKeyInsurance(int Id, int Rank) : base(Id, Rank)
			{

			}
			public ContractSubjectKeyInsurance(int Id, int Rank, int InsuranceKeyId) : base(Id, Rank)
			{
				_insuranceKeyId = InsuranceKeyId;
			}

			private int _insuranceKeyId;
		}


	}
}