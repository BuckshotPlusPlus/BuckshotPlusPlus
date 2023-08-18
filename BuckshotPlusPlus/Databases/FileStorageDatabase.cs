using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BuckshotPlusPlus
{
    public class FileStorageDatabase : BaseDatabase
    {
        public string BaseUrl { get; set; }
        // This module is a simple database stored in an XmlFile

        public FileStorageDatabase(Dictionary<string, string> Parameters) : base(Parameters)
        {
            BaseUrl = "/data/";
        }

        public override string Get(string Path)
        {
            string[] FilePath = GetFilePath(Path);

            string FileContent = File.ReadAllText(FilePath[0]);
            Dictionary<string, string> FileData = JsonConvert.DeserializeObject<Dictionary<string, string>>(FileContent);

            if (FileData[FilePath[1]] != null )
            {
                return FileData[FilePath[1]];
            }
            return "not_found";
        }

        public override bool Write(string path, string value)
        {
            throw new NotImplementedException();
        }

        public string[] GetFilePath(string Path)
        {
            string[] PathElements = Path.Split('/');
            string CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory + BaseUrl;

            for (int i = 0; i < PathElements.Length - 2; i++)
            {
                if (!Directory.Exists(CurrentDirectory)) { 
                    Directory.CreateDirectory(CurrentDirectory);
                }
                CurrentDirectory += PathElements[i] += "/";
            }

            if (!Directory.Exists(CurrentDirectory))
            {
                Directory.CreateDirectory(CurrentDirectory);
            }

            string FilePath = CurrentDirectory + PathElements[^2];
            if (!File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, "{}");
            }

            string[] result = { FilePath, PathElements[^1] };
            return result;
        }
    }
}
