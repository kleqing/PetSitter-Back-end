using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSitter.DataAccess.Services.Interfaces;
using PetSitter.Models.DTO;
using PetSitter.Models.Models;
using PetSitter.Models.Request;
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
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = new BaseResultResponse<Users>();

        try
        {
            var user = await _authServices.CreateAccount(request);
            response.Success = true;
            response.Message = "Registration successful";
            response.Data = user;
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
            return BadRequest(response);
        }
    }
}