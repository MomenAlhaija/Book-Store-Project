using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class SaleDetailesModel
    {
        public int SaleId { get; set; } 
        public DateTime SalesDate { get; set; } 
        public int SaleQuantity { get; set; }   
        public decimal Price { get; set; }  
        public decimal TotalUnitPrice { get; set; }
        public string BookTitle { get; set; }
        public string CoverImageUrl { get; set; }
    }
}
