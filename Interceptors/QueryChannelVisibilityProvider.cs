public class QueryChannelVisibilityProvider(ApplicationDbContext context): IChannelVisibilityProvider
{
    public IEnumerable<int> VisibleChannels
    {
        get => QueryVisibleChannels();
        set { return; }
    }

    public IEnumerable<int> QueryVisibleChannels()
    {
        return context.VisibleChannels.Select(vc => vc.Id);
    }
}