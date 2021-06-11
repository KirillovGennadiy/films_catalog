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
    public class FilmsService : IFilmsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IFilesService _filesService;

        public FilmsService(
            ApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager,
            IMapper mapper,
            IFilesService filesService)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _filesService = filesService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<FilmViewModel> GetViewModel(int id)
        {
            var entity = await _dbContext.Films.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<FilmViewModel>(entity);
        }

        public async Task<Film> CreateAsync(FilmViewModel model)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            model.CreatorId = user.Id;

            var entityResult = await _dbContext.Films.AddAsync(_mapper.Map<Film>(model));
            await _dbContext.SaveChangesAsync();

            await _filesService.Create(model.Poster, model.Name);

            return entityResult.Entity;
        }

        public async Task<Film> UpdateAsync(FilmViewModel model)
        {
            var entityResult = _dbContext.Films.Update(_mapper.Map<Film>(model));
            await _dbContext.SaveChangesAsync();

            return entityResult.Entity;
        }

        public async Task<IndexViewModel> GetGrigAsync(int pageSize, int page)
        {
            var model = new IndexViewModel();

            model.PageSize = pageSize;
            model.CurrentPage = page;
            model.Total = await _dbContext.Films.CountAsync();
            model.Films = await _dbContext.Films.AsNoTracking().Skip(pageSize * page).Take(pageSize).ToListAsync();

            return model;
        }
    }
}
