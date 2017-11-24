using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAppNetCore.Models
{
    public class NewsModel
    {
        public string title { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public string img { get; set; }
        public string date { get; set; }
    }
}
