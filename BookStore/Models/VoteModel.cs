using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class VoteModel
    {
        public int Rate { get; set; }
        public string UserFullName { get; set; }  
        public int BookId { get; set; } 
    }
}
