using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface IPricePlanService
    {
        Task<int> AddPricePlanAsync(PricePlan pricePlan);
        Task<List<PricePlan>> GetPricePlansAsync();
        Task<PricePlan> GetPricePlanByIdAsync(int Id);
        Task<PackageService> GetPackageByPlanIdAsync(int Id);
        Task<PricePlan> UpdatePricePlanAsync(PricePlan pricePlan);
        Task DeletePricePlanAsync(int Id);
        Task<int> AddServiceAsync(Service service);
        Task<Service> GetServiceByIdAsync(int Id);
        Task<Service> UpdateServiceAsync(Service service);
        Task DeleteServiceAsync(int Id);
        Task<List<Service>> GetServicesAsync();
        Task UpdatePackageAsync(PackageService packageService);
        Task<List<PackageService>> GetPackageServices();
    }
}
