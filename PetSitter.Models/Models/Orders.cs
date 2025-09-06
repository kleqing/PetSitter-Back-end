using PetSitter.Models.Enums;

namespace PetSitter.Models.Models;

public class Orders
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    public virtual Users User { get; set; } = new Users();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}