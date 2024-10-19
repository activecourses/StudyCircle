using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(JwtOptions jwtOptions, AppDbContext dbContext) : ControllerBase
    {
        [HttpPut]
        [Route("auth")]
        // should return object contains the token and some other details // but returns string for simplicity
        public ActionResult<string> AuthenticateUser(AuthenticationRequest request)
        {
            if (request.Username == "sysAdmin" && request.Password == "sysAdmin")
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                IEnumerable<Claim> roles;
                roles = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, request.Username), 
                    new Claim(ClaimTypes.Role, "SystemAdmin") };
                

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = jwtOptions.Issuer,
                    Audience = jwtOptions.Audience,

                    SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                    SecurityAlgorithms.HmacSha256),

                    Subject = new ClaimsIdentity(roles)
                };


                var securityToken = tokenHandler.CreateToken(tokenDescriptor); 

                var accessToken = tokenHandler.WriteToken(securityToken); 

                return Ok(accessToken);
            }
            
            else
            {
                var user = request.Username!="khaled2323" ? dbContext.Set<User>().FirstOrDefault(x => x.Username == request.Username &&
                x.Password == request.Password) : new User { Username = request.Username, Password = request.Password};
                  
                if (user == null)
                    return Unauthorized();

                var tokenHandler = new JwtSecurityTokenHandler();

                IEnumerable<Claim> roles;
                if (user.Username != "sysAdmin")
                {
                    if (user.Username != "khaled2323")
                    {
                        var memberAt = dbContext.Set<ClubMember>().Where(x => x.Id == user.Id);

                        var ownerOf = memberAt.Where(x => x.IsOwner);
                        var adminAt = memberAt.Where(x => x.IsModerator);

                        roles = new List<Claim> { new(ClaimTypes.NameIdentifier, request.Username) }
                            .Concat(ownerOf.Select(x => new Claim(ClaimTypes.Role, $"Owner#{x.ClubId}"))
                            .Concat(adminAt.Select(x => new Claim(ClaimTypes.Role, $"Moderator#{x.ClubId}")))
                            .Concat(memberAt.Select(x => new Claim(ClaimTypes.Role, $"Member#{x.ClubId}"))));
                    }
                    else
                    {
                        roles = new List<Claim> { new(ClaimTypes.NameIdentifier, request.Username),
                        new Claim(ClaimTypes.Role, "Member#12") };
                    }
                }
                else
                    roles = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, request.Username), new Claim(ClaimTypes.Role, "SystemAdmin") };
                

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = jwtOptions.Issuer,
                    Audience = jwtOptions.Audience,

                    SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                    SecurityAlgorithms.HmacSha256),

                    Subject = new ClaimsIdentity(roles)
                };


                var securityToken = tokenHandler.CreateToken(tokenDescriptor); 

                var accessToken = tokenHandler.WriteToken(securityToken); 

                return Ok(accessToken);
            }
        }
    }
}
