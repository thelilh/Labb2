
using Labb2;

var customers = new Customer().ReadCustomers();
var products = new Product().ReadProducts();
var loggedIn = false;
Customer loggedCustomer = null!;
foreach (var x in products)
{
    Console.WriteLine(x.ToString());
}
products = new Product().PriceCurrency(products, Shop.Currencies.USD);
foreach (var x in products)
{
    Console.WriteLine(x.ToString());
}
while (!loggedIn)
{
    Console.WriteLine("VÄLKOMMEN TILL C#-KÖP\nFör att handla, behöver du logga in!");
    Console.WriteLine("Skriv in ditt namn:");
    var userName = Console.ReadLine();
    var userPass = string.Empty;
    var isUser = false;
    foreach (var x in customers)
    {
        if (string.Equals(x.Name, userName))
        {
            loggedCustomer = x;
            isUser = true;
        }
    }

    if (isUser)
    {
        //Logga in användaren.
        Console.WriteLine($"Hej {userName}!");
        var attempts = 0;
        while (true)
        {
            if (attempts > 3)
            {
                Console.WriteLine("För många försök, försök igen senare!");
                break;
            }
            Console.WriteLine("Lösenord:");
            userPass = Console.ReadLine();
            if (loggedCustomer != null && loggedCustomer.Password.Equals(userPass))
            {
                loggedIn = true;
                break;
            }
            attempts++;
            Console.WriteLine("Fel lösenord.");
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
            userPass = Console.ReadLine();
            if (userName != null && userPass != null)
            {
                var tempCustomer = new Customer(userName, userPass, true);
                customers.Add(tempCustomer);
                loggedCustomer = tempCustomer;
                loggedIn = true;
            }
        }
    }
}
if (loggedCustomer != null)
{
    while (loggedIn)
    {
        //Meny system där vi visar följande
        //1. Handla något => visa menyn och lägga till i korgen
        //2. Ändra inställningar => Ändra lösenord och valuta
        //3. Avsluta => loggedIn = false
        Console.Clear();
        Console.WriteLine($"VÄLKOMMEN TILL C#-KÖP!\n{loggedCustomer.ToString()}");
        Console.WriteLine("Vad vill du göra?");
        break;

    }
}