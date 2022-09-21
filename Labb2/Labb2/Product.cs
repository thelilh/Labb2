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
    }
}
