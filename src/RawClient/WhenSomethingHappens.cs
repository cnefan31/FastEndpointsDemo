using Contracts;

using FastEndpoints;

using Microsoft.Extensions.Logging;

namespace RawClient;

internal class WhenSomethingHappens : IEventHandler<SomethingHappened>
{
    private readonly ILogger<WhenSomethingHappens> _logger;

    public WhenSomethingHappens(ILogger<WhenSomethingHappens> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(SomethingHappened evnt, CancellationToken ct)
    {
        _logger.LogInformation("{number} - {description}", evnt.Id, evnt.Description);
        return Task.CompletedTask;
    }
}
