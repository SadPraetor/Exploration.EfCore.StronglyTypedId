namespace StronglyTypedId.Models
{
	public record struct ContactKey
	{
		public ContactKey(int Id, int ContractPartyId, int ContractId)
		{
			this.Id = Id;
			ContractPartyKey = new ContractPartyKey(ContractPartyId, ContractId);
		}

		public ContactKey(int Id, ContractPartyKey ContractPartyKey)
		{
			this.Id = Id;
			this.ContractPartyKey = ContractPartyKey;
		}

		public int Id { get; init; }

		public ContractPartyKey ContractPartyKey { get; init; }
	}

	public class Contact
	{
		public ContractPartyKey Key { get; private set; }

		public string City { get; set; } = default!;

		public string Street { get; set; } = default!;
	}


	//public class ContactConfiguration : IEntityTypeConfiguration<Contact>
	//{
	//	public void Configure(EntityTypeBuilder<Contact> builder)
	//	{
	//		throw new NotImplementedException();
	//	}
	//}

}
