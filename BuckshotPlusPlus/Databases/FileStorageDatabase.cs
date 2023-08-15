using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BuckshotPlusPlus
{
    public class FileStorageDatabase : BaseDatabase
    {
        public XmlReader DatabaseXmlReader { get; set; }
        // This module is a simple database stored in an XmlFile

        public FileStorageDatabase(string url, string username = "", string password = "") : base(url, username, password)
        {
            this.Url = url;
            DatabaseXmlReader = XmlReader.Create(url);
        }

        public override string Read(string Path)
        {

            return "";
        }

        public override bool Write(string path, string value)
        {
            throw new NotImplementedException();
        }
    }
}
