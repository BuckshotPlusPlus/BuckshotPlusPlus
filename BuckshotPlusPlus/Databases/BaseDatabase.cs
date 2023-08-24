using System.Collections.Generic;

namespace BuckshotPlusPlus
{
    public abstract class BaseDatabase
    {

        public Dictionary<string, string> DatabaseParameters { get; set; }
        public Tokenizer MyTokenizer { get; set; }

        public BaseDatabase(Dictionary<string, string> Parameters, Tokenizer MyTokenizer) { 
            DatabaseParameters = Parameters;
            this.MyTokenizer = MyTokenizer;
        }

        public abstract Token Query(string Query);
    }
}
