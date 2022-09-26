
using Labb2;

var customers = Customer.ReadCustomers();
var products = Product.ReadProducts();
var isLoggedIn = false;
Customer loggedCustomer = null!;
while (!isLoggedIn)
{
    Console.WriteLine("VÄLKOMMEN TILL C#-KÖP\nFör att handla, behöver du logga in!");
    Console.WriteLine("Skriv in ditt namn:");
    var userName = Console.ReadLine();
    foreach (var x in customers)
    {
        if (userName == x.Name)
        {
            loggedCustomer = x;
        }
    }

    if (loggedCustomer != null)
    {
        //Logga in användaren.
        Console.WriteLine($"Hej {userName}!");
        isLoggedIn = loggedCustomer.CheckPassword();
        if (!isLoggedIn)
        {
            loggedCustomer = null!;
        }
    }
    else
    {
        //Skapa användaren.
        Console.WriteLine($"Vi kunde ej hitta {userName}. Vill du skapa ett konto? (Y/N)");
        var userInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(userInput) && userInput.ToLower()[0] == 'y')
        {
            while (true)
            {
                Console.WriteLine("Skriv in önskat lösenord:");
                var userPass = Console.ReadLine();
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPass))
                {
                    if (!userPass.Contains(','))
                    {
                        var tempCustomer = new Customer(name: userName, password: userPass, level: Customer.CustomerLevel.Bronze, currency: Shop.Currencies.SEK);
                        customers.Add(tempCustomer);
                        loggedCustomer = tempCustomer;
                        Customer.SaveCustomers(customers);
                        isLoggedIn = true;
                        break;
                    }
                    Console.WriteLine("Lösenord kan inte innehålla ','");
                }
            }
        }
    }
}
if (isLoggedIn && loggedCustomer != null)
{
    //Meny system där vi visar följande
    //0. Huvudmeny
    //1. Handla något => visa menyn och lägga till i korgen
    //2. Ändra inställningar => Ändra lösenord och valuta
    //3. Avsluta => isLoggedIn = false
    var showMenu = true;
    var menu = 0;
    var selection = 0;
    var list = new List<string?>();
    while (showMenu)
    {
        Console.Clear();
        Console.WriteLine($"VÄLKOMMEN TILL C#-KÖP!\n{loggedCustomer}");
        Console.WriteLine("Vad vill du göra?");
        foreach (var product in products)
        {
            product.Currency = loggedCustomer.Currency;
        }
        switch (menu)
        {
            case 1:
                list.Clear();
                for (var i = 0; i < products.Count; i++)
                {
                    list.Add($"{i + 1}. Köp {products[i]}");
                }
                list.Add($"{products.Count + 1}. Kolla Varukorgen");
                list.Add($"{products.Count + 2}. Betala");
                list.Add($"{products.Count + 3}. Gå Tillbaka");
                break;
            case 2:
                list.Clear();
                list.Add("1. Ändra Valuta");
                list.Add("2. Ändra Lösenord");
                list.Add("3. Gå Tillbaka");
                break;
            case 3:
                list.Clear();
                list.Add("1. Ändra Valuta till SEK");
                list.Add("2. Ändra Valuta till USD");
                list.Add("3. Ändra Valuta till EUR");
                list.Add("4. Ändra Valuta till DKK");
                list.Add("5. Gå Tillbaka");
                break;
            default:
                list.Clear();
                list.Add("1. Handla Något");
                list.Add("2. Ändra Inställningar");
                list.Add("3. Avsluta");
                break;
        }

        var trueListLength = list.Count - 1;

        for (var i = 0; i < list.Count; i++)
        {
            if (selection == i)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }
            Console.WriteLine(list[i]);
            Console.ResetColor();
        }

        var curKey = Console.ReadKey();
        var shouldDo = false;
        switch (curKey.Key)
        {
            case ConsoleKey.UpArrow:
                if (selection <= 0)
                {
                    selection = trueListLength;
                }
                else
                {
                    selection--;
                }
                break;
            case ConsoleKey.DownArrow:
                if (selection == trueListLength)
                {
                    selection = 0;
                }
                else
                {
                    selection++;
                }
                break;
            case ConsoleKey.Enter:
                shouldDo = true;
                break;
        }

        if (shouldDo)
        {
            if (menu == 0)
            {
                switch (selection)
                {
                    case 0:
                        menu = 1;
                        selection = 0;
                        break;
                    case 1:
                        menu = 2;
                        selection = 0;
                        break;
                    default:
                        showMenu = false;
                        break;
                }
            }
            else if (menu == 1)
            {
                if (selection < trueListLength - 2)
                {
                    loggedCustomer.AddToCart(products[selection]);
                }
                else if (selection == trueListLength - 2)
                {
                    Console.WriteLine(loggedCustomer.DisplayCart());
                    Console.WriteLine("Tryck ENTER för att fortsätta...");
                    Console.ReadKey();
                }
                else if (selection == trueListLength - 1)
                {
                    loggedCustomer.BuyProducts();
                }
                else
                {
                    menu = 0;
                    selection = 0;
                }
            }
            else if (menu == 2)
            {
                if (selection == 0)
                {
                    menu = 3;
                    selection = 0;
                }
                else if (selection == 1)
                {
                    Console.WriteLine("Vad vill du ändra ditt lösenord till?");
                    var temp = Console.ReadLine();
                    if (!string.IsNullOrEmpty(temp) && !temp.Contains(',') && temp != loggedCustomer.Password)
                    {
                        Console.WriteLine($"Lösenord för {loggedCustomer.Name} ändrades till \"{temp}\" från \"{loggedCustomer.Password}\".");
                        loggedCustomer.Password = temp;
                        Customer.SaveCustomers(customers);
                    }
                    else if (temp == loggedCustomer.Password)
                    {
                        Console.WriteLine($"Lösenordet för {loggedCustomer.Name} är redan \"{temp}\".");
                    }
                    else if (temp.Contains(','))
                    {
                        Console.WriteLine("Lösenordet kan inte innehålla ','.");
                    }
                    Console.WriteLine("Tryck ENTER för att fortsätta...");
                    Console.ReadKey();
                }
                else
                {
                    menu = 0;
                    selection = 0;
                }
            }
            else if (menu == 3)
            {
                if (selection != trueListLength)
                {
                    if (loggedCustomer.Currency != (Shop.Currencies)selection)
                    {
                        loggedCustomer.Currency = (Shop.Currencies)selection;
                        Console.WriteLine($"Valuta för {loggedCustomer.Name} är nu {loggedCustomer.Currency}");
                        Customer.SaveCustomers(customers);
                    }
                    else
                    {
                        Console.WriteLine($"Valuta för {loggedCustomer.Name} är redan {loggedCustomer.Currency}");
                    }
                    Console.WriteLine("Tryck ENTER för att fortsätta...");
                    Console.ReadKey();
                }
                menu = 2;
                selection = 0;
            }

        }
    }
}