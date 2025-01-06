using System.ComponentModel.DataAnnotations;

namespace test1.Models
{
    public class Owner
    {
        public int OwnerID { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string Account { get; set; } = string.Empty; 
        [Required]
        public string Password { get; set; } = string.Empty; 

        public List<Pet> Pets { get; set; } = new List<Pet>();


    }
}

    

