using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shards.DTOS.AuthDtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "First Name Is Required")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name Is Required")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email Address Is Required")]
        [EmailAddress(ErrorMessage = "Email Address is not valid")]
        public string Email { get; set; }   = string.Empty;

        [Required(ErrorMessage = "User Name Is Required")]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Number Is Required")]
        [Phone(ErrorMessage = "Phone Number is not valid")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password Is Required")]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = " Confirm Password Is Required")]
        [Compare("Password", ErrorMessage = "The password and confirmation do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }   = string.Empty;
        public string role { get; set; } = string.Empty;
    }
}
