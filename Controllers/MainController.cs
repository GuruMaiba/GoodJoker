using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GoodJoker.Models;
using Microsoft.Owin.Security;

namespace GoodJoker.Controllers
{
    public class MainController : Controller
    {
        // Работа с Owin авторизацией
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        // Инициализация входа в Базу Данных
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var model = new Main() {
                Lott = db.Lotteries.OrderByDescending(l => l.OddsFingers).Take(5).ToList(),
                News = db.News.Where(n => n.Publish).OrderByDescending(n => n.DateCreate).Take(2).ToList(),
                Reviews = (User.Identity.GetUserRole() == "Admin") ? db.Assess.OrderByDescending(r => r.Id).ToList() : db.Assess.OrderByDescending(r => r.Id).Where(r => r.OnTheMain).ToList(),
                Videos = db.Videos.Where(v => v.Type.FirstOrDefault(t => t.Name == "review") != null).OrderByDescending(v => v.Id).ToList()
            };
            return View(model);
        }

        public ActionResult Email()
        {
            return View();
        }

        public ActionResult Auth(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        public ActionResult Joker(int? id, int? Lott)
        {
            if (id <= 0 || id == null)
                return HttpNotFound();

            var user = db.Users.Find(id);

            if (user == null)
                return HttpNotFound();

            var myId = User.Identity.GetUserId<int>();

            var viewModel = new ViewJoker() { User = user };
            var prizes = new List<PrizesProfile>();

            foreach (var prize in user.ArchiveWinLott)
                prizes.Add(new PrizesProfile()
                {
                    DataWin = prize.Lottery.HoldingLottery,
                    Place = prize.Place,
                    Prize = prize.Prize
                });

            foreach (var prize in user.ArchiveWinEver)
                prizes.Add(new PrizesProfile()
                {
                    DataWin = prize.Date,
                    Prize = prize.Prize
                });

            foreach (var prize in user.WinLott.Where(w => w.Confirm))
                prizes.Add(new PrizesProfile()
                {
                    DataWin = prize.Lottery.DateLottery,
                    Place = prize.Place,
                    Prize = prize.Name
                });

            foreach (var prize in user.WinEveryday.Where(w => w.Confirm))
                prizes.Add(new PrizesProfile()
                {
                    DataWin = prize.HoldingLott,
                    Prize = prize.Prize
                });

            viewModel.Prizes = prizes.OrderByDescending(p => p.DataWin).ToList();
            
            var lott = (Lott != null && Lott > 0) ? user.Lotteries.FirstOrDefault(l =>
                // Проверяем относится ли этот пользователь к лоттереи, организатор которой запрашивает информацию
                // Является ли организатор организатором
                // Существует ли приз, который данный пользователь выйграл
                // Запрашивал ли организатор, какую-нибудь информацию у пользователя
                l.Id == Lott &&
                l.Studio.Team.FirstOrDefault(t => t.Member.Id == myId && t.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin" || r.Name == "CreateLottery") != null) != null &&
                l.Prizes.FirstOrDefault(p => p.Winner == user && !p.Confirm) != null &&
                l.MandatoryInfo.Count > 0
                ) : null;

            ViewBag.Lott = (lott != null) ? Lott : 0;
            ViewBag.Info = (lott != null) ? true : false;

            return View(viewModel);
        }

        public ActionResult Studio(int? id, bool? preview)
        {
            if (id <= 0 || id == null)
                return HttpNotFound();

            var studio = db.Studios.Find(id);

            if (studio == null)
                return HttpNotFound();

            if (preview != null && preview == true)
                ViewBag.Preview = true;

            return View(studio);
        }
        
        [Authorize]
        public async Task Complaint(int? IdNews, int? IdLottery, int? IdComment, string Text, bool Reply = false)
        {
            if (Text != null && Text != "")
            {
                var reply = "";
                if (!Reply)
                    reply = "Письмо оставить без ответа!";
                else
                    reply = "Пользователь просит ответить ему!";

                string subscriber = "Жалоба на ",
                       body = "Поступила жалоба на ",
                       commentAuthor = "",
                       commentText = "",
                       id = "",
                       title = "",
                       NewsOrLott = "";

                if (IdNews != null && IdNews > 0)
                {
                    var news = await db.News.FindAsync(IdNews);
                    NewsOrLott = "News";
                    if (news != null && IdComment != null && IdComment > 0)
                    {
                        var comment = news.Comments.FirstOrDefault(c => c.Id == IdComment);
                        if (comment != null)
                        {
                            commentAuthor = comment.Author.Nick;
                            commentText = comment.Text;
                        }
                    }

                    id += news.Id;
                    title = news.Title;
                }
                else if (IdLottery != null && IdLottery > 0)
                {
                    var lott = await db.Lotteries.FindAsync(IdLottery);
                    NewsOrLott = "Lottery";
                    if (lott != null && IdComment != null && IdComment > 0)
                    {
                        var comment = lott.Comments.FirstOrDefault(c => c.Id == IdComment);
                        if (comment != null)
                        {
                            commentAuthor = comment.Author.Nick;
                            commentText = comment.Text;
                        }
                    }

                    id += lott.Id;
                    title = lott.Title;
                }

                if (NewsOrLott != "" && id != "" && title != "")
                {
                    if (commentAuthor != "")
                    {
                        subscriber += "комментарий ";

                        if (NewsOrLott == "News")
                            subscriber += $"в новостной статье";
                        else if (NewsOrLott == "Lottery")
                            subscriber += $"в розыгрыше";

                        body += $"Комментарий пользователя: <a href='https://goodjoker.ru/Main/Joker/{commentAuthor}'>{commentAuthor}</a><br />" +
                        $"С текстом: {commentText}<br />";
                    }
                    else
                    {
                        if (NewsOrLott == "News")
                            subscriber += $"новостную статью";
                        else if (NewsOrLott == "Lottery")
                            subscriber += $"розыгрыш";
                    }

                    if (NewsOrLott == "News")
                        body += $"Новостная статья: ";
                    else if (NewsOrLott == "Lottery")
                        body += $"Розыгрыш: ";

                    EmailMessage.SendMessage(
                        "goodjokerteam@gmail.com",
                        $"{subscriber} - {title}",
                        $"{body} \"<a href='https://goodjoker.ru/{NewsOrLott}/Details/{id}'>{title}</a>\",<br />" +
                        $"От пользователя: <a href='https://goodjoker.ru/Main/Joker/{User.Identity.GetUserId<int>()}'>{User.Identity.Name}</a>,<br />" +
                        $"Текс жалобы: {Text}<br />{reply}"
                    );
                }
            }
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task AddAssess(float Rate, string Text)
        {
            var id = User.Identity.GetUserId<int>();
            var user = await db.Users.FindAsync(id);
            var assess = db.Assess.FirstOrDefault(a => a.User.Id == id);
            var info = db.InfoSite.First();
            // вставить дату...
            if (assess == null)
            {
                db.Assess.Add(new Assess {
                    Rate = Rate,
                    Text = Text,
                    User = user
                });
                ++info.CountAssessUser;
            }
            else
            {
                user.Assess.Rate = Rate;
                user.Assess.Text = Text;
                user.Assess.OnTheMain = false;
                user.Assess.Publish = false;
                user.Assess.ReplyText = null;
            }
            await db.SaveChangesAsync();

            info.AverageAssess = db.Assess.Average(a => a.Rate);
            await db.SaveChangesAsync();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public string InfoSite()
        {
            var info = db.InfoSite.First();
            string userRate = null;
            if (User.Identity.IsAuthenticated)
                userRate = db.Users.Find(User.Identity.GetUserId<int>())?.Assess?.Rate.ToString();
            if (userRate == null)
                userRate = "-";

            return $"{{ " +
                        $"\"Reg\": \"{Thousand(info.CountRegUser)}\", " +
                        $"\"Arch\": \"{Thousand(info.CountArchLott)}\", " +
                        $"\"Average\": \"{info.AverageAssess.ToString("0.0")}\", " +
                        $"\"Assess\": \"{Thousand(info.CountAssessUser)}\", " +
                        $"\"UserRate\": \"{userRate}\"" +
                    $" }}";
        }

        // Функция добавления точек в большие числа
        public static string Thousand(int Count)
        {
            string val = $"{Count}";
            if (Count > 999)
                val = Count.ToString("##,0");
            return val;
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