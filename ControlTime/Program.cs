using System;
using Microsoft.Extensions.DependencyInjection;
using Travelport.AE.DataAccess.Repositories;
using Travelport.AE.Domain.Interfaces.Repositories;
using Travelport.AE.DataAccess.Context;

namespace ControlTime
{
    class Program
    {
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            RegisterServices();
            var service = _serviceProvider.GetService<IRuleRepository>();
            var rules = service.GetAll("xavier.vallesvicedo");
            DisposeServices();
        }
        private static void RegisterServices()
        {
            AEContext aEContext = new AEContext();
            var collection = new ServiceCollection();
            //collection.AddScoped<IContextFactory, ContextFactory>();
            collection.AddScoped<IContextFactory>(s => new ContextFactory(aEContext));
            collection.AddScoped<IRuleRepository, RuleRepository>();
            _serviceProvider = collection.BuildServiceProvider();
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }

    }
}
