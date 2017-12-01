using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAppNetCore.Models
{
    public class NewsModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Img { get; set; }
        public string PublishDate { get; set; }
        public bool Status { get; set; }
    }

}
