namespace BankSystem.Models
{
    internal interface IPerson
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int PassportID { get; set; }
    }
}
