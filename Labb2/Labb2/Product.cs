using System.Text;

namespace Labb2
{
    public class Product : Shop
    {
        public double Price { get; set; }
        public string Name { get; set; } = null!;
        private Currencies _productCurrencies = Currencies.SEK;

        public Product(string name, double price, bool shouldSave)
        {
            Name = name;
            Price = price;
            if (shouldSave)
            {
                SaveProduct();
            }
        }

        public Product()
        {

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
        public void SaveProduct()
        {
            var sw = new StreamWriter(path: $"{Directory.GetCurrentDirectory()}\\product.txt", append: true, Encoding.Default);
            sw.Write($"\nName:{Name},Price:{Price}");
            sw.Close();
        }

        public List<Product> PriceCurrency(List<Product> list, Currencies currency)
        {
            foreach (var x in list)
            {
                x.Price = CurrencyConversion(x.Price, x._productCurrencies, currency);
                x._productCurrencies = currency;
            }
            return list;
        }

        public double CurrencyConversion(double price, Currencies currencyProduct, Currencies currencyConvert)
        {
            //Priser är rätt den 22 september 2022 10:00 UTC
            if (currencyProduct == Currencies.SEK)
            {
                if (currencyConvert is Currencies.DKK)
                {
                    price *= 0.68;
                }
                else if (currencyConvert is Currencies.EUR)
                {
                    price *= 0.092;
                }
                else if (currencyConvert is Currencies.USD)
                {
                    price *= 0.091;
                }
            }
            else if (currencyProduct == Currencies.DKK)
            {
                if (currencyConvert is Currencies.SEK)
                {
                    price *= 1.46;
                }
                else if (currencyConvert is Currencies.EUR or Currencies.USD)
                {
                    price *= 0.13;
                }
            }
            else if (currencyProduct == Currencies.EUR)
            {
                if (currencyConvert is Currencies.SEK)
                {
                    price *= 10.86;
                }
                else if (currencyConvert is Currencies.DKK)
                {
                    price *= 7.44;
                }
                else if (currencyConvert is Currencies.USD)
                {
                    price *= 0.99;
                }
            }
            else if (currencyProduct == Currencies.USD)
            {
                if (currencyConvert is Currencies.SEK)
                {
                    price *= 11.02;
                }
                else if (currencyConvert is Currencies.DKK)
                {
                    price *= 7.54;
                }
                else if (currencyConvert is Currencies.EUR)
                {
                    price *= 1.01;
                }
            }
            return price;
        }

        public override string ToString()
        {
            return $"{Name} kostar {Price} {_productCurrencies}";
        }
    }
}
