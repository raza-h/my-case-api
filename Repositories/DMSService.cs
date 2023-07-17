using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class DMSService : IDMSService
    {
        private readonly ApiDbContext dbContext;
        public DMSService(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> AddDirectoryAsync(DocumentFolder documentFolder)
        {
            try
            {
                await dbContext.DocumentFolders.AddAsync(documentFolder);
                await dbContext.SaveChangesAsync();
                return documentFolder.DocumentFolderId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> AddSubDirectoryAsync(DocSub1Folder docSub1Folder)
        {
            try
            {
                await dbContext.DocSub1Folder.AddAsync(docSub1Folder);
                await dbContext.SaveChangesAsync();
                return docSub1Folder.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DocumentFolder> GetFolderByIdAsync(int Id)
        {
            try
            {
                var data = await dbContext.DocumentFolders.Where(x => x.DocumentFolderId == Id).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DocumentFolder>> GetFolderAsync()
        {
            try
            {
                List<DocumentFolder> documentFolders = new List<DocumentFolder>();
                documentFolders = await dbContext.DocumentFolders.ToListAsync();
                return documentFolders;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DocSub1Folder>> GetFolder1Async()
        {
            try
            {
                List<DocSub1Folder> documentFolders1 = new List<DocSub1Folder>();
                documentFolders1 = await dbContext.DocSub1Folder.ToListAsync();
                return documentFolders1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DocSub2Folder>> GetFolder2Async()
        {
            try
            {
                List<DocSub2Folder> documentFolders = new List<DocSub2Folder>();
                documentFolders = await dbContext.DocSub2Folder.ToListAsync();
                return documentFolders;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DocSub3Folder>> GetFolder3Async()
        {
            try
            {
                List<DocSub3Folder> documentFolders = new List<DocSub3Folder>();
                documentFolders = await dbContext.DocSub3Folder.ToListAsync();
                return documentFolders;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DMSNewEditValidate> UpdateFolderAsync(DMSNewEditValidate editData)
        {
            try
            {
                if (editData.sub1Id == null && editData.sub2Id == null && editData.sub3Id == null)
                {//Main Folder
                    var getfolder = dbContext.DocumentFolders.ToList().Where(x => x.DocumentFolderId == editData.mainId).FirstOrDefault();
                    if (getfolder != null)
                    {
                        getfolder.Name = editData.mainName;
                    }
                }

                //var getfolder = dbContext.DocumentFolders.ToList().Where(x => x.DocumentFolderId == editData.mainId).FirstOrDefault();
                //if(getfolder != null)
                //{
                //    getfolder.Name = editData.mainName;
                //    dbContext.Entry(getfolder).State = EntityState.Modified;
                //    await dbContext.SaveChangesAsync();
                //}


                else if (editData.sub2Id == null && editData.sub3Id == null)
                {//Sub1 Folder
                    var data = dbContext.DocSub1Folder.Where(x => x.Id == editData.sub1Id)?.FirstOrDefault();
                    var data1 = dbContext.DocumentFolders.ToList().Where(x => x.DocumentFolderId == editData.mainId)?.FirstOrDefault();
                    data.Name = editData.sub1Name;
                    data1.Name = editData.mainName;
                }
                //var get1folder = dbContext.docSub1Folders.ToList().Where(x => x.Id == editData.sub1Id).FirstOrDefault();
                //if (get1folder != null)
                //{
                //    get1folder.Name = editData.sub1Name;
                //    dbContext.Entry(get1folder).State = EntityState.Modified;
                //    await dbContext.SaveChangesAsync();
                //}
                var get2folder = dbContext.DocSub2Folder.ToList().Where(x => x.Id == editData.sub2Id).FirstOrDefault();
                if (get2folder != null)
                {
                    get2folder.Name = editData.sub2Name;
                    dbContext.Entry(get2folder).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                }
                var get3folder = dbContext.DocSub3Folder.ToList().Where(x => x.Id == editData.sub3Id).FirstOrDefault();
                if (get3folder != null)
                {
                    get3folder.Name = editData.sub3Name;
                    dbContext.Entry(get3folder).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                }
                
                return editData;

                //if (sub1Id == null && sub2Id == null && sub3Id == null)
                //{//Main Folder
                //    var data = db.DocumentFolders.Where(x => x.DocumentFolderId == mainId)?.FirstOrDefault();
                //    data.Name = mainName;
                //}
                //else if (sub2Id == null && sub3Id == null)
                //{//Sub1 Folder
                //    var data = db.DocSub1Folders.Where(x => x.Id == sub1Id)?.FirstOrDefault();
                //    var data1 = db.DocumentFolders.Where(x => x.DocumentFolderId == mainId)?.FirstOrDefault();
                //    data.Name = sub1Name;
                //    data1.Name = mainName;
                //}




                //else if (sub3Id == null)
                //{//Sub2 Folder
                //    var data = db.DocSub1Folders.Where(x => x.Id == sub1Id)?.FirstOrDefault();
                //    var data1 = db.DocumentFolders.Where(x => x.DocumentFolderId == mainId)?.FirstOrDefault();
                //    var data2 = db.DocSub2Folders.Where(x => x.Id == sub2Id)?.FirstOrDefault();
                //    data.Name = sub1Name;
                //    data1.Name = mainName;
                //    data2.Name = sub2Name;
                //}
                //else
                //{//Sub3 Folder
                //    var data = db.DocSub1Folders.Where(x => x.Id == sub1Id)?.FirstOrDefault();
                //    var data1 = db.DocumentFolders.Where(x => x.DocumentFolderId == mainId)?.FirstOrDefault();
                //    var data2 = db.DocSub2Folders.Where(x => x.Id == sub2Id)?.FirstOrDefault();
                //    var data3 = db.DocSub3Folders.Where(x => x.Id == sub3Id)?.FirstOrDefault();
                //    data.Name = sub1Name;
                //    data1.Name = mainName;
                //    data2.Name = sub2Name;
                //    data3.Name = sub3Name;
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task DeleteFolderAsync(int Id)
        {
            try
            {
                var data = await dbContext.DocumentFolders.Where(x => x.DocumentFolderId == Id).FirstOrDefaultAsync();
                if (data != null)
                {
                    dbContext.Entry(data).State = EntityState.Deleted;
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
