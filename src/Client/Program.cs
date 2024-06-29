using Contracts;
using FastEndpoints;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
        });
        builder.Services.AddScoped<HttpMessageHandler>(sp => 
        new GrpcWebHandler(new HttpClientHandler()
            {
                AllowAutoRedirect = true,
            })
        {
            GrpcWebMode = GrpcWebMode.GrpcWebText
        });
        var app = builder.Build();

        app.Services.MapRemoteCore("http://localhost:8081", c =>
        {
            c.Register<CreateOrderCommand, CreateOrderResult>();
            c.RegisterServerStream<StatusStreamCommand, StatusUpdate>();
            c.RegisterClientStream<CurrentPosition, ProgressReport>();
            c.Subscribe<SomethingHappened, WhenSomethingHappens>();
        });

        await app.RunAsync();
    }
}
