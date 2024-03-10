using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class BasicBookInfoModel
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string Subject { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public string CoverImageUrl { get; set; }

    }
}
