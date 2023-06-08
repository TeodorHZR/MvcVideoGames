using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace MvcVideoGames.Models
{
    public class VideoGame
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
