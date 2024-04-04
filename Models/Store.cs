namespace Models;

public class Store
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ChannelId { get; set; }
    public ICollection<Sale> Sales { get; set; }
    public Channel Channel { get; set; }
}