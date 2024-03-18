
using AutoFixture;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Behaviours;
using System.Reflection;
using Ordering.Application.Mapping;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Infrastructure.Repositories;
using Ordering.Infrastructure.Persistence;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Infrastructure.Mail;


namespace Ordering.Application.IntegrationTests
{
    public class ApplicationTestFixture : IAsyncLifetime
    {
        public const string ApplicationTestFixtureCollection =
            $"Ordering.{nameof(ApplicationTestFixture)}";

        private readonly Fixture _fakeData;

        public ApplicationTestFixture()
        {
            var services = new ServiceCollection();
            var serviceProvider = ConfigureServices(services);
            Services = serviceProvider;

            _fakeData = new Fixture();
            _fakeData.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fakeData.Behaviors.Remove(b));
            _fakeData.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        private static IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // TODO: Use Azure App Config/Key Vault for this?
            var testsConfiguration = new Dictionary<string, string> { };

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .AddInMemoryCollection(testsConfiguration)
                .Build();
         
            services.AddSingleton<IConfiguration>(configuration);

            // services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(typeof(CheckoutOrderCommandValidator).Assembly);
            // services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatR(typeof(CheckoutOrderCommand).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            


            services.AddDbContext<OrderContext>(
                (_, dbOptions) =>
                {
                    dbOptions.UseInMemoryDatabase("OrderBookingTest").EnableSensitiveDataLogging();
                }
            );
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddTransient<IOrderRepository, OrderRepository>();
            //services.AddScoped<IOrderRepository, OrderRepository>();
            //services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IEmailService, EmailService>();

            services.AddLogging(loggingBuilder => loggingBuilder
                .AddConsole());

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

        public IServiceProvider Services { get; }


        public async Task InitializeAsync()
        {
            var context = Services.GetRequiredService<OrderContext>();
            await context.Database.EnsureDeletedAsync().ConfigureAwait(true);
            await context.Database.EnsureCreatedAsync().ConfigureAwait(true);

            // await SetupSpeciesAndBreeds().ConfigureAwait(true);
            // await SetupMasterData().ConfigureAwait(true);
        }

        public async Task DisposeAsync()
        {
            var context = Services.GetRequiredService<OrderContext>();

            // Teardown Database
            await context.Database.EnsureDeletedAsync().ConfigureAwait(true);
            await context.DisposeAsync().ConfigureAwait(true);
        }

      
    }
}
