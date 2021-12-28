using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines;

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()
        {
            //TODO: parse the receivedRequest using the \r\n delimeter   
            string[] delimeters = { "\r\n" };
            requestLines = requestString.Split(delimeters, StringSplitOptions.None);
            
            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
            if (requestLines.Length < 3)
            {
                return false;
            }
            // Parse Request line
            if(!ParseRequestLine())
            {
                return false;
            }  
            //Check The URI
            if (!ValidateIsURI(relativeURI))
            {
                return false;
            }
            // Load header lines into HeaderLines dictionary
            if(!LoadHeaderLines())
            {
                return false;
            }
            // Validate blank line exists
            if (!ValidateBlankLine())
            {
                return false;
            }

            return true;
        }

        private bool ParseRequestLine()
        {
            string[] tokens = requestLines[0].Split(' ');
            if (tokens[0].Equals("GET"))
            {
                method = RequestMethod.GET;
            }
            else if (tokens[0].Equals("POST"))
            {
                method = RequestMethod.POST;
            }
            else if (tokens[0].Equals("HEAD"))
            {
                method = RequestMethod.HEAD;
            }
            ////////////////////////
            relativeURI = tokens[1];
            ////////////////////////
            if (tokens[2].Equals("HTTP/1.0"))
            {
                httpVersion = HTTPVersion.HTTP10;
            }
            else if (tokens[2].Equals("HTTP/1.1"))
            {
                httpVersion = HTTPVersion.HTTP11;
            }
            else if (tokens[2].Equals("HTTP/0.9"))
            {
                httpVersion = HTTPVersion.HTTP09;
            }
            if (tokens[0]== null || tokens[2] == null)
            {
                return false;
            }
            //print the methode type and the http version
            Console.WriteLine(method.ToString() + ": " + httpVersion.ToString());

            return true;

        }

        private bool ValidateIsURI(string uri)
        {
            Console.WriteLine("the uri: " + uri);
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
            string[] delimeters1 = { ": " };
            headerLines = new Dictionary<string, string>();
            for (int i = 0; i < requestLines.Length; i++)
            {
                if (requestLines[i].Contains(':'))
                {
                    string[] attributes = requestLines[i].Split(delimeters1, StringSplitOptions.None);
                    headerLines.Add(attributes[0], attributes[1]);
                   
                }
            }
            for (int i = 0;i < headerLines.Count(); i++)
            {
                //print key : value of the headers
                Console.WriteLine(headerLines.ElementAt(i).Key + " : " + headerLines.ElementAt(i).Value);
            }
            if (headerLines.Count() == 0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateBlankLine()
        {
            //The BlankLine will befor the last in the request as in get method the last line is an empty content
            string blankline = requestLines[(requestLines.Length)-2];
            Console.WriteLine(blankline);
            if (blankline.Equals(String.Empty))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
