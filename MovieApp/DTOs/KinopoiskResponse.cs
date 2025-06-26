namespace MovieApp.DTOs
{

    public class KinopoiskResponse
    {
        public List<FilmDto> items { get; set; }
        public int totalPages { get; set; }
    }
} 