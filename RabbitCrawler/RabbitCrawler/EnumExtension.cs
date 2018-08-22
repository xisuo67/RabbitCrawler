using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrawler
{
    public static class EnumExtension
    {
        public static string EnumMetadataDisplay(this Enum value)
        {
            string name = Enum.GetName(value.GetType(), value);
            if (string.IsNullOrEmpty(name))
                return value.ToString();
            var attribute = value.GetType().GetField(name).GetCustomAttributes(
                 typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false)
                 .Cast<System.ComponentModel.DataAnnotations.DisplayAttribute>()
                 .FirstOrDefault();
            if (attribute != null)
                return attribute.Name;

            return value.ToString();
        }
    }
}
