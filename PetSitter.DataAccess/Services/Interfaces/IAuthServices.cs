using PetSitter.Models.Models;
using PetSitter.Models.Request;

namespace PetSitter.DataAccess.Services.Interfaces;

public interface IAuthServices
{
    Task<Users?> CreateAccount(RegisterRequest request);
}