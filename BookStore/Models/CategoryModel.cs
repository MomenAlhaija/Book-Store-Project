using BookStore.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class CategoryModel
    {
        public int? CategoryId { get; set; }
        [Required]
        [StringLength(BookConst.CategoryNameMaxLength)]
        public string CategoryName { get; set; }
        [Required]
        [StringLength(BookConst.CategoryDescMaxLength)]
        public string CategoryDescription { get; set; } 
    }
}
