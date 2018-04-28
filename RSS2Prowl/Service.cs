using FluentScheduler;
using Prowlin;
using Prowlin.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace RSS2Prowl
{
    class Service : IJob
    {
        public string FeedUrl { get; set; }
        public string ApiKey { get; set; }
        public string ApplicationName { get; set; }
        public string ArchiveFilePath { get; set; }

        public Service(string feedUrl, string apiKey, string appName, string archivePath)
        {
            this.FeedUrl = feedUrl;
            this.ApiKey = apiKey;
            this.ApplicationName = appName;
            this.ArchiveFilePath = archivePath;
        }

        public void Execute()
        {
            ConsoleLog("Checking Feed..");

            FeedParser parser = new FeedParser();
            var items = parser.ReadFeed(this.FeedUrl);

            foreach (var item in items)
            {

                //Check if notification for this item was sent already
                if (File.ReadAllText(this.ArchiveFilePath).Contains(item.Guid))
                {
                    continue;
                }

                ConsoleLog("Item found, sending notification..");
                //sent notification
                INotification notification = new Prowlin.Notification()
                {
                    Application = this.ApplicationName,
                    Description = "",//item.Content,
                    Event = item.Title,
                    Priority = NotificationPriority.Normal,
                    Url = item.Url
                };
                notification.AddApiKey(this.ApiKey);

                ProwlClient prowlClient = new ProwlClient();
                NotificationResult notificationResult = prowlClient.SendNotification(notification);

                //Add to archive file
                File.AppendAllText(this.ArchiveFilePath, item.Guid + Environment.NewLine);
            }
        }

        private void ConsoleLog(string txt)
        {
            Console.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " + txt);
        }
    }
}
