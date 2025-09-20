using PetSitter.Models.Enums;

namespace PetSitter.Models.Request;

public class RegisterRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Address { get; set; } = string.Empty;
    public string ShopName { get; set; }
    public string Description { get; set; }
}