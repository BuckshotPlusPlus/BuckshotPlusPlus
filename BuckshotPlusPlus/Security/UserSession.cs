using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus
{
    public class UserSession
    {
        public string SessionID { get; set; }
        public string SessionIP { get; set; }
        public List<AnalyticTimedEvent> UrlHistory { get; set; }
        public DateTime LastUserInteraction { get; set; }

        public UserSession(string session_ip) {
            SessionID = Keys.CreateRandomUniqueKey();
            UrlHistory = new List<AnalyticTimedEvent>();
            SessionIP = session_ip;
            LastUserInteraction = DateTime.Now;
        }

        public string GetUserSessionLineData()
        {
            return "data session{\n" +
                "ip = \"" + SessionIP + "\"\n" +
                "id = \"" + SessionID + "\"\n" +
                "url_visited_num = \"" + UrlHistory.Count.ToString() + "\"\n" +
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
