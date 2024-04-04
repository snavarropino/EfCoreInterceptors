using Interceptors;
using Microsoft.EntityFrameworkCore;
using Models;

public class ApplicationDbContext : DbContext
{
    private readonly TaggedQueryCommandInterceptor _interceptor;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IChannelVisibilityProvider channelVisibilityProvider) : base(options)
    {
        _interceptor = new TaggedQueryCommandInterceptor(channelVisibilityProvider);
    }

    public DbSet<Store> Stores { get; set; }
    public DbSet<Channel> Channels { get; set; }
    public DbSet<VisibleChannel> VisibleChannels { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Sale> Sales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_interceptor);
        base.OnConfiguring(optionsBuilder);
    }
}