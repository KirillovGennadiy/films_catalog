using FilmsCatalog.Models;
using FilmsCatalog.ViewModels.Film;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmsCatalog.Services.Interfaces
{
    public interface IFilmService
    {
        Task<FilmViewModel> GetViewModelAsync(int id);
        Task<Film> CreateAsync(FilmViewModel model);
        Task<Film> UpdateAsync(FilmViewModel model);
        Task DeleteAsync(int id);
        Task<IndexViewModel> GetGrigAsync(int? page);
        Task<bool> IsUserCreatorAsync(int id, string userName);
    }
}
