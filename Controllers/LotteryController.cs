using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using GoodJoker.Models;
using ImageResizer;
using Microsoft.Owin.Security;

namespace GoodJoker.Controllers
{
    public class LotteryController : Controller
    {
        // Работа с Owin авторизацией
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        // Инициализация входа в Базу Данных
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // How Match to Show
        const int HMS = 10;
        const int HMSC = 20; // Comment

        // GET: Lottery
        public ActionResult Index()
        {
            var lotts = db.Lotteries.ToList();
            var All = new AllLotteries
            {
                EndLott = lotts.Where(l => l.Prizes.Where(p => !p.Confirm).Count() == 0).OrderByDescending(l => l.DateLottery).ToList(),
                Everyday = db.Everyday.Where(l => l.Publish).OrderByDescending(l => l.Winners.Count).ToList(),
                GJLott = lotts.Where(l => l.Studio.Id == 1 && l.Prizes.Where(p => !p.Confirm).Count() > 0).Take(HMS).ToList(),
                OtherLott = lotts.Where(l => l.Studio.Id != 1 && l.Prizes.Where(p => !p.Confirm).Count() > 0).Take(HMS*2).ToList(),
                AllCountGJ = lotts.Where(l => l.Studio.Id == 1 && l.Prizes.Where(p => !p.Confirm).Count() > 0).Count(),
                AllCountUser = lotts.Where(l => l.Studio.Id != 1 && l.Prizes.Where(p => !p.Confirm).Count() > 0).Count(),
                HMS = HMS
            };
            return View(All);
        }

        public async Task<ActionResult> Lazyload(int ViewCount, bool AjaxGJ)
        {
            if (ViewCount <= 0)
                return new HttpStatusCodeResult(500);

            var lotts = (AjaxGJ) ?
                    await db.Lotteries.Where(l => l.Studio.Id == 1 && l.Prizes.Where(p => !p.Confirm).Count() > 0).ToListAsync():
                    await db.Lotteries.Where(l => l.Studio.Id != 1 && l.Prizes.Where(p => !p.Confirm).Count() > 0).ToListAsync();

            if (lotts.Count <= ViewCount)
                return Content("0");

            var localHMS = (AjaxGJ) ? HMS : HMS * 2;

            if (lotts.Count - ViewCount > localHMS)
                lotts = lotts.GetRange(ViewCount, localHMS);
            else if (lotts.Count > ViewCount)
                lotts.RemoveRange(0, ViewCount);
            else
                return new HttpStatusCodeResult(500);

            if (AjaxGJ)
                return PartialView("_LottGJ", lotts);
            else
                return PartialView("_LottUser", lotts);
        }

        // КОНТРОЛЛЕР ПРЕДСТАВЛЕНИЯ ЕЖЕДНЕВНОГО РОЗЫГРЫША
        // ----------------------------------------------------------------------
        public ActionResult Everyday(int? id)
        {
            // 404
            if (id == null || id <= 0)
                return HttpNotFound();

            // находим розыгрыш
            var lott = db.Everyday.Find(id);

            // 404
            if (lott == null)
                return HttpNotFound();
            
            // логика
            LogicsEveryday(lott, lott.Winners.Last());

            return View(lott);
        }

        // ПОЛУЧЕНИЕ ПОБЕДИТЕЛЯ ЕЖЕДНЕВНОГО РОЗЫГРЫША
        // ----------------------------------------------------------------------
        [HttpPost]
        public async Task<string> GetWinnerEveryday(int LottId)
        {
            // Находим розыгрыш
            var lott = await db.Everyday.FindAsync(LottId);
            // Не разыгранное место
            var place = lott.Winners.Last();
            // Логика
            var logics = LogicsEveryday(lott, place);

            // Если победитель есть, то...
            if (logics && place.Winner != null)
                // Отправляем данные для парса ID,Nick,Avatar
                return $"{place.Winner.Id};{place.Winner.Nick};{place.Winner.Option.Ava}";

            // default
            return "0";
        }

        // УЧАСТИЕ В РОЗЫГРШЕ
        // -------------------------------------------------------
        [Authorize]
        [HttpPost]
        public async Task<string> ParticipateEveryday(int Id)
        {
            if (Id > 0)
            {
                var lott = await db.Everyday.FindAsync(Id);
                if (lott != null)
                {
                    var id = User.Identity.GetUserId<int>();
                    var user = await db.Users.FindAsync(id);
                    if (lott.Participants.FirstOrDefault(u => u.Id == id) == null)
                        lott.Participants.Add(user);
                    else
                        lott.Participants.Remove(user);

                    var rnd = new Random();
                    var users = lott.Participants.ToList();
                    var winner = users[rnd.Next(users.Count)];

                    lott.Winners.Last().Winner = winner;

                    await db.SaveChangesAsync();

                    return "1";
                }
            }
            return "0";
        }

        // ЛАЙК ЕЖЕДНЕВНОГО РОЗЫГРЫША
        // ----------------------------------------------------------
        [Authorize]
        [HttpPost]
        public async Task<string> EverydayLike(int LotteryId)
        {
            if (LotteryId > 0)
            {
                var id = User.Identity.GetUserId<int>();
                var user = await db.Users.FindAsync(id);
                var lottery = await db.Everyday.FindAsync(LotteryId);
                if (lottery != null)
                {
                    if (lottery.Likes.FirstOrDefault(l => l.Id == id) == null)
                        lottery.Likes.Add(user);
                    else
                        lottery.Likes.Remove(user);

                    await db.SaveChangesAsync();
                    return "1";
                }
            }
            return "0";
        }

        // ПОДРОБНОЕ ОПИСАНИЕ РОЗЫГРЫША
        // -----------------------------------------
        public ActionResult Details(int? id)
        {
            if (id == null || id <= 0)
                return HttpNotFound();

            Lottery lott = db.Lotteries.Find(id);
            if (lott == null)
                return HttpNotFound();

            TimeSpan ts = DateTime.UtcNow - lott.DateLottery.AddDays(1);
            if (lott.DateLottery < DateTime.UtcNow && lott.Auto && !lott.BeginLott)
            {
                lott.BeginLott = true;
                foreach (var pr in lott.Prizes)
                    pr.Confirm = true;
                db.SaveChanges();
            }
            else if (ts.Days >= 1 && lott.Prizes.Where(p => !p.Confirm && !p.NoWin).Count() > 0)
            {
                var penaltyPoints = 0;

                foreach (var def in lott.Prizes.Where(p => !p.Confirm && !p.NoWin))
                {
                    def.Confirm = true;
                    def.NoWin = true;
                    ++penaltyPoints;
                }

                // Если есть не разыгранные призы, то статистика студии понижается исходя из формулы
                if (penaltyPoints > 0)
                    // Статистика = Статистика - (Участники / Призы) * Штрафные баллы
                    lott.Studio.Option.Statistics -= (lott.Users.Count / lott.Prizes.Count) * penaltyPoints;

                db.SaveChanges();
            }

            var Id = User.Identity.GetUserId<int>();
            ViewBag.user = db.Users.FirstOrDefault(u => u.Id == Id);
            ViewBag.allComments = lott.Comments.ToList();
            ViewBag.last = (lott.Comments.Count <= HMSC) ? '1' : '0';

            // Просмотр
            HttpCookie cookie = Request.Cookies["cookieWork"];
            if (cookie.Value == "1")
            {
                cookie = Request.Cookies["ViewLottery"];
                
                if (cookie != null)
                {
                    if (cookie["Lot" + Id] != "1")
                    {
                        cookie["Lot" + Id] = "1";
                        ++lott.View;
                    }
                }
                else
                {
                    cookie = new HttpCookie("ViewLottery");
                    cookie["Lot" + Id] = "1";
                    ++lott.View;
                }

                cookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(cookie);

                db.SaveChanges();
            }

            lott.Comments = (lott.Comments.Count > HMSC)
                ? lott.Comments.OrderByDescending(c => c.DateCreate).Take(HMSC).ToList()
                : lott.Comments.OrderByDescending(c => c.DateCreate).ToList();
            return View(lott);
        }

        // ДОБАВЛЕНИЕ УЧАСТНИКА РОЗЫГРЫША
        // -----------------------------------------------------------------------------------
        [Authorize]
        [HttpPost]
        public async Task<string> AddParticipate(int LotteryId, List<AddParticipate> Info)
        {
            if (LotteryId > 0)
            {
                var lottery = await db.Lotteries.FindAsync(LotteryId);
                
                var id = User.Identity.GetUserId<int>();
                var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (lottery.Users.FirstOrDefault(u => u.Id == id) == null)
                {
                    // Проверяем возраст
                    if ((lottery.BeginAgeLimit > 0 || lottery.EndAgeLimit > 0) && user != null && user.Option.Birthday.Hour == 14)
                    {
                        TimeSpan span = DateTime.UtcNow.Subtract(user.Option.Birthday);
                        var age = (int)span.TotalDays / 360;
                        if ((lottery.BeginAgeLimit > 0 && age < lottery.BeginAgeLimit) || (lottery.EndAgeLimit > 0 && age > lottery.EndAgeLimit))
                            return "4";
                    }

                    // Проверям нахождение пользователя в нужном регионе
                    if ((lottery.Cities.Count > 0 || lottery.Regions.Count > 0) && (user.Option.CityID > 1 || user.Option.BigCityID > 1))
                    {
                        var exist = false;

                        if (lottery.Cities.Count > 0 && lottery.Cities.FirstOrDefault(c => c.Id == user.Option.CityID || c.Id == user.Option.BigCityID) != null)
                            exist = true;

                        if (lottery.Regions.Count > 0 && lottery.Regions.FirstOrDefault(r => r.Id == user.Option.City.Region.Id || r.Id == user.Option.BigCity.Region.Id) != null)
                            exist = true;

                        if (!exist)
                            return "5";
                    }

                    // Проверяем пол
                    if (lottery.GenderLimit > 0 && user.Option.Gender > 0 && user.Option.Gender != lottery.GenderLimit)
                    { return "6"; }

                    if (lottery.MandatoryInfo.Where(l => l.Type != "info").Count() > 0)
                    {
                        foreach (var info in lottery.MandatoryInfo.Where(l => l.Type != "info"))
                        {
                            switch (info.Type)
                            {
                                case "vkontakte":
                                    var social = user.SocialNetworks.FirstOrDefault(u => u.Name == "VK" && u.UserId == id);
                                    if (social == null)
                                        return "2";
                                    info.Return.Add(new ReturnMandatoryInfo()
                                    {
                                        Text = "https://vk.com/id" + social.SocialId.Substring(0, social.SocialId.Length - 2),
                                        User = user
                                    });
                                    break;
                                case "facebook":
                                    social = user.SocialNetworks.FirstOrDefault(u => u.Name == "FB" && u.UserId == id);
                                    if (social == null)
                                        return "2";
                                    info.Return.Add(new ReturnMandatoryInfo()
                                    {
                                        Text = "https://www.facebook.com/app_scoped_user_id/" + social.SocialId.Substring(0, social.SocialId.Length - 2) + "/",
                                        User = user
                                    });
                                    break;
                                case "odnoklassniki":
                                    social = user.SocialNetworks.FirstOrDefault(u => u.Name == "OK" && u.UserId == id);
                                    if (social == null)
                                        return "2";
                                    info.Return.Add(new ReturnMandatoryInfo()
                                    {
                                        Text = "https://ok.ru/profile/" + social.SocialId.Substring(0, social.SocialId.Length - 2),
                                        User = user
                                    });
                                    break;
                                case "google":
                                    social = user.SocialNetworks.FirstOrDefault(u => u.Name == "GL" && u.UserId == id);
                                    if (social == null)
                                        return "2";
                                    info.Return.Add(new ReturnMandatoryInfo()
                                    {
                                        Text = "https://plus.google.com/u/0/" + social.SocialId.Substring(0, social.SocialId.Length - 2),
                                        User = user
                                    });
                                    break;
                                case "email":
                                    if (user.Email == null || user.Email == "")
                                        return "2";
                                    info.Return.Add(new ReturnMandatoryInfo()
                                    {
                                        Text = user.Email,
                                        User = user
                                    });
                                    break;
                                case "phone":
                                    if (user.Option.Phone == null || user.Option.Phone == "")
                                        return "2";
                                    info.Return.Add(new ReturnMandatoryInfo()
                                    {
                                        Text = user.Option.Phone,
                                        User = user
                                    });
                                    break;
                                case "name":
                                    if (user.Option.FirstName == null || user.Option.FirstName == "" ||
                                        user.Option.MiddleName == null || user.Option.MiddleName == "" ||
                                        user.Option.LastName == null || user.Option.LastName == "")
                                        return "2";
                                    info.Return.Add(new ReturnMandatoryInfo()
                                    {
                                        Text = user.Option.FirstName + " " + user.Option.MiddleName + " " + user.Option.LastName,
                                        User = user
                                    });
                                    break;
                                case "address":
                                    if (user.Option.ExactAddress == null && user.Option.ExactAddress == "")
                                        return "2";
                                    info.Return.Add(new ReturnMandatoryInfo()
                                    {
                                        Text = user.Option.ExactAddress,
                                        User = user
                                    });
                                    break;
                            }
                        }
                    }

                    if (lottery.MandatoryInfo.Where(l => l.Type == "info").Count() > 0)
                    {
                        if (Info != null && Info.Count == lottery.MandatoryInfo.Where(l => l.Type == "info").Count())
                        {
                            foreach (var info in Info)
                            {
                                var manInfo = lottery.MandatoryInfo.FirstOrDefault(m => m.Id == info.InfoId);
                                if (info.Text != "" && manInfo != null)
                                    manInfo.Return.Add(new ReturnMandatoryInfo()
                                    {
                                        Text = info.Text,
                                        User = user
                                    });
                                else
                                    return "3";
                            }
                        }
                        else
                            return "3";
                    }

                    ++user.Option.CountShareLottery;
                    lottery.Users.Add(user);

                    if (lottery.Auto)
                        await AutoSelect(lottery, "add");

                    await db.SaveChangesAsync();
                    return "1";
                }
            }
            return "0";
        }

        // УДАЛЕНИЕ УЧАСТНИКА
        // ---------------------------------------------------
        [Authorize]
        [HttpPost]
        public async Task DelParticipate(int LotteryId)
        {
            var id = User.Identity.GetUserId<int>();
            var user = await db.Users.FindAsync(id);
            if (LotteryId > 0)
            {
                var lottery = await db.Lotteries.FindAsync(LotteryId);
                if (lottery.Users.FirstOrDefault(u => u.Id == id) != null)
                {
                    var returnInfo = user.ReturnMandatoryInfo.Where(i => i.Info.Lottery == lottery).ToList();
                    if (returnInfo.Count > 0)
                        db.ReturnMandatoryInfo.RemoveRange(returnInfo);
                    --user.Option.CountShareLottery;
                    lottery.Users.Remove(user);

                    if (lottery.Auto)
                        await AutoSelect(lottery, "add");

                    await db.SaveChangesAsync();
                }
            }
        }

        // ЛАЙК И ДИЗЛАЙК РОЗЫГРЫША
        // -------------------------------------------------------------
        [Authorize]
        [HttpPost]
        public string FingerLottery(int LotteryId, bool UpOrDown)
        {
            var returnString = "0";
            if (LotteryId > 0)
            {
                var id = User.Identity.GetUserId<int>();
                var user = db.Users.Find(id);
                var lottery = db.Lotteries.Find(LotteryId);
                if (lottery != null)
                {
                    var finger = lottery.Fingers.FirstOrDefault(f => f.User.Id == id && f.UpOrDown == UpOrDown);
                    var notFinger = lottery.Fingers.FirstOrDefault(f => f.User.Id == id && f.UpOrDown == !UpOrDown);

                    if (finger == null)
                    {
                        lottery.Fingers.Add(new Finger
                        {
                            UpOrDown = UpOrDown,
                            User = user
                        });

                        if (UpOrDown)
                        {
                            ++lottery.OddsFingers;
                            ++lottery.Studio.Option.Statistics;
                        }
                        else
                        {
                            --lottery.OddsFingers;
                            --lottery.Studio.Option.Statistics;
                        }

                        if (notFinger != null)
                        {
                            db.Fingers.Remove(notFinger);
                            if (UpOrDown)
                            {
                                ++lottery.OddsFingers;
                                ++lottery.Studio.Option.Statistics;
                            }
                            else
                            {
                                --lottery.OddsFingers;
                                --lottery.Studio.Option.Statistics;
                            }
                        }

                        returnString = "add";
                    }
                    else
                    {
                        db.Fingers.Remove(finger);
                        if (UpOrDown)
                        {
                            --lottery.OddsFingers;
                            --lottery.Studio.Option.Statistics;
                        }
                        else
                        {
                            ++lottery.OddsFingers;
                            ++lottery.Studio.Option.Statistics;
                        }
                        returnString = "del";
                    }

                    db.SaveChanges();
                }
            }
            return returnString;
        }

        // ИЗМЕНЕНИЕ КАРТИНКИ РОЗЫГРЫША
        // -------------------------------------------------------------------------
        [Authorize]
        [HttpPost]
        public async Task<string> EditCover(HttpPostedFileBase Cover, int Id)
        {
            if (Cover != null)
            {
                var imgSize = Cover.ContentLength;
                var imgType = Cover.ContentType;

                if (imgSize < 10000000 && imgType.IndexOf("image") > -1)
                {
                    var id = User.Identity.GetUserId<int>();
                    var user = await db.Users.FindAsync(id);
                    var lott = await db.Lotteries.FindAsync(Id);
                    var myRole = user.Studios.FirstOrDefault(s => s.Studio.Id == lott.Studio.Id && s.Member.Id == id);

                    if (myRole != null && myRole.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin" || r.Name == "EditInfo") != null)
                    {
                        var imgName = PathImg.Inspection(Cover.FileName, "~/Content/style/images/Lotteries/Normal/");
                        WebImage cover = new WebImage(Cover.InputStream);
                        cover.Resize(900, 500, true, true);
                        cover.Save($"~/Content/style/images/Lotteries/Normal/{imgName}", "JPG", true);
                        cover.Resize(550, 300, true, true);
                        cover.Save($"~/Content/style/images/Lotteries/Reduced/{imgName}", "JPG", true);

                        if (System.IO.File.Exists(Server.MapPath($"~/Content/style/images/Lotteries/Normal/{lott.Cover}")))
                            System.IO.File.Delete(Server.MapPath($"~/Content/style/images/Lotteries/Normal/{lott.Cover}"));
                        if (System.IO.File.Exists(Server.MapPath($"~/Content/style/images/Lotteries/Reduced/{lott.Cover}")))
                            System.IO.File.Delete(Server.MapPath($"~/Content/style/images/Lotteries/Reduced/{lott.Cover}"));

                        lott.Cover = imgName;
                        await db.SaveChangesAsync();

                        return imgName;
                    }
                }
            }

            return "0";
        }

        // ИЗМЕНЕНИЕ РОЗЫГРЫША
        // --------------------------------------------------------
        [Authorize]
        [HttpPost]
        public async Task EditLottery(UserEditLottery NewInfo)
        {
            if (NewInfo.Id > 0)
            {
                var id = User.Identity.GetUserId<int>();
                var user = await db.Users.FindAsync(id);
                var lott = await db.Lotteries.FindAsync(NewInfo.Id);
                var studio = user.Studios.FirstOrDefault(s => s.Studio.Id == lott.Studio.Id && s.Member.Id == id);

                if ((studio != null && studio.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin" || r.Name == "CreateLottery") != null) || User.Identity.GetUserRole() == "Admin")
                {
                    lott.YoutubeId = YouTube.ParseId(NewInfo.YoutubeId);
                    NewInfo.Date = NewInfo.Date.AddHours(-NewInfo.GMT);
                    if (NewInfo.Title != null && NewInfo.Title != "")
                        lott.Title = NewInfo.Title;
                    else if (NewInfo.Date > DateTime.UtcNow)
                    {
                        if (User.Identity.GetUserRole() != "Admin")
                            lott.DateLottery = new DateTime(
                                lott.DateLottery.Year, lott.DateLottery.Month, lott.DateLottery.Day,
                                NewInfo.Date.Hour, NewInfo.Date.Minute, 0);
                        else
                            lott.DateLottery = NewInfo.Date;
                    }
                    else if (NewInfo.EditCountUser && User.Identity.GetUserRole() == "Admin")
                        lott.CountUserForLottery = NewInfo.CountUser;
                    else if (NewInfo.ShortDescription != null && NewInfo.ShortDescription != "" && NewInfo.ShortDescription.Length <= 300)
                        lott.ShortDescription = NewInfo.ShortDescription;
                    else if (NewInfo.Text != null && NewInfo.Text != "")
                        lott.Text = NewInfo.Text;
                    else if (NewInfo.PrizeName != null && NewInfo.PrizePlace > 0 && lott.DateLottery > DateTime.UtcNow)
                        lott.Prizes.Add(new Prize() { Name = NewInfo.PrizeName, Place = NewInfo.PrizePlace });
                    else if (NewInfo.Link.Count > 0)
                    {
                        db.LinkLotteries.RemoveRange(lott.Link);
                        lott.Link = NewInfo.Link;
                    }
                    else if (lott.Link.Count > 0)
                        db.LinkLotteries.RemoveRange(lott.Link);

                    await db.SaveChangesAsync();
                }
            }
        }

        // ИЗМЕНЕНИЕ ОПЦИЙ ПРОВЕДЕНИЯ РОЗЫГРЫША
        // ---------------------------------------------------------------
        [Authorize]
        [HttpPost]
        public async Task<byte> EditOptionLott(int Id, string Name)
        {
            if (Id > 0)
            {
                var id = User.Identity.GetUserId<int>();
                var user = await db.Users.FindAsync(id);
                var lott = await db.Lotteries.FindAsync(Id);
                var studio = user.Studios.FirstOrDefault(s => s.Studio.Id == lott.Studio.Id && s.Member.Id == id);

                if ((studio != null && studio.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin" || r.Name == "CreateLottery") != null) || User.Identity.GetUserRole() == "Admin")
                {
                    switch (Name)
                    {
                        case "Auto":

                            lott.Auto = (lott.Auto) ? false : true;
                            var act = (lott.Auto) ? "add" : "del";
                            bool isAuto = await AutoSelect(lott, act);

                            if (!isAuto)
                                return 0;

                            break;

                        case "First":
                            lott.FromTheFirst = (lott.FromTheFirst) ? false : true;
                            if (lott.Auto)
                                await AutoSelect(lott, "add");
                            break;

                        default:
                            return 0;
                    }

                    await db.SaveChangesAsync();
                    return 1;
                }
            }
            return 0;
        }

        // ДОБАВЛЕНИЕ КОММЕНТАРИЯ
        // ---------------------------------------------------------------------------------------
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddComment(int LotteryId, int CommentId, string Text)
        {
            if (LotteryId > 0 && Text != null && Text != "")
            {
                var lottery = await db.Lotteries.FindAsync(LotteryId);
                if (lottery != null)
                {
                    var id = User.Identity.GetUserId<int>();
                    var user = await db.Users.FindAsync(id);
                    if (lottery.BanComment.FirstOrDefault(b => b.Id == user.Id) == null)
                    {
                        var comment = new CommentLottery()
                        {
                            Author = user,
                            DateCreate = DateTime.UtcNow,
                            Text = Text,
                            ReplyId = CommentId
                        };
                        ++user.Option.CountComments;

                        if (lottery.Comments.Count == 0)
                            ViewBag.last = '1';

                        ViewBag.allComments = lottery.Comments.ToList();
                        lottery.Comments.Add(comment);

                        await db.SaveChangesAsync();
                        return PartialView("_CommentLottery", new List<CommentLottery>() { comment });
                    }
                    else
                        return Content("ban");
                }
            }
            return Content("0");
        }

        // LAZYLOAD COMMENT
        // -----------------------------------------------------------------------------
        [HttpPost]
        public async Task<ActionResult> _CommentLottery(int LotteryId, int ViewCount)
        {
            if (LotteryId > 0 && ViewCount > 0)
            {
                var comments = await db.CommentsLottery.Where(c => c.Lottery.Id == LotteryId).OrderByDescending(c => c.DateCreate).ToListAsync();
                if (comments.Count > ViewCount)
                {
                    if (comments.Count - ViewCount <= HMSC)
                        ViewBag.last = '1';
                    if (comments.Count > HMSC)
                    {
                        if (comments.Count - ViewCount > HMSC)
                            comments = comments.GetRange(ViewCount, HMSC);
                        else
                            comments.RemoveRange(0, ViewCount);
                    }
                    return PartialView(comments);
                }
            }
            return Content("0");
        }

        // ИЗМЕНЕНИЕ КОММЕНТАРИЯ
        // -------------------------------------------------------------------
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<string> EditComment(int CommentId, string Text)
        {
            if (CommentId > 0 && Text != null && Text != "")
            {
                var comment = await db.CommentsLottery.FindAsync(CommentId);
                if (comment != null)
                {
                    comment.Text = Text;
                    await db.SaveChangesAsync();

                    return "1";
                }
            }
            return "0";
        }

        // УДАЛЕНИЕ КОММЕНТАРИЯ
        // -----------------------------------------------------
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<string> DelComment(int CommentId)
        {
            if (CommentId > 0)
            {
                var comment = await db.CommentsLottery.FindAsync(CommentId);
                if (comment != null)
                {
                    var replyId = comment.Id;
                    var lottCom = comment.Lottery.Comments;
                    var delId = "" + comment.Id;
                    var delCom = new List<CommentLottery>() { comment };
                    var checkCom = new List<CommentLottery>() { comment };

                    var flag = true;
                    do
                    {
                        if (lottCom.FirstOrDefault(c => c.Id == replyId) != null)
                        {
                            checkCom.Remove(checkCom.FirstOrDefault(c => c.Id == replyId));
                            var com = lottCom.Where(c => c.ReplyId == replyId).ToList();
                            checkCom.AddRange(com);
                            delCom.AddRange(com);
                        }

                        if (checkCom.Count > 0)
                        {
                            replyId = checkCom.First().Id;
                            delId += ";" + replyId;
                        }
                        else
                            flag = false;

                    } while (flag);

                    db.CommentsLottery.RemoveRange(delCom);
                    await db.SaveChangesAsync();

                    return delId;
                }
            }
            return "0";
        }

        // ПРОВЕДЕНИЕ РОЗЫГРЫША
        // ----------------------------------------
        public ActionResult Holding(int? Id)
        {
            if (Id == null || Id <= 0)
                return HttpNotFound();

            var lottery = db.Lotteries.Find(Id);
            if (lottery == null || (lottery.DateLottery > DateTime.UtcNow && User.Identity.GetUserRole() != "Admin") || lottery.Auto)
                return HttpNotFound();

            if (lottery.Chat.Count > 50)
                lottery.Chat = lottery.Chat.OrderByDescending(c => c.DateCreate).ToList().GetRange(0, 50);

            var id = User.Identity.GetUserId<int>();
            // Если пользователь авторизован и имеет роль позволяющую проводить розыгрыш
            ViewBag.Organizer = (User.Identity.IsAuthenticated && CheckRoleOrganizer(lottery, id));

            if (!lottery.BeginLott)
            {
                if (ViewBag.Organizer)
                {
                    lottery.BeginLott = true;
                    db.SaveChanges();
                }
                else
                    return HttpNotFound();
            }

            ViewBag.IsBan = (lottery.BanComment.FirstOrDefault(l => l.Id == id) != null) ? true : false;
            ViewBag.HaveInfo = (lottery.MandatoryInfo.Count > 0) ? true : false;
            return View(lottery);
        }

        // ИЗМЕНЕНИЕ ПРЯМОГО ЭФИРА
        // -------------------------------------------------------------------
        [Authorize]
        [HttpPost]
        public async Task EditLifeYoutube(int LotteryId, string YoutubeId)
        {
            if (LotteryId > 0 && YoutubeId != null && YoutubeId != "")
            {
                var id = User.Identity.GetUserId<int>();
                var lottery = await db.Lotteries.FindAsync(LotteryId);
                if (lottery != null && lottery.Studio.Team.FirstOrDefault(t => t.Member.Id == id && t.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin" || r.Name == "CreateLottery") != null) != null)
                {
                    var yuid = YouTube.ParseId(YoutubeId);
                    lottery.LifeYoutubeId = (yuid != null) ? yuid : "";
                    await db.SaveChangesAsync();
                }
            }
        }
        
        // СЛУШАТЕЛЬ НАЧАЛА РОЗЫГРЫША
        // ---------------------------------------------------------
        [HttpPost]
        public async Task<string> CheckBeginLott(int LotteryId)
        {
            if (LotteryId > 0)
            {
                var lottery = await db.Lotteries.FindAsync(LotteryId);
                if (lottery != null)
                {
                    if (lottery.Auto)
                    {
                        var edit = false;
                        if (!lottery.BeginLott)
                        {
                            edit = true;
                            lottery.BeginLott = true;
                            await db.SaveChangesAsync();
                        }

                        var result = "<div class=\"title\"><h1><span>Победители</span></h1></div><div class=\"drawListWinner\">";
                        foreach(var prize in lottery.Prizes.OrderBy(p => p.Place))
                        {
                            var cls = "otherPlace";
                            var p = prize.Place;
                            if (p == 1) { cls = "firstPlace"; }
                            if (p == 2) { cls = "secondPlace"; }
                            if (p == 3) { cls = "thirdPlace"; }

                            result += $"<div class=\"{cls}\">{p}. ";
                            if (prize.NoWin)
                                result += " Не разыгран ";
                            else
                                result += $"<a href=\"/Main/Joker/{prize.Winner.Id}\">{prize.Winner.Nick}</a>";
                            result += $" - {prize.Name} </div>";

                            if (edit)
                            {
                                prize.Confirm = true;
                                if (prize.Winner != null)
                                    prize.Winner.Option.HaveReviews = true;
                            }
                        }

                        if (edit)
                            await db.SaveChangesAsync();

                        result += "</div>";
                        return result;
                    }
                    else if (lottery.BeginLott)
                        return "1";
                }
            }
            return "0";
        }

        // ПОЛУЧЕНИЕ ПОБЕДИТЕЛЯ
        // ----------------------------------------------------
        [Authorize]
        [HttpPost]
        public async Task<string> GetWinner(int LotteryId)
        {
            if (LotteryId > 0)
            {
                var id = User.Identity.GetUserId<int>();
                var lottery = await db.Lotteries.FindAsync(LotteryId);
                if (lottery != null && lottery.Studio.Team.FirstOrDefault(t => t.Member.Id == id && t.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin" || r.Name == "CreateLottery") != null) != null)
                {
                    var users = lottery.Users.Where(
                       u => u.RefreshWinner.FirstOrDefault(r => r.Prize.Lottery.Id == LotteryId) == null &&
                            u.WinLott.FirstOrDefault(p => p.Lottery.Id == LotteryId) == null
                            ).ToList();

                    var notUser = (users.Count == 0) ? true : false;
                    var prizes = lottery.Prizes.Where(p => p.Winner == null);

                    Prize prize = null;
                    if (lottery.FromTheFirst)
                        prize = prizes.OrderBy(p => p.Place).First();
                    else
                        prize = prizes.OrderByDescending(p => p.Place).First();

                    if (prize.Confirm)
                        return "0";

                    if (!notUser)
                    {
                        var rng = new Random();
                        var winner = users[rng.Next(users.Count)];

                        prize.Winner = winner;
                        if (lottery.MandatoryInfo.Count == 0)
                        {
                            prize.Confirm = true;
                            winner.Option.HaveReviews = true;
                        }
                        await db.SaveChangesAsync();
                        return $"[{winner.Id}, \"{winner.Nick}\", \"{prize.Name.Replace('"', '\'')}\"]";
                    }
                    else
                    {
                        var penaltyPoints = 1;
                        foreach (var noPrize in prizes)
                        {
                            noPrize.Confirm = true;
                            noPrize.Winner = null;
                            noPrize.NoWin = true;
                            ++penaltyPoints;
                        }
                        
                        // Статистика = Статистика - (Участники / Призы) * Штрафные баллы
                        lottery.Studio.Option.Statistics -= (lottery.Users.Count / lottery.Prizes.Count) * penaltyPoints;

                        await db.SaveChangesAsync();
                        return $"[0, \"Не разыгран\", \"{prize.Name.Replace('"', '\'')}\"]";
                    }
                }
            }
            return "0";
        }

        // ПЕРЕВЫБОР ПОБЕДИТЕЛЯ
        // ----------------------------------------------------------------------------
        [Authorize]
        public ActionResult RefreshWinner(int LotteryId, int WinnerId, string Text)
        {
            if (LotteryId > 0 && WinnerId > 0)
            {
                var id = User.Identity.GetUserId<int>();
                var prize = db.Prizes.FirstOrDefault(w => w.Lottery.Id == LotteryId && w.Winner.Id == WinnerId);
                if (prize != null && prize.Lottery.Studio.Team.FirstOrDefault(t => t.Member.Id == id && t.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin" || r.Name == "CreateLottery") != null) != null)
                {
                    prize.RefreshWinners.Add(new RefreshWinner()
                    {
                        DelWinner = prize.Winner,
                        Explanatory = Text
                    });

                    // Точно так же как и в получение победителя
                    var users = prize.Lottery.Users.Where(
                        u => u.RefreshWinner.FirstOrDefault(r => r.Prize.Lottery.Id == LotteryId) == null &&
                             u.WinLott.FirstOrDefault(p => p.Lottery.Id == LotteryId) == null
                             ).ToList();

                    var notUser = (users.Count == 0) ? true : false;

                    if (!notUser)
                    {
                        var rng = new Random();
                        var winner = users[rng.Next(users.Count)];

                        prize.Winner = winner;

                        db.SaveChanges();
                        return RedirectToAction("Joker", "Main", new { id = prize.Winner.Id, Lott = prize.Lottery.Id });
                    }
                    else
                    {
                        foreach (var noPrize in prize.Lottery.Prizes.Where(p => p.Winner == null))
                            noPrize.NoWin = true;

                        prize.Winner = null;
                        prize.NoWin = true;

                        db.SaveChanges();
                        return RedirectToAction("Holding", new { id = LotteryId });
                    }
                }
            }
            return new HttpStatusCodeResult(500);
        }

        // ПОДТВЕРЖДЕНИЕ ПОБЕДИТЕЛЯ
        // ----------------------------------------------------------------
        [Authorize]
        public ActionResult ConfirmWinner(int LotteryId, int WinnerId)
        {
            if (LotteryId > 0 && WinnerId > 0)
            {
                var prize = db.Prizes.FirstOrDefault(w => w.Lottery.Id == LotteryId && w.Winner.Id == WinnerId);
                var id = User.Identity.GetUserId<int>();
                if (prize != null && prize.Lottery.Studio.Team.FirstOrDefault(t => t.Member.Id == id && t.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin" || r.Name == "CreateLottery") != null) != null)
                {
                    prize.Confirm = true;
                    prize.Winner.Option.HaveReviews = true;
                    db.SaveChanges();
                    return RedirectToAction("Holding", new { id = LotteryId });
                }
            }
            return new HttpStatusCodeResult(500);
        }
        
        // ПРОВЕРКА ПОБЕДИТЕЛЯ
        // ---------------------------------------------------------------
        [HttpPost]
        public async Task<string> CheckWinner(int LotteryId, int Place)
        {
            if (LotteryId > 0 && Place > 0)
            {
                var prize = await db.Prizes.FirstOrDefaultAsync(w => w.Lottery.Id == LotteryId && w.Place == Place && ((w.Winner != null || w.NoWin) && w.Confirm));

                if (prize != null)
                {
                    prize.Name = prize.Name.Replace('"', '\'');

                    if (!prize.NoWin)
                        return $"[{prize.Winner.Id}, \"{prize.Winner.Nick}\", \"{prize.Name}\"]";
                    else
                        return $"[0, \"Не разыгран\", \"{prize.Name}\"]";
                }
            }
            return "0";
        }

        // ДОБАВЛЕНИЕ КОММЕНТАРИЯ В ЧАТ
        // --------------------------------------------------------------------------
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddCommentChat(int LotteryId, string Text)
        {
            if (LotteryId > 0 && Text != null && Text != "")
            {
                var lottery = await db.Lotteries.FindAsync(LotteryId);
                if (lottery != null)
                {
                    var id = User.Identity.GetUserId<int>();
                    var user = await db.Users.FindAsync(id);
                    if (lottery.BanComment.FirstOrDefault(b => b.Id == user.Id) == null)
                    {
                        var comment = new CommentChatLottery()
                        {
                            Author = user,
                            DateCreate = DateTime.UtcNow,
                            Text = Text
                        };

                        lottery.Chat.Add(comment);

                        await db.SaveChangesAsync();
                        // Если пользователь имеет роль позволяющую проводить розыгрыш
                        ViewBag.Organizer = CheckRoleOrganizer(lottery, id);
                        return PartialView("_ChatMessageLottery", new List<CommentChatLottery> { comment });
                    }
                    else
                        return Content("ban");
                }
            }
            return Content("0");
        }

        // БЛОКИРОВКА ПОЛЬЗОВАТЕЛЯ
        // --------------------------------------------------------------------------
        [Authorize]
        [HttpPost]
        public async Task<byte> BanChat(int Id)
        {
            if (Id > 0)
            {
                var comment = await db.ChatLottery.FindAsync(Id);
                var id = User.Identity.GetUserId<int>();
                var user = await db.Users.FindAsync(id);
                var studio = user.Studios.FirstOrDefault(s => s.Studio.Id == comment.Lottery.Studio.Id && s.Member.Id == id);

                if (studio != null /*&& comment.Author.Role.Name != "Admin"*/
                    && (studio.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin" || r.Name == "CreateLottery") != null
                        || user.Role.Name == "Admin"))
                {
                    if (comment.Lottery.BanComment.FirstOrDefault(b => b.Id == comment.Author.Id) == null)
                    {
                        comment.Lottery.BanComment.Add(comment.Author);
                        await db.SaveChangesAsync();
                    }
                    return 1;
                }
            }
            return 0;
        }

        // ПРОВЕРКА НА НОВЫЕ СООБЩЕНИЯ В ЧАТЕ
        // ------------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<ActionResult> CheckCommentChat(int LotteryId, int CommentId, DateTime LastCommentDate)
        {
            if (LotteryId > 0)
            {
                var newMessage = new List<CommentChatLottery>();

                if (CommentId > 0)
                    newMessage = await db.ChatLottery.Where(c => c.Lottery.Id == LotteryId && c.DateCreate >= LastCommentDate && c.Id != CommentId).ToListAsync();
                else
                    newMessage = await db.ChatLottery.Where(c => c.Lottery.Id == LotteryId).ToListAsync();

                if (newMessage.Count > 0)
                {
                    if (User.Identity.IsAuthenticated)
                        // Если пользователь имеет роль позволяющую проводить розыгрыш
                        ViewBag.Organizer = CheckRoleOrganizer(await db.Lotteries.FindAsync(LotteryId), User.Identity.GetUserId<int>());
                    else
                        ViewBag.Organizer = false;
                    
                    return PartialView("_ChatMessageLottery", newMessage);
                }
            }
            return Content("0");
        }

        // ПРЕДСТАВЛЕНИЕ СОЗДАНИЯ РОЗЫГРЫША
        // ------------------------------------------
        [Authorize]
        [HttpGet]
        public ActionResult Create(int? Id)
        {
            if (Id != null || Id > 0)
            {
                var studio = db.Studios.Find(Id);

                if (studio != null)
                {
                    var id = User.Identity.GetUserId<int>();
                    var user = studio.Team.FirstOrDefault(m => m.Member.Id == id && m.Member.Id == id);
                    if (user.Role.Where(r => r.Name == "Creator" || r.Name == "Admin" || r.Name == "CreateLottery").Count() > 0)
                    {
                        ViewBag.Id = Id;
                        return View();
                    }
                }
            }

            return HttpNotFound();
        }

        // СОЗДАНИЕ РОЗЫГРЫША
        // -----------------------------------------------------------------------------------
        [Authorize]
        [HttpPost]
        public async Task<string> Create(HttpPostedFileBase Cover, LotteryCreate NewLottery)
        {
            var errorMessage = "Ошибка, перезапустите браузер!";

            if (NewLottery.Id > 0)
            {
                var id = User.Identity.GetUserId<int>();
                var user = await db.Users.FindAsync(id);
                var myRole = user.Studios.FirstOrDefault(s => s.Studio.Id == NewLottery.Id && s.Member.Id == id);
                var lottery = new Lottery();
                errorMessage = "У Вас недостаточно прав, для создания розыгрыша от имени этой студии!";

                if (myRole != null && myRole.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin" || r.Name == "CreateLottery") != null)
                {
                    errorMessage = "[";

                    if (NewLottery.Title != null && NewLottery.Title != "")
                        lottery.Title = NewLottery.Title;
                    else
                        errorMessage += "\"title\",";

                    if (NewLottery.ShortDescription != null && NewLottery.ShortDescription != "" && NewLottery.ShortDescription.Length <= 300)
                        lottery.ShortDescription = NewLottery.ShortDescription;
                    else
                        errorMessage += "\"shortDesc\",";

                    if (NewLottery.Text != null && NewLottery.Text != "")
                        lottery.Text = NewLottery.Text;
                    else
                        errorMessage += "\"text\",";

                    NewLottery.DateLottery = NewLottery.DateLottery.AddHours(-NewLottery.GMT);
                    var maxDate = DateTime.UtcNow.AddDays(182);
                    if (NewLottery.DateLottery > DateTime.UtcNow && NewLottery.DateLottery <= maxDate)
                        lottery.DateLottery = NewLottery.DateLottery;
                    else
                        errorMessage += "\"date\",";

                    if (Cover == null)
                        errorMessage += "\"ava\",";

                    if (NewLottery.Prizes.Count > 0)
                        lottery.Prizes = NewLottery.Prizes;
                    else
                        errorMessage += "\"prize\",";

                    if (errorMessage == "[")
                    {
                        var yuid = YouTube.ParseId(NewLottery.YouTubeId);

                        lottery.Studio = myRole.Studio;
                        lottery.DateCreate = DateTime.UtcNow;
                        lottery.YoutubeId = (yuid != null) ? yuid : "";
                        lottery.BeginAgeLimit = NewLottery.BeginAge;
                        lottery.EndAgeLimit = NewLottery.EndAge;
                        lottery.CountUserForLottery = NewLottery.CountUserForLottery;
                        lottery.GenderLimit = NewLottery.Gender;

                        if (NewLottery.Conditions.Count > 0)
                        {
                            var i = 1;
                            foreach (var lot in NewLottery.Conditions)
                            {
                                lottery.Conditions += lot;
                                if (i < NewLottery.Conditions.Count)
                                    lottery.Conditions += "&Conditions;";

                                ++i;
                            }
                        }

                        if (NewLottery.CitiesId != null && NewLottery.CitiesId != "")
                        {
                            var arr = NewLottery.CitiesId.Split(';');
                            foreach (var cityId in arr)
                            {
                                var city = await db.Cities.FindAsync(int.Parse(cityId));
                                if (city != null)
                                    lottery.Cities.Add(city);
                            }
                        }

                        if (NewLottery.RegionsId != null && NewLottery.RegionsId != "")
                        {
                            var arr = NewLottery.RegionsId.Split(';');
                            foreach (var regId in arr)
                            {
                                var reg = await db.Regions.FindAsync(int.Parse(regId));
                                if (reg != null)
                                    lottery.Regions.Add(reg);
                            }
                        }

                        if (NewLottery.MandatoryInfo.Count > 0)
                            lottery.MandatoryInfo = NewLottery.MandatoryInfo;

                        if (NewLottery.LinksLottery.Count > 0)
                            lottery.Link = NewLottery.LinksLottery;

                        var imgName = PathImg.Inspection(Cover.FileName, "~/Content/style/images/Lotteries/Normal/");
                        WebImage cover = new WebImage(Cover.InputStream);
                        cover.Resize(900, 500, true, true);
                        cover.Save($"~/Content/style/images/Lotteries/Normal/{imgName}", "JPG", true);
                        cover.Resize(550, 300, true, true);
                        cover.Save($"~/Content/style/images/Lotteries/Reduced/{imgName}", "JPG", true);

                        lottery.Cover = imgName;
                        db.Lotteries.Add(lottery);
                        await db.SaveChangesAsync();

                        return "1";
                    }

                    errorMessage = errorMessage.Substring(0, errorMessage.Length - 1);
                    errorMessage += "]";

                }
            }

            return errorMessage;
        }

        // Поиск города или региона
        // --------------------------------------------------------------------------------------------------
        // search - поисковый запрос, CitiesID - идентификаторы городов, RegionsID - идентификаторы регионов
        // --------------------------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _LocalLimiter(string Search, string CitiesID, string RegionsID)
        {
            if (Search == null || Search == "")
                return Content("0");

            var count = 6;
            var limiter = new LocalLimiter();
            var cities = await db.Cities.Where(c => c.Name.Contains(Search) || c.Index.ToString().Contains(Search)).Take(count).ToListAsync();
            
            var citiesId = (CitiesID != null && CitiesID != "") ? CitiesID.Split(';') : null;
            var regionsId = (RegionsID != null && RegionsID != "") ? RegionsID.Split(';') : null;
            if (citiesId != null || regionsId != null)
            {
                var newRange = new List<City>();
                foreach (var city in cities)
                {
                    // Если город содержит идентификатор уже выбранного города, или находится в регионе, который уже полностью участвует, то удаляем.
                    if ( (citiesId != null && Array.IndexOf(citiesId, city.Id.ToString()) == -1) || (regionsId != null && Array.IndexOf(regionsId, city.Region.Id.ToString()) == -1) )
                        newRange.Add(city);
                }
                cities = newRange;
            }

            if (cities.Count < count)
            {
                limiter.Cities = cities;
                var regs = await db.Regions.Where(r => r.Name.Contains(Search)).Take(count - cities.Count).ToListAsync();

                if (regionsId != null)
                {
                    foreach (var id in regionsId)
                    {
                        var reg = regs.FirstOrDefault(c => c.Id == int.Parse(id));
                        if (reg != null)
                            regs.Remove(reg);
                    }
                }

                limiter.Regions = regs;
            }

            return PartialView(limiter);
        }

        // ОСНОВНАЯ ЛОГИКА ЕЖЕДНЕВНЫХ РОЗЫГРЫШЕЙ
        // -------------------------------------------------
        public bool LogicsEveryday(EverydayLott lott, WinnersEverydayLott place)
        {
            // Если приз существует и время розыгрыша уже прошло, то...
            if (place != null && lott.Holding < DateTime.UtcNow)
            {
                // Если победитель есть, то...
                if (place.Winner != null)
                {
                    // Подтверждаем победу
                    place.Confirm = true;
                    // даём возможность оставить отзыв
                    place.Winner.Option.HaveReviews = true;
                }
                // Если нет, удаляем место
                else
                    db.WinnersEveryday.Remove(place);

                // Находим разницу в днях, в сучае если долгое время никто не учавствовал в розыгрыше
                TimeSpan odds = DateTime.UtcNow - lott.Holding;
                // Обнуляем участников
                lott.Participants = new List<User>();
                // Добавляем количество прошедших дней + 1
                lott.Holding = lott.Holding.AddDays(odds.Days + 1);
                // Резервируем место
                lott.Winners.Add(new WinnersEverydayLott()
                {
                    HoldingLott = lott.Holding,
                    Winner = null,
                    Prize = lott.Prize
                });

                db.SaveChanges(); // save

                return true;
            }

            return false;
        }

        // Выбор победителя
        public async Task<bool> AutoSelect(Lottery Lottery, string Action)
        {
            if (Lottery.MandatoryInfo.Count > 0)
                return false;

            var users = Lottery.Users.ToList();

            var prizes = new List<Prize>();
            if (Lottery.FromTheFirst)
                prizes.AddRange(Lottery.Prizes.OrderBy(p => p.Place));
            else
                prizes.AddRange(Lottery.Prizes.OrderByDescending(p => p.Place));

            foreach (var prize in prizes)
            {
                if (Action == "add")
                {
                    if (users.Count > 0)
                    {
                        var rng = new Random();
                        var winner = users[rng.Next(users.Count)];
                        prize.NoWin = false;
                        prize.Winner = winner;
                        users.Remove(winner);
                    }
                    else
                    {
                        prize.Winner = null;
                        prize.NoWin = true;
                    }
                }
                else
                {
                    prize.Winner = null;
                    prize.NoWin = false;
                }
            }

            await db.SaveChangesAsync();
            return true;
        }

        // Выбор победителя
        public bool CheckRoleOrganizer(Lottery lott, int Id)
        {
            if (lott.Studio.Team.FirstOrDefault(t => t.Member.Id == Id && t.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin" || r.Name == "CreateLottery") != null) != null)
                return true;
            else
                return false;
        }

        // Закрываем соединение с БД
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
