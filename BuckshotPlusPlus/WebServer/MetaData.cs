using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuckshotPlusPlus.WebServer
{
    public enum WebMetaDataType
    {
        Query,
        Header
    }
    public class MetaData
    {
        public Dictionary<string, string> data;
        public WebMetaDataType type; 
    }
}
