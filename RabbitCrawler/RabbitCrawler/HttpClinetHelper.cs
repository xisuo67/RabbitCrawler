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
            url = "https://yuudnn.lz-qs.com:6026/lzmh_app_shop_api/api_shop/v1/Tip/getIsGrabTip?timestamp=1539933969&openid=603e9ce494ced0a8f2532189e9d65d66&sign=7f1eab21657618631264d6686930130f";
            //string postData = "{'body':{'com_id':'1398'},'header':{'type':'2','appVersion':'4.2.0','imei':'1a1018970af071e69a1','operator':'联通','apkName':'com.project.LZMH','appCode':'201808150','andModel':'iPhone SE','netWorkType':'WiFi','sdk':'ios11.4','phoneMac':'88:25:93:a4: 5b: d9'}}";
            var postData = JsonConvert.SerializeObject(PostData);
            try
            {
                //HttpWebRequest request =(HttpWebRequest)HttpWebRequest.Create(url);
                //request.ContentType = "application/json";
                ////request.Connection = "keep-alive";
                ////request.Host = "yuudnn.lz-qs.com:6026";
                ////request.Headers.Add(HttpRequestHeader.AcceptEncoding, "br,gzip,deflate");
                ////request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-Hans-CN;q=1");
                //request.KeepAlive = true;
                ////request.Accept = "*/*";
                //request.UserAgent = "LZMH/4.2.0 (iPhone; iOS 11.4.1; Scale/2.00)";
                //byte[] postdatabyte = Encoding.UTF8.GetBytes(postData);
                //request.Method = "post";
                //request.ContentLength= postdatabyte.Length;
                ////提交请求
                //Stream stream;
                //stream = request.GetRequestStream();
                //stream.Write(postdatabyte, 0, postdatabyte.Length);
                //stream.Close();

                ////接收响应
                //var response = (HttpWebResponse)request.GetResponse();
                //Stream responseStream = response.GetResponseStream();
                //StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                //string result = streamReader.ReadToEnd();
                //streamReader.Close();
                //responseStream.Close();
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
