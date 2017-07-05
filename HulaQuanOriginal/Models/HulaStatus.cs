using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HulaQuanOriginal.Models
{
    public class Publish
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public string ImageUrls { get; set; }
        public DateTime PublishDate { get; set; }

        public virtual User User { get; set; }
    }
}