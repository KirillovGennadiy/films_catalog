using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.ViewModels.Film
{
    public class FilmViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле обязательно")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Поле обязательно")]
        [MaxLength(4, ErrorMessage = "Год не может содержать больше 4-х цифр")]
        [Display(Name = "Год производства")]
        public int ProductionYear { get; set; }

        [Required(ErrorMessage = "Поле обязательно")]
        [Display(Name = "Режиссер")]
        public string Producer { get; set; }

        [Display(Name = "Постер")]
        public IFormFile Poster { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string CreatorId { get; set; }
    }
}
