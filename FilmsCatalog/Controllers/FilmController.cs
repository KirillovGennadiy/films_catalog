using FilmsCatalog.Services.Interfaces;
using FilmsCatalog.ViewModels.Film;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Controllers
{
    public class FilmController : Controller
    {
        private readonly IFilmService _filmsService;
        private readonly IFileService _fileService;
        private readonly ILogger<FilmController> _logger;
        public FilmController(
            IFilmService filmsService,
            IFileService fileService,
            ILogger<FilmController> logger
            )
        {
            _filmsService = filmsService;
            _fileService = fileService;
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
            try 
            { 
                var model = await _filmsService.GetViewModelAsync(User, id, isInfoPage: true);
                return View(model);
            }
            catch(Exception e)
            {
                return View("Error", e.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateOrUpdate(int? id = null)
        {
            try
            {
                return View(await _filmsService.GetViewModelAsync(User, id));
            }
            catch (Exception e)
            {
                return View("Error", e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrUpdate(FilmViewModel model)
        {
            if (model.FormPoster != null)
            {
                var errors = _fileService.CheckFile(model.FormPoster);
                if (errors.Any())
                {
                    ModelState.AddModelError("FormPoster", string.Join("<br />", errors));
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _filmsService.CreateOrUpdateAsync(model, User);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View("Error", e.Message);
            }

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                return View(await _filmsService.GetViewModelAsync(User, id));
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _filmsService.DeleteAsync(id, User);
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return View("Error", e.Message);
            }
        }
    }
}
