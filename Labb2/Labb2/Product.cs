using System.Text;

namespace Labb2;

public class Product : Shop
{
    public double[] Price { get; set; }
    public string Name { get; set; }

    public Product(string name, double[] price)
    {
        Name = name;
        Price = price;
        Currency = Currencies.SEK;
    }

    public override string ToString()
    {
        var temp = (int)Currency;
        return $"{Name} för {Math.Round(Price[temp], 2)} {Currency}";
    }
    public static void SaveProducts(List<Product> list)
    {
        var sw = new StreamWriter(path: $"{Directory.GetCurrentDirectory()}\\product.txt", append: false, Encoding.Default);
        foreach (var product in list)
        {
            sw.Write($"Name:{product.Name},Price:{product.Price[0]}\n");
        }
        sw.Close();
    }
    public static List<Product> ReadProducts()
    {
        var tempList = new List<Product>();
        if (!File.Exists($"{Directory.GetCurrentDirectory()}\\product.txt"))
        {
            tempList.Add(new Product(name: "Kaffe", price: ChangePrices(50.0)));
            tempList.Add(new Product(name: "Dricka", price: ChangePrices(23.0)));
            tempList.Add(new Product(name: "Äpple", price: ChangePrices(5.0)));
        }
        else
        {
            var sr = new StreamReader(path: $"{Directory.GetCurrentDirectory()}\\product.txt");
            var line = sr.ReadLine();
            while (line != null)
            {
                var tempSplit = line.Split(separator: ",");
                var tempName = string.Empty;
                double tempPrice = 0;
                foreach (var x in tempSplit)
                {
                    if (x.Contains("Name"))
                    {
                        tempName = x.Replace("Name:", string.Empty);
                    }
                    else if (x.Contains("Price"))
                    {
                        tempPrice = double.Parse(x.Replace("Price:", string.Empty));
                    }
                }
                var tempProduct = new Product(tempName, ChangePrices(tempPrice));
                tempList.Add(tempProduct);
                line = sr.ReadLine();
            }
            sr.Close();
        }
        SaveProducts(tempList);
        return tempList;
    }

    private static double[] ChangePrices(double price)
    {
        //Priser är i enligheten med Googles funktion av valuta förändring.
        //Utfördes senaste den 26 sep. 2022 08:28 UTC (2022/09/26 @ 08:28 UTC)
        var tempDoubles = new double[4];
        tempDoubles[0] = price; //SEK
        tempDoubles[1] = Math.Round(price * 0.088, 2); //USD
        tempDoubles[2] = Math.Round(price * 0.091, 2); ; //EUR
        tempDoubles[3] = Math.Round(price * 0.68, 2); ; //DKK
        return tempDoubles;
    }
}