using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using GoodJoker.Models;
using Microsoft.Owin.Security;

namespace GoodJoker.Controllers
{
    [Authorize]
    public class PrivateOfficeController : Controller
    {
        // Работа с Owin авторизацией
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        // Инициализация входа в Базу Данных
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // Мои розыгрыши
        // -----------------------------------------------------
        public ActionResult Index()
        {
            var id = User.Identity.GetUserId<int>();
            var user = db.Users.Find(id);
            UpdateLastOnline(user);

            var model = new MainLK()
            {
                EverydayPrize = user.WinEveryday.Where(w => !w.View && w.Confirm).ToList(),
                LottPrize = user.WinLott.Where(p => !p.View && p.Confirm).ToList(),
                LottRefresh = user.RefreshWinner.Where(r => !r.View && r.Prize.Confirm).ToList(),
                HaveReview = user.Option.HaveReviews,
                Everyday = user.Everyday,
                Lott = user.Lotteries
            };
            ViewBag.Ava = GetAvaCookie(user.Option.Ava);
            return View(model);
        }

        // Жалоба
        // --------------------------------------------------
        [HttpPost]
        public async Task Complaint(int Id, bool Everyday, string Text, bool Reply = false)
        {
            var id = User.Identity.GetUserId<int>();
            var user = await db.Users.FindAsync(id);
            var reply = "";

            if (Reply)
                reply = "Пользователь просит об обратной связи!";
            else
                reply = "Пользователь отказался от обратной связи.";

            if (Everyday)
            {
                var everyday = await db.WinnersEveryday.FindAsync(Id);

                if (everyday != null && Text != null)
                {
                    EmailMessage.SendMessage(
                        "goodjokerteam@gmail.com",
                        $"Жалоба на победу в ежедневном розыгрыше - {everyday.Lottery.Prize}",
                        $"Розыгрыш: \"<a href='https://goodjoker.ru/Lottery/Everyday/{everyday.Lottery.Id}'>{everyday.Lottery.Prize}</a>\",<br />" +
                        $"От пользователя: <a href='https://goodjoker.ru/Main/Joker/{id}'>{user.Nick}</a>,<br />" +
                        $"Текс жалобы: {Text}<br />{reply}"
                    );
                }
            }
            else
            {
                var prize = await db.Prizes.FindAsync(Id);

                if (prize != null && Text != null)
                {
                    var subscriber = "Жалоба на ";

                    if (prize.Winner.Id != id)
                        subscriber = "победу в розыгрыше";
                    else
                        subscriber = "замену победителя в розыгрыше";

                    EmailMessage.SendMessage(
                        "goodjokerteam@gmail.com",
                        $"{subscriber} {prize.Lottery.Title}",
                        $"Розыгрыш: \"<a href='https://goodjoker.ru/Lottery/Details/{prize.Lottery.Id}'>{prize.Lottery.Title}</a>\",<br />" +
                        $"Место: {prize.Place},<br />" +
                        $"Приз: {prize.Name}<br />" +
                        $"От пользователя: <a href='https://goodjoker.ru/Main/Joker/{id}'>{user.Nick}</a>,<br />" +
                        $"Текс жалобы: {Text}<br />{reply}"
                    );
                }
            }
            
        }

        // Закрытие уведомления
        // --------------------------------------------------
        [HttpPost]
        public async Task CloseNotice(int PrizeId, bool Everyday)
        {
            var id = User.Identity.GetUserId<int>();

            if (Everyday)
            {
                var prize = await db.WinnersEveryday.FindAsync(PrizeId);

                if (prize != null)
                {
                    if (prize.Winner.Id == id)
                        prize.View = true;

                    await db.SaveChangesAsync();
                }
            }
            else
            {
                var prize = await db.Prizes.FindAsync(PrizeId);

                if (prize != null)
                {
                    if (prize.Winner.Id != id)
                    {
                        var refresh = prize.RefreshWinners.FirstOrDefault(p => p.DelWinner.Id == id);
                        if (refresh != null)
                            refresh.View = true;
                    }
                    else
                        prize.View = true;

                    await db.SaveChangesAsync();
                }
            }
        }

        // Диалоги (Идентификатор диалога)
        // -----------------------------------------------------
        // Id - Идентификатор диалога
        // -----------------------------------------------------
        public ActionResult Dialogs(int? Id)
        {
            var id = User.Identity.GetUserId<int>();
            var user = db.Users.Find(id);
            UpdateLastOnline(user);

            ViewBag.Ava = user.Option.Ava;

            var dialogs = new List<Dialog>();
            dialogs.AddRange(user.AuthorDialog.Where(d => !d.DelAuthor));
            dialogs.AddRange(user.InvitationDialog.Where(d => !d.DelInvitation));

            if (Id != null && Id != 0 && dialogs.FirstOrDefault(d => d.Id == Id) != null)
                ViewBag.Id = Id;
            else
                ViewBag.Id = 0;

            dialogs.OrderByDescending(d => d.Messages.Last()?.DateCreate);
            ViewBag.Ava = user.Option.Ava;
            return View(dialogs);
        }

        // Поиск пользователя при создание диалога
        // -----------------------------------------------------
        // Nick - Ник пользователя
        // -----------------------------------------------------
        [HttpPost]
        public async Task<string> SearchUser(string Nick)
        {
            var nick = User.Identity.Name;
            var users = await db.Users.Where(u => u.Nick.Contains(Nick) && u.Nick != nick).ToListAsync();
            var retStr = "";

            if (users.Count > 0)
            {
                var i = 0;
                foreach (var user in users)
                {
                    if (i == 5)
                        break;

                    retStr += $"<div userId='{user.Id}'>{user.Nick}</div>";
                    ++i;
                }
            }
            else
                retStr = "<div userId='0'>Совпадений не найдено!</div>";

            return retStr;
        }

        // Создание диалога
        // -----------------------------------------------------
        // Id - Идентификатор пользователя
        // -----------------------------------------------------
        [HttpGet]
        public ActionResult AddDialogLK(int Id)
        {
            if (Id <= 0)
                return new HttpStatusCodeResult(500);

            var addDialog = FuncAddDialog(Id);

            if (addDialog == 0)
                return new HttpStatusCodeResult(500);

            return RedirectToAction("Dialogs", new { Id = addDialog });
        }

        // Создание диалога
        // -----------------------------------------------------
        // Id - Идентификатор пользователя
        // -----------------------------------------------------
        [HttpPost]
        public ActionResult AddDialog(int Id)
        {
            if (Id <= 0)
                return Content("0");

            var addDialog = FuncAddDialog(Id);

            if (addDialog == 0)
                return Content("0");

            return Content($"{addDialog};{db.Users.Find(Id).Option.Ava}");
        }

        // Получения сообщений для диалога
        // -----------------------------------------------------
        // Id - Идентификатор диалога
        // -----------------------------------------------------
        [HttpPost]
        public async Task<ActionResult> DialogMessage(int Id)
        {
            if (Id != 0)
            {
                var id = User.Identity.GetUserId<int>();
                var dialog = await db.Dialogs.FindAsync(Id);

                foreach (var d in dialog.Messages.Where(m => m.Author.Id != id && !m.View))
                {
                    d.View = true;
                }
                await db.SaveChangesAsync();

                ViewBag.date = new DateTime(1970, 1, 1);
                var messages = new List<MessageDialog>();

                if (dialog.Messages.Count > 0)
                    messages = dialog.Messages.OrderByDescending(m => m.DateCreate).ToList();
                    
                if (dialog.Messages.Count > 100)
                    messages = messages.GetRange(0, 100);

                return PartialView("_DialogMessage", messages);
            }
            return Content("");
        }

        // Добавление сообщения 
        // -----------------------------------------------------
        // Id - Идентификатор диалога, Text - Текст сообщения
        // -----------------------------------------------------
        [HttpPost]
        public async Task<ActionResult> AddMessage(int Id, string Text)
        {
            if (Id != 0 && Text != "" && Text != null)
            {
                var id = User.Identity.GetUserId<int>();
                var dialog = await db.Dialogs.FindAsync(Id);
                if (dialog != null && (dialog.AuthorId == id || dialog.InvitationId == id))
                {
                    ViewBag.date = (dialog.Messages.Count > 0) ? dialog.Messages.Last().DateCreate : new DateTime(1970, 1, 1);
                    var message = new List<MessageDialog>();

                    message.Add(new MessageDialog()
                    {
                        Dialog = dialog,
                        Author = await db.Users.FindAsync(id),
                        DateCreate = DateTime.UtcNow,
                        Text = Text
                    });

                    db.MessagesDialog.Add(message.First());

                    if (dialog.DelAuthor)
                        dialog.DelAuthor = false;

                    if (dialog.DelInvitation)
                        dialog.DelInvitation = false;

                    await db.SaveChangesAsync();
                    return PartialView("_DialogMessage", message);
                }
            }
            return Content("");
        }

        // Слушатель новых сообщений
        // -----------------------------------------------------
        // Id - Идентификатор диалога
        // -----------------------------------------------------
        [HttpPost]
        public async Task<ActionResult> NewMessage(int Id)
        {
            var id = User.Identity.GetUserId<int>();
            var dialog = await db.Dialogs.FindAsync(Id);
            var messages = dialog.Messages.Where(m => m.Author.Id != id && !m.View).ToList();

            if (dialog != null && messages.Count > 0)
            {
                ViewBag.date = dialog.Messages.Where(m => m.View).Last().DateCreate;
                foreach (var m in messages)
                    m.View = true;
                await db.SaveChangesAsync();
                return PartialView("_DialogMessage", messages);
            }
            
            return Content("0");
        }
        
        // Проверка новых сообщений
        // -----------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<int> CheckNewMessage()
        {
            var id = User.Identity.GetUserId<int>();
            var user = await db.Users.FindAsync(id);
            var dialogs = new List<Dialog>();
            var count = 0;

            dialogs.AddRange(user.AuthorDialog);
            dialogs.AddRange(user.InvitationDialog);

            foreach (var dialog in dialogs)
                count += dialog.Messages.Where(m => !m.View && m.Author.Id != id).Count();

            return count;
        }

        // Удаление диалога
        // -----------------------------------------------------
        public async Task DelDialog(int Id)
        {
            var dialog = await db.Dialogs.FindAsync(Id);

            if (dialog.AuthorId == User.Identity.GetUserId<int>())
                dialog.DelAuthor = true;
            else
                dialog.DelInvitation = true;

            if (dialog.DelAuthor && dialog.DelInvitation)
            {
                db.MessagesDialog.RemoveRange(dialog.Messages);
                db.Dialogs.Remove(dialog);
            }

            await db.SaveChangesAsync();
        }

        // Студии
        // -----------------------------------------------------
        public ActionResult Studios()
        {
            var id = User.Identity.GetUserId<int>();
            var user = db.Users.Find(id);
            UpdateLastOnline(user);
            ViewBag.Ava = user.Option.Ava;
            return View(user.Studios);
        }

        // Новая студия
        // -----------------------------------------------------
        public ActionResult NewStudio()
        {
            ViewBag.Ava = GetAvaCookie();
            return View();
        }

        // Комментарии
        // -----------------------------------------------------
        public ActionResult Comments()
        {
            ViewBag.Ava = GetAvaCookie();
            return View();
        }

        // Настройки
        // -----------------------------------------------------
        public ActionResult Settings()
        {
            var id = User.Identity.GetUserId<int>();
            var user = db.Users.Find(id);
            UpdateLastOnline(user);
            ViewBag.Ava = user.Option.Ava;
            return View(user);
        }

        // Изменение аватарки
        // ---------------------------------------------------------------------------
        // ava - Картинка, Х - координата Х, У - координата У, W - ширина, H - высота
        // ---------------------------------------------------------------------------
        [HttpPost]
        public async Task<string> AvaEdit(HttpPostedFileBase ava, string X, string Y, string W, string H)
        {
            if (ava != null && X != null && Y != null && W != null && H != null)
            {
                var imgSize = ava.ContentLength;
                var imgType = ava.ContentType;

                if (imgSize < 10000000 && imgType.IndexOf("image") > -1)
                {
                    var id = User.Identity.GetUserId<int>();
                    var user = await db.Users.FindAsync(id);

                    var imgName = PathImg.Inspection(ava.FileName, "~/Content/style/images/Users/Avatars/Normal/");
                    var saveImg = FramResImg(ava, X, Y, W, H, "~/Content/style/images/Users/Avatars/Reduced/", imgName, 80, 80);
                    if (saveImg)
                    {
                        saveImg = FramResImg(ava, X, Y, W, H, "~/Content/style/images/Users/Avatars/Normal/", imgName, 200, 200);

                        if (saveImg)
                        {
                            var oldName = user.Option.Ava;
                            user.Option.Ava = imgName;
                            await db.SaveChangesAsync();

                            HttpCookie cookie = Request.Cookies["cookieWork"];
                            if (cookie.Value == "1")
                            {
                                cookie = Request.Cookies["ava"];
                                cookie.Value = imgName;
                            }

                            if (oldName != "defaultAva.jpg")
                            {
                                if (System.IO.File.Exists(Server.MapPath($"~/Content/style/images/Studios/Avatars/Normal/{oldName}")))
                                    System.IO.File.Delete(Server.MapPath($"~/Content/style/images/Studios/Avatars/Normal/{oldName}"));
                                if (System.IO.File.Exists(Server.MapPath($"~/Content/style/images/Studios/Avatars/Reduced/{oldName}")))
                                    System.IO.File.Delete(Server.MapPath($"~/Content/style/images/Studios/Avatars/Reduced/{oldName}"));
                            }
                            
                            return imgName;
                        }
                    }
                }
            }
                
            return "0";
        }

        // Получение Аватарки из cookie
        // -----------------------------------------------------
        [HttpPost]
        private string GetAvaCookie(string UpdateAva = null)
        {
            var ava = "defaultAva.jpg";
            HttpCookie cookie = Request.Cookies["cookieWork"];
            if (cookie.Value == "1")
            {
                cookie = Request.Cookies["ava"];
                if (UpdateAva != null)
                {
                    if (cookie == null)
                        cookie = new HttpCookie("ava");
                    cookie.Value = UpdateAva;
                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(cookie);
                }
                ava = (cookie != null) ? cookie.Value : ava;
            }
            return ava;
        }

        // Добавление города
        // -----------------------------------------------------
        [HttpPost]
        public async Task<string> AddCity(string SizeCity, string AddCity, int Index)
        {
            if (AddCity != null && AddCity != "" && Index != 0)
            {
                var id = User.Identity.GetUserId<int>();
                var option = await db.OptionUsers.FirstOrDefaultAsync(o => o.Id == id);
                EmailMessage.SendMessage("goodjokerteam@gmail.com", "Добавление населённого пункта", $"Индекс: {Index}<br />Тип населённого пункта: {SizeCity}<br />Адрес: {AddCity}<br />С уважением от пользователя {User.Identity.Name},<br />С идентификатором {id}");

                option.ExactAddress = AddCity;
                await db.SaveChangesAsync();
                return "1";
            }
            return "0";
        }

        // Поиск города
        // -----------------------------------------------------
        // search - поисковый запрос
        // -----------------------------------------------------
        [HttpPost]
        public async Task<ActionResult> _SettingSearchCity(string search)
        {
            var cities = await db.Cities.Where(c => c.Name.Contains(search) || c.Index.ToString().Contains(search)).Take(6).ToListAsync();
            return PartialView(cities);
        }

        // Изменение настроек
        // ------------------------------------------------------
        // NewOpt - Настройки которые пользователь хочет изменить
        // ------------------------------------------------------
        [HttpPost]
        public async Task<string> ChangeOption(ChangeOption NewOpt)
        {
            var id = User.Identity.GetUserId<int>();
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            var errorString = "";
            var flagEmail = false;
            var defaultDate = new DateTime(1, 1, 1, 0, 0, 0);

            if (user.Email != NewOpt.Email && NewOpt.Email != null && NewOpt.Email != "")
            {
                if (NewOpt.Email.IndexOf('@') > -1 && NewOpt.Email.IndexOf('.') > -1 && NewOpt.Email.Length > 8)
                {
                    var confEmail = await db.Users.FirstOrDefaultAsync(u => u.Email == NewOpt.Email);
                    if (confEmail == null)
                    {
                        user.Token = Token.GetToken();
                        var Subject = "GoodJoker - Подтверждение E-mail.";
                        var Body = "<h1 style='margin: 0; padding: 0; '>GoodJoker</h1>"
                                 + "<p>Для подтверждения Вашего нового почтового адресса, пожалуйста перейдите по ссылке: <a href='https://goodjoker.ru/Account/ConfirmEmail"
                                 + "?Id=" + user.Id
                                 + "&Token=" + user.Token
                                 + "&Email=" + NewOpt.Email
                                 + "'>Подтвердить</a></p>";
                        EmailMessage.SendMessage(NewOpt.Email, Subject, Body);
                        flagEmail = true;
                    }
                    else
                        errorString = "<li>Этот E-mail уже зарегистрирован в нашей базе данных!</li>";
                }
                else
                    errorString = "<li>Введите корректный email.</li>";
            }

            if (user.Nick != NewOpt.Nick && NewOpt.Nick != null && NewOpt.Nick != "")
            {
                string pattern = @"^[а-яА-ЯёЁa-zA-Z0-9]+$";
                Regex regex = new Regex(pattern);
                if ((regex.IsMatch(NewOpt.Nick) || User.Identity.GetUserRole() == "Admin") && NewOpt.Nick.IndexOf(' ') == -1 && NewOpt.Nick.Length >= 3 && NewOpt.Nick.Length <= 20)
                {
                    if (await db.Users.FirstOrDefaultAsync(u => u.Nick == NewOpt.Nick) == null)
                    {
                        user.Nick = NewOpt.Nick;
                        ClaimsIdentity claim = new ClaimsIdentity("ApplicationCookie",
                            ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                        claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String));
                        claim.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Nick, ClaimValueTypes.String));
                        claim.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "OWIN Provider", ClaimValueTypes.String));
                        if (user.Role.Name != "")
                            claim.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name, ClaimValueTypes.String));

                        AuthenticationManager.SignOut();

                        var ctx = Request.GetOwinContext();
                        var authenticationManager = ctx.Authentication;
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);
                    }
                    else
                        errorString += "<li>Пользователь с таким ником уже существует.</li>";
                }
                else
                    errorString += "<li>Ник (разрешено использовать буквы и цифры без пробелов, длина от 3 до 20 символов).</li>";
            }
            

            if (user.Option.FirstName != NewOpt.FirstName)
            {
                if (NewOpt.FirstName?.IndexOf(' ') > -1)
                    NewOpt.FirstName.Replace(" ", "");

                user.Option.FirstName = NewOpt.FirstName;
            }

            if (user.Option.MiddleName != NewOpt.MiddleName)
            {
                if (NewOpt.MiddleName?.IndexOf(' ') > -1)
                    NewOpt.MiddleName.Replace(" ", "");

                user.Option.MiddleName = NewOpt.MiddleName;
            }

            if (user.Option.LastName != NewOpt.LastName)
            {
                if (NewOpt.LastName?.IndexOf(' ') > -1)
                    NewOpt.LastName.Replace(" ", "");

                user.Option.LastName = NewOpt.LastName;
            }

            if (user.Option.Phone != NewOpt.Phone)
            {
                if ( NewOpt.Phone.Length == 17 || (NewOpt.Phone != null && NewOpt.Phone != "") )
                    user.Option.Phone = NewOpt.Phone;
                else
                    errorString += "<li>Телефон должен состоять из 11 цифр.</li>";
            }

            if (NewOpt.NowPass != null && NewOpt.NewPass != null && NewOpt.ConfPass != null && NewOpt.NowPass != "" && NewOpt.NewPass != "" && NewOpt.ConfPass != "")
            {
                if (NewOpt.NewPass.Length > 5)
                {
                    if (NewOpt.NewPass == NewOpt.ConfPass)
                    {
                        NewOpt.NowPass = Hash.Get(NewOpt.NowPass);
                        if (user.HashPass == NewOpt.NowPass)
                            user.HashPass = Hash.Get(NewOpt.NewPass);
                    }
                    else
                        errorString += "<li>Новый пароль не совпадает с поролем подтверждения.</li>";
                }
                else
                    errorString += "<li>Пароль должен состоять из 6 или более символов.</li>";
            }
            
            if (NewOpt.City > 1 && user.Option.City.Id != NewOpt.City)
            {
                var city = await db.Cities.FindAsync(NewOpt.City);
                user.Option.City = city;
                if (NewOpt.BigCity <= 1)
                    user.Option.BigCity = city;
            }

            if (NewOpt.BigCity > 1 && user.Option.BigCity.Id != NewOpt.BigCity)
            {
                var city = await db.Cities.FindAsync(NewOpt.BigCity);
                user.Option.BigCity = city;
                if (NewOpt.City <= 1)
                    user.Option.City = city;
            }

            if (user.Option.ExactAddress != NewOpt.ExactAddress)
                user.Option.ExactAddress = NewOpt.ExactAddress;

            if (user.Option.GeoMap != NewOpt.GeoMap)
                user.Option.GeoMap = NewOpt.GeoMap;

            if (user.Option.Birthday.Date != NewOpt.Birthday.Date && NewOpt.Birthday.Date != defaultDate)
                user.Option.Birthday = NewOpt.Birthday.AddHours(14);

            if (user.Option.Gender != NewOpt.Gender)
                user.Option.Gender = NewOpt.Gender;

            if (errorString == "")
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();

                if (flagEmail)
                    return "email";
                else
                    return "1";
            }
            else
                return errorString;
        }

        // Удаление привязки
        //------------------------------------------------------
        // name - Имя социальной сети
        //------------------------------------------------------
        [HttpPost]
        public async Task DeleteSocial(string name)
        {
            var id = User.Identity.GetUserId<int>();
            var social = await db.SocialNetworks.FirstOrDefaultAsync(s => s.UserId == id && s.Name == name);
            if (social != null)
            {
                db.SocialNetworks.Remove(social);
                await db.SaveChangesAsync();
            }
        }

        // Функция кадрирования и ресайза изображения
        //---------------------------------------------------------------------------------
        // картинка, координаты Х, У, ширина, высота, путь сохранения, MAXширина, MAXвысота
        //---------------------------------------------------------------------------------
        public bool FramResImg(HttpPostedFileBase img, string X, string Y, string W, string H, string pathSave, string imgName, int maxW, int maxH)
        {
            string inmedPath = Server.MapPath("~/Content/style/images/teamp/");
            if (img != null && X != null && Y != null && W != null && H != null && pathSave != null)
            {
                var x = X.Split(new char[] { '.' });
                var y = Y.Split(new char[] { '.' });
                var w = W.Split(new char[] { '.' });
                var h = H.Split(new char[] { '.' });
                var corX = int.Parse(x[0]);
                var corY = int.Parse(y[0]);
                var width = int.Parse(w[0]);
                var height = int.Parse(h[0]);

                //Create a new image from the specified location to
                img.SaveAs(inmedPath + img.FileName);
                string sourceFile = Request.MapPath("~/Content/style/images/teamp/" + img.FileName);

                Image oImage = Bitmap.FromFile(sourceFile);

                // Create a new bitmap with specified parameters
                var bmp = new Bitmap(width, height, oImage.PixelFormat);
                var g = Graphics.FromImage(bmp);
                g.DrawImage(oImage, new Rectangle(0, 0, width, height), new Rectangle(corX, corY, width, height), GraphicsUnit.Pixel);

                System.Drawing.Imaging.ImageFormat frm = oImage.RawFormat;
                oImage.Dispose();

                string destFile = Request.MapPath("~/Content/style/images/teamp/" + img.FileName);

                // Save cropped image
                bmp.Save(destFile, frm);
                
                //var sufix = $"maxwidth={maxW}&maxheight={maxH}&format=jpg";
                pathSave = Server.MapPath(pathSave);

                WebImage imgSave = new WebImage("~/Content/style/images/teamp/" + img.FileName);
                imgSave.Resize(maxW, maxH, true, true);
                imgSave.Save($"{pathSave}{imgName}", "JPG", true);

                System.IO.File.Delete(Server.MapPath($"~/Content/style/images/teamp/" + img.FileName));

                return true;
            }
            else
                return false;
        } // end FramResImg
        
        public int FuncAddDialog(int Id)
        {
            if (Id > 0)
            {
                var invit = db.Users.Find(Id);

                var id = User.Identity.GetUserId<int>();
                var author = db.Users.Find(id);

                if (invit != null && Id != id)
                {
                    var dialog = db.Dialogs.FirstOrDefault(d => (d.AuthorId == id && d.InvitationId == Id) || (d.InvitationId == id && d.AuthorId == Id));

                    if (dialog == null)
                    {
                        dialog = new Dialog()
                        {
                            Invitation = invit,
                            Author = author,
                            Messages = new List<MessageDialog>()
                        };
                        if (Id == 2)
                            dialog.Messages.Add(new MessageDialog()
                            {
                                Author = invit,
                                DateCreate = DateTime.UtcNow,
                                Text = "Здравствуйте, чем мы можем Вам помочь?",
                                View = true
                            });
                        db.Dialogs.Add(dialog);
                    }
                    else
                    {
                        dialog.DelAuthor = false;
                        dialog.DelInvitation = false;
                        var message = dialog.Messages.Where(m => m.Author.Id != id && !m.View).ToList();
                        foreach (var m in message)
                            m.View = true;
                    }

                    db.SaveChanges();
                    return dialog.Id;
                }
            }

            return 0;
        }

        public void UpdateLastOnline(User User)
        {
            User.LastOnline = DateTime.UtcNow;
            db.SaveChanges();
        }
        
        // Разрыв соединения с БД
        //------------------------------------------------
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