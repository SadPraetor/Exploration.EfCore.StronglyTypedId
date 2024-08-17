using System.Diagnostics.CodeAnalysis;

namespace StronglyTypedId.Models
{
	public record ContractPartyKey(int Id, int ContractId);

	public class ContractParty
	{
		private int _id;    //overrules value on key!
		private int _contractId;  //overrules value on key!
		public required ContractPartyKey Key { get; set; }

		public string Name { get; set; } = default!;

		public ContractParty()
		{

		}

		[SetsRequiredMembers]
		public ContractParty(int contractId, string name)
		{
			_contractId = contractId;
			Key = new ContractPartyKey(0, contractId);
			Name = name;
		}

		public List<ContractPartyRepresentative> Representatives { get; set; } = new List<ContractPartyRepresentative>();


	}
}