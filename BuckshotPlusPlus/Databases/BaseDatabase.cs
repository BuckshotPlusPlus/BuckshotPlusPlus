using System.Collections.Generic;

namespace BuckshotPlusPlus
{
    public abstract class BaseDatabase
    {

        public Dictionary<string, string> DatabaseParameters { get; set; }
        public Tokenizer MyTokenizer { get; set; }

        public BaseDatabase(Dictionary<string, string> parameters, Tokenizer myTokenizer) { 
            DatabaseParameters = parameters;
            this.MyTokenizer = myTokenizer;
        }

        public abstract Token Query(string query);
    }
}
