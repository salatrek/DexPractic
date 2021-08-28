namespace BankSystem.Models
{
    public class Currency
    {
        public string Name { get;}

        public Currency(Currencies name)
        {
            Name = name.ToString();
        }
    }
}
