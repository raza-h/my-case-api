using MyCaseApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface IFirmService
    {
        #region Firm
        Task<Firm> AddFirmAsync(Firm firm);
        Task<List<Firm>> GetFirmsAsync();
        Task<Firm> GetFirmByUserIdAsync(string userId);
        Task<Firm> UpdateFirmAsync(Firm firm);
        Task DeleteFirmAsync(int Id);
        #endregion
        
        #region FirmOffice
        Task<int> AddFirmOfficeAsync(FirmOffice firmOffice);
        Task<List<FirmOffice>> GetFirmOfficesAsync();
        Task<FirmOffice> GetFirmOfficeByIdAsync(int Id);
        Task<FirmOffice> UpdateFirmOfficeAsync(FirmOffice firmOffice);
        Task DeleteFirmOfficeAsync(int Id);
        #endregion
        
        #region FirmOfficeAddress
        Task<int> AddFirmOfficeAddressAsync(FirmOfficeAddress firmOfficeAddress);
        Task<List<FirmOfficeAddress>> GetFirmOfficeAddressesAsync();
        Task<FirmOfficeAddress> GetFirmOfficeAddressByIdAsync(int Id);
        Task<FirmOfficeAddress> UpdateFirmOfficeAddressAsync(FirmOfficeAddress firmOfficeAddress);
        Task DeleteFirmOfficeAddressAsync(int Id);
        #endregion
    }
}
