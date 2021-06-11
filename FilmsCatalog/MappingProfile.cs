using AutoMapper;
using FilmsCatalog.Models;
using FilmsCatalog.ViewModels.Film;

namespace FilmsCatalog
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Film, FilmViewModel>().ReverseMap();
        }
    }
}
