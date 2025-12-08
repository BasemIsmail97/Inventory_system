using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shards.DTOS.AuthDtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Email Address Is Required")]
        [EmailAddress(ErrorMessage = "Email Address is not valid")]
        public string Email { get; set; } = string.Empty;

        [Required (ErrorMessage ="Token Is Required")]
        public string Token { get; set; } = string.Empty;


        [Required(ErrorMessage = "New Password Is Required")]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password Is Required")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
