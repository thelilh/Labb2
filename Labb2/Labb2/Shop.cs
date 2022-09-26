namespace Labb2;

public class Shop
{

    [Flags]
    public enum Currencies
    {
        SEK = 0,
        USD = 1,
        EUR = 2,
        DKK = 3
    }
    public Currencies Currency { get; set; }
}