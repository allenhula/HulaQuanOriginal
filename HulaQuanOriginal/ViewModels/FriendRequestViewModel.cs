using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HulaQuanOriginal.ViewModels
{
    public class FriendRequestViewModel
    {
        public int Id { get; set; }
        public string FromUserName { get; set; }
        public string FromUserPictureUri { get; set; }
        public bool Confirmed { get; set; }
    }
}