using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using GoodJoker.Models;
using ImageResizer;
using Microsoft.Owin.Security;

namespace GoodJoker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministratorController : Controller
    {
        // Работа с Owin авторизацией
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        // Инициализация входа в Базу Данных
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task ArchiveLott()
        {
            var arcDate = DateTime.UtcNow.AddDays(-7);
            var archive = await db.Lotteries.Where(l => l.DateLottery <= arcDate).ToListAsync();
            if (archive.Count > 0)
            {
                var archiveList = new List<ArchiveLottery>();
                var returnMandatoriInfo = new List<ReturnMandatoryInfo>();
                var mandatoryInfo = new List<MandatoryInfo>();
                var finger = new List<Finger>();
                var link = new List<LinkLottery>();
                var comment = new List<CommentLottery>();
                var chat = new List<CommentChatLottery>();

                foreach (var arc in archive)
                {
                    var archiveWinner = new List<ArchiveWinner>();
                    foreach (var win in arc.Prizes)
                    {
                        var winner = new ArchiveWinner()
                        {
                            NoWin = win.NoWin,
                            Place = win.Place,
                            Prize = win.Name,
                            Winner = win.Winner
                        };

                        foreach (var refresh in win.RefreshWinners)
                            winner.RefreshId = (winner.RefreshId == null || winner.RefreshId == "") ? "" + refresh.DelWinner.Id : ";" + refresh.DelWinner.Id;

                        archiveWinner.Add(winner);
                    }

                    var item = new ArchiveLottery()
                    {
                        Comment = arc.Comments.Count,
                        CountParticipient = arc.Users.Count,
                        CreateLottery = arc.DateCreate,
                        HoldingLottery = arc.DateLottery,
                        OddsFingers = arc.OddsFingers,
                        Studio = arc.Studio,
                        Title = arc.Title,
                        View = arc.View,
                        Winner = archiveWinner
                    };

                    archiveList.Add(item);

                    if (System.IO.File.Exists(Server.MapPath($"~/Content/style/images/Lotteries/Normal/{arc.Cover}.jpg")))
                        System.IO.File.Delete(Server.MapPath($"~/Content/style/images/Lotteries/Normal/{arc.Cover}.jpg"));
                    if (System.IO.File.Exists(Server.MapPath($"~/Content/style/images/Lotteries/Reduced/{arc.Cover}.jpg")))
                        System.IO.File.Delete(Server.MapPath($"~/Content/style/images/Lotteries/Reduced/{arc.Cover}.jpg"));

                    returnMandatoriInfo.AddRange(await db.ReturnMandatoryInfo.Where(r => r.Info.Lottery.Id == arc.Id).ToListAsync());
                    mandatoryInfo.AddRange(arc.MandatoryInfo);
                    finger.AddRange(arc.Fingers);
                    link.AddRange(arc.Link);
                    comment.AddRange(arc.Comments);
                    chat.AddRange(arc.Chat);
                }

                db.ReturnMandatoryInfo.RemoveRange(returnMandatoriInfo);
                db.MandatoryInfo.RemoveRange(mandatoryInfo);
                db.Fingers.RemoveRange(finger);
                db.LinkLotteries.RemoveRange(link);
                db.CommentsLottery.RemoveRange(comment);
                db.ChatLottery.RemoveRange(chat);
                db.Lotteries.RemoveRange(archive);
                db.ArchiveLotteries.AddRange(archiveList);
                await db.SaveChangesAsync();

            }
        }

        [HttpGet]
        public ActionResult Users()
        {
            return View(db.Users.ToList());
        }

        [HttpPost]
        public async Task Ban(string Type, int Id, string Message)
        {
            if (Id > 0)
            {
                var user = await db.Users.FindAsync(Id);
                if (user != null)
                {
                    if (Type == "ban")
                    {
                        user.Option.Ban = (user.Option.Ban) ? false : true;
                        user.Option.СauseBan = Message;
                    }
                    else
                    {
                        ++user.Option.Warnings;
                        if (user.Option.Warnings < 3)
                        {
                            user.Notices.Add(new Notice()
                            {
                                Text = Message,
                                Title = "ПРЕДУПРЕЖДЕНИЕ",
                                Type = Type
                            });
                        }
                        else
                        {
                            user.Option.Ban = true;
                            user.Option.СauseBan = Message;
                        }
                    }
                    await db.SaveChangesAsync();
                }
            }
        }

        [HttpGet]
        public ActionResult SignIn(int Id)
        {
            var user = db.Users.Find(Id);
            if (user != null)
            {
                ClaimsIdentity claim = new ClaimsIdentity("ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String));
                claim.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Nick, ClaimValueTypes.String));
                claim.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "OWIN Provider", ClaimValueTypes.String));
                claim.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name, ClaimValueTypes.String));

                AuthenticationManager.SignOut();

                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                AuthenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = true
                }, claim);

                return RedirectToAction("Index", "PrivateOffice");
            }
            return RedirectToAction("Users", "Administrator");
        }

        [HttpGet]
        public ActionResult News()
        {
            return View(db.News.ToList());
        }

        [HttpGet]
        public ActionResult NewsCreate(int? id)
        {
            ViewBag.Tags = db.Tags.ToList();
            return View(db.News.Find(id));
        }

        [HttpPost]
        public async Task<string> NewsCreate([Bind(Include = "Title")] News News)
        {
            News.DateCreate = DateTime.Now;
            News.AuthorName = "GoodJoker";
            db.News.Add(News);
            await db.SaveChangesAsync();
            var last = db.News.ToList().Last();
            return last.Id.ToString();
        }

        [HttpPost]
        public async Task<string> NewsEdit(NewsEdit NewsEdit)
        {
            if (NewsEdit.Id != 0 && NewsEdit.Title != null && NewsEdit.Title != "" && NewsEdit.ShortDescription != null && NewsEdit.Content != null && NewsEdit.Publish != 0)
            {
                var news = await db.News.FindAsync(NewsEdit.Id);
                news.Title = NewsEdit.Title;
                news.ShortDescription = NewsEdit.ShortDescription;
                news.Publish = (NewsEdit.Publish == 1) ? true : false;

                if (news.Content.Count == 0)
                    news.Content.Add(new ContentNews()
                    {
                        Place = 1,
                        Text = NewsEdit.Content,
                        Type = "Main",
                        Images = new List<ImageNews>()
                    });
                else
                    news.Content.First().Text = NewsEdit.Content;

                if (NewsEdit.Tags != null && NewsEdit.Tags != "")
                {
                    foreach (var id in NewsEdit.Tags.Split(';'))
                    {
                        var Id = int.Parse(id);
                        if (news.Tags.FirstOrDefault(t => t.Id == Id) == null)
                            news.Tags.Add(await db.Tags.FindAsync(Id));
                    }
                }
                
                await db.SaveChangesAsync();

                return "1";
            }
            return "0";
        }

        //[HttpPost]
        //public async Task<ActionResult> NewsDel(NewsEdit NewsEdit)
        //{
        //    return new HttpStatusCodeResult(500);
        //}

        public async Task<string> AddTag(int Id, string Name)
        {
            if (Name != null && Name != "")
            {
                var tag = new Tag();
                if (Id > 0)
                {
                    tag = await db.Tags.FindAsync(Id);
                    if (tag != null)
                        tag.Name = Name;
                }
                else
                {
                    tag.Name = Name;
                    db.Tags.Add(tag);
                }
                await db.SaveChangesAsync();
                return $"{tag.Id}";
            }
            return "0";
        }

        [HttpPost]
        public async Task<string> DelTag(int Id)
        {
            if (Id > 0)
            {
                var tag = await db.Tags.FindAsync(Id);
                db.Tags.Remove(tag);
                await db.SaveChangesAsync();
                return "1";
            }
            return "0";
        }

        public async Task<string> LoadImg(HttpPostedFileBase img, string type, int id)
        {
            if (img != null && type != null && id != 0)
            {
                var news = await db.News.FindAsync(id);
                string imgName = img.FileName,
                       pathSave = ""; 
                int maxW = 0, maxH = 0;

                if (type == "Cover")
                {
                    pathSave = "~/Content/style/images/News/Cover/Normal/";
                    maxW = 1900;
                    maxH = 1200;
                }
                else if (type == "Big")
                {
                    pathSave = "~/Content/style/images/News/";
                    maxW = 1200;
                    maxH = 800;
                }
                else
                {
                    pathSave = "~/Content/style/images/News/";
                    maxW = 700;
                    maxH = 400;
                }

                // проверяем существование имён, при совпадении, изменяем их
                imgName = PathImg.Inspection(imgName, Server.MapPath(pathSave));

                WebImage imgSave = new WebImage(img.InputStream);
                imgSave.Resize(maxW, maxH, true, true);
                imgSave.Save($"{pathSave}{imgName}", "JPG", true);

                if (type == "Cover")
                {
                    pathSave = "~/Content/style/images/News/Cover/Reduced/";
                    maxW = 1000;
                    maxH = 600;
                    
                    imgSave.Resize(maxW, maxH, true, true);
                    imgSave.Save($"{pathSave}{imgName}", "JPG", true);
                    news.Cover = imgName;
                }
                else
                {
                    var cont = (news.Content.Count > 0) ? news.Content.First() : null;
                    if (cont == null)
                        cont = new ContentNews()
                        {
                            Place = 1,
                            Text = null,
                            Type = "Main",
                            Images = new List<ImageNews>()
                        };

                    cont.Images.Add(new ImageNews() { Name = imgName, Type = type,  });
                    if (cont.Text == null)
                        news.Content.Add(cont);
                }

                await db.SaveChangesAsync();
                if (type == "Cover")
                    return imgName;
                else
                    return $"{imgName};{news.Content.First().Images.FirstOrDefault(i => i.Name == imgName).Id}";
            }
            return "0";
        }

        public async Task DelImg(int Id)
        {
            if (Id > 0)
            {
                var img = await db.ImagesNews.FindAsync(Id);

                if (System.IO.File.Exists(Server.MapPath($"~/Content/style/images/News/{img.Name}")))
                    System.IO.File.Delete(Server.MapPath($"~/Content/style/images/News/{img.Name}"));

                db.ImagesNews.Remove(img);
                await db.SaveChangesAsync();
            }
        }

        public ActionResult Lotteries(bool? Everyday)
        {
            var lotts = new AdminLottery
            {
                Everyday = (Everyday != null && Everyday == true) ? db.Everyday.ToList() : null,
                Lotteries = (Everyday == null) ? db.Lotteries.OrderByDescending(l => l.DateLottery).ToList() : null
            };
            return View(lotts);
        }

        [HttpPost]
        public async Task<string> DelLottery(int Id)
        {
            if (Id > 0)
            {
                var lott = await db.Lotteries.FindAsync(Id);
                var returnInfo = await db.ReturnMandatoryInfo.Where(r => r.Info.Lottery.Id == Id).ToListAsync();
                var refresh = await db.RefreshWinners.Where(r => r.Prize.Lottery.Id == Id).ToListAsync();

                if (returnInfo.Count > 0)
                    db.ReturnMandatoryInfo.RemoveRange(returnInfo);
                if (lott.MandatoryInfo.Count > 0)
                    db.MandatoryInfo.RemoveRange(lott.MandatoryInfo);
                if (lott.Fingers.Count > 0)
                    db.Fingers.RemoveRange(lott.Fingers);
                if (lott.Link.Count > 0)
                    db.LinkLotteries.RemoveRange(lott.Link);
                if (lott.Comments.Count > 0)
                    db.CommentsLottery.RemoveRange(lott.Comments);
                if (lott.Chat.Count > 0)
                    db.ChatLottery.RemoveRange(lott.Chat);
                if (refresh.Count > 0)
                    db.RefreshWinners.RemoveRange(refresh);
                if (lott.Prizes.Count > 0)
                    db.Prizes.RemoveRange(lott.Prizes);

                if (System.IO.File.Exists(Server.MapPath($"~/Content/style/images/Lotteries/Normal/{lott.Cover}.jpg")))
                    System.IO.File.Delete(Server.MapPath($"~/Content/style/images/Lotteries/Normal/{lott.Cover}.jpg"));
                if (System.IO.File.Exists(Server.MapPath($"~/Content/style/images/Lotteries/Reduced/{lott.Cover}.jpg")))
                    System.IO.File.Delete(Server.MapPath($"~/Content/style/images/Lotteries/Reduced/{lott.Cover}.jpg"));

                db.Lotteries.Remove(lott);
                await db.SaveChangesAsync();

                return "1";
            }
            return "0";
        }

        [HttpGet]
        public ActionResult EverydayCreate(int? id)
        {
            if (id <= 0)
                return HttpNotFound();

            var lott = new EverydayLott() { Holding = DateTime.Now };

            if (id != null)
            {
                lott = db.Everyday.Find(id);
                if (lott == null)
                    return HttpNotFound();
            }

            return View(lott);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EverydayCreate(HttpPostedFileBase Cover, EverydayLott Everyday)
        {
            if (Everyday.Holding == null || Everyday.FromWhom == null || Everyday.Prize == null || (Everyday.Id == 0 && Cover == null))
                return View(Everyday);

            if (Everyday.Id != 0)
            {
                var lott = await db.Everyday.FindAsync(Everyday.Id);
                if (lott == null)
                    return View(Everyday);

                Everyday.Id = lott.Id;
                lott.Description = Everyday.Description;
                lott.FromWhom = Everyday.FromWhom;
                lott.Prize = Everyday.Prize;
                lott.Publish = Everyday.Publish;

                var prize = lott.Winners.FirstOrDefault(w => w.HoldingLott == lott.Holding && w.Winner == null);
                if (prize != null)
                    db.WinnersEveryday.Remove(prize);

                lott.Holding = Everyday.Holding.ToUniversalTime();

                if (lott.Publish)
                {
                    prize = lott.Winners.FirstOrDefault(w => w.HoldingLott == Everyday.Holding && w.Winner == null);
                    if (prize == null)
                        lott.Winners.Add(new WinnersEverydayLott() {
                            HoldingLott = lott.Holding,
                            Winner = null,
                            Prize = lott.Prize
                        });
                }

            }
            else
            {
                Everyday.Holding = Everyday.Holding.ToUniversalTime();
                Everyday.Winners.Add(new WinnersEverydayLott()
                {
                    HoldingLott = Everyday.Holding,
                    Winner = null,
                    Prize = Everyday.Prize
                });
                db.Everyday.Add(Everyday);
            }

            if (Cover != null)
            {
                var imgName = PathImg.Inspection(Cover.FileName, "~/Content/style/images/Everyday/Normal/");
                WebImage imgSave = new WebImage(Cover.InputStream);
                imgSave.Resize(1900, 1200, true, true);
                imgSave.Save($"~/Content/style/images/Everyday/Normal/{imgName}", "JPG", true);
                imgSave.Resize(1000, 600, true, true);
                imgSave.Save($"~/Content/style/images/Everyday/Reduced/{imgName}", "JPG", true);
                
                Everyday.Cover = imgName;
            }

            await db.SaveChangesAsync();

            return RedirectToAction("Lotteries", new { Everyday = "true" });
        }

        [HttpPost]
        public async Task<byte> DelEveryday(int Id)
        {
            if (Id > 0)
            {
                var lott = await db.Everyday.FindAsync(Id);

                if (System.IO.File.Exists(Server.MapPath($"~/Content/style/images/Everyday/{lott.Cover}.jpg")))
                    System.IO.File.Delete(Server.MapPath($"~/Content/style/images/Everyday/{lott.Cover}.jpg"));

                var archive = new List<ArchiveWinnerEveryday>();
                foreach (var winner in lott.Winners)
                {
                    archive.Add(new ArchiveWinnerEveryday()
                    {
                        FromWhom = winner.Lottery.FromWhom,
                        Prize = winner.Prize,
                        Date = winner.Lottery.Holding,
                        Winner = winner.Winner
                    });
                }

                db.WinnersEveryday.RemoveRange(lott.Winners);
                db.Everyday.Remove(lott);
                db.ArchiveWinnersEveryday.AddRange(archive);

                await db.SaveChangesAsync();
                return 1;
            }
            return 0;
        }

        public ActionResult Adverts()
        {
            var list = db.Adverts.ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult AddAdvert(int? Id)
        {
            var advert = new Advert();
            if (Id != null && Id > 0)
            {
                advert = db.Adverts.Find(Id);
                if (advert == null)
                    return HttpNotFound();
            }

            ViewBag.Places = db.PlacesAdverts.ToList();
            return View(advert);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddAdvert(HttpPostedFileBase Ava, Advert NewAdvert, string CitiesId, string RegionsId, string PlacesId)
        {
            var advert = NewAdvert;
            var citId = CitiesId.Split(';');
            var regId = RegionsId.Split(';');
            var plcId = PlacesId.Split(';');
            var cities = new List<City>();
            var regions = new List<RegionCity>();
            var places = new List<PlaceAdvert>();
            var imgName = "";

            if (NewAdvert.Id > 0)
            {
                advert = await db.Adverts.FindAsync(NewAdvert.Id);
                advert.AdvertiserId = NewAdvert.AdvertiserId;
                advert.Advertiser = NewAdvert.Advertiser;
                advert.Cost = NewAdvert.Cost;
                advert.DateBeginAdvert = NewAdvert.DateBeginAdvert;
                advert.DateEndAdvert = NewAdvert.DateEndAdvert;
                advert.Link = NewAdvert.Link;
                advert.Title = NewAdvert.Title;
                advert.Text = NewAdvert.Text;
                advert.Video = NewAdvert.Video;
            }

            if (citId[0] != "")
                foreach (var id in citId)
                    cities.Add(await db.Cities.FindAsync(Convert.ToInt32(id)));

            if (regId[0] != "")
                foreach (var id in regId)
                    regions.Add(await db.Regions.FindAsync(Convert.ToInt32(id)));

            if (plcId[0] != "")
                foreach (var id in plcId)
                    places.Add(await db.PlacesAdverts.FindAsync(Convert.ToInt32(id)));

            advert.Cities = cities;
            advert.Regions = regions;
            advert.Place = places;

            if (NewAdvert.Id == 0)
                db.Adverts.Add(advert);

            if (Ava != null)
            {
                imgName = PathImg.Inspection(Ava.FileName, "~/Content/style/images/Advert/Normal/");
                advert.Ava = imgName;
            }

            await db.SaveChangesAsync();

            if (imgName != "")
            {
                WebImage cover = new WebImage(Ava.InputStream);
                cover.Resize(1100, 600, true, true);
                cover.Save($"~/Content/style/images/Advert/Normal/{imgName}", "JPG", true);
                cover.Resize(500, 250, true, true);
                cover.Save($"~/Content/style/images/Advert/Reduced/{imgName}", "JPG", true);
            }

            return RedirectToAction("Adverts");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task DelAdvert(int Id)
        {
            if (Id > 0)
            {
                var adv = await db.Adverts.FindAsync(Id);
                if (adv != null)
                {
                    if (System.IO.File.Exists(Server.MapPath($"~/Content/style/images/Advert/Normal/{adv.Ava}")))
                        System.IO.File.Delete(Server.MapPath($"~/Content/style/images/Advert/Normal/{adv.Ava}"));
                    if (System.IO.File.Exists(Server.MapPath($"~/Content/style/images/Advert/Reduced/{adv.Ava}")))
                        System.IO.File.Delete(Server.MapPath($"~/Content/style/images/Advert/Reduced/{adv.Ava}"));
                    db.Adverts.Remove(adv);
                    await db.SaveChangesAsync();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<int> PlaceAdvert(int Id, string Name)
        {
            if (Id > 0)
            {
                var edit = await db.PlacesAdverts.FindAsync(Id);
                if (Name != null && Name != "")
                    edit.Name = Name;
                else
                    db.PlacesAdverts.Remove(edit);             
                await db.SaveChangesAsync();
                return 1;
            }
            else
            {
                if (Name != null && Name != "")
                {
                    var newPlace = new PlaceAdvert() { Name = Name };
                    db.PlacesAdverts.Add(newPlace);
                    await db.SaveChangesAsync();
                    return newPlace.Id;
                }
                else
                    return 0;
            }
        }

        public ActionResult Videos()
        {
            return View(db.Videos.ToList());
        }

        [HttpGet]
        public ActionResult AddVideo(int? Id)
        {
            var video = new Video();
            if (Id != null && Id > 0)
            {
                video = db.Videos.Find(Id);
                if (video == null)
                    return HttpNotFound();
            }

            ViewBag.Types = db.TypesVideos.ToList();
            return View(video);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddVideo(Video Video, int StudioId, string TypesId)
        {
            if (Video.VideoId != null && Video.VideoId != "")
            {
                var video = Video;
                var typeId = TypesId.Split(';');
                var newType = new List<TypeVideo>();
                var yuid = YouTube.ParseId(video.VideoId);

                if (Video.Id > 0)
                {
                    video = await db.Videos.FindAsync(Video.Id);
                    if (video == null)
                        Response.End();
                }

                if (yuid != null && yuid != "")
                    video.VideoId = yuid;
                else
                    Response.End();

                if (StudioId > 0)
                    video.Studio = await db.Studios.FindAsync(StudioId);

                if (typeId[0] != "")
                    foreach (var id in typeId)
                        newType.Add(await db.TypesVideos.FindAsync(Convert.ToInt32(id)));

                video.Type = newType;

                if (Video.Id == 0)
                    db.Videos.Add(video);

                await db.SaveChangesAsync();

                return RedirectToAction("Videos");
            }

            return View(new Video());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<int> TypeVideo(int Id, string Name)
        {
            if (Id > 0)
            {
                var edit = await db.TypesVideos.FindAsync(Id);
                if (Name != null && Name != "")
                    edit.Name = Name;
                else
                    db.TypesVideos.Remove(edit);
                await db.SaveChangesAsync();
                return 1;
            }
            else
            {
                if (Name != null && Name != "")
                {
                    var newPlace = new TypeVideo() { Name = Name };
                    db.TypesVideos.Add(newPlace);
                    await db.SaveChangesAsync();
                    return newPlace.Id;
                }
                else
                    return 0;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> UserSearch(string Search)
        {
            string list = "";
            if (Search != null && Search != "")
            {
                var users = await db.Users.Where(u => u.Nick.Contains(Search) || u.Id.ToString().Contains(Search)).Take(6).ToListAsync();
                if (users.Count > 0)
                {
                    foreach (var user in users)
                        list += $"<div class='item' id='{user.Id}'>{user.Nick}</div>";
                    return list;
                }
            }
            return "<div class='item' id='0'>Пользователь не найден!</div>";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}