using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
   public interface INewsService
    {
        Task<List<News>> GetAllNews();
        Task<int> AddNews(News model);
        Task<News> GetNewsById(int Id);
        Task<News> UpdateNews(News model);
        Task DeleteNews(int Id); 

    }
}
