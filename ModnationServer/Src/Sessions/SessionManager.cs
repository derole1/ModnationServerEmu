using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

using ModnationServer.Src.DataTypes;
using ModnationServer.Src.Log;

namespace ModnationServer.Src.Sessions
{
    class SessionManager
    {
        const uint SESSION_TIMEOUT = 30000;
        const uint SESSION_CHECK_INTERVAL = 10000;

        public class Session
        {
            public int SessionId;
            public NPTicket NPTicket { get; set; }
            public IPAddress IPAddress { get; set; }
            public int PlayerId { get; set; }
            public int Username { get; set; }
            public string ConsoleId { get; set; }
            public string LanguageCode { get; set; }
            public string Platform { get; set; }
            public string RegionCode { get; set; }
            public string Presence { get; set; }
            public DateTime LastRequest { get; set; }
        }

        public static List<Session> Sessions { get; private set; } = new List<Session>();

        private static Timer SessionChecker = new Timer(SessionCheck, new AutoResetEvent(false), 0, SESSION_CHECK_INTERVAL);

        public static bool CreateSession(Session session)
        {
            if (Sessions.Where(x => x.IPAddress == session.IPAddress).Count() > 0)
                return false;

            Sessions.Add(session);
            return true;
        }

        public static Session GetSessionBySessionId(int sessionId) => Sessions.Where(x => x.SessionId == sessionId).FirstOrDefault();
        public static Session GetSessionByPlayerId(int playerId) => Sessions.Where(x => x.PlayerId == playerId).FirstOrDefault();
        public static int GetSessionCount() => Sessions.Count;

        static void SessionCheck(object stateInfo)
        {
            try
            {
                foreach (var session in Sessions.ToList())
                {
                    if (session.LastRequest.AddMilliseconds(SESSION_TIMEOUT) < DateTime.UtcNow)
                    {
                        Logging.Log(typeof(SessionManager), "Session {0} has not been updated for 30 seconds. Closing...", LogType.Warning, session.SessionId);
                        Sessions.Remove(session);
                    }
                }
            }
            catch (Exception e)
            {
                Logging.Log(typeof(SessionManager), e.ToString(), LogType.Error);
            }
        }
    }
}
