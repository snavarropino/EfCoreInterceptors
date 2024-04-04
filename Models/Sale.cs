namespace Models;

public class Sale
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
    public Store Store { get; set; }
    public Product Product { get; set; }
}