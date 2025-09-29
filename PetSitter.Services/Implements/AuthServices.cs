﻿using Microsoft.EntityFrameworkCore;
using PetSitter.DataAccess;
using PetSitter.Models.Enums;
using PetSitter.Models.Models;
using PetSitter.Models.Request;
using PetSitter.Services.Interfaces;
using PetSitter.Utility.Ex;
using RegisterRequest = PetSitter.Models.Request.RegisterRequest;

namespace PetSitter.Services.Implements;

public class AuthServices : IAuthServices
{
    private readonly ApplicationDbContext _context;
    
    public AuthServices(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Users> Register(RegisterRequest request)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (existingUser != null)
        {
            throw new GlobalException("Email already in use");
        }
        
        Random rnd = new Random();
        //* random image from link
        var imageUrl = new[]
        {
            "https://avatar.iran.liara.run/public/8",
            "https://avatar.iran.liara.run/public/45",
            "https://avatar.iran.liara.run/public/47",
            "https://avatar.iran.liara.run/public/65",
            "https://avatar.iran.liara.run/public/64",
            "https://avatar.iran.liara.run/public/78",
        };

        var user = new Users
        {
            UserId = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Role = request.Role,
            DateOfBirth = request.DateOfBirth ?? DateTime.MinValue,
            Address = request.Address,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            ProfilePictureUrl = imageUrl[rnd.Next(imageUrl.Length)],
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        if (user.Role == UserRole.ShopOwner)
        {
            var shop = new Shops
            {
                ShopId = Guid.NewGuid(),
                UserId = user.UserId,
                ShopName = !string.IsNullOrWhiteSpace(request.ShopName) ? request.ShopName : $"{user.FullName}'s Shop",
                Description = !string.IsNullOrWhiteSpace(request.Description) ? request.Description : "Welcome to our shop!",
                Address = user.Address,
                Location = user.Address.Split(",").FirstOrDefault()?.Trim() ?? user.Address,
                SocialMediaLinks = string.Empty,
                ShopImageUrl = user.ProfilePictureUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                BankName = request.BankName,
                BankNumber = request.BankNumber
            };
            
            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();
        }

        return user;
    }

    public async Task<Users> Login(LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new GlobalException("Invalid email or password");
        }
        
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new GlobalException("Invalid email or password");
        }
        return user;
    }

}