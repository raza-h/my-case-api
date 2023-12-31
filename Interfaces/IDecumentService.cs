﻿using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
  public  interface IDecumentService
    {
        Task<int> AddDecuments(Decuments Decument);
        Task<List<Decuments>> GetDecuments();
        Task<Decuments> GetDecumentsByid(int Id);
        Task<Decuments> UpdateDecument(Decuments Decument);

        Task DeleteDecumentAtPath(int id);
     
        Task DeleteDecuments(int Id);
    }
}
