using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebShopApi.Dtos;
using WebShopApi.Filter;

namespace WebShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly WebShopDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthenticationController(ILogger<AuthenticationController> logger, WebShopDbContext dbContext, IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPost("SignIn")]
        [UseApiKey]
        public async Task<ActionResult> SignIn(SignInDto signInDto)
        {
            if (string.IsNullOrEmpty(signInDto.Email) || string.IsNullOrEmpty(signInDto.Password))
                return BadRequest("Incorrect email address or password");

            var userEntity = await _dbContext.Users.Include(u => u.ContactInformation).FirstOrDefaultAsync(x => x.ContactInformation.Email == signInDto.Email);

            if (userEntity is null)
                return BadRequest("Incorrect email address or password");

            var validPassword = true;
            using (var hmac = new HMACSHA512(userEntity.ContactInformation.PasswordSalt))
            {
                var _hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signInDto.Password));

                for (int i = 0; i < userEntity.ContactInformation.PasswordHash.Length; i++)
                    if (_hash[i] != userEntity.ContactInformation.PasswordHash[i])
                        validPassword = false;
            }

            if (!validPassword)
                return BadRequest("Incorrect email address or password");

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", userEntity.Id.ToString()),
                    new Claim(ClaimTypes.Name, userEntity.ContactInformation.Email),
                    new Claim("code", _configuration.GetValue<string>("ApiKey"))
                }),
                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Secret"))),
                    SecurityAlgorithms.HmacSha512Signature
                )
            };

            return Ok(tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor)));
        }
    }
}
