using NLog.Extensions.Logging;

namespace Serve;

public class Program
{
    public static async Task Main(string[] args)
    {
        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        var builder = WebApplication.CreateBuilder(args);

        builder.AddHandlerServer();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                              policy =>
                              {
                                  policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                              });
        });
        builder.Services.AddLogging(configure =>
        {
            configure.ClearProviders();
            configure.AddNLog("nlog.config");
        });

        builder.Services.AddHostedService<RemoteWorkerJob>();

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
            app.UseWebAssemblyDebugging();
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseGrpcWeb(new() { DefaultEnabled = true });

        app.UseCors(MyAllowSpecificOrigins);

        //app.MapRemote("http://localhost:8081", c =>
        //{
        //    c.Register<CreateOrderCommand, CreateOrderResult>();
        //});

        app.MapHandlers(h =>
        {
            h.Register<CreateOrderCommand, CreateOrderHandler, CreateOrderResult>();
            h.RegisterServerStream<StatusStreamCommand, StatusUpdateHandler, StatusUpdate>();
            h.RegisterClientStream<CurrentPosition, PositionProgressHandler, ProgressReport>();
            h.RegisterEventHub<SomethingHappened>();
        });
        app.MapFallbackToFile("index.html");

        await app.RunAsync();
    }
}