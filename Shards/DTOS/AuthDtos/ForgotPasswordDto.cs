using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shards.DTOS.AuthDtos
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Email Address Is Required")]
        [EmailAddress(ErrorMessage = "Email Address is not valid")]
        public string Email { get; set; } = string.Empty;
    }
}
