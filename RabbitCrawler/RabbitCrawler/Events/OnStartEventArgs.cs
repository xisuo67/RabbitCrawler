using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RabbitCrawler.Events
{
    /// <summary>
    /// 文件下载启动事件
    /// </summary>
    public class OnStartEventArgs
    {
        public Uri Uri { get; set; }// 爬虫URL地址
        //public OnStartEventArgs(Uri uri)
        //{
        //    this.Uri = uri;
        //}
        public OnStartEventArgs(Uri uri)
        {
            this.Uri = uri;
        }
    }
}
