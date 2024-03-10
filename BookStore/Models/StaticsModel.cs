using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class StaticsModel
    {
        public int CountUsers { get; set; } 
        public int CountOrders { get; set; }
        public int CountProducts { get; set; }
        public decimal TotalSales { get; set; } 
        public List<SalesModel> TodaySales { get; set; }    
    }
}
