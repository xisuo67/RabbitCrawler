using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrawler
{
    public class ApiResult
    {
        public string msg { get; set; }

        public string name { get; set; }

        public string ret { get; set; }

        public Content content { get; set; }
    }

    public class Content
    {
        public List<SpecialList> specialList { get; set; }

        public List<MusicList> musicList { get; set; }
    }
    public class MusicList
    {
        /// <summary>
        /// 故事名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 文件下载路径
        /// </summary>
        public string path { get; set; }
    }
    public class SpecialList
    {
        /// <summary>
        /// 分组ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string classname { get; set; }
        /// <summary>
        /// 故事名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 种子数量
        /// </summary>
        public int musicCount { get; set; }
    }

    public class DownLoadInfo
    {
        public string Path { get; set; }
        
        public string MusicPath { get; set; }
    }

    public class DownLoadParams
    {
        public DownLoadInfo[] downLoadInfo { get; set; }
    }
}
