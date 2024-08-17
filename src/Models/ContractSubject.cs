namespace StronglyTypedId.Models
{
	public class ContractSubject
	{
		private int _id;    //overrules value on contractSubjectKey!
		private int _rank;  //overrules value on contractSubjectKey!
		public ContractSubject()
		{
			Key = new ContractSubjectKey(0, 0);
		}
		public ContractSubject(int rank)
		{
			Key = new ContractSubjectKey(0, rank);
			_rank = rank;
		}
		public ContractSubject(ContractSubjectKey key)
		{
			Key = new ContractSubjectKey(key.Id, key.Rank);
			_id = key.Id;
			_rank = key.Rank;
		}
		public ContractSubjectKey Key { get; set; }

		public int Ident { get; set; }
		public string? Description { get; set; }

		public List<Insurance> Insurances { get; set; } = new();

		public Insurance AddInsurance(DateTime validFrom)
		{
			var insurance = new Insurance(this, validFrom);
			Insurances.Add(insurance);
			return insurance;
		}

	}
}