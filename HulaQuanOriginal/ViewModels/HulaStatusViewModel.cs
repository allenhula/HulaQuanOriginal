using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HulaQuanOriginal.ViewModels
{
    public class HulaStatusViewModel
    {
        public int Id { get; set; }
        public string PublisherName { get; set; }
        public int PublisherId { get; set; }
        public string PublisherPortraitUri { get; set; }
        public DateTime PublishDate { get; set; }
        public string Content { get; set; }
        public string ImageUris { get; set; }
    }
    
}