using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class FirmService : IFirmService
    {
        private readonly ApiDbContext dbContext;

        public FirmService(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #region Firm
        public async Task<Firm> AddFirmAsync(Firm firm)
        {
            try
            {
                await dbContext.Firm.AddAsync(firm);
                await dbContext.SaveChangesAsync();
                List<FirmOffice> offices = new List<FirmOffice>();
                List<FirmOfficeAddress> addresses = new List<FirmOfficeAddress>();
                if (firm.FirmOffices != null && firm.FirmOffices.Count > 0)
                {
                    foreach (var office in firm.FirmOffices)
                    {
                        office.FirmId = firm.Id;
                        offices.Add(office);
                    }
                    await dbContext.FirmOffice.AddRangeAsync(offices);
                    await dbContext.SaveChangesAsync();
                    foreach (var office in offices)
                    {
                        if (office.FirmOfficeAddresses != null && office.FirmOfficeAddresses.Count > 0)
                        {
                            foreach (var address in office.FirmOfficeAddresses)
                            {
                                address.FirmOfficeId = office.Id;
                                addresses.Add(address);
                            }
                        }
                    }
                }
                if (addresses != null && addresses.Count > 0)
                {
                    await dbContext.FirmOfficeAddress.AddRangeAsync(addresses);
                    await dbContext.SaveChangesAsync();
                }
                return firm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Firm>> GetFirmsAsync()
        {
            try
            {
                List<Firm> firms = await dbContext.Firm.ToListAsync();
                return firms;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Firm> GetFirmByUserIdAsync(string userId)
        {
            try
            {
                var GetParentUser = dbContext.User.ToList().Where(x => x.Id == userId).FirstOrDefault();
                if(GetParentUser != null)
                {
                    Firm firm = await dbContext.Firm.Where(x => x.UserId == GetParentUser.Id).FirstOrDefaultAsync();
                    if (firm != null)
                    {
                        firm.FirmOffices = await dbContext.FirmOffice.Where(x => x.FirmId == firm.Id).ToListAsync();
                        if (firm.FirmOffices != null && firm.FirmOffices.Count > 0)
                        {
                            List<FirmOfficeAddress> firmOfficeAddresses = await dbContext.FirmOfficeAddress.ToListAsync();
                            foreach (var office in firm.FirmOffices)
                            {
                                office.FirmOfficeAddresses = firmOfficeAddresses.Where(x => x.FirmOfficeId == office.Id).ToList();
                            }
                        }
                    }
                    return firm;
                }
                Firm objfirm = new Firm();
                return objfirm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteFirmAsync(int Id)
        {
            try
            {
                Firm firm = await dbContext.Firm.Where(x => x.Id == Id).FirstOrDefaultAsync();
                dbContext.Entry(firm).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Firm> UpdateFirmAsync(Firm firm)
        {
            try
            {
                dbContext.Firm.Update(firm);
                await dbContext.SaveChangesAsync();
                if (firm.FirmOffices != null && firm.FirmOffices.Count > 0)
                {
                    firm.FirmOffices = firm.FirmOffices.Where(x => !firm.DeletedOffice.Contains(x.Id)).ToList();
                    if (firm.FirmOffices != null && firm.FirmOffices.Count > 0)
                    {
                        foreach (var office in firm.FirmOffices)
                        {
                            if (office.Id > 0)
                                dbContext.FirmOffice.Update(office);
                            else
                                dbContext.FirmOffice.Add(office);
                        }
                    }
                    await dbContext.SaveChangesAsync();
                    foreach (var office in firm.FirmOffices)
                    {
                        if (office.FirmOfficeAddresses != null && office.FirmOfficeAddresses.Count > 0)
                        {
                            office.FirmOfficeAddresses = office.FirmOfficeAddresses.Where(x => !firm.DeletedAddresses.Contains(x.Id)).ToList();
                            if (office.FirmOfficeAddresses != null && office.FirmOfficeAddresses.Count > 0)
                            {
                                foreach (var address in office.FirmOfficeAddresses)
                                {
                                    if (address.Id > 0)
                                    {
                                        address.FirmOfficeId = office.Id;
                                        dbContext.FirmOfficeAddress.Update(address);
                                    }
                                    else
                                    {
                                        address.FirmOfficeId = office.Id;
                                        dbContext.FirmOfficeAddress.Add(address);
                                    }
                                }
                            }
                        }
                    }
                    if(firm !=null && firm.DeletedAddresses !=null && firm.DeletedAddresses.Length > 0)
                    {
                        var officeaddresses = await dbContext.FirmOfficeAddress.Where(x => firm.DeletedAddresses.Contains(x.Id)).ToListAsync();
                        dbContext.FirmOfficeAddress.RemoveRange(officeaddresses);
                    }
                    if (firm != null && firm.DeletedOffice != null && firm.DeletedOffice.Length > 0)
                    {
                        var offices = await dbContext.FirmOffice.Where(x => firm.DeletedOffice.Contains(x.Id)).ToListAsync();
                        dbContext.FirmOffice.RemoveRange(offices);
                    }
                    await dbContext.SaveChangesAsync();
                }
                return firm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region FirmOffice
        public async Task<int> AddFirmOfficeAsync(FirmOffice firmOffice)
        {
            try
            {
                await dbContext.FirmOffice.AddAsync(firmOffice);
                await dbContext.SaveChangesAsync();
                return firmOffice.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<FirmOffice>> GetFirmOfficesAsync()
        {
            try
            {
                List<FirmOffice> firmOffice = await dbContext.FirmOffice.ToListAsync();
                return firmOffice;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteFirmOfficeAsync(int Id)
        {
            try
            {
                FirmOffice firmOffice = await dbContext.FirmOffice.Where(x => x.Id == Id).FirstOrDefaultAsync();
                dbContext.Entry(firmOffice).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<FirmOffice> GetFirmOfficeByIdAsync(int Id)
        {
            try
            {
                FirmOffice firmOffice = await dbContext.FirmOffice.Where(x => x.Id == Id).FirstOrDefaultAsync();
                if (firmOffice != null)
                    firmOffice.FirmOfficeAddresses = await dbContext.FirmOfficeAddress.Where(x => x.FirmOfficeId == firmOffice.Id).ToListAsync();
                return firmOffice;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<FirmOffice> UpdateFirmOfficeAsync(FirmOffice firm)
        {
            try
            {
                dbContext.Entry(firm).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return firm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region FirmOfficeAddress
        public async Task<int> AddFirmOfficeAddressAsync(FirmOfficeAddress firmOfficeAddress)
        {
            try
            {
                await dbContext.FirmOfficeAddress.AddAsync(firmOfficeAddress);
                await dbContext.SaveChangesAsync();
                return firmOfficeAddress.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteFirmOfficeAddressAsync(int Id)
        {
            try
            {
                FirmOfficeAddress firmOfficeAddress = await dbContext.FirmOfficeAddress.Where(x => x.Id == Id).FirstOrDefaultAsync();
                dbContext.Entry(firmOfficeAddress).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FirmOfficeAddress>> GetFirmOfficeAddressesAsync()
        {
            try
            {
                List<FirmOfficeAddress> firmOfficeAddress = await dbContext.FirmOfficeAddress.ToListAsync();
                return firmOfficeAddress;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FirmOfficeAddress> GetFirmOfficeAddressByIdAsync(int Id)
        {
            try
            {
                FirmOfficeAddress firmOfficeAddress = await dbContext.FirmOfficeAddress.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return firmOfficeAddress;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FirmOfficeAddress> UpdateFirmOfficeAddressAsync(FirmOfficeAddress firmOfficeAddress)
        {
            try
            {
                dbContext.Entry(firmOfficeAddress).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return firmOfficeAddress;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
