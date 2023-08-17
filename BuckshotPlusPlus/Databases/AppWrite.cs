using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appwrite;
using Appwrite.Services;
using Appwrite.Models;

namespace BuckshotPlusPlus.Databases
{
    public class AppWrite : BaseDatabase
    {

        string Endpoint { get; set; }
        string Project { get; set; }
        string SecretKey { get; set; }
        Client AppwriteCLient { get; set; }

        public AppWrite(Dictionary<string, string> Parameters) : base(Parameters)
        {
            this.Endpoint = Parameters["endpoint"];
            this.Project = Parameters["project"];
            this.SecretKey = Parameters["secret_key"];
            this.AppwriteCLient = new Client().SetEndpoint(Endpoint).SetProject(Project).SetKey(SecretKey);
        }

        public override string Read(string path)
        {
            throw new NotImplementedException();
        }

        public override bool Write(string path, string value)
        {
            throw new NotImplementedException();
        }
    }
}
