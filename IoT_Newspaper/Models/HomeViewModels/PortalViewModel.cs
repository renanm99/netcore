using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IoT_Newspaper.Model;

namespace IoT_Newspaper.Models.HomeViewModels
{
    public class PortalViewModel
    {
        public ICollection<Section> Sections { get; set; }

        public IEnumerable<IoT_Newspaper.Models.FeedModel> Feeds { get; set; }

}
}
