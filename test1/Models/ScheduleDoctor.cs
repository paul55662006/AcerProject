using System.Numerics;

namespace test1.Models
{
    public class ScheduleDoctor
    {
        public int ScheduleId { get; set; }
        public Schedules Schedule { get; set; }

        public int DoctorId { get; set; }
        public Doctors Doctor { get; set; }
    }
}
