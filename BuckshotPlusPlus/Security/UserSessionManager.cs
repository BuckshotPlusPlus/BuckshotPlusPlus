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
            bool sessionCookieFound = false;
            string userSessionId = null;

            Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
            requestHeaders.Add("ip", req.RemoteEndPoint.ToString());

            System.Collections.Specialized.NameValueCollection headers = req.Headers;
            // Get each header and display each value.
            foreach (string key in headers.AllKeys)
            {
                string[] values = headers.GetValues(key);
                if (values.Length > 0)
                {
                    if(key == "sec-ch-ua-platform")
                    {
                        requestHeaders.Add("platform", values[0]);
                    }else if(key == "Accept-Language")
                    {
                        requestHeaders.Add("lang", values[0]);
                    }
                }
            }

            foreach (Cookie cook in req.Cookies)
            {
                if (cook.Name == "bpp_session_id")
                {
                    sessionCookieFound = true;
                    userSessionId = cook.Value;
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

            if (sessionCookieFound)
            {
                UserSession session;
                if (ActiveUsers.TryGetValue(userSessionId, out session))
                {
                    return session;
                }

                return CreateNewUserSession(requestHeaders, response);
            }

            return CreateNewUserSession(requestHeaders, response);
        }

        public UserSession CreateNewUserSession(Dictionary<string, string> requestHeaders, HttpListenerResponse response)
        {
            UserSession newUserSession = new UserSession(requestHeaders);
            ActiveUsers.Add(newUserSession.SessionId, newUserSession);

            Cookie sessionIdCookie = new Cookie("bpp_session_id", newUserSession.SessionId);
            response.SetCookie(sessionIdCookie);

            return newUserSession;
        }

        public void RemoveInactiveUserSessions()
        {
            DateTime now = DateTime.Now;
            foreach (KeyValuePair<string, UserSession> user in ActiveUsers)
            {
                if ((now - user.Value.LastUserInteraction).TotalSeconds > 10)
                {
                    ActiveUsers.Remove(user.Key);
                }
            }
        }
    }
}
