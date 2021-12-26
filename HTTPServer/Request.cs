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
            string[] res = requestString.Split(delimeters, StringSplitOptions.None);

            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
            if (res.Count() < 3)
            {
                return false;
            }
            // Parse Request line
            if (!ParseRequestLine())
            {
                return false;
            }
            // Validate blank line exists
            if (!ValidateBlankLine())
            {
                return false;
            }
            // Load header lines into HeaderLines dictionary

            for (int i = 0; i < res.Count(); i++)
            {
                if (res[i].Contains(':'))
                {
                    string[] attributes = res[i].Split(':');
                    headerLines.Add(attributes[0], attributes[1]);
                }
            }
        }

        private bool ParseRequestLine()
        {
            throw new NotImplementedException();
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
            throw new NotImplementedException();
        }

        private bool ValidateBlankLine()
        {
            throw new NotImplementedException();
        }

    }
}
