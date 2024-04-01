using System.Globalization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DayOneImporterCore;
using DayOneImporterCore.Importers.Facebook;
using DayOneImporterCore.Importers.Facebook.Model;
using DayOneImporterCore.Importers.Twitter;
using DayOneImporterCore.Importers.Twitter.Model;
using DayOneImporterCore.Importers.Wordpress;
using DayOneImporterCore.Importers.Wordpress.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceCollection = new ServiceCollection();
serviceCollection.AddLogging(x => x.AddConsole());

var containerBuilder = new ContainerBuilder();

containerBuilder.RegisterType<FacebookImporter>();
containerBuilder.RegisterType<TwitterImporter>();
containerBuilder.RegisterType<WordpressImporter>();
containerBuilder.RegisterType<FacebookMapper>().As<IEntryMapper<Post>>();
containerBuilder.RegisterType<TwitterMapper>().As<IEntryMapper<Tweet>>();
containerBuilder.RegisterType<WordpressMapper>().As<IEntryMapper<Item>>();
containerBuilder.Populate(serviceCollection);
var container = containerBuilder.Build();

var startDate = ArgsParser.GetStartDate(args);

var facebookImporter = container.Resolve<FacebookImporter>();
facebookImporter.Import(startDate);

var twitterImporter = container.Resolve<TwitterImporter>();
twitterImporter.Import(startDate);

var wordPressImporter = container.Resolve<WordpressImporter>();
wordPressImporter.Import(startDate);

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
