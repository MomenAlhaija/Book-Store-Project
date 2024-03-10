using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BookStore.Consts;


namespace BookStore.Models
{
    public class AddOrUpdateBookModel
    {
        public int? BookId { get; set; }
        [Required]
        [StringLength(BookConst.BookTitleMaxLength)]
        public string BookTitle { get; set; }
        [Required]
        [StringLength(BookConst.BookSubjectMaxLength)]
        public string Subject { get; set; }
        [Required]
        [StringLength(BookConst.BookDescriptionMaxLength)]
        public string Description { get; set; }
        [Required]
        [StringLength(BookConst.AuthorNameMaxLength)]
        public string Author {  get; set; }
        [StringLength(BookConst.PublisherNameMaxLength)]
        public string Publisher { get; set; }

        public DateTime? Published { get; set; }
        [StringLength(BookConst.CategoryNameMaxLength)]
        public string CategoryName { get; set; }
        [Required]

        public int CategoryId { get; set; } 
        [Required]
        [Range(BookConst.MinBookQuantity,BookConst.MaxBookQuantity)]
        public int QuantityInStore { get; set; }
        [Required]
        [Range((int)BookConst.MinBookPrice,(int)BookConst.MaxBookPrice)]
        public decimal Price { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        [MaxLength(BookConst.FileNameMaxLength)]
        public string CoverImageUrl { get; set; }
        public List<SelectListItem> CategorySelectList { get; set; }
    }
}
