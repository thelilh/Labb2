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
            return $"{Name} kostar {Price} {Currency}";
        }
        public void SaveProduct(Product product)
        {
            var sw = new StreamWriter(path: $"{Directory.GetCurrentDirectory()}\\product.txt", append: true, Encoding.Default);
            sw.Write($"\nName:{product.Name},Price:{product.Price}");
            sw.Close();
        }
        public List<Product> ReadProducts()
        {
            var tempList = new List<Product>();
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
            return tempList;
        }


        public List<Product> PriceCurrency(List<Product> list, Currencies currency)
        {
            foreach (var x in list)
            {
                //Priser är rätt den 22 september 2022 10:00 UTC
                if (x.Currency == Currencies.SEK)
                {
                    if (currency is Currencies.DKK)
                    {
                        x.Price *= 0.68;
                    }
                    else if (currency is Currencies.EUR)
                    {
                        x.Price *= 0.092;
                    }
                    else if (currency is Currencies.USD)
                    {
                        x.Price *= 0.091;
                    }
                }
                else if (x.Currency == Currencies.DKK)
                {
                    if (currency is Currencies.SEK)
                    {
                        x.Price *= 1.46;
                    }
                    else if (currency is Currencies.EUR or Currencies.USD)
                    {
                        x.Price *= 0.13;
                    }
                }
                else if (x.Currency == Currencies.EUR)
                {
                    if (currency is Currencies.SEK)
                    {
                        x.Price *= 10.86;
                    }
                    else if (currency is Currencies.DKK)
                    {
                        x.Price *= 7.44;
                    }
                    else if (currency is Currencies.USD)
                    {
                        x.Price *= 0.99;
                    }
                }
                else if (x.Currency == Currencies.USD)
                {
                    if (currency is Currencies.SEK)
                    {
                        x.Price *= 11.02;
                    }
                    else if (currency is Currencies.DKK)
                    {
                        x.Price *= 7.54;
                    }
                    else if (currency is Currencies.EUR)
                    {
                        x.Price *= 1.01;
                    }
                }
                x.Currency = currency;
            }
            return list;
        }
    }
}
