using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class LibraryModel
    {
        public List<CategoryModel> Categories { get; set; } 
        public List<BookModel> Books { get; set; }
        public List<String> Authors { get; set; }
    }
}
