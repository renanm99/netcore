using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Newspaper.Models
{
    public class FeedModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public string Img { get; set; }
        public DateTime PublishDate { get; set; }
        public bool ValidImg { get; set; }

        public FeedModel()
        {
            Title = "";
            Content = "";
            Link = "";
            Img = "";
            PublishDate = DateTime.Today;
            ValidImg = false;
        }
    }
}
