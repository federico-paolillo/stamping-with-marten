using Marten;
using Microsoft.Extensions.DependencyInjection;
using Stampings;

namespace Stampings.Tests.Helpers;

public abstract class TestBase
{
    private ServiceProvider? serviceProvider;
    private IServiceScope? serviceProviderScope;

    [SetUp]
    public void SetUp()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddMarten(new CustomMartenOptions())
            .ApplyAllDatabaseChangesOnStartup();

        serviceProvider = serviceCollection.BuildServiceProvider();
        serviceProviderScope = serviceProvider.CreateScope();
    }

    [TearDown]
    public void TearDown()
    {
        serviceProviderScope?.Dispose();
        serviceProvider?.Dispose();
    }

    protected IDocumentStore GetStore()
    {
        if (serviceProviderScope is null)
        {
            throw new InvalidOperationException("Setup the test suite properly");
        }

        return serviceProviderScope.ServiceProvider.GetRequiredService<IDocumentStore>();
    }
}