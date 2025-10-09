using Microsoft.EntityFrameworkCore;
using PetSitter.DataAccess.Repository.Interfaces;
using PetSitter.Models.DTO;
using PetSitter.Models.Models;
using PetSitter.Models.Request;
using PetSitter.Utility.Ex;

namespace PetSitter.DataAccess.Repository.Implements;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductDto>> ListAllProducts()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Tags)
            .Include(p => p.Reviews)
            .Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                ProductImageUrl = p.ProductImageUrl,
                CategoryName = p.Category.CategoryName,
                BrandName = p.Brand.BrandName,
                Tags = new List<string> { p.Tags.ProductTagName },
                Description = p.Description,
                AvailabilityStatus = p.AvailabilityStatus,
                Rating = p.Reviews.Any() ? Math.Round(p.Reviews.Average(r => r.Rating), 1) : 0,
                StockQuantity = p.StockQuantity,
                ShopId = p.ShopId,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .ToListAsync();

        return products;
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
    
    public async Task<ProductReview> WriteReviewForProduct(ProductReviewRequest request)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == request.ProductId);
        if (product == null)
        {
            throw new Exception("Service not found");
        }
        var review = new ProductReview
        {
            ReviewId = Guid.NewGuid(),
            UserId = request.UserId,
            ProductId = request.ProductId,
            Comment = request.Context,
            Rating = request.Rating,
            CreatedAt = DateTime.UtcNow
        };
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
        return review;
    }
}