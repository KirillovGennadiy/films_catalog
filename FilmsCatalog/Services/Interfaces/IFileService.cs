using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Services.Interfaces
{
    public interface IFileService
    {
        Task<int> Create(IFormFile file, string subFolder);
        Task Delete(int id);
    }
}
