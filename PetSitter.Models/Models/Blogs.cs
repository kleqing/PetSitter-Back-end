namespace PetSitter.Models.Models;

public class Blogs
{
    public Guid BlogId { get; set; }
    public Guid AuthorId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int ReadTimeMinutes { get; set; }
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public virtual Users Author { get; set; }
    public virtual ICollection<Tags> Tags { get; set; } = new List<Tags>();
}