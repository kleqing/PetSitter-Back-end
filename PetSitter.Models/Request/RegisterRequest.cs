using System.ComponentModel.DataAnnotations;
using PetSitter.Models.Enums;

namespace PetSitter.Models.Request;

public class RegisterRequest
{
    [Required] public string FullName { get; set; } = string.Empty;
    [Required] public string Email { get; set; } = string.Empty;
    [Required] public UserRole UserType { get; set; }
    public DateTime DateOfBirth { get; set; }
    [Required] public string Address { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}