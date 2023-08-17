using System;
using System.Collections.Generic;
using System.Xml;

namespace BuckshotPlusPlus
{
    public class FileStorageDatabase : BaseDatabase
    {
        public XmlReader DatabaseXmlReader { get; set; }
        // This module is a simple database stored in an XmlFile

        public FileStorageDatabase(Dictionary<string, string> Parameters) : base(Parameters)
        {
            
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
