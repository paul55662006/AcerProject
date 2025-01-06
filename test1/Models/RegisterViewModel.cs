namespace test1.Models
{
    public class RegisterViewModel
    {
        public Owner Owner { get; set; }
        public Pet Pet { get; set; }

        // 下拉式選單選項
        public List<string> TypeNames { get; set; } = new List<string> { "Canine(犬)", "Feline(貓)", "Rabbit(兔)", "Rodent(鼠)", "Else(其他)" };
        public List<string> Genders { get; set; } = new List<string> { "Female(雌性)", "Male(雄性)", "Non-Binary(不確定性別)" };
        public List<string> Neuterings { get; set; } = new List<string> { "Intact(尚未結紮)", "Neutered(已結紮)", "Unknown(不確定)" };
        public List<string> States { get; set; } = new List<string> { "遺失", "送養", "死亡", "正常" };
    }
}
