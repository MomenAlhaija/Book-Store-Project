using System;
using System.Collections.Generic;
using System.Linq;
namespace BookStore.Models
{
    public class SalesModel
    {
        public int SalesId { get; set; }    
        public int? UserId { get; set; }
        public DateTime? SalesDate { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public decimal TotalPriced { get; set; }
        public string FullName { get; set; }    

    }
}
