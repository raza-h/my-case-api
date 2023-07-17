using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class DecumentServices : IDecumentService
    {
        private readonly ApiDbContext dbContext;
        private IWebHostEnvironment env;
        public DecumentServices(ApiDbContext dbContext, IWebHostEnvironment env)
        {
            this.dbContext = dbContext;
            this.env = env;
        }
        public async Task<int> AddDecuments(Decuments model)
        {
            try
            {
                model.CreatedDate = DateTime.Now.ToString("dd-MM-yyyyy");
                await dbContext.Decuments.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return model.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Decuments>> GetDecuments()
        {
            try
            {
                List<Decuments> Decuments = await dbContext.Decuments.Where(x => x.WorkflowId == null).ToListAsync();
                return Decuments;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Decuments> GetDecumentsByid(int Id)
        {
            try
            {
                Decuments model = await dbContext.Decuments.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task DeleteDecuments(int Id)
        {
            try
            {
                Decuments model = await dbContext.Decuments.Where(x => x.Id == Id).FirstOrDefaultAsync();
                string hostRootPath = env.WebRootPath;
                string webRootPath = env.ContentRootPath;
                string _decumentPath = string.Empty;

                if (model != null && !string.IsNullOrEmpty(model.DecumentPath))
                {
                    _decumentPath = webRootPath + "\\" + model.DecumentPath.Replace("/", "\\");
                    var fileInfo = new System.IO.FileInfo(_decumentPath);
                    fileInfo.Delete();
                }
                dbContext.Entry(model).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
