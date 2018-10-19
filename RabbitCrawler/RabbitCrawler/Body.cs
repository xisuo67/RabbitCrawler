using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrawler
{
    public class Body
    {
        public string com_id { get; set; }
    }
    public class Header
    {
        public string type { get; set; }

        public string appVersion { get; set; }

        public string imei { get; set; }

        public string Operator {get;set;}

        public string apkName { get; set; }

        public string appCode { get; set; }

        public string andModel { get; set; }

        public string netWorkType { get; set; }

        public string sdk { get; set; }

        public string phoneMac { get; set; }
    }
    public class MessageBody
    {
        public Body body { get; set; }

        public Header header { get; set; }
    }
}
