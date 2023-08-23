using System.Collections.Generic;

namespace BuckshotPlusPlus
{
    public abstract class BaseDatabase
    {

        public Dictionary<string, string> DatabaseParameters { get; set; }

        public BaseDatabase(Dictionary<string, string> Parameters) { 
            DatabaseParameters = Parameters;
        }

        public abstract List<Token> Query(string Query);
    }
}
