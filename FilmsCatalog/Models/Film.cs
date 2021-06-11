namespace FilmsCatalog.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductionYear { get; set; }
        public string Producer { get; set; }

        #region navigation
        public int? PosterId { get; set; }
        public virtual File Poster { get; set; }

        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }
        #endregion
    }
}
