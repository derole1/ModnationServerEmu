using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using HttpLib;

using ModnationServer.Src.Protocols;
using ModnationServer.Src.Helpers;
using ModnationServer.Src.Log;

namespace ModnationServer.Src.Services.Handlers
{
    class ContentUrl
    {
        public static void ContentUrlListHandler(TcpClient client, HttpRequest req, HttpResponse res)
        {
            var doc = new XML(Encoding.UTF8);
            var root = doc.CreateElement("response");
            var urls = doc.CreateElement(root, "content_urls", null, new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("total", "4"),
            });
            {
                doc.CreateElement(urls, "content_url", "http://127.0.0.1:10050/resources/player_avatars", new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>("name", "player_avatars"),
                    new KeyValuePair<string, string>("formats", ".png"),
                });
                doc.CreateElement(urls, "content_url", "http://127.0.0.1:10050/resources/player_creations", new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>("name", "player_creations"),
                    new KeyValuePair<string, string>("formats", "data.bin, preview_image.png"),
                });
                doc.CreateElement(urls, "content_url", "http://127.0.0.1:10050/resources/content_updates", new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>("name", "content_updates"),
                    new KeyValuePair<string, string>("formats", "data.bin"),
                });
                doc.CreateElement(urls, "content_url", "http://127.0.0.1:10050/resources/ghost_car_data", new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>("name", "ghost_car_data"),
                    new KeyValuePair<string, string>("formats", "data.bin"),
                });
            }
            doc.SetResult(0);
            res.Data = doc.Serialize();
        }
    }
}
