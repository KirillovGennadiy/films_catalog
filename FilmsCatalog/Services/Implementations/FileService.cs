using FilmsCatalog.Data;
using FilmsCatalog.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace FilmsCatalog.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ApplicationDbContext _dbContext;
        public FileService(
            IWebHostEnvironment appEnvironment,
            ApplicationDbContext dbContext
            )
        {
            _appEnvironment = appEnvironment;
            _dbContext = dbContext;
        }


        public async Task<int> Create(IFormFile file, string subFolder)
        {
            string directoryPath = $"/files/{subFolder}";

            if (!Directory.Exists(_appEnvironment.WebRootPath + directoryPath))
            {
                Directory.CreateDirectory(_appEnvironment.WebRootPath + directoryPath);
            }

            string path = $"{directoryPath}/{file.FileName}";
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            Models.File entity = new Models.File { Name = file.FileName, MimeType = file.ContentType, Path = path };
            var entityResult = await _dbContext.Files.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entityResult.Entity.Id;
        }

        public async Task Delete(int id)
        {
            var file = await _dbContext.Files.FirstOrDefaultAsync(x => x.Id == id); 
            
            if (file != null)
            {
                if (System.IO.File.Exists(_appEnvironment.WebRootPath + file.Path)) { 
                    System.IO.File.Delete(_appEnvironment.WebRootPath + file.Path);
                }

                _dbContext.Files.Remove(file);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
