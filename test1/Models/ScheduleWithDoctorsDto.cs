namespace test1.Models
{
    public class ScheduleWithDoctorsDto
    {
        public DateTime Date { get; set; }
        public string TimeSlot { get; set; }
        public string PetType { get; set; }
        public string Title { get; set; }
        public List<int> DoctorIds { get; set; }  // 改為 int
    }

}
