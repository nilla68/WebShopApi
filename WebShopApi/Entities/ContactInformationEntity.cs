using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopApi.Entities
{
    public class ContactInformationEntity
    {
        public ContactInformationEntity(string phone, string email, byte[] passwordHash, byte[] passwordSalt)
        {
            Phone = phone;
            Email = email;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Phone { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        public int UserEntityId { get; set; }
    }
}
