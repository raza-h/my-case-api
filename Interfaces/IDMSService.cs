using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface IDMSService
    {
        Task<int> AddDirectoryAsync(DocumentFolder documentFolder);
        Task<int> AddSubDirectoryAsync(DocSub1Folder docSub1Folder);
        Task<List<DocumentFolder>> GetFolderAsync();
        Task<List<DocSub1Folder>> GetFolder1Async();
        Task<List<DocSub2Folder>> GetFolder2Async();
        Task<List<DocSub3Folder>> GetFolder3Async();
        Task<DocumentFolder> GetFolderByIdAsync(int Id);
        Task<DMSNewEditValidate> UpdateFolderAsync(DMSNewEditValidate editData);
        Task DeleteFolderAsync(int Id);
    }
}
