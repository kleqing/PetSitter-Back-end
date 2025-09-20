﻿using PetSitter.Models.Models;
using PetSitter.Models.Request;

namespace PetSitter.DataAccess.Repository.Interfaces;

public interface IShopRepository
{
    Task<Shops?> ListProductFromShopId(Guid shopId);
    Task<int> CountProductFromShopId(Guid shopId);
    Task<int> CountOrderFromShopId(Guid shopId);
    Task<Products> AddProductFromShopId(ProductRequest request, Guid shopId);
    Task<Products> UpdateProductFromShopId(Guid productId, ProductRequest request, Guid shopId);
    Task<Shops?> GetShopFromUserId(Guid userId);
}