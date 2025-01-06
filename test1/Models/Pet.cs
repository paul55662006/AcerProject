namespace test1.Models
{
    public class Pet
    {
        public int PetID { get; set; } // 主鍵
        public required string PetName { get; set; }
        public required string TypeName { get; set; }
        public int OwnerID { get; set; } // 外鍵

        public required DateTime BirthDate { get; set; }
        public required string Gender { get; set; }
        public required string Neutering { get; set; }
        public string? MicrochipID { get; set; }
        public string? State { get; set; }
        public string? ImagePath { get; set; }

        public Owner? Owner { get; set; }
    }
}
