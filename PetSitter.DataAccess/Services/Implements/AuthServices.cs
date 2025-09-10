using System.Net;
using Microsoft.AspNetCore.Identity;
using PetSitter.DataAccess.Services.Interfaces;
using PetSitter.Models.Models;
using PetSitter.Models.Request;
using PetSitter.Utility.Ex;

namespace PetSitter.DataAccess.Services.Implements;

public class AuthServices : IAuthServices
{
    private readonly UserManager<Users> _userManager;
    public AuthServices(UserManager<Users> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Users?> CreateAccount(RegisterRequest request)
    {
        try
        {
            var isUserExists = await _userManager.FindByEmailAsync(request.Email);

            if (isUserExists != null)
            {
                throw new GlobalException("Register", "Email already exists");
            }

            var user = new Users
            {
                Id = new Guid(),
                FullName = request.FullName,
                Email = request.Email,
                Role = request.UserType,
                Address = request.Address,
                DateOfBirth = request.DateOfBirth,
                ProfilePictureUrl = request.ProfilePictureUrl ?? string.Empty,
                CreatedAt = request.CreatedAt
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new GlobalException("Register",
                    $"Unable to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            await _userManager.AddToRoleAsync(user, "User");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);

            var confirmationLink =
                $"{Environment.GetEnvironmentVariable("BACKEND_URL")}/api/Auth/confirm-email?userId={user.Id}&token={encodedToken}";

            //await _emailSender.SendEmailAsync(user.Email, "Verify your email", confirmationLink);

            return user;
        }
        catch (Exception ex)
        {
            throw new GlobalException("Register", ex.Message);
        }
    }
}