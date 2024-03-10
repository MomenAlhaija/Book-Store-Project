﻿using BookStore.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class ResetPasswordModel
    {
        [Required]
        [RegularExpression(UserConsts.PasswordFormat, ErrorMessage = UserConsts.PasswordErrorMessage)]

        public string OldPassword { get; set;}
        [Required]
        [RegularExpression(UserConsts.PasswordFormat, ErrorMessage = UserConsts.PasswordErrorMessage)]
        public string NewPassword { get; set;}
        [Required]
        [RegularExpression(UserConsts.PasswordFormat, ErrorMessage = UserConsts.PasswordErrorMessage)]
        public string ConfirmPassword { get; set;}
    }
}
