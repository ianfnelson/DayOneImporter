using System.Globalization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DayOneImporterCore;
using DayOneImporterCore.Facebook;
using DayOneImporterCore.Twitter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceCollection = new ServiceCollection();
serviceCollection.AddLogging(x => x.AddConsole());

var containerBuilder = new ContainerBuilder();

containerBuilder.RegisterType<FacebookImporter>();
containerBuilder.RegisterType<TwitterImporter>();
containerBuilder.RegisterType<FacebookMapper>().As<IEntryMapper<Post>>();
containerBuilder.RegisterType<TwitterMapper>().As<IEntryMapper<Tweet>>();
containerBuilder.Populate(serviceCollection);
var container = containerBuilder.Build();

var startDate = ArgsParser.GetStartDate(args);

var facebookImporter = container.Resolve<FacebookImporter>();
facebookImporter.Import(startDate);

var twitterImporter = container.Resolve<TwitterImporter>();
twitterImporter.Import(startDate);

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
