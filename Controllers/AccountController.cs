using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using GoodJoker.Models;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using RestSharp;

namespace GoodJoker.Controllers
{
    public class AccountController : Controller
    {
        // Работа с Owin авторизацией
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        // Инициализация входа в Базу Данных
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // VKontakte
        // ------------------------------------------------------------------------
        private const string client_id_vk = "5869965";
        private const string client_secret_vk = "McqYeIIjKs7qeQv792dV";

        // Facebook
        // ------------------------------------------------------------------------
        private const string client_id_fb = "257488578009120";
        private const string client_secret_fb = "90ffe8c19bfc076f4a65aae68e4d9e30";

        // Odnoklassniki
        // ------------------------------------------------------------------------
        private const string client_id_ok = "1250916352";
        private const string client_secret_ok = "B649FD9314CDC3A2EBE3C9AE";
        private const string public_key_ok = "CBAJHQILEBABABABA";

        // Google
        // ------------------------------------------------------------------------
        private const string client_id_gl = "406011974796-g5qccf92uhm8lc4k3avvlgcatb8ptb79.apps.googleusercontent.com";
        private const string client_secret_gl = "im6_jpegOb_xawrtiFukmTfj";

        private const string domen = "https://goodjoker.ru";

        public async Task<string> Login(Login Login)
        {
            if (!ModelState.IsValid)
                return "Проверьте правильность введенных данных";

            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == Login.Email);
            var pass = Hash.Get(Login.Pass);

            if (user != null && !user.ConfirmEmail)
            {
                return $"Подтвердите Ваш почтовый адресс!<br /><a href='/Account/MailEmailConfirm?Email={user.Email}'>Отправить письмо с подтверждением повторно</a>";
            }

            if (user != null && pass == user.HashPass)
            {
                AuthClaim(user.Id.ToString(), user.Nick, user.Role.Name);
                return "1";
            }
            else
                return "Неправильный логин или пароль! Проверьте раскладку клавиатуры или нажатие клавиши \"Caps Lock\"";
        }

        [Authorize]
        public ActionResult Logout()
        {
            var id = User.Identity.GetUserId<int>();
            var user = db.Users.Find(id);
            AuthenticationManager.SignOut();
            System.Web.HttpCookie cookie = Request.Cookies["cookieWork"];
            if (cookie.Value == "1")
            {
                cookie = Request.Cookies["ava"];

                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }
            }
            user.LastOnline = DateTime.UtcNow;
            db.SaveChanges();
            return RedirectToAction("Index", "Main");
        }

        public async Task<string> Registration(Reg Reg)
        {
            if (Reg.Pass.Length < 6)
                return "Пароль должен быть больше 6 символов. Для Вашей безопасности рекомендуем использовать буквы разного регистра, цифры и символы.";

            if (Reg.Email.IndexOf('@') == -1 || Reg.Email.IndexOf('.') == -1)
                return "Проверьте правильность ввода почтового ящика";

            if (Reg.Pass != Reg.PassConfirm)
                return "Поле \"Пароль\" и \"Подтверждение пароля\" не совпадают!";

            var User = await db.Users.FirstOrDefaultAsync(u => u.Email == Reg.Email);

            if (User != null)
                return "Пользователь с таким почтовым адрессом уже зарегистрирован на сайте.<br /><a href=\"/Account/RePass\">Вспомнить пароль...</a>";
            
            var pass = Hash.Get(Reg.Pass);
            var nick = Reg.Email.Substring(0, Reg.Email.IndexOf('@'));
            User = await db.Users.FirstOrDefaultAsync(u => u.Nick == nick);
            var defaultCity = await db.Cities.FindAsync(1);

            if (User != null)
                nick = "Joker_" + (db.Users.Last().Id + 1);

            User = new User()
            {
                Email = Reg.Email,
                Nick = nick,
                HashPass = pass,
                Role = await db.Roles.FirstOrDefaultAsync(r => r.Name == "User"),
                LastOnline = DateTime.UtcNow,
                DateRegistration = DateTime.UtcNow,
                Token = Token.GetToken(),
                Option = new OptionUser()
                {
                    Ava = "defaultAva.jpg",
                    City = defaultCity,
                    BigCity = defaultCity,
                    Birthday = new DateTime(1970, 1, 1)
                }
            };

            db.Users.Add(User);
            await db.SaveChangesAsync();

            var Subject = "GoodJoker - Подтверждение E-mail.";
            var Body = "<h1 style='margin: 0; padding: 0; '>GoodJoker</h1>" +
                       "<h4>Мы рады приветсвовать Вас на нашем проекте</h4>" +
                       "<p>Для подтверждения Вашей почты перейдите по ссылке: " +
                       "<a href='https://goodjoker.ru/Account/ConfirmEmail?Id=" + User.Id + "&Token=" + User.Token + "'>Подтвердить</a></p>";

            EmailMessage.SendMessage(User.Email, Subject, Body);

            return "1";
        }

        public ActionResult ConfirmEmail(int Id, string Token, string Email)
        {
            var User = db.Users.Find(Id);

            if (User != null && User.Token == Token)
            {
                if (Email != null && Email != "")
                    User.Email = Email;
                User.ConfirmEmail = true;
                db.SaveChanges();
                AuthClaim(User.Id.ToString(), User.Nick, User.Role.Name);

                return RedirectToAction("Index", "PrivateOffice");
            }

            return RedirectToAction("Index", "Main");
        }

        [HttpGet]
        public ActionResult ForgotPass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPass(string Email)
        {
            var User = db.Users.FirstOrDefault(u => u.Email == Email);
            User.Token = Token.GetToken();
            db.SaveChanges();
            var Subject = "GoodJoker - Восстановление пароля.";
            var Body = "<h1 style='margin: 0; padding: 0; '>GoodJoker</h1>"
                     + "<h4>Изменение пароля</h4>"
                     + "<p>Для изменения пароля перейдите по ссылке: <a href='https://goodjoker.ru/Account/NewPass?Id=" + User.Id + "&Token=" + User.Token + "'>Изменить</a></p>";

            EmailMessage.SendMessage(User.Email, Subject, Body);

            return RedirectToAction("RedirectEmail", new { Email = Email });
        }

        [HttpGet]
        public ActionResult NewPass(int Id, string Token)
        {
            var user = db.Users.Find(Id);
            if (Token != user.Token)
                return HttpNotFound();
            ViewBag.Id = user.Id;
            ViewBag.Token = user.Token;
            return View();
        }

        [HttpPost]
        public ActionResult NewPass(int Id, string Token, string Pass, string PassConfirm)
        {
            var user = db.Users.Find(Id);
            if (Token != user.Token && Pass != PassConfirm)
            {
                ViewBag.Error = "Поля \"Пароль\" и \"Подтверждение пароля\" не совпадают!";
                return View();
            }
            user.Token = "gj1331";
            user.HashPass = Hash.Get(Pass);
            user.LastOnline = DateTime.UtcNow;
            db.SaveChanges();
            AuthClaim(user.Id.ToString(), user.Nick, user.Role.Name);
            return RedirectToAction("Index", "PrivateOffice");
        }

        public ActionResult MailEmailConfirm (string Email)
        {
            var User = db.Users.FirstOrDefault(u => u.Email == Email);
            var Subject = "GoodJoker - Подтверждение E-mail.";
            var Body = "<h1 style='margin: 0; padding: 0; '>GoodJoker</h1>"
                     + "<h4>Мы рады приветсвовать Вас на нашем проекте</h4>"
                     + "<p>Для подтверждения Вашей почты перейдите по ссылке: <a href='https://goodjoker.ru/Account/ConfirmEmailUser?Id=" + User.Id + "&Token=" + User.Token + "'>Подтвердить</a></p>";

            EmailMessage.SendMessage(User.Email, Subject, Body);

            return RedirectToAction("ConfirmEmail", "Account");
        }

        public ActionResult RedirectEmail(string Email, bool NewPass = false)
        {
            if (Email.IndexOf("@") > -1)
                ViewBag.emailLast = Email.Substring(Email.IndexOf("@") + 1, Email.Length - Email.IndexOf("@") - 1);
            else
                ViewBag.emailLast = "";

            ViewBag.newPass = NewPass;

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> AuthVK(string code, string error)
        {
            if (code != null && error == null)
            {
                string auth;
                string url = domen + "/Account/AuthVK";
                string[] nameParameter = new string[] { "client_id", "client_secret", "code", "redirect_uri", "v" };
                string[] valParameter = new string[] { client_id_vk, client_secret_vk, code, url, "5.62" };
                AuthResultObject vk = new AuthResultObject();
                vk = JsonConvert.DeserializeObject<AuthResultObject>(GetStrOutSite("https://oauth.vk.com/access_token", "get", nameParameter, valParameter));

                if (vk.user_id != null)
                {
                    vk.id = vk.user_id;
                    vk.name = "VK";

                    if (User.Identity.IsAuthenticated)
                        auth = await AuthOrConnect(User.Identity.GetUserId<int>(), vk.id, vk.name, true);
                    else
                    {
                        User user = null;
                        if (vk.email != null)
                            user = await db.Users.FirstOrDefaultAsync(u => u.Email == vk.email);
                        if (user != null)
                            auth = await AuthOrConnect(user.Id, vk.id, vk.name, false);
                        else
                            auth = await AuthOrConnect(0, vk.id, vk.name, false);
                    }

                    ActionResult vkResult = await ReturnUserSocial(auth, vk);

                    return vkResult;
                }
                else
                    return RedirectToAction("Index", "Main");
            }
            else
                return RedirectToAction("Index", "Main");
        }

        [HttpGet]
        public async Task<ActionResult> AuthFB(string code, string error)
        {
            if (code != null && error == null)
            {
                string auth;
                string url = domen + "/Account/AuthFB";
                string[] nameParameter = new string[] { "client_id", "client_secret", "code", "redirect_uri" };
                string[] valParameter = new string[] { client_id_fb, client_secret_fb, code, url };
                AuthResultObject fb = new AuthResultObject();

                fb = JsonConvert.DeserializeObject<AuthResultObject>(GetStrOutSite("https://graph.facebook.com/oauth/access_token", "get", nameParameter, valParameter));

                nameParameter = new string[] { "fields", "access_token" };
                valParameter = new string[] { "id, name, first_name, last_name, gender, email", fb.access_token };

                fb = JsonConvert.DeserializeObject<AuthResultObject>(GetStrOutSite("https://graph.facebook.com/me", "get", nameParameter, valParameter));

                if (fb.id != null)
                {
                    fb.name = "FB";

                    if (User.Identity.IsAuthenticated)
                        auth = await AuthOrConnect(User.Identity.GetUserId<int>(), fb.id, fb.name, true);
                    else
                    {
                        User user = null;
                        if (fb.email != null)
                            user = await db.Users.FirstOrDefaultAsync(u => u.Email == fb.email);
                        if (user != null)
                            auth = await AuthOrConnect(user.Id, fb.id, fb.name, false);
                        else
                            auth = await AuthOrConnect(0, fb.id, fb.name, false);
                    }

                    ActionResult fbResult = await ReturnUserSocial(auth, fb);

                    return fbResult;
                }
                else
                    return RedirectToAction("Index", "Main");
            }
            else
                return RedirectToAction("Index", "Main");
        }

        [HttpGet]
        public async Task<ActionResult> AuthOK(string code, string error, string email)
        {
            if (code != null && error == null)
            {
                string auth;
                string url = domen + "/Account/AuthOK";
                string[] nameParameter = new string[] { "code", "client_id", "client_secret", "redirect_uri", "grant_type" };
                string[] valParameter = new string[] { code, client_id_ok, client_secret_ok, url, "authorization_code" };
                AuthResultObject ok = new AuthResultObject();

                ok = JsonConvert.DeserializeObject<AuthResultObject>(GetStrOutSite("https://api.ok.ru/oauth/token.do", "post", nameParameter, valParameter));

                var hash = Hash.Get($"application_key={public_key_ok}method=users.getCurrentUser{Hash.Get(ok.access_token + client_secret_ok)}");
                nameParameter = new string[] { "method", "access_token", "application_key", "sig" };
                valParameter = new string[] { "users.getCurrentUser", ok.access_token, public_key_ok, hash };
                
                ok = JsonConvert.DeserializeObject<AuthResultObject>(GetStrOutSite("http://api.odnoklassniki.ru/fb.do", "get", nameParameter, valParameter));

                if (ok.uid != null)
                {
                    ok.id = ok.uid;
                    ok.name = "OK";

                    if (User.Identity.IsAuthenticated)
                        auth = await AuthOrConnect(User.Identity.GetUserId<int>(), ok.id, ok.name, true);
                    else
                        auth = await AuthOrConnect(0, ok.id, ok.name, false);

                    ActionResult okResult = await ReturnUserSocial(auth, ok);

                    return okResult;
                }
                else
                    return RedirectToAction("Index", "Main");
            }
            else
                return RedirectToAction("Index", "Main");
        }

        [HttpGet]
        public async Task<ActionResult> AuthGL(string code, string error)
        {
            if (code != null && error == null)
            {
                string auth;
                string url = domen + "/Account/AuthGL";
                string[] nameParameter = new string[] { "client_id", "client_secret", "code", "redirect_uri", "grant_type" };
                string[] valParameter = new string[] { client_id_gl, client_secret_gl, code, url, "authorization_code" };
                AuthResultObject gl = new AuthResultObject();

                gl = JsonConvert.DeserializeObject<AuthResultObject>(GetStrOutSite("https://accounts.google.com/o/oauth2/token", "post", nameParameter, valParameter));

                nameParameter = new string[] { "access_token" };
                valParameter = new string[] { gl.access_token };

                gl = JsonConvert.DeserializeObject<AuthResultObject>(GetStrOutSite("https://www.googleapis.com/oauth2/v1/userinfo", "get", nameParameter, valParameter));

                if (gl.id != null)
                {
                    gl.name = "GL";

                    if (User.Identity.IsAuthenticated)
                        auth = await AuthOrConnect(User.Identity.GetUserId<int>(), gl.id, gl.name, true);
                    else
                    {
                        User user = null;
                        if (gl.email != null)
                            user = await db.Users.FirstOrDefaultAsync(u => u.Email == gl.email);
                        if (user != null)
                            auth = await AuthOrConnect(user.Id, gl.id, gl.name, false);
                        else
                            auth = await AuthOrConnect(0, gl.id, gl.name, false);
                    }

                    ActionResult glResult = await ReturnUserSocial(auth, gl);

                    return glResult;
                }
                else
                    return RedirectToAction("Index", "Main");
            }
            else
                return RedirectToAction("Index", "Main");
        }

        // Функция удаления соединения аккаунта соц.сети с аккаунтом Vgorodah
        [Authorize]
        public async Task DeleteConnectSocail(string nameSocial)
        {
            var userId = User.Identity.GetUserId<int>();
            SocialNetwork del = await db.SocialNetworks.FirstOrDefaultAsync(n => n.UserId == userId && n.Name == nameSocial);
            if (del != null)
            {
                db.SocialNetworks.Remove(del);
                await db.SaveChangesAsync();
            }
        }

        // Функция получения строки со сторонних сайтов (адресс сайта, get или post, массив имён параметров, массив значений)
        //---------------------------------------------------------------------------------------------------------------------
        public static string GetStrOutSite(string url, string method, string[] nameParameter, string[] valParameter)
        {
            var client = new RestClient(url);
            var request = new RestRequest((method == "get") ? Method.GET : Method.POST);

            for (int i = 0; i < nameParameter.Length; i++)
                request.AddParameter(nameParameter[i], valParameter[i]);

            var response = client.Execute(request);
            return response.Content;
        }

        // Функция авторизации и входа через социальные сети
        // ( id пользователя, id пользователя из соц.сети, название соц.сети("VK","FB","OK","GL"), флаг авторизации )
        //---------------------------------------------------------------------------------------------------------------------
        public async Task<string> AuthOrConnect(int UserId, string SoclUserId, string NameSocNet, bool Auth)
        {
            User user = new User();
            SocialNetwork searchConnect = new SocialNetwork();
            if (SoclUserId != null && SoclUserId != "" && NameSocNet != null)
            {
                if (UserId != 0)
                {
                    user = await db.Users.FirstOrDefaultAsync(u => u.Id == UserId);
                    searchConnect = user.SocialNetworks.FirstOrDefault(x => x.SocialId == SoclUserId + NameSocNet);
                }
                else
                    searchConnect = await db.SocialNetworks.FirstOrDefaultAsync(x => x.SocialId == SoclUserId + NameSocNet);

                if (searchConnect == null && UserId != 0)
                {
                    var checkSocial = await db.SocialNetworks.FirstOrDefaultAsync(s => s.SocialId == SoclUserId + NameSocNet);
                    if (checkSocial == null)
                    {
                        db.SocialNetworks.Add(new SocialNetwork() { SocialId = SoclUserId + NameSocNet, Name = NameSocNet, UserId = user.Id, User = user });
                        await db.SaveChangesAsync();
                        if (!Auth)
                        {
                            AuthClaim(user.Id.ToString(), user.Nick, user.Role.Name);
                            user.LastOnline = DateTime.UtcNow;
                            await db.SaveChangesAsync();
                            return "auth";
                        }
                        return "add";
                    }
                    else
                    {
                        AuthClaim(checkSocial.UserId.ToString(), checkSocial.User.Nick, checkSocial.User.Role.Name);
                        user.LastOnline = DateTime.UtcNow;
                        await db.SaveChangesAsync();
                        return "auth";
                    }
                }
                else if (searchConnect != null && UserId == 0)
                {
                    user = await db.Users.FirstOrDefaultAsync(u => u.Id == searchConnect.UserId);
                    if (!user.Option.Ban)
                    {
                        AuthClaim(user.Id.ToString(), user.Nick, user.Role.Name);
                        user.LastOnline = DateTime.UtcNow;
                        await db.SaveChangesAsync();
                        return "auth";
                    }
                    else
                        return "ban";
                }
                else if (searchConnect != null && UserId != 0)
                {
                    if (!user.Option.Ban)
                    {
                        AuthClaim(user.Id.ToString(), user.Nick, user.Role.Name);
                        user.LastOnline = DateTime.UtcNow;
                        await db.SaveChangesAsync();
                        return "auth";
                    }
                    else
                        return "ban";
                }
                else
                    return "reg";
            }
            else
                return "none";
        }

        // Функция регистрации через соц. сети
        //---------------------------------------------------------------------------------------------------------------------
        public async Task<ActionResult> ReturnUserSocial (string auth, AuthResultObject social)
        {
            if (auth == "add")
                return RedirectToAction("Settings", "PrivateOffice");
            else if (auth == "auth")
                return RedirectToAction("Index", "PrivateOffice");
            else if (auth == "ban")
                return RedirectToAction("Index", "Main");
            else if (auth == "reg")
            {
                var nick = "";
                if (social.email != null)
                    nick = social.email.Substring(0, social.email.IndexOf("@"));
                else
                    nick = "Joker_" + (db.Users.ToList().Last().Id + 1);

                var flag = true;
                var i = 0;
                do
                {
                    var user = await db.Users.FirstOrDefaultAsync(u => u.Nick == nick);
                    if (user != null)
                        nick += i;
                    else
                        flag = false;
                    ++i;
                } while (flag);

                byte gender = 0;
                if (social.gender == "male") gender = 1;
                if (social.gender == "female") gender = 2;

                var birthday = social.birthday?.Split('-');
                var defaultCity = await db.Cities.FindAsync(1);
                var socialNet = new SocialNetwork() { SocialId = social.id + social.name, Name = social.name };

                var User = new User()
                {
                    Role = await db.Roles.FirstOrDefaultAsync(r => r.Name == "User"),
                    Email = social.email,
                    ConfirmEmail = (social.email != null) ? true : false,
                    Nick = nick,
                    LastOnline = DateTime.UtcNow,
                    DateRegistration = DateTime.UtcNow,
                    SocialNetworks = new List<SocialNetwork> { socialNet },
                    Option = new OptionUser()
                    {
                        Ava = "defaultAva.jpg",
                        FirstName = social.last_name,
                        MiddleName = social.first_name,
                        Gender = gender,
                        City = defaultCity,
                        BigCity = defaultCity,
                        Birthday = (birthday != null) ? new DateTime(int.Parse(birthday[0]), int.Parse(birthday[1]), int.Parse(birthday[2])) : new DateTime(1970, 1, 1, 14, 14, 14)
                    }
                };

                db.Users.Add(User);
                await db.SaveChangesAsync();

                AuthClaim(User.Id.ToString(), User.Nick, User.Role.Name);
                return RedirectToAction("Index", "PrivateOffice");
            }

            return RedirectToAction("Index", "Main");
        }

        public void AuthClaim (string Id, string Nick, string Role)
        {
            ClaimsIdentity claim = new ClaimsIdentity("ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, Id, ClaimValueTypes.String));
            claim.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, Nick, ClaimValueTypes.String));
            claim.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "OWIN Provider", ClaimValueTypes.String));
            claim.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, Role, ClaimValueTypes.String));

            AuthenticationManager.SignOut();

            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            AuthenticationManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = true
            }, claim);
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