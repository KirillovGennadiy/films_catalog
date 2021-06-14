using FilmsCatalog.Services.Interfaces;
using FilmsCatalog.ViewModels.Film;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FilmsCatalog.Controllers
{
    public class FilmController : Controller
    {
        private readonly IFilmService _filmsService;
        private readonly ILogger<FilmController> _logger;
        public FilmController(
            IFilmService filmsService,
            ILogger<FilmController> logger
            )
        {
            _filmsService = filmsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            try
            {
                var model = await _filmsService.GetGrigAsync(page);
                return View(model);
            } 
            catch(Exception err)
            {
                _logger.LogError(err.Message);
            }

            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> Info(int id)
        {
            return View(await _filmsService.GetViewModelAsync(id));
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new FilmViewModel());
        }
        [Authorize]
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
                await _filmsService.CreateAsync(model);
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occured while creating the film");
                return View(model);
            }

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await _filmsService.IsUserCreatorAsync(id, User.Identity.Name)) 
            {
                return RedirectToAction("Index");
            }

            return View(await _filmsService.GetViewModelAsync(id));
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FilmViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _filmsService.UpdateAsync(model);
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occured while editing the film");
                return View(model);
            }

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                if (!await _filmsService.IsUserCreatorAsync(id, User.Identity.Name))
                {
                    return RedirectToAction("Index");
                }

                return View(await _filmsService.GetViewModelAsync(id));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occured while editing the film");
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _filmsService.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            catch(Exception err)
            {
                ModelState.AddModelError(string.Empty, "An error occured while editing the film");
                return View("ConfirmDelete", await _filmsService.GetViewModelAsync(id));
            }
        }
    }
}
