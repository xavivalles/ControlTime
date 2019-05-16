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
using System.Collections.Generic;
using Travelport.AE.Domain.Entities;
using System.Linq;
using ControlTime.Models;
using Travelport.AE.EventEmulator.Domain.Interfaces;
using Travelport.AE.EventEmulator.Domain;

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
            var eventAndMinutes = new EventAndTime();
            if (args == null || args.Length == 0)
            {
                eventAndMinutes = RetrieveArgsFromConsole(events);
            }

            var ruleRepository = _serviceProvider.GetService<IRuleRepository>();
            var getRulesForEvent = ruleRepository.GetAllRulesWithEventId(eventAndMinutes.EventId);

            var triggerFieldService = _serviceProvider.GetService<ITriggerFieldService>();
            foreach (var rule in getRulesForEvent)
            {
                var containsIntervalFieldDB = rule.TriggerFields.Where(x => x.Key == "ExecutionIntervalInMinutes");
                if (containsIntervalFieldDB == null)
                {
                    //var a = "creem la vulve";
                    triggerFieldService.AddTriggerField(rule, eventAndMinutes.Time);
                }
                else
                {
                    var b = "s'actualitza le mamelons";
                }
            }

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
            collection.AddScoped<IEventRepository, EventRepository>();
            collection.AddScoped<IEventService, EventService>();
            collection.AddScoped<ITriggerFieldService, TriggerFieldService>();            

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

        private static EventAndTime RetrieveArgsFromConsole(IEnumerable<Event> events)
        {
            var dictionaryEvents = new Dictionary<int, Event>();
            var index = 0;
            Console.WriteLine("What event you want update the interval execution?");
            foreach (var evnt in events)
            {
                Console.WriteLine(String.Format("{0}. {1}", index, evnt.Name));
                dictionaryEvents.Add(index, evnt);
                index++;
            }
            var getEventIndex = Console.ReadLine();

            Console.WriteLine("How much minutes you want for these events?");
            var time = Console.ReadLine();

            var eventId = GetEventId(dictionaryEvents, int.Parse(getEventIndex));
            var EventAndMinutesObject = new EventAndTime { EventId= eventId, Time = int.Parse(time) };
            return EventAndMinutesObject;
        }

        private static Guid GetEventId(Dictionary<int, Event> listEvents, int evntIndex)
        {
            var evnt = listEvents.FirstOrDefault(e => e.Key == evntIndex);
            return evnt.Value.Id;
        }
    }
}
