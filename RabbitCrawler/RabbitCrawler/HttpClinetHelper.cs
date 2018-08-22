using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrawler
{
    public class HttpClinetHelper
    {
        /// <summary>
        /// Http模拟Post请求
        /// </summary>
        /// <param name="url">API接口地址</param>
        /// <param name="PostData">提交数据</param>
        /// <returns>返回结果</returns>
        public static string DoPost(string url, object PostData)
        {
            try
            {
                int TimeOut = 200;
                //设置HttpClientHandler的AutomaticDecompression
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                //创建HttpClient（注意传入HttpClientHandler）
                using (var http = new HttpClient(handler))
                {
                    http.Timeout = TimeSpan.FromSeconds(TimeOut);
                    var response = http.PostAsync(url, PostData,
                            new System.Net.Http.Formatting.JsonMediaTypeFormatter());

                    //确保HTTP成功状态值
                    return response.Result.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
