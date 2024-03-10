using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Consts
{
    public  static class UserConsts
    {
        public const int UserNameMaxLength = 20;
        public const int PasswordMaxLength = 20;
        public const int FirstNameMaxLength = 50;
        public const int LastNameMaxLength = 50;  
        public const int EmailMaxLength = 50;
        public const int ConatctNumberMaxLength = 50;
        public const int UserTypeDescMaxLength = 50;
        public const int GenderDescMaxLength = 50;
        public const string DateFormat = "{0:yyyy/MM/dd}";
        public const string PasswordFormat = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*_\-]).{8,}$";
        public const string EmailFormat = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
        public const string ErrorEmailMessage = "he email address is not valid. Please enter a valid email address in the format example@example.com";
        public const string PasswordErrorMessage = "The password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character (!@#$%^&*).";
    }
}
