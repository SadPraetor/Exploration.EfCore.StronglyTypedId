using System.Diagnostics.CodeAnalysis;

namespace StronglyTypedId.Models
{
	public record ContractPartyRepresentativeKey(int Id, int ContractPartyId, int ContractId)
	{
		public static implicit operator ContractPartyKey(ContractPartyRepresentativeKey key)
		{
			return new ContractPartyKey(key.ContractPartyId, key.ContractId);
		}
	}

	public class ContractPartyRepresentative
	{
		private int _id;
		private int _contractPartyId;
		private int _contractId;
		public required ContractPartyRepresentativeKey Key { get; set; }

		public string Name { get; set; } = default!;


		public ContractPartyRepresentative()
		{

		}

		[SetsRequiredMembers]
		public ContractPartyRepresentative(ContractParty contractParty)
		{
			_contractPartyId = contractParty.Key.Id;
			_contractId = contractParty.Key.ContractId;
			Key = new ContractPartyRepresentativeKey(0, contractParty.Key.Id, contractParty.Key.ContractId);
		}
	}
}
