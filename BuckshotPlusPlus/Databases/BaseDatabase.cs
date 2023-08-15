using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuckshotPlusPlus
{
    public abstract class BaseDatabase
    {

        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public BaseDatabase(string url, string username = "", string password = "") { 
            
        }

        public abstract string Read(string path);

        public abstract bool Write(string path, string value);
    }
}
