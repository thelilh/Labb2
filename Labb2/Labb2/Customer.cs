using System.Text;

namespace Labb2;

public class Customer : Shop
{
    [Flags]
    public enum CustomerLevel
    {
        Bronze = 0,
        Silver = 1,
        Gold = 2
    }
    public string Name { get; internal set; }
    public string Password { get; set; }
    public Dictionary<Product, int> Cart { get; set; } = new();
    public CustomerLevel Level { get; internal set; }

    public Customer(string name, string password, CustomerLevel level, Currencies currency)
    {
        Name = name;
        Password = password;
        Level = level;
        Currency = currency;
    }

    public override string ToString()
    {
        return $"Du är inloggad som {Name}\nLösenord: {Password}\nMedlemsnivå: {Level} Medlem\nValuta: {Currency}\n";
    }

    public string DisplayCart()
    {
        string temp;
        if (Cart.Count > 0)
        {
            temp = "Detta finns i din varukorg:\n";
            foreach (var x in Cart)
            {
                temp += $"{x.Key} x {x.Value}\n";
            }
        }
        else
        {
            temp = "Din varukorg är tom!\n";
        }
        return temp;
    }

    public void BuyProducts()
    {
        var total = 0.0;
        var rabatt = Level switch
        {
            CustomerLevel.Bronze => 0.95,
            CustomerLevel.Silver => 0.90,
            CustomerLevel.Gold => 0.85,
            _ => 0
        };
        Console.WriteLine($"Kvitto för {Name}");
        foreach (var x in Cart)
        {
            Console.WriteLine($"{x.Key} x {x.Value}");
            total += x.Key.Price[(int)Currency] * x.Value;
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
        if (!File.Exists(path: $"{Directory.GetCurrentDirectory()}\\customer.txt"))
        {
            tempList.Add(new Customer(name: "Knatte", password: "123", currency: Currencies.SEK, level: CustomerLevel.Gold));
            tempList.Add(new Customer(name: "Fnatte", password: "321", currency: Currencies.DKK, level: CustomerLevel.Silver));
            tempList.Add(new Customer(name: "Tjatte", password: "213", currency: Currencies.EUR, level: CustomerLevel.Bronze));
        }
        else
        {
            var sr = new StreamReader(path: $"{Directory.GetCurrentDirectory()}\\customer.txt");
            var line = sr.ReadLine();
            while (!string.IsNullOrEmpty(line))
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

                tempList.Add(new Customer(name: tempName, password: tempPassword, level: tempLevel, currency: tempCurrency));
                line = sr.ReadLine();
            }
            sr.Close();
        }
        SaveCustomers(tempList);
        return tempList;
    }
    public static void SaveCustomers(List<Customer> list)
    {
        var sw = new StreamWriter(path: $"{Directory.GetCurrentDirectory()}\\customer.txt", append: false, Encoding.Default);
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
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Fel lösenord.");
            Console.ResetColor();
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