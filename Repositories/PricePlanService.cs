using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class PricePlanService : IPricePlanService
    {
        public readonly ApiDbContext dbContext;
        public PricePlanService(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddServiceAsync(Service service)
        {
            await dbContext.Service.AddAsync(service);
            await dbContext.SaveChangesAsync();
            return service.Id;
        }

        public async Task<int> AddPricePlanAsync(PricePlan pricePlan)
        {
            try
            {
                List<PackageService> packages = new List<PackageService>();
                pricePlan.Status = true;
                pricePlan.CreatedDate = DateTime.Now;
                await dbContext.PricePlan.AddAsync(pricePlan);
                await dbContext.SaveChangesAsync();
                if (pricePlan != null && pricePlan.ServicesIds != null && pricePlan.ServicesIds.Length > 0)
                {
                    foreach (int serviceId in pricePlan.ServicesIds)
                    {
                        PackageService package = new PackageService();
                        package.PricePlanId = pricePlan.PlanID;
                        package.ServiceId = serviceId;
                        packages.Add(package);
                    }
                    await dbContext.packageService.AddRangeAsync(packages);
                }
                await dbContext.SaveChangesAsync();
                return pricePlan.PlanID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteServiceAsync(int Id)
        {
            try
            {
                Service service = dbContext.Service.Where(x => x.Id == Id).FirstOrDefault();
                if (service != null)
                {
                    dbContext.Entry(service).State = EntityState.Deleted;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeletePricePlanAsync(int Id)
        {
            try
            {
                PricePlan pricePlan = await dbContext.PricePlan.Where(x => x.PlanID == Id).FirstOrDefaultAsync();
                if (pricePlan != null)
                {
                    dbContext.Entry(pricePlan).State = EntityState.Deleted;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Service> GetServiceByIdAsync(int Id)
        {
            try
            {
                Service service = await dbContext.Service.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return service;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Service>> GetServicesAsync()
        {
            try
            {
                List<Service> services = await dbContext.Service.ToListAsync();
                return services;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PricePlan>> GetPricePlansAsync()
        {
            try
            {
                List<PricePlan> pricePlan = await dbContext.PricePlan.ToListAsync();
                return pricePlan;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PackageService> GetPackageByPlanIdAsync(int Id)
        {
            try
            {
                PricePlan pricePlan = await dbContext.PricePlan.Where(x => x.PlanID == Id).FirstOrDefaultAsync();
                List<PackageService> packageService = await dbContext.packageService.Where(x => x.PricePlanId == Id).ToListAsync();
                if (packageService != null && packageService.Count > 0)
                {
                    List<Service> services = new List<Service>();
                    int?[] serviceIds = packageService.Select(x => x.ServiceId).ToArray();
                    if (serviceIds.Length > 0)
                        services = await dbContext.Service.Where(x => serviceIds.Contains(x.Id)).ToListAsync();

                    PackageService package = new PackageService();
                    package.services = services;
                    package.pricePlan = pricePlan;
                    return package;
                }
                else
                {
                    PackageService package = new PackageService();
                    package.pricePlan = pricePlan;
                    return package;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Service> UpdateServiceAsync(Service service)
        {
            try
            {
                dbContext.Entry(service).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return service;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PricePlan> UpdatePricePlanAsync(PricePlan pricePlan)
        {
            try
            {
                dbContext.Entry(pricePlan).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return pricePlan;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdatePackageAsync(PackageService packageService)
        {
            try
            {
                List<PackageService> packages = await dbContext.packageService.Where(x => x.PricePlanId == packageService.PricePlanId).ToListAsync();
                if (packages != null && packages.Count > 0)
                {
                    dbContext.packageService.RemoveRange(packages);
                    await dbContext.SaveChangesAsync();
                }

                if (packageService != null && packageService.ServicesIds != null && packageService.ServicesIds.Length > 0)
                {
                    foreach (int i in packageService.ServicesIds)
                    {
                        PackageService packageServices = new PackageService();
                        packageServices.PricePlanId = packageService.PricePlanId;
                        packageServices.ServiceId = i;
                        await dbContext.packageService.AddAsync(packageServices);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<PackageService>> GetPackageServices()
        {
            try
            {
                List<PackageService> packageServices = new List<PackageService>();
                var packageService = dbContext.packageService.ToList().GroupBy(x => x.PricePlanId).ToList();
                if (packageService != null && packageService.Count > 0)
                {
                    List<Service> allServices = await dbContext.Service.ToListAsync();
                    List<PricePlan> allPricePlans = await dbContext.PricePlan.ToListAsync();
                    foreach (var plan in packageService)
                    {
                        List<Service> services = new List<Service>();
                        int?[] serviceIds = plan.Select(x => x.ServiceId).ToArray();
                        if (serviceIds.Length > 0)
                            services = allServices.Where(x => serviceIds.Contains(x.Id)).ToList();


                        PricePlan pricePlan = allPricePlans.Where(x => x.PlanID == plan.Key).FirstOrDefault();
                        if (pricePlan != null)
                        {
                            PackageService package = new PackageService();
                            package.Id = plan.FirstOrDefault().Id;
                            package.services = services;
                            package.pricePlan = pricePlan;
                            packageServices.Add(package);
                        }
                    }
                }
                return packageServices;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PricePlan> GetPricePlanByIdAsync(int Id)
        {
            try
            {
                PricePlan pricePlan = await dbContext.PricePlan.Where(x => x.PlanID == Id).FirstOrDefaultAsync();
                return pricePlan;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
