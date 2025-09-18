namespace PetSitter.Models.Models;

public class OrderItem
{
    public Guid OrderItemId { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    
    public virtual Orders Order { get; set; }
    public virtual Products Product { get; set; }
}