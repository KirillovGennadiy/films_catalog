using AutoMapper;
using FilmsCatalog.Data;
using FilmsCatalog.Models;
using FilmsCatalog.Services.Interfaces;
using FilmsCatalog.ViewModels.Film;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using X.PagedList;

namespace FilmsCatalog.Services.Implementations
{
    public class FilmService : IFilmService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IFileService _filesService;

        public FilmService(
            ApplicationDbContext dbContext,
            UserManager<User> userManager,
            IMapper mapper,
            IFileService filesService)
        {
            _dbContext = dbContext;
            _filesService = filesService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<FilmViewModel> GetViewModelAsync(IPrincipal principal, int? id = null, bool isInfoPage = false)
        {
            if (!id.HasValue)
            {
                return new FilmViewModel();
            }

            var entity = 
                await _dbContext.Films
                                .AsNoTracking()
                                .Include(x => x.Poster)
                                .Include(x => x.Creator)
                                .Where(x => isInfoPage || x.Creator.UserName == principal.Identity.Name)
                                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new Exception("Фильм не найден");
            }

            return _mapper.Map<FilmViewModel>(entity);
        }

        public async Task<Film> CreateOrUpdateAsync(FilmViewModel model, IPrincipal principal)
        {
            var oldPosterId = await _dbContext.Films.AsNoTracking().Where(x => x.Id == model.Id).Select(x => x.PosterId).FirstOrDefaultAsync();
            
            if (model.FormPoster != null)
            {
                model.PosterId = await _filesService.Create(model.FormPoster, model.ProductionYear);
            }

            if (model.Id == 0)
            {
                var user = await _userManager.GetUserAsync((ClaimsPrincipal)principal);
                model.CreatorId = user.Id;
            }

            var entityResult = 
                    model.Id > 0 ? 
                    _dbContext.Films.Update(_mapper.Map<Film>(model)) : 
                    await _dbContext.Films.AddAsync(_mapper.Map<Film>(model));

            await _dbContext.SaveChangesAsync();

            if (model.FormPoster != null && oldPosterId.HasValue)
            {
                await _filesService.Delete(oldPosterId.Value);
            }

            return entityResult.Entity;
        }

        public async Task DeleteAsync(int id, IPrincipal principal)
        {
            var entity = await _dbContext.Films
                                        .AsNoTracking()
                                        .Include(x => x.Creator)
                                        .Where(x => x.Creator.UserName == principal.Identity.Name)
                                        .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new Exception("Фильм не найден");
            }

            var entityResult = _dbContext.Films.Remove(entity);
            await _dbContext.SaveChangesAsync();

            if (entity.PosterId.HasValue)
            {
                await _filesService.Delete(entity.PosterId.Value);
            }
        }

        public async Task<IPagedList<Film>> GetGrigAsync(int? page)
        {
            var model =
                await _dbContext.Films
                .AsNoTracking()
                .Include(x => x.Poster)
                .Include(x => x.Creator)
                .ToPagedListAsync(page ?? 1, 5);

            return model;
        }
    }
}
