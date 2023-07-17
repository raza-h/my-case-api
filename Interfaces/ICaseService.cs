using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface ICaseService
    {
        Task<int> AddCase(CaseDetail caseDetail);
        Task<List<CaseDetail>> GetCases();
        Task<CaseDetail> GetCasesById(int Id);
        Task<List<CaseDetail>> GetCasesByClientId(string userId);
        Task<List<CaseDetail>> GetCasesByStaffId(string userId);
        Task<ClientLocation> GetCasesLocation(string userId, int CaseId);
        Task<int> AddCasesLocation(ClientLocation model);
        Task<int> CloseCase(int id);
    }
}
