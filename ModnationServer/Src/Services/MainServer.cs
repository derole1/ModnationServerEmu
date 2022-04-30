using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Diagnostics;

using ModnationServer.Src.Log;
using ModnationServer.Src.Protocols;
using ModnationServer.Src.Services.Handlers;
using ModnationServer.Src.Database;
using ModnationServer.Src.Helpers;

namespace ModnationServer.Src.Services
{
    class MainServer
    {
        public static string conStr = "";

        public HttpApi Api { get; }
        public SQLiteDB Database { get; }   //TODO: Make this work

        public MainServer(string ip, ushort port, string dbFile, string schemaFile)
        {
            conStr = string.Format("Data Source={0};Version=3;", dbFile);
            //Database.CheckDB(conStr, schemaFile);
            Api = new HttpApi("PlayerConnect", ip, port);
            //Resources
            Api.RegisterMethod("/resources/preferences.update.xml", Resources.GetResourceHandler);

            Api.RegisterMethod("/resources/policy.view.xml", Resources.GetResourceHandler);
            Api.RegisterMethod("/resources/policy.accept.xml", Resources.GetResourceHandler);

            Api.RegisterMethod("/resources/session.login_np.xml", Resources.GetResourceHandler);
            Api.RegisterMethod("/resources/session.ping.xml", Resources.GetResourceHandler);
            Api.RegisterMethod("/resources/session.set_presence.xml", Resources.GetResourceHandler);
            Api.RegisterMethod("/resources/session.logout.xml", Resources.GetResourceHandler);

            Api.RegisterMethod("/resources/content_url.list.xml", Resources.GetResourceHandler);

            Api.RegisterMethod("/resources/server.select.xml", Resources.GetResourceHandler);
            //Methods
            Api.RegisterMethod("/preferences.xml", Preferences.PreferencesHandler);

            Api.RegisterMethod("/policy.view.xml", Policy.PolicyViewHandler);
            Api.RegisterMethod("/policy.accept.xml", Policy.PolicyAcceptHandler);

            Api.RegisterMethod("/session.login_np.xml", Session.SessionLoginHandler);
            Api.RegisterMethod("/session.ping.xml", Session.SessionPingHandler);
            Api.RegisterMethod("/session.set_presence.xml", Session.SessionSetPresenceHandler);
            Api.RegisterMethod("/session.logout.xml", Session.SessionLogoutHandler);

            Api.RegisterMethod("/content_url.list.xml", ContentUrl.ContentUrlListHandler);

            Api.RegisterMethod("/server.select.xml", Server.ServerSelectHandler);
            Logging.Log(typeof(MainServer), "Started on {0}:{1}", LogType.Info, ip, port);
        }
    }
}
