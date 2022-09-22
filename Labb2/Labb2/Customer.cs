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
        public Dictionary<Product, int> Cart { get; set; } = new Dictionary<Product, int>();
        public CustomerLevel Level { get; internal set; }

        public Customer(string name, string password)
        {
            Name = name;
            Password = password;
            Level = CustomerLevel.Bronze;
            Currency = Currencies.SEK;
        }

        public override string ToString()
        {
            var temp = string.Empty;
            foreach (var x in Cart)
            {
                var tempTotal = $"{Math.Round(x.Key.Price * x.Value, 2)} {x.Key.Currency}";
                temp += $"{x.Key} x {x.Value} = {tempTotal}\n";
            }
            return $"Du är inloggad som {Name}\nLösenord: {Password}\nMedlemsnivå: {Level} Medlem\nValuta: {Currency}\nFöljande finns i din varukorg:\n{temp}";
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
                Console.WriteLine($"{x.Key} x {x.Value}");
                total += x.Key.Price;
            }
            total *= rabatt;
            Console.WriteLine($"Totalt = {Math.Round(total, 2)} {Currency} med {Math.Round((1.0 - rabatt) * 100, 2)}% rabatt");
            Cart.Clear();
            if ((int)Level < (int)CustomerLevel.Gold)
            {
                Level = (CustomerLevel)((int)Level + 1);
            }
            Console.WriteLine("Tryck enter för att fortsätta...");
            Console.ReadKey();
        }
        public static List<Customer> ReadCustomers()
        {
            var tempList = new List<Customer>();
            if (!File.Exists($"{Directory.GetCurrentDirectory()}\\customer.txt"))
            {
                tempList.Add(new Customer(name: "Knatte", password: "123"));
                tempList.Add(new Customer(name: "Fnatte", password: "321"));
                tempList.Add(new Customer(name: "Tjatte", password: "213"));
            }
            else
            {
                var sr = new StreamReader(path: $"{Directory.GetCurrentDirectory()}\\customer.txt");
                var line = sr.ReadLine();
                while (line != null)
                {
                    var tempSplit = line.Split(separator: ",");
                    var tempName = string.Empty;
                    var tempPassword = string.Empty;
                    var tempLevel = CustomerLevel.Bronze;
                    var tempCurrency = Currencies.SEK;
                    foreach (var x in tempSplit)
                    {
                        if (x.Contains("Name"))
                        {
                            tempName = x.Replace("Name:", string.Empty);
                        }
                        else if (x.Contains("Password"))
                        {
                            tempPassword = x.Replace("Password:", string.Empty);
                        }
                        else if (x.Contains("Level"))
                        {
                            tempLevel = (CustomerLevel)int.Parse(x.Replace("Level:", string.Empty));
                        }
                        else if (x.Contains("Currency"))
                        {
                            tempCurrency = (Currencies)int.Parse(x.Replace("Currency:", string.Empty));
                        }
                    }

                    var tempCustomer = new Customer(tempName, tempPassword)
                    {
                        Level = tempLevel,
                        Currency = tempCurrency
                    };
                    tempList.Add(tempCustomer);
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            SaveCustomer(tempList);
            return tempList;
        }
        public static void SaveCustomer(List<Customer> list)
        {
            File.Delete($"{Directory.GetCurrentDirectory()}\\customer.txt");
            var sw = new StreamWriter(path: $"{Directory.GetCurrentDirectory()}\\customer.txt", append: true, Encoding.Default);
            foreach (var customer in list)
            {
                sw.Write($"Name:{customer.Name},Password:{customer.Password},Level:{(int)customer.Level},Currency:{(int)customer.Currency}\n");
            }
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

        public void AddToCart(Product product)
        {
            if (!Cart.ContainsKey(product))
            {
                Cart.Add(product, 1);
            }
            else
            {
                Cart[product]++;
            }
        }
    }
}
