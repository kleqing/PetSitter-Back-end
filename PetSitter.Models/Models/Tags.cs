namespace PetSitter.Models.Models;

public class Tags
{
    public Guid TagId { get; set; }
    public Guid? ProductId { get; set; }
    public Guid? BlogId { get; set; }
    public string TagName { get; set; } = string.Empty;

    public virtual ICollection<Products> Products { get; set; } = new List<Products>();
    public virtual ICollection<Blogs> Blogs { get; set; } = new List<Blogs>();
}