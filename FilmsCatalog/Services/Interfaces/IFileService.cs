using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Services.Interfaces
{
    public interface IFileService
    {
        List<string> CheckFile(IFormFile file);
        Task<int> Create(IFormFile file, string subFolder);
        Task Delete(int id);
    }
}
