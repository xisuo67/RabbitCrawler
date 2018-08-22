using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RabbitCrawler.Events
{
    /// <summary>
    /// 文件完成事件
    /// </summary>
    public class OnCompletedEventArgs
    {
        public string Uri { get; private set; }// 爬虫URL地址
        public int ThreadId { get; private set; }// 任务线程ID
        public string PageSource { get; private set; }// 页面源代码
        public long Milliseconds { get; private set; }// 爬虫请求执行事件
        public OnCompletedEventArgs(string uri, int threadId, long milliseconds)
        {
            this.Uri = uri;
            this.ThreadId = threadId;
            this.Milliseconds = milliseconds;
        }
    }
}
