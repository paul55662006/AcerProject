namespace test1.Models
{
    public class Reservation
    {
        public int Reserve_id { get; set; }
        public int Client_id { get; set; }
        public int Schedule_id { get; set; }
        public string Pet_type  { get; set; }
        public DateTime Date {  get; set; }
        public string Time_period { get; set; }
    }
}
