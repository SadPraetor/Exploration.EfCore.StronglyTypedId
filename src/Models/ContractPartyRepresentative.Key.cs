namespace StronglyTypedId.Models
{
	public record ContractPartyRepresentativeKey(int Id, int ContractPartyId, int ContractId)
	{
		public static implicit operator ContractPartyKey(ContractPartyRepresentativeKey key)
		{
			return new ContractPartyKey(key.ContractPartyId, key.ContractId);
		}
	}
}
