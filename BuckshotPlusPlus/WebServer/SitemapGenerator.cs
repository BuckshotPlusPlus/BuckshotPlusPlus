using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace BuckshotPlusPlus.WebServer
{
    public static class SitemapGenerator
    {
        /// <summary>
        /// Generates an XML sitemap for the given list of URLs.
        /// </summary>
        /// <param name="urls">List of URLs to include in the sitemap.</param>
        public static void GenerateSitemap(Tokenizer myTokenizer,List<string> urls)
        {
            // Create a new XML document
            XmlDocument doc = new XmlDocument();

            // Add necessary XML declarations and namespaces
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.CreateElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
            doc.AppendChild(xmlDeclaration);
            doc.AppendChild(root);

            // Iterate over the list of URLs and add them to the XML document
            foreach (var url in urls)
            {
                // Create a new url element
                XmlElement urlElement = doc.CreateElement("url");

                // Create loc element and set its value
                XmlElement locElement = doc.CreateElement("loc");
                locElement.InnerText = url;
                urlElement.AppendChild(locElement);

                // Add url element to root
                root.AppendChild(urlElement);
            }

            // Save the XML document to a file
            doc.Save(myTokenizer.RelativePath + "/sitemap.xml");
        }
        
        /// <summary>
        /// Generates an XML sitemap from a list of Tokens.
        /// </summary>
        /// <param name="tokens">List of Tokens to generate the sitemap from.</param>
        public static void GenerateSitemapFromTokens(Tokenizer myTokenizer)
        {
            List<string> urls = new List<string>();
            
            foreach (Token token in myTokenizer.FileTokens)
            {
                if (token.Data.GetType() == typeof(TokenDataContainer))
                {
                    TokenDataContainer tokenData = (TokenDataContainer)token.Data;

                    if (tokenData.ContainerType == "page")
                    {
                        string pageName = tokenData.ContainerName;
                        string envBaseUrl = Environment.GetEnvironmentVariable("base_url");

                        if (envBaseUrl == null)
                        {
                            envBaseUrl = "http://localhost:8080";
                        }
                        
                        if (pageName == "index")
                        {
                            urls.Add( envBaseUrl + "/");
                        }
                        else
                        {
                            urls.Add(envBaseUrl+"/" + pageName);
                        }
                        
                    }
                }
            }
            
            GenerateSitemap(myTokenizer, urls);
        }
    }
}
