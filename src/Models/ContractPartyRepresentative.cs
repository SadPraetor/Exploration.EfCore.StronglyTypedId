using System.Diagnostics.CodeAnalysis;

namespace StronglyTypedId.Models
{
	public class ContractPartyRepresentative
	{
		private readonly int _id;
		private readonly int _contractPartyId;
		private readonly int _contractId;
		private readonly int _contractNumber;

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
			_contractNumber = contractParty.Key.ContractNumber;
			Key = new ContractPartyRepresentativeKey(0, contractParty.Key.ContractPartyId, contractParty.Key.ContractId, contractParty.Key.ContractNumber);
		}
	}
}
