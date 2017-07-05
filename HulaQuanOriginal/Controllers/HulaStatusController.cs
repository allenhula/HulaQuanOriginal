using HulaQuanOriginal.DAL;
using HulaQuanOriginal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HulaQuanOriginal.Controllers
{
    public class HulaStatusController : Controller
    {
        private HulaContext hulaContext = new HulaContext();
        // GET: HulaStatus
        public ActionResult Index()
        {
            var portraitUri = "https://allenlsharest.blob.core.chinacloudapi.cn/share/me3.jpg";
            var imageUris = "https://allenlsharest.blob.core.chinacloudapi.cn/share/me3.jpg;https://allenlsharest.blob.core.chinacloudapi.cn/share/rabbit.png";
            var demoList = new List<HulaStatusViewModel>()
            {
                new HulaStatusViewModel() { Id=1, PublisherId=1, PublisherName="allen", PublisherPortraitUri=portraitUri, Content="hello hula", ImageUris=imageUris, PublishDate=DateTime.UtcNow},
                new HulaStatusViewModel() { Id=2, PublisherId=1, PublisherName="allen", PublisherPortraitUri=portraitUri, Content="hello hula", ImageUris=imageUris, PublishDate=DateTime.UtcNow},
                new HulaStatusViewModel() { Id=3, PublisherId=1, PublisherName="allen", PublisherPortraitUri=portraitUri, Content="hello hula", ImageUris=imageUris, PublishDate=DateTime.UtcNow}
            };

            //var currentUserId = 1;
            //var currentUser = hulaContext.Users.Find(currentUserId);
            //var myPublishs = currentUser.Publishs.ToList();

            //var hulaStatusVMs = myPublishs.Select(p => new HulaStatusViewModel()
            //{
            //    Id = p.Id,
            //    PublisherId = currentUserId,
            //    PublisherName = currentUser.Name,
            //    PublishDate = p.PublishDate,
            //    PublisherPortraitUri = currentUser.PortraitUrl,
            //    Content = p.Content,
            //    ImageUris = p.ImageUrls
            //}).ToList<HulaStatusViewModel>();

            return View(demoList);
        }
    }
}