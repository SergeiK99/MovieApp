using MovieApp.Models;

namespace MovieApp.DTOs
{
    public class KinopoiskPageResult
    {
        public List<Movie> Movies { get; set; }
        public int TotalPages { get; set; }
    }
}
