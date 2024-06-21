namespace Serve;

public sealed class CreateOrderHandler : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public Task<CreateOrderResult> ExecuteAsync(CreateOrderCommand cmd, CancellationToken _)
    {
        return Task.FromResult(new CreateOrderResult()
        {
            Message = $"Order {cmd.OrderId} created for {cmd.CustomerName}"
        });
    }
}