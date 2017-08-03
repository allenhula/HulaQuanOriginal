using HulaQuanOriginal.DAL;
using HulaQuanOriginal.Helpers;
using HulaQuanOriginal.Models;
using HulaQuanOriginal.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HulaQuanOriginal.Controllers
{
    [Authorize]
    public class HulaStatusController : Controller
    {
        // GET: HulaStatus
        public ActionResult GetFriendPublishs()
        {
            // TODO: Add time filter
            var currentUserId = int.Parse(User.Identity.Name.Split(',')[1]);
            using (var hulaDb = new HulaContext())
            {
                var currentUser = hulaDb.Users.Find(currentUserId);
                var friendPublishs = hulaDb.FriendPublishs.Where(fp => fp.UserId == currentUserId).ToList();

                var hulaStatusVMs = friendPublishs.Select(fp =>
                {
                    var publish = hulaDb.Publishs.Find(fp.PublishId);
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
                }).OrderByDescending(fp => fp.PublishDate).ToList();

                return View(hulaStatusVMs);
            }
        }

        public ActionResult NewPublish()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewPublish(IEnumerable<HttpPostedFileBase> images, string content)
        {
            var currentUserId = int.Parse(User.Identity.Name.Split(',')[1]);
            StringBuilder sb = new StringBuilder();
            foreach (var image in images)
            {
                if (image != null)
                {
                    var namePrefix = StringHelper.GetRandomString(DateTime.UtcNow);
                    var imageExtension = Path.GetExtension(image.FileName);

                    // save the original image
                    var imageFileName = $"{namePrefix}{imageExtension}";
                    var imagePath = $"~/PublishImages/{imageFileName}";
                    var imageSrvSavePath = Path.Combine(Server.MapPath(imagePath));
                    image.SaveAs(imageSrvSavePath);

                    // combine image urls into one string to save in db
                    var imageUrl = $"{Request.Url.GetLeftPart(UriPartial.Authority)}{VirtualPathUtility.ToAbsolute(imagePath)}";
                    sb.Append(imageUrl);
                    sb.Append(Constants.ImageStringSpliter);

                    // save the image thumpnail with 90*90 size
                    var imageThumpnailFileName = $"{namePrefix}{Constants.Image90X90Suffix}{imageExtension}";
                    var imageThumpnailPath = $"~/PublishImages/{imageThumpnailFileName}";
                    var imageThumpnailSrvSavePath = Path.Combine(Server.MapPath(imageThumpnailPath));
                    ImageHelper.ResizeAndSaveImage(
                        Image.FromStream(image.InputStream),
                        Constants.Image90X90Width,
                        Constants.Image90X90Height,
                        imageThumpnailSrvSavePath);                    
                }
            }

            // save publish into database
            var publish = new Publish()
            {
                UserId = currentUserId,
                Content = content,
                ImageUrls = sb.ToString().TrimEnd(Constants.ImageStringSpliter),
                PublishDate = DateTime.UtcNow
            };

            using (var hulaDb = new HulaContext())
            {
                var publishInDb = hulaDb.Publishs.Add(publish);
                hulaDb.SaveChanges();

                // add reference of this publish to all friends
                var friendIds = hulaDb.Relationships.Where(r => r.UserId == currentUserId).Select(r => r.FriendId).ToList();
                foreach (var friendId in friendIds)
                {
                    hulaDb.FriendPublishs.Add(
                    new FriendPublish()
                    {
                        UserId = friendId,
                        PublishId = publishInDb.Id,
                        PublishDate = publishInDb.PublishDate
                    });
                }
                hulaDb.SaveChanges();

                return RedirectToAction("GetFriendPublishs");
            }
        }

        public ActionResult GetMyPublishs()
        {
            var currentUserId = int.Parse(User.Identity.Name.Split(',')[1]);

            using (var hulaDb = new HulaContext())
            {
                var currentUser = hulaDb.Users.Find(currentUserId);
                var myPublishs = currentUser.Publishs.OrderByDescending(p => p.PublishDate).ToList();

                return View(myPublishs);
            }
        }
    }
}