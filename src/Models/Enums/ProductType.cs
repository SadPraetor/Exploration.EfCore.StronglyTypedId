namespace StronglyTypedId.Models
{
	public enum ProductType
	{
		None = 0,
		Stock = 1,  //(10000000,39999999)
		Bond = 2,   //(40000000,69999999)
		Loan = 3,   //(70000000,89999999)
		Insurance = 4   //(90000000,99999999)
	}
}