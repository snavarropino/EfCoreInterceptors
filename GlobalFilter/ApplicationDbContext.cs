using Microsoft.EntityFrameworkCore;
using Models;

public class ApplicationDbContext : DbContext
{
    private readonly IChannelVisibilityProvider _channelVisibilityProvider;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                IChannelVisibilityProvider channelVisibilityProvider=null) : base(options)
    {
        _channelVisibilityProvider = channelVisibilityProvider;
    }

    public DbSet<Store> Stores { get; set; }
    public DbSet<Channel> Channels { get; set; }
    public DbSet<VisibleChannel> VisibleChannels { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Sale> Sales { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (_channelVisibilityProvider is null)
        {
            return;
        }

        modelBuilder.Entity<Store>()
            .HasQueryFilter(s => _channelVisibilityProvider.VisibleChannels.Contains(s.Channel.Id));

        modelBuilder.Entity<Channel>()
            .HasQueryFilter(c => _channelVisibilityProvider.VisibleChannels.Contains(c.Id));
    }
}