using System.Globalization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DayOneImporter;
using DayOneImporter.Importers.BookLog;
using DayOneImporter.Importers.BookLog.Model;
using DayOneImporter.Importers.Facebook;
using DayOneImporter.Importers.Facebook.Model;
using DayOneImporter.Importers.Twitter;
using DayOneImporter.Importers.Twitter.Model;
using DayOneImporter.Importers.Wordpress;
using DayOneImporter.Importers.Wordpress.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceCollection = new ServiceCollection();
serviceCollection.AddLogging(x => x.AddConsole());

var containerBuilder = new ContainerBuilder();

containerBuilder.RegisterType<FacebookImporter>();
containerBuilder.RegisterType<FacebookMapper>().As<IEntryMapper<Post>>();

containerBuilder.RegisterType<TwitterImporter>();
containerBuilder.RegisterType<TwitterMapper>().As<IEntryMapper<Tweet>>();

containerBuilder.RegisterType<WordpressImporter>();
containerBuilder.RegisterType<WordpressMapper>().As<IEntryMapper<Item>>();

containerBuilder.RegisterType<BookLogImporter>();
containerBuilder.RegisterType<BookLogMapper>().As<IEntryMapper<Book>>();

containerBuilder.Populate(serviceCollection);
var container = containerBuilder.Build();

var startDate = ArgsParser.GetStartDate(args);

// var facebookImporter = container.Resolve<FacebookImporter>();
// facebookImporter.Import(startDate);
//
// var twitterImporter = container.Resolve<TwitterImporter>();
// twitterImporter.Import(startDate);
//
// var wordPressImporter = container.Resolve<WordpressImporter>();
// wordPressImporter.Import(startDate);

var bookLogImporter = container.Resolve<BookLogImporter>();
bookLogImporter.Import(startDate);

namespace DayOneImporter
{
    public static class ArgsParser
    {
        public static DateTimeOffset GetStartDate(string[] args)
        {
            if (args.Length == 0)
            {
                return DateTimeOffset.MinValue;
            }

            return DateTimeOffset.ParseExact(args[0], "yyyyMMdd", CultureInfo.InvariantCulture);
        } 
    }
}
