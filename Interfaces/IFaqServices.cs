using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
   public interface IFaqServices
    {
        
        Task<int> AddFaqAsync(Faq model);
        Task<List<Faq>> GetFaqAsync();
        Task<Faq> GetFaqByIdAsync(int Id);
        Task<Faq> UpdateFaqAsync(Faq model);
        Task DeleteFaqAsync(int Id);
      
    }
}
