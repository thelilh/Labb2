using System.Text;

namespace Labb2
{
    public class Customer : Shop
    {
        [Flags]
        public enum CustomerLevel
        {
            Bronze = 0,
            Silver = 1,
            Gold = 2
        }
        public string Name { get; private set; } = null!;
        public string Password { get; set; } = null!;
        public List<Product> Cart { get; } = null!;
        public CustomerLevel Level { get; private set; }
        public Currencies Currency { get; set; }

        public Customer(string name, string password, bool shouldSave)
        {
            Name = name;
            Password = password;
            Level = CustomerLevel.Bronze;
            Cart = new List<Product>();
            Currency = Currencies.SEK;
            if (shouldSave)
            {
                SaveCutomer();
            }
        }

        public Customer()
        {
        }

        public List<Customer> ReadCustomers()
        {
            var tempList = new List<Customer>();
            var sr = new StreamReader(path: $"{Directory.GetCurrentDirectory()}\\customer.txt");
            var line = sr.ReadLine();
            while (line != null)
            {
                var tempSplit = line.Split(separator: ",");
                var tempCustomer = new Customer();
                foreach (var x in tempSplit)
                {
                    if (x.Contains("Name"))
                    {
                        tempCustomer.Name = x.Replace("Name:", string.Empty);
                    }
                    else if (x.Contains("Password"))
                    {
                        tempCustomer.Password = x.Replace("Password:", string.Empty);
                    }
                    else if (x.Contains("Level"))
                    {
                        tempCustomer.Level = (CustomerLevel)int.Parse(x.Replace("Level:", string.Empty));
                    }
                    else if (x.Contains("Currency"))
                    {
                        tempCustomer.Currency = (Currencies)int.Parse(x.Replace("Currency:", string.Empty));
                    }
                }
                tempList.Add(tempCustomer);
                line = sr.ReadLine();
            }
            sr.Close();
            return tempList;
        }
        public void SaveCutomer()
        {
            var sw = new StreamWriter(path: $"{Directory.GetCurrentDirectory()}\\customer.txt", append: true, Encoding.Default);
            sw.Write($"\nName:{Name},Password:{Password},Level:{(int)Level},Currency:{(int)Currency}");
            sw.Close();
        }
        public override string ToString()
        {
            return $"Du är inloggad som {Name}\nMedlemsnivå: {Level} Medlem\nValuta: {Currency}";
        }

        public void BuyProducts()
        {
            double total = 0;
            foreach (var x in Cart)
            {
                Console.WriteLine($"{x.ToString()} {Currency}");
                total += x.Price;
            }
            Console.WriteLine($"Du behöver betala {total}");
        }
    }
}
