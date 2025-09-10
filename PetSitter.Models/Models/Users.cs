using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using PetSitter.Models.Enums;

namespace PetSitter.Models.Models;

public class Users : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
    [EmailAddress]
    public string ProfilePictureUrl  { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    [MaxLength(100)] public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    
    public virtual Shops Shop { get; set; } = new Shops();
    public virtual ICollection<Blogs> Blogs { get; set; } = new List<Blogs>();
    public virtual ICollection<Reviews> Reviews { get; set; } = new List<Reviews>();
    public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
    public virtual ICollection<Pets> Pets { get; set; } = new List<Pets>();
    public virtual ICollection<Bookings> Bookings { get; set; } = new List<Bookings>();
}