using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Models
{
    public class BookModel
    {
        public DateTime? publishDateValue { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }   
        public string Subject { get;set; }
        public string Author { get; set; }
        public DateTime? PublishDate { get;set; }    
        public string PublishHouse { get; set; }
        public int QuantityInStore { get; set; }
        public string CoverImageUrl { get;set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public  CategoryModel Category { get; set; }    
        public int? Rate { get;set; }
     

    }
}
