using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GoodJoker.Models;
using Microsoft.Owin.Security;

namespace GoodJoker.Controllers
{
    public class NewsController : Controller
    {
        // Работа с Owin авторизацией
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        // Инициализация входа в Базу Данных
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // How Match to Show
        const int HMS = 3;
        const int HMSC = 3; // Comment

        // GET: News
        public ActionResult Index(string Tag)
        {
            var model = new NewsMain() {
                Lotteries = db.Lotteries.OrderByDescending(l => l.DateCreate).Take(10).ToList(),
                Winners = db.Prizes.Where(p => p.Place == 1 && p.Winner != null).OrderByDescending(p => p.Id).Take(10).ToList(),
                Tags = db.Tags.OrderByDescending(t => t.News.Count).ToList()
            };

            if (User.Identity.GetUserRole() == "Admin")
            {
                model.News = (Tag != null && Tag != "")
                            ? db.News.Where(n => n.Tags.FirstOrDefault(t => t.Name == Tag) != null).OrderByDescending(n => n.DateCreate).Take(HMS).ToList()
                            : db.News.OrderByDescending(n => n.DateCreate).Take(HMS).ToList();
            }
            else
            {
                model.News = (Tag != null && Tag != "")
                            ? db.News.Where(n => n.Tags.FirstOrDefault(t => t.Name == Tag) != null && n.Publish).OrderByDescending(n => n.DateCreate).Take(HMS).ToList()
                            : db.News.Where(n => n.Publish).OrderByDescending(n => n.DateCreate).Take(HMS).ToList();
            }

            ViewBag.AllCount = (Tag != null && Tag != "") ? db.News.Where(n => n.Tags.FirstOrDefault(t => t.Name == Tag) != null).Count() : db.News.Count();
            ViewBag.Tag = (Tag != null && Tag != "") ? Tag : "all";
            return View(model);
        }

        public async Task<ActionResult> _NewsItem(string Tag, int CountView)
        {
            if (CountView <= 0)
                return Content("0");

            var news = (Tag != null && Tag != "" && Tag != "all")
                            ? await db.News.Where(n => n.Tags.FirstOrDefault(t => t.Name == Tag) != null).OrderByDescending(n => n.DateCreate).ToListAsync()
                            : await db.News.OrderByDescending(n => n.DateCreate).ToListAsync();

            if (news.Count <= CountView)
                return Content("1");

            news.RemoveRange(0, CountView);
            news = news.GetRange(0, (news.Count > HMS) ? HMS : news.Count);

            return PartialView(news);
        }

        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            News news = db.News.Find(id);
            if (news == null || (!news.Publish && User.Identity.GetUserRole() != "Admin"))
                return HttpNotFound();

            ++news.View;
            db.SaveChanges();

            ViewBag.allComments = news.Comments.ToList();
            ViewBag.last = (news.Comments.Count <= HMSC) ? '1' : '0';
            ViewBag.NoSlider = 1;
            news.Comments = (news.Comments.Count > HMSC) ? news.Comments.OrderByDescending(c => c.DateCreate).Take(HMSC).ToList() : news.Comments;

            return View(news);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddComment(int NewsId, int CommentId, string Text)
        {
            if (NewsId != 0 && Text != null && Text != "")
            {
                var news = await db.News.FindAsync(NewsId);
                if (news != null)
                {
                    var id = User.Identity.GetUserId<int>();
                    var user = await db.Users.FindAsync(id);
                    var comment = new CommentNews()
                    {
                        Author = user,
                        DateCreate = DateTime.UtcNow,
                        Text = Text,
                        ReplyId = CommentId
                    };
                    ++user.Option.CountComments;

                    if (news.Comments.Count == 0)
                        ViewBag.last = '1';

                    var list = new List<CommentNews>();
                    if (CommentId > 0)
                        list.Add(news.Comments.FirstOrDefault(c => c.Id == CommentId));
                    ViewBag.allComments = list;
                    news.Comments.Add(comment);

                    await db.SaveChangesAsync();
                    return PartialView("_CommentNews", new List<CommentNews>() { comment });
                }
            }
            return Content("0");
        }

        // Lazyload
        [HttpPost]
        public async Task<ActionResult> _CommentNews(int NewsId, int ViewCount)
        {
            if (NewsId > 0 && ViewCount > 0)
            {
                var comments = await db.CommentsNews.Where(c => c.News.Id == NewsId).OrderByDescending(c => c.DateCreate).ToListAsync();
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
                    return PartialView("_CommentNews", comments);
                }
            }
            return Content("0");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> EditComment(int CommentId, string Text)
        {
            if (CommentId > 0 && Text != null && Text != "")
            {
                var comment = await db.CommentsNews.FindAsync(CommentId);
                if (comment != null)
                {
                    comment.Text = Text;
                    await db.SaveChangesAsync();

                    return Content("1");
                }
            }
            return Content("0");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> DelComment(int CommentId)
        {
            if (CommentId > 0)
            {
                var comment = await db.CommentsNews.FindAsync(CommentId);
                if (comment != null)
                {
                    var replyId = comment.Id;
                    var lottCom = comment.News.Comments;
                    var delId = "" + comment.Id;
                    var delCom = new List<CommentNews>() { comment };
                    var checkCom = new List<CommentNews>() { comment };

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

                    db.CommentsNews.RemoveRange(delCom);
                    await db.SaveChangesAsync();

                    return Content(delId);
                }
            }
            return Content("0");
        }

        [Authorize]
        [HttpPost]
        public async Task<string> Like(int NewsId)
        {
            var returnString = "0";
            if (NewsId != 0)
            {
                var id = User.Identity.GetUserId<int>();
                var user = await db.Users.FindAsync(id);
                var news = await db.News.FindAsync(NewsId);
                if (news != null)
                {
                    var IsLike = news.UsersLike.FirstOrDefault(u => u.Id == id);
                    if (IsLike != null)
                    {
                        news.UsersLike.Remove(IsLike);
                        returnString = "del";
                    }
                    else
                    {
                        news.UsersLike.Add(user);
                        returnString = "add";
                    }

                    await db.SaveChangesAsync();
                }
            }
            return returnString;
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
