using HulaQuanOriginal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HulaQuanOriginal.DAL
{
    public class HulaDbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<HulaContext>
    {
        protected override void Seed(HulaContext context)
        {
            var users = new List<User>
            {
                new User { Name="allenl", Email="lyp311307@163.com", PhoneNumber="12345607890", PortraitUrl="https://allenlsharest.blob.core.chinacloudapi.cn/share/me3.jpg" },
                new User { Name="cindyl", Email="lqx311307@126.com", PhoneNumber="12345617891", PortraitUrl="https://allenlsharest.blob.core.chinacloudapi.cn/share/me3.jpg" }
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();

            var publishs = new List<Publish>
            {
                new Publish { UserId = 1,
                    Content ="Haha you are the best as always, Haha you are the best as always, Haha you are the best as always!",
                    ImageUrls ="https://allenlsharest.blob.core.chinacloudapi.cn/share/me3.jpg;https://allenlsharest.blob.core.chinacloudapi.cn/share/rabbit.png",
                    PublishDate = DateTime.Parse("7/10/2017 8:32:20")
                },
                new Publish { UserId = 2,
                    Content ="Haha you are the best as always, Haha you are the best as always, Haha you are the best as always!",
                    ImageUrls ="https://allenlsharest.blob.core.chinacloudapi.cn/share/me3.jpg;https://allenlsharest.blob.core.chinacloudapi.cn/share/rabbit.png",
                    PublishDate = DateTime.Parse("7/10/2017 16:22:10")
                },
                new Publish { UserId = 1,
                    Content ="Haha you are the best as always, Haha you are the best as always, Haha you are the best as always!",
                    ImageUrls ="https://allenlsharest.blob.core.chinacloudapi.cn/share/me3.jpg;https://allenlsharest.blob.core.chinacloudapi.cn/share/rabbit.png",
                    PublishDate = DateTime.Parse("7/11/2017 8:12:00")
                },
                new Publish { UserId = 2,
                    Content ="Haha you are the best as always, Haha you are the best as always, Haha you are the best as always!",
                    ImageUrls ="https://allenlsharest.blob.core.chinacloudapi.cn/share/me3.jpg;https://allenlsharest.blob.core.chinacloudapi.cn/share/rabbit.png",
                    PublishDate = DateTime.Parse("7/11/2017 10:46:25")
                },
            };
            publishs.ForEach(p => context.Publishs.Add(p));
            context.SaveChanges();

            var friendRequests = new List<FriendRequest>
            {
                new FriendRequest { FromUserId = 1, ToUserId = 2, Confirmed = true, CreatedTime = DateTime.Parse("7/9/2017 16:22:10") }
            };
            friendRequests.ForEach(fr => context.FriendRequests.Add(fr));
            context.SaveChanges();

            var relationships = new List<Relationship>
            {
                new Relationship { UserId = 1, FriendId = 2},
                new Relationship { UserId = 2, FriendId = 1},
            };
            relationships.ForEach(r => context.Relationships.Add(r));
            context.SaveChanges();

            var friendPublishs = new List<FriendPublish>
            {
                new FriendPublish { PublishId = 2, UserId = 1, PublishDate = DateTime.Parse("7/10/2017 16:22:10")},
                new FriendPublish { PublishId = 4, UserId = 1, PublishDate = DateTime.Parse("7/11/2017 10:46:25")},
                new FriendPublish { PublishId = 1, UserId = 2, PublishDate = DateTime.Parse("7/10/2017 8:32:20")},
                new FriendPublish { PublishId = 3, UserId = 2, PublishDate = DateTime.Parse("7/11/2017 8:12:00")}
            };
            friendPublishs.ForEach(fp => context.FriendPublishs.Add(fp));
            context.SaveChanges();
        }
    }
}