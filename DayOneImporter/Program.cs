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

// var facebookImporter = container.Resolve<FacebookImporter>();
// facebookImporter.Import();

var twitterImporter = container.Resolve<TwitterImporter>();
twitterImporter.Import();