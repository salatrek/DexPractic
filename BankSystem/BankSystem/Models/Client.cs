namespace BankSystem.Models
{
    internal class Client : IPerson
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int PassportID { get; set; }
        public string Status { get; set; }

        public override int GetHashCode()
        {
            return PassportID;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is Client))
            {
                return false;
            }
            var client = (Client)obj;

            return client.PassportID == PassportID;
        }

    }

    
}
