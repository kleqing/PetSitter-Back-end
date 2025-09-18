using PetSitter.Models.Models;

namespace PetSitter.DataAccess.Repository.Interfaces;

public interface IProductRepository
{
    Task<List<Products>> ListAllProducts();
    Task<Products> PrintProductFromId(Guid productId);
    Task<List<Products>> ListRelatedProductsFromCurrentProduct(Guid productId);
    Task<List<ProductReview>> ListReviewFromCurrentProduct(Guid productId);
}