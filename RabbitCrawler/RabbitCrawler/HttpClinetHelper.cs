using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        public static string DoPostTest(string url, object PostData)
        {
            
            url = "https://yuudnn.lz-qs.com:6026/lzmh_app_shop_api/api_shop/v1/Tip/getIsGrabTip?timestamp=1539935717&openid=603e9ce494ced0a8f2532189e9d65d66&sign=0638b37f41be2d81c9944c9d8df26c63";
            var postData = JsonConvert.SerializeObject(PostData);
            try
            {
                var requestMessage = new HttpRequestMessage();
                requestMessage.RequestUri = new Uri(url);
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Content = new StringContent(postData, Encoding.UTF8, "application/json");
                var task = Task.Factory.StartNew(() =>
                {
                    return Task.Run(() => { return _client.SendAsync(requestMessage); }).GetAwaiter().GetResult();
                });
                task.Wait();
                var taskNew = Task.Factory.StartNew(() =>
                {
                    return Task.Run(task.Result.Content.ReadAsStringAsync).GetAwaiter().GetResult();
                });
                taskNew.Wait();
                return taskNew.Result;

                //return result; 
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static HttpClient _client = new HttpClient(new HttpClientHandler
        {
            UseDefaultCredentials = false,
            AllowAutoRedirect = false,
            UseCookies = false,
            Proxy = null,
            UseProxy = false,
            AutomaticDecompression = DecompressionMethods.GZip
        });

    }
}
