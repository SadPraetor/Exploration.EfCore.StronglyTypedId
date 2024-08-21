using System.Diagnostics.CodeAnalysis;

namespace StronglyTypedId.Models
{

	public class ContractParty
	{
		private int _id;    //overrules value on key!
		private int _contractId;  //overrules value on key!
		private int _contractNumber;  //overrules value on key!
		public required ContractPartyKey Key { get; init; }

		public string Name { get; set; } = default!;

		[SetsRequiredMembers]
		public ContractParty()
		{
			Key = new ContractPartyKey(default, default, default);
		}

		[SetsRequiredMembers]
		public ContractParty(ContractKey contractKey, string name)
		{
			_contractId = contractKey.ContractId;
			_contractNumber = contractKey.ContractNumber;
			Key = new ContractPartyKey(0, _contractId, _contractNumber);
			Name = name;
		}

		public List<ContractPartyRepresentative> Representatives { get; set; } = new List<ContractPartyRepresentative>();


	}
}