using System;
using Microsoft.Extensions.DependencyInjection;
using Travelport.AE.DataAccess.Repositories;
using Travelport.AE.Domain.Interfaces.Repositories;
using Travelport.AE.DataAccess.Context;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace ControlTime
{
	class Program
	{
		private static IServiceProvider _serviceProvider;

		static void Main(string[] args)
		{
			var configuration = GetConfiguration();
			//string connectionString = configuration["Data:DefaultConnection"];

			RegisterServices(configuration);
			var service = _serviceProvider.GetService<IRuleRepository>();

			var rules = service.GetAll("xavier.vallesvicedo");
			DisposeServices();
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

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//    if (!optionsBuilder.IsConfigured)
		//    {
		//        IConfigurationRoot configuration = new ConfigurationBuilder()
		//           .SetBasePath(Directory.GetCurrentDirectory())
		//           .AddJsonFile("appsettings.json")
		//           .Build();
		//        var connectionString = configuration.GetConnectionString("DbCoreConnectionString");
		//        optionsBuilder.UseSqlServer(connectionString);
		//    }
		//}

		private static IConfiguration GetConfiguration()
		{
			var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
			return builder.Build();
		}
	}
}
