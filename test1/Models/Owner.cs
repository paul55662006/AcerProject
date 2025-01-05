using System.ComponentModel.DataAnnotations;

namespace test1.Models
{
    public class Owner
    {
        public int OwnerID { get; set; } // 主鍵
        public string Name { get; set; } // 名字
        public string PhoneNumber { get; set; } // 電話號碼
        public string? Email { get; set; } // 電子郵件 (允許 Null)
        public string Address { get; set; } // 地址
        public string Account { get; set; } // 帳號
        public string Password { get; set; } // 密碼
    }
}
