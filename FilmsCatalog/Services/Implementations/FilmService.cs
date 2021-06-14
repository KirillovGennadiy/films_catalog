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
using System.Threading.Tasks;

namespace FilmsCatalog.Services.Implementations
{
    public class FilmService : IFilmService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IFileService _filesService;

        public FilmService(
            ApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager,
            IMapper mapper,
            IFileService filesService)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _filesService = filesService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<FilmViewModel> GetViewModelAsync(int id)
        {
            var entity = await _dbContext.Films.Include(x => x.Poster).Include(x => x.Creator).FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<FilmViewModel>(entity);
        }

        public async Task<bool> IsUserCreatorAsync(int id, string userName)
        {
            var film = await _dbContext.Films.AsNoTracking().Include(x => x.Creator).FirstOrDefaultAsync(x => x.Id == id);

            return film.Creator.UserName == userName;
        }

        public async Task<Film> CreateAsync(FilmViewModel model)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            model.CreatorId = user.Id;
            model.PosterId = await _filesService.Create(model.FormPoster, model.Name + "_" + model.ProductionYear);

            var entityResult = await _dbContext.Films.AddAsync(_mapper.Map<Film>(model));
            await _dbContext.SaveChangesAsync();

            return entityResult.Entity;
        }

        public async Task<Film> UpdateAsync(FilmViewModel model)
        {
            var entity = await _dbContext.Films.AsNoTracking().FirstOrDefaultAsync(x => x.Id == model.Id);
            if (model.FormPoster != null)
            {
                model.PosterId = await _filesService.Create(model.FormPoster, model.Name + "_" + model.ProductionYear);
            }

            model.CreatorId = entity.CreatorId;

            var entityResult = _dbContext.Films.Update(_mapper.Map<Film>(model));
            await _dbContext.SaveChangesAsync();

            if (model.FormPoster != null && entity.PosterId.HasValue)
            {
                await _filesService.Delete(entity.PosterId.Value);
            }

            return entityResult.Entity;
        }
        
        public async Task DeleteAsync(int id)
        {
            var entity = await _dbContext.Films.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            var entityResult = _dbContext.Films.Remove(entity);
            await _dbContext.SaveChangesAsync();

            if (entity.PosterId.HasValue)
            {
                await _filesService.Delete(entity.PosterId.Value);
            }
        }

        public async Task<IndexViewModel> GetGrigAsync(int? page)
        {
            if (!await _dbContext.Films.AnyAsync())
            {
                return new IndexViewModel();
            }

            var model = new IndexViewModel
            {
                CurrentPage = page ?? 0,
                Total = await _dbContext.Films.CountAsync()
            };

            model.Films = 
                await _dbContext.Films
                .AsNoTracking()
                .Include(x => x.Poster)
                .Include(x => x.Creator)
                .Skip(model.PageSize * model.CurrentPage)
                .Take(model.PageSize)
                .ToListAsync();

            return model;
        }
    }
}
