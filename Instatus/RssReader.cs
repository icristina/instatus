using Instatus.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Instatus
{
    public class RssReader : IReader
    {
        public bool CanRead(string uri)
        {
            return uri.Contains("rss");
        }

        public async Task<IList> GetListAsync(string uri)
        {
            using(var httpClient = new HttpClient()) 
            {
                var rssResponse = await httpClient.GetStringAsync(uri);
                var xmlDocument = XDocument.Parse(rssResponse);
                var tiles = from channel in xmlDocument.Element("rss").Elements("channel")
                        from item in channel.Elements("item")
                        select new TileViewModel()
                        {
                            Uri = item.Element("link").Value,
                            Title = item.Element("title").Value,
                            Description = item.Element("description").Value
                        };

                return tiles.ToList();
            }
        }
    }
}
