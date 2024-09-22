namespace StronglyTypedId.Models
{
	public record ContractPartyRepresentativeKey(int Id, int ContractPartyId, int ContractId, int ContractNumber) : ContractPartyKey(ContractPartyId, ContractId, ContractNumber);

}
