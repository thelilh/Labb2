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
        public string Name { get; internal set; } = null!;
        public string Password { get; set; } = null!;
        public List<Product> Cart { get; } = null!;
        public CustomerLevel Level { get; internal set; }

        public Customer(string name, string password, bool shouldSave)
        {
            Name = name;
            Password = password;
            Level = CustomerLevel.Bronze;
            Cart = new List<Product>();
            Currency = Currencies.SEK;
            if (shouldSave)
            {
                SaveCutomer(this);
            }
        }

        public Customer()
        {
        }

        public override string ToString()
        {
            return $"Du är inloggad som {Name}\nMedlemsnivå: {Level} Medlem\nValuta: {Currency}";
        }

        public void BuyProducts()
        {
            double total = 0;
            double rabatt = 0; //0-1 (0%-100%)
            switch (Level)
            {
                case CustomerLevel.Bronze:
                    rabatt = 0.95;
                    break;
                case CustomerLevel.Silver:
                    rabatt = 0.90;
                    break;
                case CustomerLevel.Gold:
                    rabatt = 0.85;
                    break;
            }
            Console.WriteLine($"Kvitto för {Name}");
            foreach (var x in Cart)
            {
                Console.WriteLine($"{x.Name} = {x.Price} {Currency}");
                total += x.Price;
            }
            total *= rabatt;
            Console.WriteLine($"Totalt = {total} {Currency} med {(1.0 - rabatt) * 100}% rabatt");
        }
        public List<Customer> ReadCustomers()
        {
            var tempList = new List<Customer>();
            if (!File.Exists($"{Directory.GetCurrentDirectory()}\\customer.txt"))
            {
                tempList.Add(new Customer(name: "Knatte", password: "123", shouldSave: true));
                tempList.Add(new Customer(name: "Fnatte", password: "321", shouldSave: true));
                tempList.Add(new Customer(name: "Tjatte", password: "213", shouldSave: true));
            }
            else
            {
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
            }
            return tempList;
        }
        public void SaveCutomer(Customer customer)
        {
            var sw = new StreamWriter(path: $"{Directory.GetCurrentDirectory()}\\customer.txt", append: true, Encoding.Default);
            sw.Write($"\nName:{customer.Name},Password:{customer.Password},Level:{(int)customer.Level},Currency:{(int)Currency}");
            sw.Close();
        }

        public bool CheckPassword()
        {
            var attempts = 0;
            while (true)
            {
                if (attempts > 3)
                {
                    Console.WriteLine("För många försök, försök igen senare!");
                    break;
                }
                Console.WriteLine("Lösenord:");
                var userPass = Console.ReadLine();
                if (Password.Equals(userPass))
                {
                    return true;
                }
                attempts++;
                Console.WriteLine("Fel lösenord.");
            }
            return false;
        }
    }
}
