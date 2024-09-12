namespace Modelbouwer.Model;

public class SupplierContactModel
{
	public int SupplierContactId { get; set; }
	public int SupplierContactSuppplierId { get; set; }
	public string? SupplierContactName { get; set; }
	public int SupplierContactContactTypeId { get; set; }
	public string? SupplierContactContactType { get; set; }
	public string? SupplierContactMail { get; set; }
	public string? SupplierContactPhone { get; set; }
}