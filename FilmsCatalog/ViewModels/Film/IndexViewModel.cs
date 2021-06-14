using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.ViewModels.Film
{
    public class IndexViewModel
    {
        public int CurrentPage { get; set; }
        public int PageSize => 5;
        public int Total { get; set; }
        public string CurrentUserId { get; set; }
        public List<Models.Film> Films { get; set; }

        public bool HasNext => PageSize * (CurrentPage + 1) < Total;
        public bool HasPrev => CurrentPage > 0;
        public int PagesCount => Total % PageSize == 0 ? Total / PageSize : (Total / PageSize) + 1;

        public IndexViewModel()
        {
            CurrentPage = 0;
            Total = 0;
            Films = new List<Models.Film>();
        }
    }
}
