
using Labb2;

var customers = new Customer().ReadCustomers();
var products = new Product().ReadProducts();
var loggedIn = false;
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
        loggedIn = loggedCustomer.CheckPassword();
        if (loggedIn)
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
                var tempCustomer = new Customer(userName, userPass, true);
                customers.Add(tempCustomer);
                loggedCustomer = tempCustomer;
                loggedIn = true;
                break;
            }
        }
    }
}
if (loggedIn)
{
    //Meny system där vi visar följande
    //0. Huvudmeny
    //1. Handla något => visa menyn och lägga till i korgen
    //2. Ändra inställningar => Ändra lösenord och valuta
    //3. Avsluta => loggedIn = false
    var showMenu = true;
    var menu = 0;
    var selection = 0;
    var list = new List<string?>();
    while (showMenu)
    {
        Console.Clear();
        Console.WriteLine($"VÄLKOMMEN TILL C#-KÖP!\n{loggedCustomer}");
        Console.WriteLine("Vad vill du göra?");
        products = new Product().PriceCurrency(products, loggedCustomer.Currency);
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
                    list.Add($"{i + 1}. Köp {products[i].Name} ({products[i].Price} {loggedCustomer.Currency})");
                }
                list.Add($"{products.Count + 1}. Kolla Varukorgen");
                list.Add($"{products.Count + 2}. Gå tillbaka");
                break;
            case 2:
                list.Clear();
                list.Add("1. Ändra valuta till SEK");
                list.Add("2. Ändra valuta till USD");
                list.Add("3. Ändra valuta till EUR");
                list.Add("4. Ändra valuta till DKK");
                list.Add("5. Gå tillbaka");
                break;
            case 3:
                list.Clear();
                showMenu = false;
                break;
        }

        foreach (var x in list)
        {
            Console.WriteLine(x);
        }
        showMenu = false;
    }
}