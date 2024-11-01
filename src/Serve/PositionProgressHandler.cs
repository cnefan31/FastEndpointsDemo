﻿namespace Serve;

public sealed class PositionProgressHandler : IClientStreamCommandHandler<CurrentPosition, ProgressReport>
{
    readonly ILogger<PositionProgressHandler> logger;

    public PositionProgressHandler(ILogger<PositionProgressHandler> logger)
    {
        this.logger = logger;
    }

    public async Task<ProgressReport> ExecuteAsync(IAsyncEnumerable<CurrentPosition> stream, CancellationToken ct)
    {
        var currentNumber = 0;
        var iterator = stream.GetAsyncEnumerator(ct);

        while (await iterator.MoveNextAsync())
        {
            var position = iterator.Current;
            logger.LogInformation("Current number: {pos}", position.Number);
            currentNumber = position.Number;
        }
        //await foreach (var position in stream)
        //{
        //    logger.LogInformation("Current number: {pos}", position.Number);
        //    currentNumber = position.Number;
        //}
        return new ProgressReport { LastNumber = currentNumber };
    }
}
