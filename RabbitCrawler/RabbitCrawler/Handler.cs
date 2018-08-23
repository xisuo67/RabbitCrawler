using Newtonsoft.Json;
using RabbitCrawler.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitCrawler
{
    public class Handler : ICrawler
    {
        /// <summary>
        /// 单线程最大下载文件数
        /// </summary>
        private int Number100 = 100;
        /// <summary>
        /// 最大线程数
        /// </summary>
        private readonly int MaxThreadCount = 10;
        public event EventHandler<OnStartEventArgs> OnStart;//启动事件

        public event EventHandler<OnCompletedEventArgs> OnCompleted;//完成事件

        public event EventHandler<OnErrorEventArgs> OnError;//出错事件
        /// <summary>
        /// 文件下载
        /// </summary>
        private  void DownLoad(DownLoadInfo downLoad)
        {
            
                string ext = string.Empty;
                var fileName = Path.GetFileName(downLoad.MusicPath);
                var fileSuffix = fileName.Split('.');
                if (fileName.Contains("mp3"))
                {
                    ext = fileSuffix.FirstOrDefault(e => e.Contains("mp3"));
                }
                else
                {
                    ext = "mp3";
                }
                downLoad.Path = Path.Combine($"{downLoad.Path}.{ext}");
                try
                {
                    if (this.OnStart != null)
                    {
                        OnStart(this, new OnStartEventArgs(new Uri(downLoad.MusicPath)));
                    }
                    var watch = new Stopwatch();
                    watch.Start();
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(downLoad.MusicPath, downLoad.Path);
                    }
                    watch.Stop();
                    var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;//获取当前任务线程ID
                    var milliseconds = watch.ElapsedMilliseconds;//获取请求执行时间
                    if (this.OnCompleted != null) this.OnCompleted(this, new OnCompletedEventArgs(downLoad.MusicPath, threadId, milliseconds));
                }
                catch (Exception ex)
                {
                    if (this.OnError != null)
                    {
                        this.OnError(this, new OnErrorEventArgs(downLoad.MusicPath, ex));
                    }
                }
           
        }

        public List<DownLoadInfo> Execute(List<SpecialList> specialList, string savePath)
        {
            List<DownLoadInfo> downList = new List<DownLoadInfo>();
            foreach (var item in specialList)
            {
                string url = $"https://cloud.alilo.com.cn/baby/api/t/external/getSpecialInfo?id={item.id}";

                string result = HttpClinetHelper.DoPost(url, null);
                ApiResult apiResult = new ApiResult();
                apiResult = JsonConvert.DeserializeAnonymousType(result, apiResult);
                if (apiResult.msg == "成功")
                {
                    foreach (var items in apiResult.content.musicList)
                    {
                        DownLoadInfo downLoadInfo = new DownLoadInfo();
                        string Path = string.Empty;
                        string tempPath = string.Empty;
                        tempPath = $"{savePath}/{item.classname}/{item.name}";
                        if (!Directory.Exists(tempPath))
                        {
                            Directory.CreateDirectory(tempPath);
                        }
                        Path = $"{tempPath}/{items.name}";
                        downLoadInfo.Path = Path;
                        downLoadInfo.MusicPath = items.path;
                        downList.Add(downLoadInfo);
                    }
                };
            }
            return downList;
        }
        private void DownLoadDork(object obj)
        {
            DownLoadParams dparm = obj as DownLoadParams;
            foreach (var item in dparm.downLoadInfo)
            {
                DownLoad(item);
            }
        }
        public string Start(List<DownLoadInfo> list)
        {
            
                var pathsCount = list.Count;//资源总数
                                            // 计算编译线程数量
                int threadCount = pathsCount % Number100 == 0 ? pathsCount / Number100 : pathsCount / Number100 + 1;
                if (threadCount > MaxThreadCount)
                {
                    threadCount = MaxThreadCount;
                }
                int threadPqgeSize = (pathsCount / threadCount) + 1;
                int typeSum = 0;
                List<DownLoadParams> parameters = new List<DownLoadParams>(threadCount);
                DownLoadInfo[] downloadParam = null;
                DownLoadParams downLoadParams = null;
                int index, endSize = 0; ;
                for (int i = 0; i < threadCount; i++)
                {
                    downLoadParams = new DownLoadParams();
                    endSize = threadPqgeSize * (i + 1);
                    if (endSize > pathsCount)
                    {
                        var a = threadPqgeSize + pathsCount - endSize;
                        downloadParam = new DownLoadInfo[threadPqgeSize + pathsCount - endSize];
                        endSize = pathsCount;
                    }
                    else
                    {
                        downloadParam = new DownLoadInfo[threadPqgeSize];
                    }
                    index = 0;
                    for (int j = typeSum; j < endSize; j++)
                    {
                        downloadParam[index++] = list[j];
                    }
                    downLoadParams.downLoadInfo = downloadParam;
                    typeSum += downloadParam.Count();
                    parameters.Add(downLoadParams);
                }
                // 创建编译线程
                List<Thread> threads = new List<Thread>(threadCount);
                for (int i = 1; i < threadCount; i++)
                {
                    Thread thread = new Thread(DownLoadDork);
                    thread.IsBackground = true;
                    thread.Name = "DownloadThread #" + i.ToString();
                    threads.Add(thread);
                    thread.Start(parameters[i]);
                }
                // 重用当前线程：为当前线程指派下载任务。
                DownLoadDork(parameters[0]);

                // 等待所有的下载线程执行线束。
                foreach (Thread thread in threads)
                {
                    thread.Join();
                }
                return "下载完成";
      
            
        }
    }
}
