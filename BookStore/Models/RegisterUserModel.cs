using BookStore.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class RegisterUserModel
    {
        public int? UserId { get; set; }
        [Required]
        [StringLength(UserConsts.UserNameMaxLength)]
        public string UserName { get; set; }
        [MaxLength(UserConsts.PasswordMaxLength)]
        [RegularExpression(UserConsts.PasswordFormat,ErrorMessage =UserConsts.PasswordErrorMessage)]
        public string Password { get; set; }
        [Required]
        [MaxLength(UserConsts.EmailMaxLength)]
        [RegularExpression(UserConsts.EmailFormat,ErrorMessage =UserConsts.ErrorEmailMessage)]
        public string Email { get; set; }
        [MaxLength(UserConsts.ConatctNumberMaxLength)]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = UserConsts.DateFormat, ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        [Required]
        [StringLength(UserConsts.FirstNameMaxLength)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(UserConsts.LastNameMaxLength)]
        public string LastName { get; set; }
        public int UserType { get; set; }
        [Required]
        public int Gender { get; set; }
        [MaxLength(UserConsts.PasswordMaxLength)]
        [RegularExpression(UserConsts.PasswordFormat, ErrorMessage = UserConsts.PasswordErrorMessage)]

        public string ConfirmPassword { get; set; }
    }
}
