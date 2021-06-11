using FilmsCatalog.Models;
using FilmsCatalog.ViewModels.Film;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmsCatalog.Services.Interfaces
{
    public interface IFilmsService
    {
        Task<FilmViewModel> GetViewModel(int id);
        Task<Film> CreateAsync(FilmViewModel model);
        Task<Film> UpdateAsync(FilmViewModel model);
        Task<IndexViewModel> GetGrigAsync(int pageSize, int page);
    }
}
