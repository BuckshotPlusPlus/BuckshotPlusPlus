using System;
using System.Collections.Generic;
using System.Net;

namespace BuckshotPlusPlus.Security
{
    public class UserSessionManager
    {
        public Dictionary<string, UserSession> ActiveUsers { get; set; }

        public UserSessionManager()
        {
            ActiveUsers = new Dictionary<string, UserSession>();
        }

        public UserSession AddOrUpdateUserSession(HttpListenerRequest req, HttpListenerResponse response)
        {
            bool SessionCookieFound = false;
            string UserSessionId = null;

            Dictionary<string, string> RequestHeaders = new Dictionary<string, string>();
            RequestHeaders.Add("ip", req.RemoteEndPoint.ToString());

            System.Collections.Specialized.NameValueCollection headers = req.Headers;
            // Get each header and display each value.
            foreach (string key in headers.AllKeys)
            {
                string[] values = headers.GetValues(key);
                if (values.Length > 0)
                {
                    if(key == "sec-ch-ua-platform")
                    {
                        RequestHeaders.Add("platform", values[0]);
                    }else if(key == "Accept-Language")
                    {
                        RequestHeaders.Add("lang", values[0]);
                    }
                }
            }

            foreach (Cookie cook in req.Cookies)
            {
                if (cook.Name == "bpp_session_id")
                {
                    SessionCookieFound = true;
                    UserSessionId = cook.Value;
                }

                /*Console.WriteLine("Cookie:");
                Console.WriteLine("{0} = {1}", cook.Name, cook.Value);
                Console.WriteLine("Domain: {0}", cook.Domain);
                Console.WriteLine("Path: {0}", cook.Path);
                Console.WriteLine("Port: {0}", cook.Port);
                Console.WriteLine("Secure: {0}", cook.Secure);

                Console.WriteLine("When issued: {0}", cook.TimeStamp);
                Console.WriteLine("Expires: {0} (expired? {1})",
                    cook.Expires, cook.Expired);
                Console.WriteLine("Don't save: {0}", cook.Discard);
                Console.WriteLine("Comment: {0}", cook.Comment);
                Console.WriteLine("Uri for comments: {0}", cook.CommentUri);
                Console.WriteLine("Version: RFC {0}", cook.Version == 1 ? "2109" : "2965");

                // Show the string representation of the cookie.
                Console.WriteLine("String: {0}", cook.ToString());*/
            }

            if (SessionCookieFound)
            {
                UserSession Session;
                if (ActiveUsers.TryGetValue(UserSessionId, out Session))
                {
                    return Session;
                }

                return CreateNewUserSession(RequestHeaders, response);
            }

            return CreateNewUserSession(RequestHeaders, response);
        }

        public UserSession CreateNewUserSession(Dictionary<string, string> RequestHeaders, HttpListenerResponse response)
        {
            UserSession NewUserSession = new UserSession(RequestHeaders);
            ActiveUsers.Add(NewUserSession.SessionID, NewUserSession);

            Cookie SessionIdCookie = new Cookie("bpp_session_id", NewUserSession.SessionID);
            response.SetCookie(SessionIdCookie);

            return NewUserSession;
        }

        public void RemoveInactiveUserSessions()
        {
            DateTime Now = DateTime.Now;
            foreach (KeyValuePair<string, UserSession> User in ActiveUsers)
            {
                if ((Now - User.Value.LastUserInteraction).TotalSeconds > 10)
                {
                    ActiveUsers.Remove(User.Key);
                }
            }
        }
    }
}
