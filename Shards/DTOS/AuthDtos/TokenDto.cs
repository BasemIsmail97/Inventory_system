using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shards.DTOS.AuthDtos
{
    public class TokenDto
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
        public UserInfoDto User { get; set; }
    }
}
