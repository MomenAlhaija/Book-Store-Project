using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class CartItemsModel
    {
        public int? DetaileId { get; set; }
        public int? CartId { get; set; }
        public int Quantity { get; set; }
        public int BookId { get; set; }
        public decimal? Price { get; set; } 
        public string BookTitle { get; set; }
        public string CoverImageUrl { get; set; }
    }
}
