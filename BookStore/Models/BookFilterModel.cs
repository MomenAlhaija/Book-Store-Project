
namespace BookStore.Models
{
    public class BookFilterModel
    {
        public string[] AuthorsNames { get; set; }
        public int[] CategoryIds { get; set; }
        public string BookTitle { get; set; }
        public decimal? MinPrice { get; set; } = null;
        public decimal? MaxPrice { get; set; }=null;
    }
}
