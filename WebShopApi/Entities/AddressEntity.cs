using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Entities
{
    public class AddressEntity
    {
        public AddressEntity(string street, string postCode, string city, string country)
        {
            Street = street;
            PostCode = postCode;
            City = city;
            Country = country;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Street { get; set; }

        [Required]
        [StringLength(50)]
        public string PostCode { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }

        public ICollection<UserEntity> Users { get; set; }
    }
}
