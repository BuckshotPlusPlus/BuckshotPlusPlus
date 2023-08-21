using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        Appwrite.Services.Databases DBClient { get; set; }  


        public AppWrite(Dictionary<string, string> Parameters) : base(Parameters)
        {
            this.Endpoint = Parameters["endpoint"];
            this.Project = Parameters["project"];
            this.SecretKey = Parameters["secret_key"];
            this.AppwriteCLient = new Client().SetEndpoint(Endpoint).SetProject(Project).SetKey(SecretKey);

            DBClient = new Appwrite.Services.Databases(AppwriteCLient);
        }

        public override Dictionary<string, object> GetObject(string path)
        {
            string[] args = path.Split('/');
            Task<Appwrite.Models.Document> getDocument = DBClient.GetDocument(args[0], args[1], args[2]);
            getDocument.Wait();
            Appwrite.Models.Document doc = getDocument.Result;
            return doc.Data;
        }

        public override bool Write(string path, string value)
        {
            throw new NotImplementedException();
        }
    }
}
