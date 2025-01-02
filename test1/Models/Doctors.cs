namespace test1.Models
{
    public class Doctors
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Specialty { get; set; }

        public ICollection<ScheduleDoctor> ScheduleDoctors { get; set; }
    }
}
