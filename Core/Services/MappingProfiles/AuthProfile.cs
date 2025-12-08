using Domain.Entities.IdentityModule;
using Shards.DTOS.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<ApplicationUser, UserInfoDto>();
                
            CreateMap<RefreshToken, RefreshTokenDto>();
            
           
        }
    }
}
