using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace test1.Models
{
    public class Schedules
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string TimeSlot { get; set; }
        public string DoctorName { get; set; } // 醫生名稱
        public string DoctorEmail { get; set; } // 醫生 Email
        public string DoctorPhone { get; set; } // 醫生電話
        public string DoctorSpecialty { get; set; } // 醫生專業
    }
}
