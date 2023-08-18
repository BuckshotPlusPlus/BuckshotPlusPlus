using System.Collections.Generic;
using System.IO;

namespace BuckshotPlusPlus
{
    public abstract class BaseDatabase
    {

        public Dictionary<string, string> DatabaseParameters { get; set; }

        public BaseDatabase(Dictionary<string, string> Parameters) { 
            DatabaseParameters = Parameters;
        }

        public abstract string Get(string path);

        public abstract bool Write(string path, string value);
    }
}
