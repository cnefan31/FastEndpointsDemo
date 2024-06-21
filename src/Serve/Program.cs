namespace Serve;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddHandlerServer();
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
            });
        });

        var app = builder.Build();

        app.UseGrpcWeb();
        app.UseCors();
        app.MapHandlers(h =>
        {
            h.Register<CreateOrderCommand, CreateOrderHandler, CreateOrderResult>();
        });

        app.Run();
    }
}