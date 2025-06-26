namespace MovieApp.DTOs
{
    public class FilmDto
    {
        public int kinopoiskId { get; set; }
        public string nameRu { get; set; }
        public string nameEn { get; set; }
        public string nameOriginal { get; set; }
        public int? year { get; set; }
        public string posterUrlPreview { get; set; }
        public List<GenreDto> genres { get; set; }
        public List<CountryDto> countries { get; set; }
        public string type { get; set; }
        public string description { get; set; }
    }
} 