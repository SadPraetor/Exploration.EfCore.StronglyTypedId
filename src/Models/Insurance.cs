namespace StronglyTypedId.Models
{
	public class Insurance
	{
		private int _id;
		private int _contractSubjectId;
		private int _contractSubjectRank;

		public Insurance()
		{
			Key = new InsuranceKey(0, new ContractSubjectKey(0, 0));
		}

		public Insurance(ContractSubject contractSubject, DateTime validFrom)
		{
			Key = new InsuranceKey(0, contractSubject.Key);
			_contractSubjectId = contractSubject.Key.Id;
			_contractSubjectRank = contractSubject.Key.Rank;
			ValidFrom = validFrom;
		}

		public InsuranceKey Key { get; set; }

		public DateTime? SignedDate { get; set; }
		public DateTime ValidFrom { get; private set; }

		public Guid ValidityToken = Guid.NewGuid();
	}
}