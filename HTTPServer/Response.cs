using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }
    // response format is string you should put it in format response
    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
        //StatusCode code;
        List<string> headerLines = new List<string>();
        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {
            //throw new NotImplementedException();
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            
            string status = GetStatusLine(code);

            headerLines.Add(contentType);
            headerLines.Add(content.Length.ToString());
            headerLines.Add(DateTime.Now.ToString("ddd,dd MMM yyy HH':'mm':'ss 'EST'"));

            if (code == StatusCode.Redirect)
            {
                headerLines.Add(redirectoinPath);

                responseString = status + "\r\n" + "Content-Type: " + headerLines[0] + "\r\n" + "Content-Length: " + headerLines[1] + "\r\n" + "Date: " + headerLines[2] + "\r\n" + "location: " + headerLines[3] + "\r\n" + "\r\n" + content;

            }
            else
            {
                responseString = status + "\r\n" + "Content-Type: " + headerLines[0] + "\r\n" + "Content-Length: " + headerLines[1] + "\r\n" + "Date: " + headerLines[2] + "\r\n"+ "\r\n" + content;
            }
            
        }

        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
            string statusLine = string.Empty;
            if (code == StatusCode.OK)
            {
                statusLine = "HTTP/1.1" + " " + code + " " + "OK";
            }
            else if (code == StatusCode.Redirect)
            {
                statusLine = "HTTP/1.1" + " " + code + " " + "Redirect";
            }
            else if (code == StatusCode.BadRequest)
            {
                statusLine = "HTTP/1.1" + " " + code + " " + "BadRequest";
            }
            else if (code == StatusCode.NotFound)
            {
                statusLine = "HTTP/1.1" + " " + code + " " + "NotFound";
            }
            else if (code == StatusCode.InternalServerError)
            {
                statusLine = "HTTP/1.1" + " " + code + " " + "InternalServerError";
            }
            return statusLine;
        }
    }
}