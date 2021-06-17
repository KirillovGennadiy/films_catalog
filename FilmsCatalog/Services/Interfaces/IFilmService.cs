using FilmsCatalog.Models;
using FilmsCatalog.ViewModels.Film;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using X.PagedList;

namespace FilmsCatalog.Services.Interfaces
{
    public interface IFilmService
    {
        Task<FilmViewModel> GetViewModelAsync(IPrincipal principal, int? id = null, bool isInfoPage = false);
        Task<Film> CreateOrUpdateAsync(FilmViewModel model, IPrincipal principal);
        Task DeleteAsync(int id, IPrincipal principal);
        Task<IPagedList<Film>> GetGrigAsync(int? page);
    }
}
