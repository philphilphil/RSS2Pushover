using FluentScheduler;
using Prowlin;
using Prowlin.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RSS2Prowl
{
    class Service : IJob
    {
        public void Execute()
        {
            Console.WriteLine("Checking Feed..");
            string feed = "https://boardgamegeek.com/rss/subscriptions/useriasdf";
            string apiKey = "a";
            string applicationName = "BGG";
            string archiveFileName = "archive.txt";

            FeedParser parser = new FeedParser();
            var items = parser.ReadFeed(feed);

            foreach (var item in items)
            {

                //Check if notification for this item was sent already
                if (File.ReadAllText(archiveFileName).Contains(item.Guid))
                {
                    continue;
                }

                //sent notification
                INotification notification = new Prowlin.Notification()
                {
                    Application = applicationName,
                    Description = "",//item.Content,
                    Event = item.Title,
                    Priority = NotificationPriority.Normal,
                    Url = item.Url
                };
                notification.AddApiKey(apiKey);

                ProwlClient prowlClient = new ProwlClient();
                NotificationResult notificationResult = prowlClient.SendNotification(notification);

                //Add to archive file
                File.AppendAllText(archiveFileName, item.Guid + Environment.NewLine);
            }
        }
    }
}
