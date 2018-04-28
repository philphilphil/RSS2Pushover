using System;
using FluentScheduler;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace RSS2Pushover
{
    public class SchedulerRegistry : Registry
    {
        public static IConfiguration Configuration { get; set; }

        public SchedulerRegistry()
        {

            var builder = new ConfigurationBuilder().SetBasePath(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath)).AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            string fullFilePath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath) + "\\" + Configuration["ArchiveFileName"];

            Schedule(() => new Service(Configuration["RssFeedUrl"], Configuration["UserApiKey"], Configuration["AppApiKey"], fullFilePath)).ToRunNow().AndEvery(int.Parse(Configuration["ScanIntervalInMinutes"])).Minutes();
        }
    }
}
