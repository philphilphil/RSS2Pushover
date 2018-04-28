using FluentScheduler;
using PushoverNet;
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
        public string UserApiKey { get; set; }
        public string AppApiKey { get; set; }
        public string ArchiveFilePath { get; set; }

        public Service(string feedUrl, string userApiKey, string appApiKey, string archivePath)
        {
            this.FeedUrl = feedUrl;
            this.UserApiKey = userApiKey;
            this.AppApiKey = appApiKey;
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
                SentNotification(item);

                //Add to archive file
                File.AppendAllText(this.ArchiveFilePath, item.Guid + Environment.NewLine);
            }
        }

        private void SentNotification(Item item)
        {
            PushoverClient client = new PushoverClient(this.AppApiKey);
            client.SendAsync(this.UserApiKey, item.Content, item.Title, new Uri(item.Url));
        }

        private void ConsoleLog(string txt)
        {
            Console.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " + txt);
        }
    }
}
