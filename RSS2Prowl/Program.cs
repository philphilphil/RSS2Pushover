using Prowlin;
using Prowlin.Interfaces;
using System;
using System.IO;
using System.Net.Http;
using System.Xml;

namespace RSS2Prowl
{
    class Program
    {
        static void Main(string[] args)
        {
            string feed = "-";
            string applicationName = "BGG";
            string archiveFileName = "archive.txt";

            FeedParser parser = new FeedParser();
            var items = parser.ReadFeed(feed);

            foreach (var item in items)
            {

                //Check if notification for this item was sent already
                if (File.ReadAllText(archiveFileName).Contains(item.Md5Hash))
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
                notification.AddApiKey("-");

                ProwlClient prowlClient = new ProwlClient();
                NotificationResult notificationResult = prowlClient.SendNotification(notification);

                //Add to archive file
                File.AppendAllText(archiveFileName, item.Md5Hash + Environment.NewLine);
            }

            Console.Read();
        }
    }
}
