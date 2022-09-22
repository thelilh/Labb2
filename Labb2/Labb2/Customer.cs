namespace Labb2
{
    public class Customer
    {
        public enum CustomerLevel
        {
            Gold,
            Silver,
            Bronze
        }
        public string Name { get; private set; }
        public string Password { get; set; }
        public List<Product> Cart { get; }

        public Customer(string name, string password)
        {
            Name = name;
            Password = password;
            Cart = new List<Product>();
        }

        public List<Customer> ReadCustomers()
        {
            var tempList = new List<Customer>();
            var sr = new StreamReader(path: $"{Directory.GetCurrentDirectory()}\\customer.txt");
            var line = sr.ReadLine();
            while (line != null)
            {
                var tempSplit = line.Split(separator: ",");
                var tempName = string.Empty;
                var tempPassword = string.Empty;
                foreach (var x in tempSplit)
                {
                    if (x.Contains("Name"))
                    {
                        tempName = x.Replace("Name:", string.Empty).Replace(" ", string.Empty);
                    }
                    else if (x.Contains("Password"))
                    {
                        tempPassword = x.Replace("Password:", string.Empty).Replace(" ", string.Empty);
                    }
                }
                var tempCustomer = new Customer(name: tempName, password: tempPassword);
                tempList.Add(tempCustomer);
                line = sr.ReadLine();
            }
            sr.Close();
            return tempList;
        }
    }
}
