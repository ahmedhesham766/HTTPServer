using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 
            CreateRedirectionRulesFile();
            //Start server
           string filePath =  "RedirectionRules.txt";
            // 1) Make server object on port 1000
            Server httpserver = new Server(1000, filePath);
            // 2) Start Server
            httpserver.StartServer();

        }

        static void CreateRedirectionRulesFile()
        {
            // TODO: Create file named redirectionRules.txt
            FileStream redirectionRules_File = new FileStream("RedirectionRules.txt", FileMode.OpenOrCreate);
            StreamWriter file_write = new StreamWriter(redirectionRules_File);
            file_write.WriteLine(@"aboutus.html,aboutus2.html");
            file_write.Flush();
            redirectionRules_File.Close();
            // each line in the file specify a redirection rule
            // example: "aboutus.html,aboutus2.html"
            // means that when making request to aboustus.html,, it redirects me to aboutus2


        }

    }
}