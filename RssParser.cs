using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace RSS2Pushover
{
    public class FeedParser
    {
        public List<Item> ReadFeed(string url)
        {
            try
            {
                XDocument doc = XDocument.Load(url);
                var entries = from item in doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().Where(i => i.Name.LocalName == "item").Take(10)
                              select new Item
                              {
                                  Content = item.Elements().First(i => i.Name.LocalName == "description").Value,
                                  Url = item.Elements().First(i => i.Name.LocalName == "link").Value,
                                  Title = item.Elements().First(i => i.Name.LocalName == "title").Value,
                                  Guid = item.Elements().First(i => i.Name.LocalName == "guid").Value

                              };
                return entries.ToList();
            }
            catch
            {
                return new List<Item>();
            }
        }
    }

    public class Item
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public string Guid { get; set; }

        public Item()
        {
            Title = "";
            Url = "";
            Content = "";
        }
    }

}
