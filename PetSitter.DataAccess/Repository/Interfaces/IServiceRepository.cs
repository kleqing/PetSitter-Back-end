using PetSitter.Models.Models;

namespace PetSitter.DataAccess.Repository.Interfaces;

public interface IServiceRepository
{
    Task<List<Services>> ListAllServices();
    Task<Services?> RetrieveServiceFromId(Guid serviceId);
    Task<List<ServiceReview>> RetrieveServiceReviewsByServiceId(Guid serviceId);
}