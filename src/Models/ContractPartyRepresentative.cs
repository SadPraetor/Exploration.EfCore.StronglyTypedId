using System.Diagnostics.CodeAnalysis;

namespace StronglyTypedId.Models
{
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
			_contractPartyId = contractParty.Key.ContractPartyId;
			_contractId = contractParty.Key.ContractId;
			Key = new ContractPartyRepresentativeKey(0, contractParty.Key.ContractPartyId, contractParty.Key.ContractId);
		}
	}
}
