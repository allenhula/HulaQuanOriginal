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
    public class HulaFriendController : Controller
    {
        private HulaContext hulaDb = new HulaContext();

        // GET: HulaFriend
        public ActionResult Get()
        {
            var currentUserId = 1;
            var friendIds = hulaDb.Relationships.Where(r => r.UserId == currentUserId).Select(r => r.FriendId).ToList();
            var friends = friendIds.Select(fid => hulaDb.Users.Find(fid)).ToList();
            return View(friends);
        }

        public ActionResult GetRequests()
        {
            var currentUserId = 1;
            var requests = hulaDb.FriendRequests.Where(fr => fr.ToUserId == currentUserId).ToList();
            var requestVMs = requests.Select(r => {
                var fromUser = hulaDb.Users.Find(r.FromUserId);
                return new FriendRequestViewModel()
                {
                    Id = r.Id,
                    FromUserName = fromUser.Name,
                    FromUserPictureUri = fromUser.PortraitUrl,
                    Confirmed = r.Confirmed
                };
                });
            return View(requestVMs);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string search)
        {
            var currentUserId = 1;
            int idToSearch;
            if (int.TryParse(search, out idToSearch))
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
            }

            ViewBag.SearchResult = "No such user exists!";
            return View();
        }

        public ActionResult Confirm(int id)
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
                    // add friend relationship 
                    hulaDb.Relationships.Add(new Relationship()
                    {
                        UserId = request.FromUserId,
                        FriendId = request.ToUserId
                    });
                    hulaDb.Relationships.Add(new Relationship()
                    {
                        UserId = request.ToUserId,
                        FriendId = request.FromUserId
                    });

                    hulaDb.SaveChanges();

                    dbTransaction.Commit();
                }
                catch (Exception)
                {
                    dbTransaction.Rollback();
                }
            }
            
            return RedirectToAction("Get");
        }
    }
}