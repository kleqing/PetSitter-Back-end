using Microsoft.AspNetCore.Mvc;
using PetSitter.DataAccess.Repository.Interfaces;
using PetSitter.Models.Models;
using PetSitter.Models.Request;
using PetSitter.Utility.Common;

namespace PetSitter.WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class ShopController : ControllerBase
{
    private readonly IShopRepository _shopRepository;
    
    public ShopController(IShopRepository shopRepository)
    {
        _shopRepository = shopRepository;
    }

    [HttpGet("{shopId}/products")]
    public async Task<IActionResult> ListProductFromShopId([FromRoute] Guid shopId)
    {
        var response = new BaseResultResponse<Shops>();
        var shop = await _shopRepository.ListProductFromShopId(shopId);
        if (shop != null)
        {
            response.Success = true;
            response.Message = "List products from shop successful";
            response.Data = shop;
        }
        else
        {
            response.Success = false;
            response.Message = "Shop not found";
            response.Data = null;
        }
        return Ok(response);
    }

    [HttpGet("{shopId}/products/count")]
    public async Task<IActionResult> CountProductFromShopId([FromRoute] Guid shopId)
    {
        var response = new BaseResultResponse<int>();
        var count = await _shopRepository.CountProductFromShopId(shopId);
        response.Success = true;
        response.Message = "Count products from shop successful";
        response.Data = count;
        return Ok(response);
    }

    [HttpGet("{shopId}/orders/count")]
    public async Task<IActionResult> CountOrderFromShopId([FromRoute] Guid shopId)
    {
        var response = new BaseResultResponse<int>();
        var count = await _shopRepository.CountOrderFromShopId(shopId);
        response.Success = true;
        response.Message = "Count orders from shop successful";
        response.Data = count;
        return Ok(response);
    }

    [HttpPost("{shopId}/products")]
    public async Task<IActionResult> AddProductFromShopId([FromRoute] Guid shopId, [FromForm] ProductRequest request)
    {
        var response = new BaseResultResponse<Products>();
        try
        {
            var product = await _shopRepository.AddProductFromShopId(request, shopId);
            response.Success = true;
            response.Message = "Add product to shop successful";
            response.Data = product;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
            response.Data = null;
        }

        return Ok(response);
    }

    [HttpPut("{shopId}/products/{productId}")]
    public async Task<IActionResult> UpdateProductFromShopId([FromRoute] Guid shopId, [FromRoute] Guid productId,
        [FromForm] ProductRequest request)
    {
        var response = new BaseResultResponse<Products>();
        try
        {
            var product = await _shopRepository.UpdateProductFromShopId(productId, request, shopId);
            response.Success = true;
            response.Message = "Update product in shop successful";
            response.Data = product;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
            response.Data = null;
        }

        return Ok(response);
    }
    
    [HttpGet("{userId}/shop")]
    public async Task<IActionResult> GetShopFromUserId([FromRoute] Guid userId)
    {
        var response = new BaseResultResponse<Shops?>();
        var shop = await _shopRepository.GetShopFromUserId(userId);
        if (shop != null)
        {
            response.Success = true;
            response.Message = "Get shop from user successful";
            response.Data = shop;
        }
        else
        {
            response.Success = false;
            response.Message = "Shop not found";
            response.Data = null; 
        }
        return Ok(response);
    }

}