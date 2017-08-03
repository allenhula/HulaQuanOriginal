using HulaQuanOriginal.DAL;
using HulaQuanOriginal.Models;
using HulaQuanOriginal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HulaQuanOriginal.Controllers
{
    [Authorize]
    public class HulaFriendController : Controller
    {
        // GET: HulaFriend
        public ActionResult Get()
        {
            using (var hulaDb = new HulaContext())
            {
                var currentUserId = int.Parse(User.Identity.Name.Split(',')[1]);
                var friendIds = hulaDb.Relationships.Where(r => r.UserId == currentUserId).Select(r => r.FriendId).ToList();
                var friends = friendIds.Select(fid => hulaDb.Users.Find(fid)).ToList();
                return View(friends);
            }            
        }

        public ActionResult GetRequests()
        {
            using (var hulaDb = new HulaContext())
            {
                var currentUserId = int.Parse(User.Identity.Name.Split(',')[1]);
                var requests = hulaDb.FriendRequests.Where(fr => fr.ToUserId == currentUserId).ToList();
                var requestVMs = requests.Select(r =>
                {
                    var fromUser = hulaDb.Users.Find(r.FromUserId);
                    return new FriendRequestViewModel()
                    {
                        Id = r.Id,
                        FromUserName = fromUser.Name,
                        FromUserPictureUri = fromUser.PortraitUrl,
                        Confirmed = r.Confirmed
                    };
                }).ToList();
                return View(requestVMs);
            }
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string search)
        {
            var currentUserId = int.Parse(User.Identity.Name.Split(',')[1]);
            int idToSearch;
            if (int.TryParse(search, out idToSearch))
            {
                using (var hulaDb = new HulaContext())
                {
                    var user = hulaDb.Users.Find(idToSearch);
                    if (user != null)
                    {
                        hulaDb.FriendRequests.Add(new FriendRequest()
                        {
                            FromUserId = currentUserId,
                            ToUserId = idToSearch,
                            Confirmed = false,
                            CreatedTime = DateTime.UtcNow
                        });
                        hulaDb.SaveChanges();

                        return RedirectToAction("Get");
                    }
                    else
                    {
                        ViewBag.SearchResult = "No such user exists!";
                    }
                }
            }
            else
            {
                ViewBag.SearchResult = "Only allow for numberic id search!";
            }
            return View();
        }

        public ActionResult Confirm(int id)
        {
            using (var hulaDb = new HulaContext())
            {
                var request = hulaDb.FriendRequests.Find(id);
                if (request == null)
                {
                    return HttpNotFound();
                }
                using (var dbTransaction = hulaDb.Database.BeginTransaction())
                {
                    try
                    {
                        // confirm                    
                        request.Confirmed = true;
                        hulaDb.SaveChanges();

                        // add friend relationship 
                        hulaDb.Relationships.Add(new Relationship()
                        {
                            UserId = request.FromUserId,
                            FriendId = request.ToUserId
                        });
                        hulaDb.SaveChanges();

                        hulaDb.Relationships.Add(new Relationship()
                        {
                            UserId = request.ToUserId,
                            FriendId = request.FromUserId
                        });
                        hulaDb.SaveChanges();

                        var fromUser = hulaDb.Users.Find(request.FromUserId);
                        var toUser = hulaDb.Users.Find(request.ToUserId);

                        // need to read the friends status history
                        var friendsPublish4ToUser = fromUser.Publishs
                            .OrderByDescending(p => p.PublishDate)
                            .Take(10)
                            .Select(p => new FriendPublish()
                        {
                            UserId = toUser.Id,
                            PublishId = p.Id,
                            PublishDate = p.PublishDate
                        }).ToList();
                        hulaDb.FriendPublishs.AddRange(friendsPublish4ToUser);

                        var friendsPublish4FromUser = toUser.Publishs
                            .OrderByDescending(p => p.PublishDate)
                            .Take(10)
                            .Select(p => new FriendPublish()
                        {
                            UserId = fromUser.Id,
                            PublishId = p.Id,
                            PublishDate = p.PublishDate
                        }).ToList();
                        hulaDb.FriendPublishs.AddRange(friendsPublish4FromUser);
                        hulaDb.SaveChanges();

                        dbTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbTransaction.Rollback();
                    }
                }
            }
            
            return RedirectToAction("Get");
        }
    }
}