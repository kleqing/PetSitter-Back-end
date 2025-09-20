using Microsoft.EntityFrameworkCore;
using PetSitter.DataAccess.Repository.Interfaces;
using PetSitter.Models.Models;

namespace PetSitter.DataAccess.Repository.Implements;

public class ServiceRepository : IServiceRepository
{
    private readonly ApplicationDbContext _context;
    
    public ServiceRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Services>> ListAllServices()
    {
        var service=  await _context.Services.Include(x => x.ServiceReviews).Include(x => x.Shop).ToListAsync();
        return service;
    }
    
    public async Task<Services?> RetrieveServiceFromId(Guid serviceId)
    {
        return await _context.Services
            .Include(x => x.ServiceReviews)
            .ThenInclude(x => x.Users)
            .Include(x => x.Shop)
            .FirstOrDefaultAsync(x => x.ServiceId == serviceId);
    }

    public async Task<List<ServiceReview>> RetrieveServiceReviewsByServiceId(Guid serviceId)
    {
        var service = await _context.ServiceReviews
            .Include(x => x.Users)
            .Where(x => x.ServiceId == serviceId)
            .ToListAsync();
        return service;
    }
}