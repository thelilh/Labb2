
using Labb2;
using System.Text;

var customers = new Customer(name: "Admin", password: "Admin").ReadCustomers();
var products = new Product(name: "Admin", price: 0).ReadProducts();
foreach (var x in customers)
{
    Console.WriteLine($"{x.Name} has password {x.Password}");
}
foreach (var x in products)
{
    Console.WriteLine($"{x.Name} costs {x.Price}");
}


//Funktionerna WriteToFile & ReadFromFile
void WriteToFile(string file, string text, bool shouldAppend)
{
    string path = Directory.GetCurrentDirectory();
    if (!file.Contains(".txt")) { file += ".txt"; }
    var sw = new StreamWriter(path: $"{path}\\{file}", append: shouldAppend, Encoding.ASCII);
    sw.Write(text);
    sw.Close();
}