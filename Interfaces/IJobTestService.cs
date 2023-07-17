using Microsoft.AspNetCore.Mvc;
using MyCaseApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface IJobTestService
    {
        public string FireAndForgetJob();
        public Task<IActionResult> ReccuringJob();
        public void DelayedJob();
        public void ContinuationJob();
    }
}
