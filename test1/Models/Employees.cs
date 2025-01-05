using System.ComponentModel.DataAnnotations;

namespace test1.Models
{
    public class Employees
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "帳號是必填的")]
        [StringLength(50, ErrorMessage = "帳號長度不可超過50字元")]
        public string Account { get; set; }

        [Required(ErrorMessage = "密碼是必填的")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email 是必填的")]
        [EmailAddress(ErrorMessage = "Email 格式不正確")]
        public string Email { get; set; }
    }
}
