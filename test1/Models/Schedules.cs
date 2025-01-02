﻿namespace test1.Models
{
    public class Schedules
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string TimeSlot { get; set; }
        public string PetType { get; set; }
        public string Title { get; set; }

        public ICollection<ScheduleDoctor> ScheduleDoctors { get; set; }

    }
}
