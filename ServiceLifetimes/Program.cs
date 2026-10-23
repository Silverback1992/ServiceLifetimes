using Microsoft.Extensions.DependencyInjection;

IServiceCollection services = new ServiceCollection();

services.AddScoped<SomeEndPoint>();
services.AddTransient<ServiceA>();
services.AddSingleton<App>();

var serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetRequiredService<App>();

for (int i = 1; i < 4; i++)
{
    app.Run(i, serviceProvider);
}

public class App
{
    //private readonly SomeEndPoint _someEndPoint;

    //public App(SomeEndPoint someEndPoint)
    //{
    //    _someEndPoint = someEndPoint;
    //}

    public void Run(int runIdx, ServiceProvider serviceProvider)
    {
        Console.WriteLine($"Request: {runIdx}");
        using var scope = serviceProvider.CreateScope();
        var endpoint = scope.ServiceProvider.GetRequiredService<SomeEndPoint>();
        endpoint.Hanlde();
        //_someEndPoint.Hanlde();
    }
}

public class BaseClass
{
    private readonly IndentTracker _indentTracker = new();

    public override string ToString()
    {
        return base.ToString() + '-' + GetHashCode().ToString()[^4..];
    }

    public void Out(string msg)
    {
        Console.WriteLine($"{_indentTracker.GetIndent()}{msg}");
    }
}

public sealed class IndentTracker
{
    public string GetIndent()
    {
        int count = Environment.StackTrace.Count(c => c == '\n') - 5;
        return new string(' ', count * 2);
    }
}

public class SomeEndPoint : BaseClass
{
    private readonly ServiceA _serviceA;

    public SomeEndPoint(ServiceA serviceA)
    {
        _serviceA = serviceA;
    }

    public void Hanlde()
    {
        Out($"{this} handling request...");
        _serviceA.DoWork();
    }
}

public class ServiceA : BaseClass
{
    public void DoWork()
    {
        Out($"{this} doing work");
    }
}