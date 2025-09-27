using Microsoft.EntityFrameworkCore;
using PetSitter.DataAccess.Repository.Interfaces;
using PetSitter.Models.Models;
using PetSitter.Utility.Ex;

namespace PetSitter.DataAccess.Repository.Implements;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Products>> ListAllProducts()
    {
        var product = await _context.Products.Include(x => x.Category)
            .Include(x => x.Tags)
            .Include(x => x.Brand)
            .Include(x => x.Reviews)
            .ToListAsync();
        return product;
    }

    public async Task<Products> PrintProductFromId(Guid productId)
    {
        var product = await _context.Products.Include(x => x.Category)
            .Include(x => x.Tags)
            .Include(x => x.Brand)
            .FirstOrDefaultAsync(x => x.ProductId == productId);
        if (product == null)
        {
            throw new GlobalException("Product not found");
        }

        return product;
    }

    public async Task<List<Products>> ListRelatedProductsFromCurrentProduct(Guid productId)
    {
        var currentProduct = await _context.Products.Include(x => x.Category)
            .Include(x => x.Tags)
            .Include(x => x.Brand)
            .FirstOrDefaultAsync(x => x.ProductId == productId);
        if (currentProduct == null)
        {
            throw new GlobalException("Product not found");
        }

        var relatedProducts = await _context.Products.Include(x => x.Category)
            .Where(x => x.CategoryId == currentProduct.CategoryId || x.BrandId == currentProduct.BrandId ||
                        x.Tags == currentProduct.Tags && x.ProductId != productId)
            .ToListAsync();
        return relatedProducts;
    }

    public async Task<List<ProductReview>> ListReviewFromCurrentProduct(Guid productId)
    {
        var reviews = await _context.Reviews.Include(x => x.Users).Where(x => x.ProductId == productId).ToListAsync();
        return reviews;
    }

    public async Task<Products> FindByIdAsync(Guid productId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
        {
            throw new GlobalException("Product not found");
        }
        return product;
    }

    public async Task<List<Products>> GetByIdsAsync(List<Guid> productIds)
    {
        var products = await _context.Products.Where(p => productIds.Contains(p.ProductId)).ToListAsync();
        return products;
    }
}