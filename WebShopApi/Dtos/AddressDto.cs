using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos
{
    public class AddressDto
    {
        public AddressDto(string street, string postCode, string city, string country)
        {
            Street = street;
            PostCode = postCode;
            City = city;
            Country = country;
        }

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
