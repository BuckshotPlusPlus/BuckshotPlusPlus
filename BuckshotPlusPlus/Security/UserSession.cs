using System;
using System.Collections.Generic;
using BuckshotPlusPlus.Analytics;

namespace BuckshotPlusPlus.Security
{
    public class UserSession
    {
        public string SessionId { get; set; }
        public string SessionLang { get; set; }
        public string SessionPlatform { get; set; }
        public string SessionIp { get; set; }
        public long SessionStarted { get; set; }
        public List<AnalyticTimedEvent> UrlHistory { get; set; }
        public DateTime LastUserInteraction { get; set; }

        public UserSession(Dictionary<string, string> requestHeaders)
        {
            SessionId = Keys.CreateRandomUniqueKey();
            UrlHistory = new List<AnalyticTimedEvent>();
            if (requestHeaders.ContainsKey("platform"))
            {
                SessionPlatform = requestHeaders["platform"];
            }
            else
            {
                SessionPlatform = "unknown";
            }

            if (requestHeaders.ContainsKey("lang"))
            {
                SessionLang = requestHeaders["lang"];
            }
            else
            {
                SessionLang = "unknown";
            }

            if (requestHeaders.ContainsKey("ip"))
            {
                SessionIp = requestHeaders["ip"];
            }
            else
            {
                SessionIp = "unknown";
            }

            SessionStarted = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            LastUserInteraction = DateTime.Now;
        }

        public string GetUserSessionLineData()
        {
            return "data session{\n" +
                   "ip = \"" + SessionIp + "\"\n" +
                   "id = \"" + SessionId + "\"\n" +
                   "lang = \"" + SessionLang + "\"\n" +
                   "platform = \"" + SessionPlatform + "\"\n" +
                   "start = \"" + SessionStarted.ToString() + "\"\n" +
                   "url_visited_num = \"" + UrlHistory.Count + "\"\n" +
                   "}\n";
        }

        public Token GetToken(Tokenizer myTokenizer)
        {
            return new Token("", GetUserSessionLineData(), 0, myTokenizer);
        }

        public void AddUrl(string url)
        {
            UrlHistory.Add(new AnalyticTimedEvent(url));
            LastUserInteraction = DateTime.Now;
        }
    }
}
