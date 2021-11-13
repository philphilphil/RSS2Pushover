using FluentScheduler;
using System;
using System.IO;
using System.Net.Http;
using System.Xml;

namespace RSS2Pushover
{
    class RSS2Pushover
    {
        static void Main(string[] args)
        {
            JobManager.Initialize(new SchedulerRegistry());
            Console.WriteLine("Starting up... ");
            Console.Read();
        }
    }
}
