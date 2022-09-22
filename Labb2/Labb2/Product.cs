using System.Text;

namespace Labb2
{
    public class Product : Shop
    {
        public double Price { get; set; }
        public string Name { get; set; } = null!;

        public Product(string name, double price, bool shouldSave)
        {
            Name = name;
            Price = price;
            Currency = Currencies.SEK;
            if (shouldSave)
            {
                SaveProduct(this);
            }
        }

        public Product()
        {

        }

        public override string ToString()
        {
            return $"{Name} för {Math.Round(Price, 2)} {Currency}";
        }
        public void SaveProduct(Product product)
        {
            var sw = new StreamWriter(path: $"{Directory.GetCurrentDirectory()}\\product.txt", append: true, Encoding.Default);
            sw.Write($"Name:{product.Name},Price:{product.Price}\n");
            sw.Close();
        }
        public List<Product> ReadProducts()
        {
            var tempList = new List<Product>();
            if (!File.Exists($"{Directory.GetCurrentDirectory()}\\product.txt"))
            {
                tempList.Add(new Product(name: "Kaffe", price: 50, shouldSave: true));
                tempList.Add(new Product(name: "Dricka", price: 23, shouldSave: true));
                tempList.Add(new Product(name: "Äpple", price: 5, shouldSave: true));
            }
            else
            {
                var sr = new StreamReader(path: $"{Directory.GetCurrentDirectory()}\\product.txt");
                var line = sr.ReadLine();
                while (line != null)
                {
                    var tempSplit = line.Split(separator: ",");
                    var tempProduct = new Product();
                    foreach (var x in tempSplit)
                    {
                        if (x.Contains("Name"))
                        {
                            tempProduct.Name = x.Replace("Name:", string.Empty);
                        }
                        else if (x.Contains("Price"))
                        {
                            tempProduct.Price = int.Parse(x.Replace("Price:", string.Empty));
                        }
                    }
                    tempList.Add(tempProduct);
                    line = sr.ReadLine();
                }
                sr.Close();
            }

            return tempList;
        }


        public List<Product> PriceCurrency(List<Product> list, Currencies currency)
        {
            foreach (var x in list)
            {
                switch (x.Currency)
                {
                    //Priser är rätt för den 22 september 2022 10:00 UTC
                    case Currencies.SEK when currency is Currencies.DKK:
                        x.Price *= 0.68;
                        break;
                    case Currencies.SEK when currency is Currencies.EUR:
                        x.Price *= 0.092;
                        break;
                    case Currencies.SEK when currency is Currencies.USD:
                        x.Price *= 0.091;
                        break;
                    case Currencies.DKK when currency is Currencies.SEK:
                        x.Price *= 1.46;
                        break;
                    case Currencies.DKK when currency is Currencies.EUR or Currencies.USD:
                        x.Price *= 0.13;
                        break;
                    case Currencies.EUR when currency is Currencies.SEK:
                        x.Price *= 10.86;
                        break;
                    case Currencies.EUR when currency is Currencies.DKK:
                        x.Price *= 7.44;
                        break;
                    case Currencies.EUR when currency is Currencies.USD:
                        x.Price *= 0.99;
                        break;
                    case Currencies.USD when currency is Currencies.SEK:
                        x.Price *= 11.02;
                        break;
                    case Currencies.USD when currency is Currencies.DKK:
                        x.Price *= 7.54;
                        break;
                    case Currencies.USD when currency is Currencies.EUR:
                        x.Price *= 1.01;
                        break;
                }
                x.Currency = currency;
            }
            return list;
        }
    }
}
