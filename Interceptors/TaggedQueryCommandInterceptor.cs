using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Interceptors;

public class TaggedQueryCommandInterceptor : DbCommandInterceptor
{
    private readonly IChannelVisibilityProvider _channelVisibilityProvider;

    public TaggedQueryCommandInterceptor(IChannelVisibilityProvider channelVisibilityProvider)
    {
        _channelVisibilityProvider = channelVisibilityProvider;
    }
    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        ManipulateCommand(command);

        return result;
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        ManipulateCommand(command);

        return new ValueTask<InterceptionResult<DbDataReader>>(result);
    }

    private void ManipulateCommand(DbCommand command)
    {
        if (command.CommandText.StartsWith("-- Filter channel"))
        {
            var visibleChannels = string.Join(",", _channelVisibilityProvider.VisibleChannels);

            command.CommandText = @$"
WITH Channel_CTE (ChannelId)
AS
(
    SELECT Id
    FROM Channels
    WHERE Id IN ({visibleChannels})  
)
" + command.CommandText + @"
INNER JOIN Channel_CTE ON S.ChannelId = Channel_CTE.ChannelId" ;
        }
    }
}