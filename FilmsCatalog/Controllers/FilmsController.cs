using FilmsCatalog.Services.Interfaces;
using FilmsCatalog.ViewModels.Film;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FilmsCatalog.Controllers
{
    [Authorize]
    public class FilmsController : Controller
    {
        private readonly Lazy<IFilmsService> _filmsService;
        public FilmsController(
            Lazy<IFilmsService> filmsService
            )
        {
            _filmsService = filmsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IndexViewModel> Grid(int pageSize, int page)
        {
            return await _filmsService.Value.GetGrigAsync(pageSize, page);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new FilmViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FilmViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _filmsService.Value.CreateAsync(model);
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occured while creating the film");
                return View(model);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _filmsService.Value.GetViewModel(id));
        }
    }
}
