namespace CSharpBasics
{
    public interface IDiscountable
    {
        decimal ApplyDiscount(decimal percentage);
    }

    class Product
    {
        private decimal _price;

        public string Name { get; set; }

        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value >= 0)
                {
                    _price = value;
                }
            }
        }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public virtual void DisplayProductDetails()
        {
            Console.WriteLine($"Product: {Name}, Price: {Price:C}");
        }

        public static decimal CalculateDiscount(decimal price, decimal discountPercentage)
        {
            return price - (price * discountPercentage / 100);
        }
    }


    class Clothing : Product, IDiscountable
    {
        public int Size { get; set; }

        public Clothing(string name, decimal price, int size) : base(name, price)
        {
            Size = size;
        }

        public string GetSizeName()
        {
            return Size switch
            {
                1 => "SM",
                2 => "MD",
                3 => "LG",
                _ => "Unknown Size"
            };
        }

        public override void DisplayProductDetails()
        {
            base.DisplayProductDetails();
            Console.WriteLine($"Size: {GetSizeName()}");
        }

        public decimal ApplyDiscount(decimal percentage)
        {
            return CalculateDiscount(Price, percentage);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Clothing> catalog = new List<Clothing>();

            catalog.Add(new Clothing("Super Cool T-Shirt", 49.99m, 2));
            catalog.Add(new Clothing("Short Pants", 79.99m, 1));
            catalog.Add(new Clothing("Short Pants", 82.99m, 2));

            for (int i = 0; i < catalog.Count; i++)
            {
                catalog[i].DisplayProductDetails();
            }

            foreach (Clothing item in catalog)
            {
                item.DisplayProductDetails();
            }

            decimal discountedPrice = catalog[0].ApplyDiscount(10);
            Console.WriteLine($"T-Shirt Price after discount: {discountedPrice:C}");
            Console.WriteLine(Product.CalculateDiscount(29.50m, 0.1m));
        }
    }
}

