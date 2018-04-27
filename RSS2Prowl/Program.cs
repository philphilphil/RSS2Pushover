using FluentScheduler;
using Prowlin;
using Prowlin.Interfaces;
using System;
using System.IO;
using System.Net.Http;
using System.Xml;

namespace RSS2Prowl
{
    class RSS2Prowl
    {
        static void Main(string[] args)
        {
            var registry = new Registry();

            registry.Schedule<Service>().ToRunEvery(30).Seconds();
            JobManager.Initialize(registry);

            Console.WriteLine("Starting up... ");
            Console.WriteLine("Next run at: " + JobManager.GetSchedule("Service").NextRun.ToShortDateString() + " at " + JobManager.GetSchedule("Service").NextRun.ToLongTimeString());

            Console.Read();
        }

    }
}
