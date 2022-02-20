using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopApi.Dtos
{
    public class UpdatePasswordDto
    {
        [Required]
        [StringLength(100)]
        public string Email{ get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string NewPassword { get; set; }
    }
}
