using System.ComponentModel.DataAnnotations;

namespace DatingAPI.DTOs
{
    public class UserDTO
    {
        [Required]
        public string userName { get; set; }

        [StringLength(8,MinimumLength=4)]
        [Required]
        public string password { get; set; }

    }
}
