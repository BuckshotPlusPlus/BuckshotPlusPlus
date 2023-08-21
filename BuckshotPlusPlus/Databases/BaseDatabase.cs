using System.Collections.Generic;

namespace BuckshotPlusPlus
{
    public abstract class BaseDatabase
    {

        public Dictionary<string, string> DatabaseParameters { get; set; }

        public BaseDatabase(Dictionary<string, string> Parameters) { 
            DatabaseParameters = Parameters;
        }

        public abstract Dictionary<string, object> GetObject(string path);

        public abstract bool Write(string path, string value);
    }
}
