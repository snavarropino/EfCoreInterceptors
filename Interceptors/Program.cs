//https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors

using Microsoft.EntityFrameworkCore;
using Models;

Console.WriteLine("Let's go!!");

const string connectionString = "Server=localhost;Database=PlayGroundIn;User id=sa;password=yourStrong(!)Password;TrustServerCertificate=true";
var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionsBuilder.UseSqlServer(connectionString);

var channelVisibilityProvider = new ChannelVisibilityProvider { VisibleChannels = new List<int> { 1 } };
using (var context = new ApplicationDbContext(optionsBuilder.Options,channelVisibilityProvider))
{
    context.Database.EnsureCreated();

    if (!context.Stores.Any())
    {
        PopulateChannels(context);
        PopulateStores(context);
        PopulateProducts(context);
        PopulateSales(context);
    }

    Console.WriteLine("There are {0} stores", context.Stores.Count());
    Console.WriteLine("There are {0} stores in channel 1", context.Stores.Count(s => s.ChannelId == 1));
    Console.WriteLine("There are {0} stores in channel 2\n", context.Stores.Count(s => s.ChannelId == 2));

    Console.WriteLine("There are {0} stores", context.Stores.TagWith("Filter channel").Count());
    Console.WriteLine("There are {0} stores in channel 1", context.Stores.TagWith("Filter channel").Count(s => s.ChannelId == 1));
    Console.WriteLine("There are {0} stores in channel 2\n", context.Stores.TagWith("Filter channel").Count(s => s.ChannelId == 2));
}

return;

void PopulateChannels(ApplicationDbContext applicationDbContext)
{
    //Populate 2 channels
    var channel1 = new Channel { Name = "Channel 1" };
    var channel2 = new Channel { Name = "Channel 2" };
    applicationDbContext.Channels.Add(channel1);
    applicationDbContext.Channels.Add(channel2);
    applicationDbContext.SaveChanges();
}

void PopulateStores(ApplicationDbContext applicationDbContext)
{
    var random = new Random();
    for (int i = 1; i <= 10; i++)
    {
        var store = new Store { Name = $"Store {i}", ChannelId = random.Next(1, 3)};
        applicationDbContext.Stores.Add(store);
    }

    applicationDbContext.SaveChanges();
}

void PopulateProducts(ApplicationDbContext context1)
{
    for (int i = 1; i <= 100; i++)
    {
        var product = new Product { Name = $"Product {i}", Price = GenerateRandomPrice() };
        context1.Products.Add(product);
    }
    context1.SaveChanges();

    static decimal GenerateRandomPrice()
    {
        var random = new Random();
        return (decimal) Math.Round(random.NextDouble() * 100, 2); // Generate a random price between 0 and 100
    }
}

void PopulateSales(ApplicationDbContext applicationDbContext1)
{
    {
        var random = new Random();
        for (int i = 1; i <= 1000; i++)
        {
            var sale = new Sale
            {
                StoreId = random.Next(1, 11), // Random store ID between 1 and 10
                ProductId = random.Next(1, 101), // Random product ID between 1 and 100
                Quantity = random.Next(1, 11), // Random quantity between 1 and 10
                Date = GenerateRandomDate() // Random date within the last year
            };
            applicationDbContext1.Sales.Add(sale);
        }
        applicationDbContext1.SaveChanges();
    }

    static DateTime GenerateRandomDate()
    {
        var random = new Random();
        return DateTime.Now.AddYears(-1).AddDays(random.Next(365));
    }
}