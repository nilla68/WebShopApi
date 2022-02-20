using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos
{
    public class CreateUserDto
    {
        public CreateUserDto(string firstName, string lastName, string email, string password, string phone, string street, string postCode, string city, string country)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Phone = phone;
            Street = street;
            PostCode = postCode;
            City = city;
            Country = country;
        }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Phone { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Street { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string PostCode { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string City { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Country { get; set; }
    }
}
