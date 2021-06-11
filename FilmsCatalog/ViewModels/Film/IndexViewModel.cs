using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.ViewModels.Film
{
    public class IndexViewModel
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public List<Models.Film> Films { get; set; }

        public bool HasNext => PageSize * CurrentPage < Total;
        public bool HasPrev => CurrentPage > 0;
    }
}
