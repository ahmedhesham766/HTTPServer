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
            throw new NotImplementedException();

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
            //relativeURI = tokens[1];
            if (!ValidateIsURI(relativeURI))
            {
                return false;
            }
            // Validate blank line exists

            if (!ValidateBlankLine())
            {
                return false;
            }
            // Load header lines into HeaderLines dictionary

            if(!LoadHeaderLines())
            {
                return false;
            }

            return true;
        }

        private bool ParseRequestLine()
        {
            // throw new NotImplementedException();
            string[] tokens = requestLines[0].Split(' ');
            relativeURI = tokens[1];
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
            return true;

        }

        private bool ValidateIsURI(string uri)
        {

            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
            //throw new NotImplementedException();
            string[] delimeters1 = { ": " };
            for (int i = 0; i < requestLines.Length; i++)
            {
                if (requestLines[i].Contains(':'))
                {
                    string[] attributes = requestLines[i].Split(delimeters1, StringSplitOptions.None);
                    headerLines.Add(attributes[0], attributes[1]);
                   
                }
            }
            if (headerLines.Count() == 0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateBlankLine()
        {
            //throw new NotImplementedException();
            string blankline = requestLines[(requestLines.Length)-2];
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
