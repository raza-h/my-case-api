using MyCaseApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface ICompanyService
    {
        Task<int> AddCompanyAsync(Company company);

        Task<List<Company>> GetAsync();

        Task<Company> GetByIdAsync(int Id);

        Task<Company> UpdateAsync(Company company);

        Task DeleteAsync(int Id);
    }
}
