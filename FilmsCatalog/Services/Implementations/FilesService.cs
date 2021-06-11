using FilmsCatalog.Data;
using FilmsCatalog.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace FilmsCatalog.Services.Implementations
{
    public class FilesService : IFilesService
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ApplicationDbContext _dbContext;
        public FilesService(
            IWebHostEnvironment appEnvironment,
            ApplicationDbContext dbContext
            )
        {
            _appEnvironment = appEnvironment;
            _dbContext = dbContext;
        }


        public async Task Create(IFormFile file, string subFolderName)
        {
            string path = $"/Files/{subFolderName}/{file.FileName}";
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            Models.File entity = new Models.File { Name = file.FileName, MimeType = file.ContentType, Path = path };
            await _dbContext.Files.AddAsync(entity);
            _dbContext.SaveChanges();
        }


        public void Delete(string path)
        {
            string fullPath = _appEnvironment.WebRootPath + path;
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
    }
}
