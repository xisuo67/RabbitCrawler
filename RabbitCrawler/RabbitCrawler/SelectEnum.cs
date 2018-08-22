using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrawler
{

    /// <summary>
    /// 枚举选择
    /// </summary>
    [Serializable]
    public enum SelectEnum
    {
        None=0,
        [Display(Name = "")]
        All =1,
        [Display(Name = "儿歌")]
        Song =2,
        [Display(Name = "故事")]
        Story = 3,
        [Display(Name = "英语")]
        Engish =4,
        [Display(Name = "国学")]
        Sinology = 5,
        [Display(Name = "古诗")]
        Poetry = 6,
        [Display(Name = "哄睡")]
        Sleep =7,
    }
}
