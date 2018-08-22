using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RabbitCrawler.Events
{
    public class OnErrorEventArgs
    {
        public string Uri { get; set; }

        public Exception Exception { get; set; }

        public OnErrorEventArgs(string uri, Exception exception)
        {
            this.Uri = uri;
            this.Exception = exception;
        }
    }
}
