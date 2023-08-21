using System;
using System.Collections.Generic;
using BuckshotPlusPlus.Analytics;

namespace BuckshotPlusPlus.Security
{
    public class UserSession
    {
        public string SessionID { get; set; }
        public string SessionLang { get; set; }
        public string SessionPlatform { get; set; }
        public string SessionIP { get; set; }
        public List<AnalyticTimedEvent> UrlHistory { get; set; }
        public DateTime LastUserInteraction { get; set; }

        public UserSession(Dictionary<string, string> RequestHeaders)
        {
            SessionID = Keys.CreateRandomUniqueKey();
            UrlHistory = new List<AnalyticTimedEvent>();
            if (RequestHeaders.ContainsKey("platform"))
            {
                SessionPlatform = RequestHeaders["platform"];
            }
            else
            {
                SessionPlatform = "unknown";
            }

            if (RequestHeaders.ContainsKey("lang"))
            {
                SessionLang = RequestHeaders["lang"];
            }
            else
            {
                SessionLang = "unknown";
            }

            if (RequestHeaders.ContainsKey("ip"))
            {
                SessionIP = RequestHeaders["ip"];
            }
            else
            {
                SessionIP = "unknown";
            }
            LastUserInteraction = DateTime.Now;
        }

        public string GetUserSessionLineData()
        {
            return "data session{\n" +
                   "ip = \"" + SessionIP + "\"\n" +
                   "id = \"" + SessionID + "\"\n" +
                   "lang = \"" + SessionLang + "\"\n" +
                   "platform = \"" + SessionPlatform + "\"\n" +
                   "url_visited_num = \"" + UrlHistory.Count + "\"\n" +
                   "}\n";
        }

        public Token GetToken(Tokenizer MyTokenizer)
        {
            return new Token("", GetUserSessionLineData(), 0, MyTokenizer);
        }

        public void AddUrl(string url)
        {
            UrlHistory.Add(new AnalyticTimedEvent(url));
            LastUserInteraction = DateTime.Now;
        }
    }
}
