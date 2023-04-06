using Microsoft.EntityFrameworkCore; // Include extension method
using Packt.Shared; // Northwind, Category, Product

partial class Program
{
    static void QueryingCategories()
    {
        using (Northwind db = new())
        {
            SectionTitle("Categories and how many products they have:");
            // a query to get all categories and their related products
            IQueryable<Category>? categories = db.Categories?
            .Include(c => c.Products);
            if ((categories is null) || (!categories.Any()))
            {
                Fail("No categories found.");
                return;
            }
            // execute query and enumerate results
            foreach (Category c in categories)
            {
                WriteLine($"{c.CategoryName} has {c.Products.Count} products.");
            }
        }
    }
    static void FilteredIncludes()
    {
        using(Northwind db = new())
        {
            SectionTitle("Products with a minimum number of units in stock.");
            string? input;
            int stock;
            do
            {
                Write("Enter a minimum for units in stock: ");
                input = ReadLine();
            }while(!int.TryParse(input, out stock));

            IQueryable<Category>? categories = db.Categories?
                .Include(c => c.Products.Where(p => p.Stock >= stock));
            
            if ((categories is null) || (!categories.Any())){
                Fail("No categories found");
                return;
            }
            Info($"ToQueryString: {categories.ToQueryString()}");
            foreach (Category c in categories)
            {
                WriteLine($"{c.CategoryName} has {c.Products.Count} products with a minimum of {stock} units in stock");
                foreach(Product p in c.Products)
                {
                    WriteLine($"    {p.ProductName} has {p.Stock} units in stock.");
                }
            }
        }
    }
    static void QueringProducts()
    {
        using (Northwind db = new())
        {
            SectionTitle("Products that cost more than a price, highest at top.");
            string? input;
            decimal price;
            do
            {
                Write("Enter a prodcts price: ");
                input = ReadLine();
            } while (!decimal.TryParse(input, out price));

            IQueryable<Product>? products = db.Products?
                .Where(p => p.Cost > price)
                .OrderByDescending(p => p.Cost);

            if ((products is null) || (!products.Any()))
            {
                Fail("No Products Found!");
                return;
            }
            Info($"ToQueryString: {products.ToQueryString()}");
            foreach (Product p in products)
            {
                WriteLine("{0}: {1} costs {2:$#,##0.00} and has {3} in stock.",
                    p.ProductId, p.ProductName, p.Cost, p.Stock);
            }
        }
    }
    static void QueryingWithLike()
    {
        using (Northwind db = new())
        {
            SectionTitle("Pattern matching with LIKE.");
            Write("Enter parrt of a product name: ");
            string? input = ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Fail("You did not enter part of a product name.");
                return; 
            }
            IQueryable<Product>? products = db.Products?
                .Where(p => EF.Functions.Like(p.ProductName, $"%{input}"));
            if((products is null) || (!products.Any()))
            {
                Fail("No products found.");
                return;
            }
            foreach (Product p in products)
            {
                WriteLine($"{p.ProductName} has {p.Stock} units in stock. Discontinued: {p.Discontinued}");
            }
        }
    }
    static void GetRandomProduct()
    {
        using(Northwind db = new())
        {
            SectionTitle("Get a random prouct");
            int? rowCount = db.Products?.Count();
            if(rowCount == null)
            {
                Fail("Products table is empty");
                return;
            }
            Product? p = db.Products?.FirstOrDefault(
                p => p.ProductId == (int)(EF.Functions.Random() * rowCount));
            if(p == null)
            {
                Fail("Product not found"); 
                return;
            }
            WriteLine($"Random product: {p.ProductId} {p.ProductName}");
            
        }
    }
}