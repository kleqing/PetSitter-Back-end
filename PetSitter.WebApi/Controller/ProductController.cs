using Microsoft.AspNetCore.Mvc;
using PetSitter.DataAccess.Repository.Interfaces;
using PetSitter.Models.Models;
using PetSitter.Utility.Common;

namespace PetSitter.WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    
    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet("list-products")]
    public async Task<IActionResult> ListAllProducts()
    {
        var response = new BaseResultResponse<List<Products>>();
        
        var products = await _productRepository.ListAllProducts();
        
        if (products != null && products.Count > 0)
        {
            response.Success = true;
            response.Message = "List all products successful";
            response.Data = products;
        }
        else
        {
            response.Success = false;
            response.Message = "No products found";
            response.Data = null;
        }
        return Ok(response);
    }
    
    [HttpGet("product/{productId}")]
    public async Task<IActionResult> PrintProductFromId([FromRoute] Guid productId)
    {
        var response = new BaseResultResponse<Products>();
        
        var product = await _productRepository.PrintProductFromId(productId);
        
        if (product != null)
        {
            response.Success = true;
            response.Message = "Get product successful";
            response.Data = product;
        }
        else
        {
            response.Success = false;
            response.Message = "Product not found";
            response.Data = null;
        }
        return Ok(response);
    }
}