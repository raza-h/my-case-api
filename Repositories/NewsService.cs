using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class NewsService : INewsService
    {
        public readonly ApiDbContext dbContext;
        public NewsService(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

     
        public async Task<List<News>> GetAllNews()
        {
            try
            {
                List<News> model = await dbContext.News.ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> AddNews(News model)
        {
            await dbContext.News.AddAsync(model);
            await dbContext.SaveChangesAsync();
            return model.Id;
        }

        public async Task<News> GetNewsById(int Id)
        {
            try
            {
                News model = await dbContext.News.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<News> UpdateNews(News model)
        {
            try
            {
                var getnews=dbContext.News.Where(x => x.Id == model.Id).FirstOrDefault();
                if (model.FirmId!=null)
                {
                    getnews.FirmId = model.FirmId;
                }
                if (model.NewsTittle!=null && model.NewsTittle!="")
                {
                    getnews.NewsTittle = model.NewsTittle;
                }
                if (model.NewsDescription!=null && model.NewsDescription!="")
                {
                    getnews.NewsDescription = model.NewsDescription;
                }
                if (model.PublishDate!=null)
                {
                    getnews.PublishDate = model.PublishDate;
                }
                if (model.ExpireDate!=null)
                {
                    getnews.ExpireDate = model.ExpireDate;
                }
                dbContext.Entry(getnews).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task DeleteNews(int Id)
        {
            try
            {
                News model = await dbContext.News.Where(x => x.Id == Id).FirstOrDefaultAsync();
                if (model != null)
                {
                    dbContext.Entry(model).State = EntityState.Deleted;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
