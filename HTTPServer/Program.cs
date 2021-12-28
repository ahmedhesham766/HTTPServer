﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Program
    {
        string redirectionPath = @"\bin\Debug\RedirectionRules.txt";
        
        static void Main(string[] args)
        {
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 
            CreateRedirectionRulesFile();
            //Start server
            Console.WriteLine("Starting Server......");
            // 1) Make server object on port 1000
            Server httpServer = new Server(1000, redirectionPath)
            // 2) Start Server
            httpServer.StartServer();
        }

        static void CreateRedirectionRulesFile()
        {
            // TODO: Create file named redirectionRules.txt
            FileStream redirectionRules_File = new FileStream(redirectionPath, FileMode.OpenOrCreate);
            // each line in the file specify a redirection rule
            StreamWriter file_write = new StreamWriter(redirectionRules_File);
            file_write.WriteLine(@"aboutus.html,aboutus2.html");
            // example: "aboutus.html,aboutus2.html"
            // means that when making request to aboustus.html,, it redirects me to aboutus2
        }
         
    }
}
