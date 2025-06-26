using System.Collections.Generic;

namespace MovieApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string PosterUrl { get; set; }
        public List<string> Genres { get; set; }
        public List<string> Actors { get; set; }
    }
} 