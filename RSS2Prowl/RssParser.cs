using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace RSS2Prowl
{
    public class FeedParser
    {
        public List<Item> ReadFeed(string url)
        {
            try
            {
                XDocument doc = XDocument.Load(url);
                var entries = from item in doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().Where(i => i.Name.LocalName == "item")
                              select new Item
                              {
                                  Content = item.Elements().First(i => i.Name.LocalName == "description").Value,
                                  Url = item.Elements().First(i => i.Name.LocalName == "link").Value,
                                  Title = item.Elements().First(i => i.Name.LocalName == "title").Value,
                                  Md5Hash = CreateMD5(item.Elements().First(i => i.Name.LocalName == "title").Value + item.Elements().First(i => i.Name.LocalName == "pubDate").Value)

                              };
                return entries.ToList();
            }
            catch
            {
                return new List<Item>();
            }
        }

        //https://stackoverflow.com/questions/11454004/calculate-a-md5-hash-from-a-string
        private string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }

    public class Item
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }

        public string Md5Hash { get; set; }

        public Item()
        {
            Title = "";
            Url = "";
            Content = "";
        }
    }

}
