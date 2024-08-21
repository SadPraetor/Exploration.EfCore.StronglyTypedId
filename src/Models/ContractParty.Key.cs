namespace StronglyTypedId.Models
{
	public record ContractPartyKey(int ContractPartyId, int ContractId, int ContractNumber) : ContractKey(ContractId, ContractNumber);
}