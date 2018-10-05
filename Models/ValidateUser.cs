using System;
using System.ComponentModel.DataAnnotations;

namespace c__exam
{
    public class ValidateUser
    {
        [Required(ErrorMessage="Must provide First Name")]
        [MinLength(2, ErrorMessage="First Name must be at least 2 characters long")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage="First Name must be valid")]
        public string FirstName { get ; set; }

        [Required(ErrorMessage="Must provide Last Name")]
        [MinLength(2, ErrorMessage="Last Name must be at least 2 characters long")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage="Last Name must be valid")]
        public string LastName { get ; set; }

        [Required(ErrorMessage="Must provide Email")]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Must be a valid email")]
        public string Email { get ; set; }

        [Required(ErrorMessage="Must provide a Password")]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters long")]
        [DataType(DataType.Password)]
        // [RegularExpression("^.*(?=.{4,10})(?=.*)(?=.*[a-zA-Z]).*$", ErrorMessage="Password must have at least one letter, number and special character")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*)(?=.*[$@$!%*#?&])[A-Za-z$@$!%*#?&].*$", ErrorMessage="Password must have at least one letter, number and special character")]
        public string Password { get ; set; }
        
        [Required(ErrorMessage="Must confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage="Passwords must match")]
        public string ConfirmPassword { get ; set; }
    }
}