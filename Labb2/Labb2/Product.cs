namespace Labb2
{
    public class Product
    {
        public int Price { get; set; }
        public string Name { get; set; }

        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }
        public List<Product> ReadProducts()
        {
            var tempList = new List<Product>();
            var sr = new StreamReader(path: $"{Directory.GetCurrentDirectory()}\\product.txt");
            var line = sr.ReadLine();
            while (line != null)
            {
                var tempSplit = line.Split(separator: ",");
                var tempName = string.Empty;
                var tempPrice = 0;
                foreach (var x in tempSplit)
                {
                    if (x.Contains("Name"))
                    {
                        tempName = x.Replace("Name:", string.Empty).Replace(" ", string.Empty);
                    }
                    else if (x.Contains("Price"))
                    {
                        tempPrice = int.Parse(x.Replace("Price:", string.Empty).Replace(" ", string.Empty));
                    }
                }
                var tempProduct = new Product(name: tempName, price: tempPrice);
                tempList.Add(tempProduct);
                line = sr.ReadLine();
            }
            sr.Close();
            return tempList;
        }
    }
}
