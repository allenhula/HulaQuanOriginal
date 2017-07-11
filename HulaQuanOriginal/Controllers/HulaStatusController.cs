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
        public ActionResult GetFriendPublishs()
        {
            //var portraitUri = "https://allenlsharest.blob.core.chinacloudapi.cn/share/me3.jpg";
            //var imageUris = "https://allenlsharest.blob.core.chinacloudapi.cn/share/me3.jpg;https://allenlsharest.blob.core.chinacloudapi.cn/share/rabbit.png";
            //var demoList = new List<HulaStatusViewModel>()
            //{
            //    new HulaStatusViewModel() { Id=1, PublisherId=1, PublisherName="allen", PublisherPortraitUri=portraitUri, Content="hello hula", ImageUris=imageUris, PublishDate=DateTime.UtcNow},
            //    new HulaStatusViewModel() { Id=2, PublisherId=1, PublisherName="allen", PublisherPortraitUri=portraitUri, Content="hello hula", ImageUris=imageUris, PublishDate=DateTime.UtcNow},
            //    new HulaStatusViewModel() { Id=3, PublisherId=1, PublisherName="allen", PublisherPortraitUri=portraitUri, Content="hello hula", ImageUris=imageUris, PublishDate=DateTime.UtcNow}
            //};

            // TODO: Add time filter
            var currentUserId = 1;
            var currentUser = hulaContext.Users.Find(currentUserId);
            var friendPublishs = hulaContext.FriendPublishs.Where(fp => fp.UserId == currentUserId).ToList();

            var hulaStatusVMs = friendPublishs.Select(fp =>
            {
                var publish = hulaContext.Publishs.Find(fp.PublishId);
                var publisher = publish.User;
                return new HulaStatusViewModel()
                {
                    Id = publish.Id,
                    PublisherId = publish.UserId,
                    PublisherName = publisher.Name,
                    PublishDate = publish.PublishDate,
                    PublisherPortraitUri = publisher.PortraitUrl,
                    Content = publish.Content,
                    ImageUris = publish.ImageUrls
                };
            }).ToList();

            return View(hulaStatusVMs);
        }

        public ActionResult NewPublish()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewPublish(HttpPostedFileBase[] images, string content)
        {
            foreach (var image in images)
            {

            }
            return RedirectToAction("Index");
        }

        public ActionResult GetMyPublishs()
        {
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

            return View();
        }
    }
}