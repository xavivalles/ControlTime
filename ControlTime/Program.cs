using System;
using Microsoft.Extensions.DependencyInjection;
using Travelport.AE.DataAccess.Repositories;
using Travelport.AE.Domain.Interfaces.Repositories;
using Travelport.AE.DataAccess.Context;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Travelport.AE.Domain.Interfaces;
using Travelport.AE.Domain.Implementation;

namespace ControlTime
{
	class Program
	{
		private static IServiceProvider _serviceProvider;

		static void Main(string[] args)
		{
			var configuration = GetConfiguration();
			RegisterServices(configuration);
			var eventService = _serviceProvider.GetService<IEventService>();
            var events = eventService.GetAll();

            if (args == null || args.Length == 0)
            {
                args = RetrieveArgsFromConsole();
            }

            //DisposeServices();
		}

		private static void RegisterServices(IConfiguration configuration)
		{
			var collection = new ServiceCollection();
			collection.AddEntityFrameworkSqlServer()
					.AddDbContext<AEContext>(contextBuilder =>
					{
						contextBuilder.UseSqlServer(configuration["Data:DefaultConnection"]);
					})
					.AddSingleton(c => configuration);
			//collection.AddScoped<IContextFactory, ContextFactory>();
			collection.AddScoped<IContextFactory, ContextFactory>();
			collection.AddScoped<IRuleRepository, RuleRepository>();
			collection.AddScoped<IEventRepository, EventRepository>();
			collection.AddScoped<IEventService, EventService>();
            
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

		private static IConfiguration GetConfiguration()
		{
			var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
			return builder.Build();
		}

        private static string[] RetrieveArgsFromConsole()
        {
            string[] args;
            Console.WriteLine("Host:");
            var host = Console.ReadLine();
            Console.WriteLine("PCC:");
            var pcc = Console.ReadLine();
            Console.WriteLine("PNR:");
            var pnr = Console.ReadLine();
            Console.WriteLine("Provider:" + Environment.NewLine +
                "1. Fare optimization." + Environment.NewLine +
                "2. File Finishing." + Environment.NewLine +
                "3. Quality Control." + Environment.NewLine +
                "4. Queue Management." + Environment.NewLine +
                "5. Schedule Changes." + Environment.NewLine +
                "6. Ticketing");
            var providerNumber = Console.ReadLine();
            string provider = null;

            switch (providerNumber)
            {
                case "1": provider = "FareOptimization"; break;
                case "2": provider = "FileFinishing"; break;
                case "3": provider = "QualityControl"; break;
                case "4": provider = "QueueManagement"; break;
                case "5": provider = "ScheduleChanges"; break;
                case "6": provider = "ticketing"; break;
                default: provider = String.Empty; break;
            }
            string arguments = $"-h {host} -p {pcc} -n {pnr} -r {provider}";
            args = arguments.Split(new char[] { ' ' });
            return args;
        }
    }
}
