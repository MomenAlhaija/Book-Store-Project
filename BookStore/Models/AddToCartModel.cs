using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class AddToCartModel
    {
        public int? CartId { get; set; }
        public int UserId { get; set; } 
        public int BookId { get; set; } 
        public int Quantiity { get; set; }
    }
}
