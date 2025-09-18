using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSitter.Models.Models;
using PetSitter.Models.Request;
using PetSitter.Services.Interfaces;
using PetSitter.Utility.Common;

namespace PetSitter.WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthServices _authServices;
    
    public AuthController(IAuthServices authServices)
    {
        _authServices = authServices;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = new BaseResultResponse<Users>();
        
        var user = await _authServices.Register(request);

        if (user != null)
        {
            response.Success = true;
            response.Message = "Registration successful";
            response.Data = user;
        }
        else
        {
            response.Success = false;
            response.Message = "Registration failed";
            response.Data = null;
        }
        return Ok(response);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = new BaseResultResponse<Users>();
        
        var user = await _authServices.Login(request);
        if (user != null)
        {
            response.Success = true;
            response.Message = "Login successful";
            response.Data = user;
        }
        else
        {
            response.Success = false;
            response.Message = "Login failed";
            response.Data = null;
        }
        return Ok(response);
    }
}