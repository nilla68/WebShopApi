using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WebShopApi.Dtos;
using WebShopApi.Entities;
using WebShopApi.Filter;

namespace WebShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseApiKey]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly WebShopDbContext _dbContext;

        public UsersController(ILogger<UsersController> logger, WebShopDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var userEntities = await _dbContext.Users
                .Include(u => u.ContactInformation)
                .Include(u => u.Address)
                .ToListAsync();

            List<UserDto> userDtos = new List<UserDto>();

            foreach (var userEntity in userEntities)
            {
                var userDto = new UserDto(
                    userEntity.Id,
                    userEntity.FirstName,
                    userEntity.LastName,
                    userEntity.ContactInformation.Email,
                    userEntity.ContactInformation.Phone,
                    userEntity.Address.Street,
                    userEntity.Address.PostCode,
                    userEntity.Address.City,
                    userEntity.Address.Country);

                userDtos.Add(userDto);
            }

            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var userEntity = await _dbContext.Users
                .Include(u => u.ContactInformation)
                .Include(u => u.Address)
                .FirstOrDefaultAsync(user => user.Id == id);

            if (userEntity is null)
                return NotFound();

            var userDto = new UserDto(
                    userEntity.Id,
                    userEntity.FirstName,
                    userEntity.LastName,
                    userEntity.ContactInformation.Email,
                    userEntity.ContactInformation.Phone,
                    userEntity.Address.Street,
                    userEntity.Address.PostCode,
                    userEntity.Address.City,
                    userEntity.Address.Country);

            return Ok(userDto);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
        {
            if (await _dbContext.Users.AnyAsync(u => u.ContactInformation.Email == createUserDto.Email))
                return BadRequest("User already exist.");

            using var hmac = new HMACSHA512();
            var passwordSalt = hmac.Key;
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(createUserDto.Password));

            var contactInformation = new ContactInformationEntity(createUserDto.Phone, createUserDto.Email, passwordHash, passwordSalt);
            var addressEntity = await _dbContext.Address.FirstOrDefaultAsync(a => a.Street == createUserDto.Street && a.PostCode == createUserDto.PostCode);

            UserEntity userEntity;
            if (addressEntity is null)
            {
                addressEntity = new AddressEntity(createUserDto.Street, createUserDto.PostCode, createUserDto.City, createUserDto.Country);
                userEntity = new UserEntity(createUserDto.FirstName, createUserDto.LastName, contactInformation, addressEntity);
            }
            else
            {
                userEntity = new UserEntity(createUserDto.FirstName, createUserDto.LastName, contactInformation, addressEntity.Id);
            }

            _dbContext.Users.Add(userEntity);
            await _dbContext.SaveChangesAsync();

            var userDto = new UserDto(
                    userEntity.Id,
                    userEntity.FirstName,
                    userEntity.LastName,
                    userEntity.ContactInformation.Email,
                    userEntity.ContactInformation.Phone,
                    userEntity.Address.Street,
                    userEntity.Address.PostCode,
                    userEntity.Address.City,
                    userEntity.Address.Country);

            return CreatedAtAction("GetUser", new { id = userEntity.Id }, userDto);
        }

        [HttpPut("{id}/address")]
        public async Task<ActionResult> UpdateUserAddress(int id, AddressDto addressDto)
        {
            var userEntity = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (userEntity is null)
                return NotFound();

            var addressEntity = await _dbContext.Address.FirstOrDefaultAsync(a => a.Street == addressDto.Street && a.PostCode == addressDto.PostCode);

            if (addressEntity is null)
            {
                addressEntity = new AddressEntity(addressDto.Street, addressDto.PostCode, addressDto.City, addressDto.Country);
                userEntity.Address = addressEntity;
            }
            else
            {
                userEntity.AddressId = addressEntity.Id;
            }

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/password")]
        public async Task<ActionResult> UpdateUserAddress(int id, UpdatePasswordDto updatePasswordDto)
        {
            var userEntity = await _dbContext.Users.Include(u => u.ContactInformation).FirstOrDefaultAsync(u => u.Id == id && u.ContactInformation.Email == updatePasswordDto.Email);

            if (userEntity is null)
                return NotFound();

            using (var hmac = new HMACSHA512(userEntity.ContactInformation.PasswordSalt))
            {
                var hashForOldPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(updatePasswordDto.OldPassword));

                for (int i = 0; i < userEntity.ContactInformation.PasswordHash.Length; i++)
                {
                    if (hashForOldPassword[i] != userEntity.ContactInformation.PasswordHash[i])
                    {
                        return BadRequest();
                    }
                }
            }

            using (var hmacNewPassword = new HMACSHA512())
            {
                var newPasswordSalt = hmacNewPassword.Key;
                var newPasswordHash = hmacNewPassword.ComputeHash(Encoding.UTF8.GetBytes(updatePasswordDto.NewPassword));

                userEntity.ContactInformation.PasswordSalt = newPasswordSalt;
                userEntity.ContactInformation.PasswordHash = newPasswordHash;

                await _dbContext.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userEntity = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (userEntity is null)
                return NotFound();

            _dbContext.Users.Remove(userEntity);
            await _dbContext.SaveChangesAsync();

            return NoContent();

        }
    }
}
