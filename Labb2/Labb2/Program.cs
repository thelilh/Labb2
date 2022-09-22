
using Labb2;

var customers = Customer.ReadCustomers();
var products = Product.ReadProducts();
bool shouldLogIn;
Customer loggedCustomer = null!;
while (true)
{
    Console.WriteLine("VÄLKOMMEN TILL C#-KÖP\nFör att handla, behöver du logga in!");
    Console.WriteLine("Skriv in ditt namn:");
    var userName = Console.ReadLine();
    foreach (var x in customers)
    {
        if (string.Equals(x.Name, userName))
        {
            loggedCustomer = x;
        }
    }

    if (loggedCustomer != null)
    {
        //Logga in användaren.
        Console.WriteLine($"Hej {userName}!");
        shouldLogIn = !loggedCustomer.CheckPassword();
        if (!shouldLogIn)
        {
            break;
        }
    }
    else
    {
        //Skapa användaren.
        Console.WriteLine($"Vi kunde ej hitta {userName}. Vill du skapa ett konto? (Y/N)");
        var userInput = Console.ReadLine();
        if (userInput != null && userInput.ToLower()[0] == 'y')
        {
            Console.WriteLine("Skriv in önskat lösenord:");
            var userPass = Console.ReadLine();
            if (userName != null && userPass != null)
            {
                var tempCustomer = new Customer(userName, userPass);
                customers.Add(tempCustomer);
                loggedCustomer = tempCustomer;
                Customer.SaveCustomer(customers);
                shouldLogIn = false;
                break;
            }
        }
    }
}
if (!shouldLogIn)
{
    //Meny system där vi visar följande
    //0. Huvudmeny
    //1. Handla något => visa menyn och lägga till i korgen
    //2. Ändra inställningar => Ändra lösenord och valuta
    //3. Avsluta => shouldLogIn = false
    var showMenu = true;
    var menu = 0;
    var selection = 0;
    var list = new List<string?>();
    while (showMenu)
    {
        Console.Clear();
        Console.WriteLine($"VÄLKOMMEN TILL C#-KÖP!\n{loggedCustomer}");
        Console.WriteLine("Vad vill du göra?");
        products = Product.PriceCurrency(products, loggedCustomer.Currency);
        switch (menu)
        {
            case 0:
                list.Clear();
                list.Add("1. Handla något");
                list.Add("2. Ändra inställningar");
                list.Add("3. Avsluta");
                break;
            case 1:
                list.Clear();
                for (var i = 0; i < products.Count; i++)
                {
                    list.Add($"{i + 1}. Köp {products[i]}");
                }
                list.Add($"{products.Count + 1}. Kolla Varukorgen");
                list.Add($"{products.Count + 2}. Betala");
                list.Add($"{products.Count + 3}. Gå tillbaka");
                break;
            case 2:
                list.Clear();
                list.Add("1. Ändra valuta till SEK");
                list.Add("2. Ändra valuta till USD");
                list.Add("3. Ändra valuta till EUR");
                list.Add("4. Ändra valuta till DKK");
                list.Add("5. Gå tillbaka");
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
                if (menu == 0)
                {
                    if (selection == 0)
                    {
                        menu = 1;
                        selection = 0;
                    }
                    else if (selection == 1)
                    {
                        menu = 2;
                        selection = 0;
                    }
                    else if (selection == trueListLength)
                    {
                        showMenu = false;
                    }
                }
                else if (menu == 1)
                {
                    if (selection < trueListLength - 2)
                    {
                        loggedCustomer.AddToCart(products[selection]);
                    }
                    if (selection == trueListLength - 2)
                    {
                        if (loggedCustomer.Cart != null)
                        {
                            Console.WriteLine("Detta finns i din varukorg:");
                            foreach (var x in loggedCustomer.Cart)
                            {
                                Console.WriteLine($"{x.Key} x {x.Value}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Din varukorg är tom!");
                        }

                        Console.WriteLine("Tryck enter för att fortsätta...");
                        Console.ReadKey();
                    }
                    if (selection == trueListLength - 1)
                    {
                        loggedCustomer.BuyProducts();
                        Customer.SaveCustomer(customers);
                    }
                    if (selection == trueListLength)
                    {
                        menu = 0;
                        selection = 0;
                    }
                }
                else if (menu == 2)
                {
                    if (selection < trueListLength)
                    {
                        loggedCustomer.Currency = (Shop.Currencies)selection;
                        Customer.SaveCustomer(customers);
                    }
                    else
                    {
                        menu = 0;
                        selection = 0;
                    }
                }
                break;
        }
    }
}