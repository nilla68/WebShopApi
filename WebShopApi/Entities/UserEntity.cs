using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Entities
{
    public class UserEntity
    {
        public UserEntity()
        {

        }

        public UserEntity(string firstName, string lastName, ContactInformationEntity contactInformation, AddressEntity address)
        {
            FirstName = firstName;
            LastName = lastName;
            ContactInformation = contactInformation;
            Address = address;
        }

        public UserEntity(string firstName, string lastName, ContactInformationEntity contactInformation, int addressId)
        {
            FirstName = firstName;
            LastName = lastName;
            ContactInformation = contactInformation;
            AddressId = addressId;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public ContactInformationEntity ContactInformation { get; set; }

        public AddressEntity Address { get; set; }
     
        public int AddressId { get; set; }
    }
}
