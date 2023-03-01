using System.Collections.Generic;

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
