using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Services.Interfaces
{
    public interface IFilesService
    {
        Task Create(IFormFile file, string subFolderName);
        void Delete(string path);
    }
}
