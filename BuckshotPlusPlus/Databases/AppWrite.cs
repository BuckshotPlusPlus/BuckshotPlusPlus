using System;
using System.Collections.Generic;
using Appwrite;

namespace BuckshotPlusPlus.Databases
{
    public class AppWrite : BaseDatabase
    {

        string Endpoint { get; set; }
        string Project { get; set; }
        string SecretKey { get; set; }
        string DatabaseId { get; set; }
        Client AppwriteCLient { get; set; }
        

        public AppWrite(Dictionary<string, string> Parameters) : base(Parameters)
        {
            this.Endpoint = Parameters["endpoint"];
            this.Project = Parameters["project"];
            this.SecretKey = Parameters["secret_key"];
            this.DatabaseId = Parameters["database_id"];
            this.AppwriteCLient = new Client().SetEndpoint(Endpoint).SetProject(Project).SetKey(SecretKey);

            Appwrite.Services.Databases Databases = new Appwrite.Services.Databases(AppwriteCLient);
        }

        public override string Get(string path)
        {
            throw new NotImplementedException();
        }

        public override bool Write(string path, string value)
        {
            throw new NotImplementedException();
        }
    }
}
