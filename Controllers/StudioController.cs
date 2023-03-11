using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GoodJoker.Models;
using ImageResizer;
using System.Drawing;
using System.Web.Helpers;

namespace GoodJoker.Controllers
{
    [Authorize]
    public class StudioController : Controller
    {
        // Инициализация входа в Базу Данных
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        
        // Создание студии
        [HttpPost]
        public async Task<int> Create(string Name)
        {
            if (Name != null && Name != "" && Name.Length >= 3 && Name.Length <= 20)
            {
                var id = User.Identity.GetUserId<int>();
                var user = await db.Users.FindAsync(id);
                var role = await db.StudioRolesMember.Where(r => r.Name == "Creator" || r.Name == "ViewTeam").ToListAsync();

                var team = new StudioTeam()
                {
                    ConfirmMember = true,
                    Member = user,
                    Role = role
                };

                var newStudio = new Studio()
                {
                    Name = Name,
                    DateCreate = DateTime.UtcNow,
                    Option = new OptionStudio() {
                        Ava = "defaultAva.jpg",
                        Cover = "defaultCover.jpg",
                        BeginVIP = DateTime.UtcNow.AddHours(-1),
                        EndVIP = DateTime.UtcNow.AddHours(-1)
                    },
                    Team = new List<StudioTeam>() { team }
                };

                db.Studios.Add(newStudio);
                await db.SaveChangesAsync();

                return newStudio.Id;
            }
            return 0;
        }

        // Изменение аватарки
        [HttpPost]
        public async Task<string> AvaEdit(HttpPostedFileBase Ava, string X, string Y, string W, string H, int Id)
        {
            if (Ava != null && X != null && Y != null && W != null && H != null)
            {
                var imgSize = Ava.ContentLength;
                var imgType = Ava.ContentType;

                if (imgSize < 10000000 && imgType.IndexOf("image") > -1)
                {
                    var id = User.Identity.GetUserId<int>();
                    var user = await db.Users.FindAsync(id);
                    var myRole = user.Studios.FirstOrDefault(s => s.Studio.Id == Id && s.Member.Id == id);

                    if (myRole != null && myRole.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin" || r.Name == "EditInfo") != null)
                    {
                        var imgName = PathImg.Inspection(Ava.FileName, "~/Content/style/images/Studios/Avatars/Normal/");
                        var saveImg = FramResImg(Ava, X, Y, W, H, "~/Content/style/images/Studios/Avatars/Reduced/", imgName, 80, 80);
                        if (saveImg)
                        {
                            saveImg = FramResImg(Ava, X, Y, W, H, "~/Content/style/images/Studios/Avatars/Normal/", imgName, 200, 200);
                            if (saveImg)
                            {
                                var ava = myRole.Studio.Option.Ava;
                                if (ava != "defaultAva.jpg")
                                {
                                    if (System.IO.File.Exists(Server.MapPath($"~/Content/style/images/Studios/Avatars/Normal/{ava}")))
                                        System.IO.File.Delete(Server.MapPath($"~/Content/style/images/Studios/Avatars/Normal/{ava}"));
                                    if (System.IO.File.Exists(Server.MapPath($"~/Content/style/images/Studios/Avatars/Reduced/{ava}")))
                                        System.IO.File.Delete(Server.MapPath($"~/Content/style/images/Studios/Avatars/Reduced/{ava}"));
                                }
                                
                                myRole.Studio.Option.Ava = imgName;
                                await db.SaveChangesAsync();
                                return imgName;
                            }
                        }
                    }
                }
            }
            return "0";
        }

        // Изменение обложки
        [HttpPost]
        public async Task<string> CoverEdit(HttpPostedFileBase Cover, int Id)
        {
            if (Cover != null)
            {
                var imgSize = Cover.ContentLength;
                var imgType = Cover.ContentType;

                if (imgSize < 10000000 && imgType.IndexOf("image") > -1)
                {
                    var id = User.Identity.GetUserId<int>();
                    var user = await db.Users.FindAsync(id);
                    var myRole = user.Studios.FirstOrDefault(s => s.Studio.Id == Id && s.Member.Id == id);

                    if (myRole != null && myRole.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin" || r.Name == "EditInfo") != null)
                    {
                        WebImage img = new WebImage(Cover.InputStream);
                        var oldImg = myRole.Studio.Option.Cover;
                        var imgName = PathImg.Inspection($"{Cover.FileName}", "~/Content/style/images/Studios/Covers/");

                        myRole.Studio.Option.Cover = imgName;
                        img.Resize(1200, 550, true, true);
                        img.Save($"~/Content/style/images/Studios/Covers/{imgName}", "JPG", true);

                        await db.SaveChangesAsync();

                        if (oldImg != "defaultCover.jpg")
                        {
                            if (System.IO.File.Exists(Server.MapPath($"~/Content/style/images/Studios/Covers/{oldImg}")))
                                System.IO.File.Delete(Server.MapPath($"~/Content/style/images/Studios/Covers/{oldImg}"));
                        }

                        return imgName;
                    }
                }
            }
            return "0";
        }

        // Изменение цитаты 
        [HttpPost]
        public async Task EditQuote(int Id, string Text, string Author)
        {
            if (Id > 0 && (Text != null || Author != null))
            {
                var id = User.Identity.GetUserId<int>();
                var user = await db.Users.FindAsync(id);
                var myRole = user.Studios.FirstOrDefault(s => s.Studio.Id == Id && s.Member.Id == id);

                if (myRole != null && myRole.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin" || r.Name == "EditInfo") != null)
                {
                    if (Text != null)
                        myRole.Studio.Option.Quote = Text;
                    else
                        myRole.Studio.Option.AuthorQuote = Author;

                    await db.SaveChangesAsync();
                }
            }
        }

        // Изменение цитаты 
        [HttpPost]
        public async Task EditAbout(int Id, string Text)
        {
            if (Id > 0 && Text != null)
            {
                var id = User.Identity.GetUserId<int>();
                var user = await db.Users.FindAsync(id);
                var myRole = user.Studios.FirstOrDefault(s => s.Studio.Id == Id && s.Member.Id == id);

                if (myRole != null && myRole.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin" || r.Name == "EditInfo") != null)
                {
                    myRole.Studio.Option.About = Text;
                    await db.SaveChangesAsync();
                }
            }
        }

        // Изменение ролей 
        [HttpPost]
        public async Task EditRoles(int StudioId, int UserId, string EditRole)
        {
            if (StudioId > 0 && UserId > 0 && EditRole != null)
            {
                var id = User.Identity.GetUserId<int>();
                var user = await db.Users.FindAsync(id);
                var myRole = user.Studios.FirstOrDefault(s => s.Studio.Id == StudioId && s.Member.Id == id);

                if (myRole != null && myRole.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin") != null)
                {
                    var newRole = myRole.Studio.Team.FirstOrDefault(r => r.Member.Id == UserId);

                    if (newRole.Role.FirstOrDefault(r => r.Name == EditRole) != null)
                        newRole.Role.Remove(newRole.Role.FirstOrDefault(r => r.Name == EditRole));
                    else
                        newRole.Role.Add(await db.StudioRolesMember.FirstOrDefaultAsync(r => r.Name == EditRole));

                    await db.SaveChangesAsync();
                }
            }
        }

        // Удаление из команды
        [HttpPost]
        public async Task<byte> DelMember(int StudioId, int UserId)
        {
            if (StudioId > 0 && UserId > 0)
            {
                var id = User.Identity.GetUserId<int>();
                var user = await db.Users.FindAsync(id);
                var myRole = user.Studios.FirstOrDefault(s => s.Studio.Id == StudioId && s.Member.Id == id);

                if (myRole != null && myRole.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin") != null)
                {
                    var delMember = myRole.Studio.Team.FirstOrDefault(t => t.Member.Id == UserId);

                    if (delMember != null && delMember.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Sub") == null) 
                        db.StudiosTeam.Remove(delMember);
                    else if (delMember.Role.FirstOrDefault(r => r.Name == "Sub") != null)
                        delMember.Role = await db.StudioRolesMember.Where(r => r.Name == "Sub").ToListAsync();

                    await db.SaveChangesAsync();
                    return 1;
                }
            }
            return 0;
        }

        // Изменение ролей 
        [HttpPost]
        public async Task<string> AddMember(int StudioId, int UserId, string Text)
        {
            if (StudioId > 0 && UserId > 0)
            {
                var id = User.Identity.GetUserId<int>();
                var user = await db.Users.FindAsync(id);
                var newUser = await db.Users.FindAsync(UserId);
                var myRole = user.Studios.FirstOrDefault(s => s.Studio.Id == StudioId && s.Member.Id == id);

                if (newUser != null && myRole != null && myRole.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin") != null)
                {
                    var checkUser = myRole.Studio.Team.FirstOrDefault(t => t.Member == newUser);

                    if (checkUser == null)
                        myRole.Studio.Team.Add(new StudioTeam()
                        {
                            HelloMessage = Text,
                            Member = newUser,
                            Role = await db.StudioRolesMember.Where(r => r.Name == "ViewTeam" || r.Name == "Member").ToListAsync()
                        });
                    else
                    {
                        checkUser.HelloMessage = Text;
                        checkUser.Role = await db.StudioRolesMember.Where(r => r.Name == "ViewTeam" || r.Name == "Member" || r.Name == "Sub").ToListAsync();
                    }

                    await db.SaveChangesAsync();
                    return newUser.Option.Ava;
                }
            }
            return "0";
        }

        // Изменение ролей 
        [HttpPost]
        public async Task<byte> ConfirmMember(int StudioId, bool YesOrNo)
        {
            if (StudioId > 0)
            {
                var id = User.Identity.GetUserId<int>();
                var user = await db.Users.FindAsync(id);
                var confirm = user.Studios.FirstOrDefault(s => s.Studio.Id == StudioId && s.Member.Id == id);

                if (confirm != null)
                {
                    if (YesOrNo)
                        confirm.ConfirmMember = true;
                    else
                        db.StudiosTeam.Remove(confirm);

                    await db.SaveChangesAsync();
                    return 1;
                }
            }
            return 0;
        }

        // Выход из студии 
        [HttpPost]
        public async Task<byte> SignStudio(int StudioId, string Type)
        {
            if (StudioId > 0)
            {
                var id = User.Identity.GetUserId<int>();
                var user = await db.Users.FindAsync(id);
                var myRole = user.Studios.FirstOrDefault(s => s.Studio.Id == StudioId && s.Member.Id == id);

                if (myRole != null)
                {
                    if (Type == "Delete" && myRole.Role.FirstOrDefault(r => r.Name == "Creator") != null)
                    {
                        if (myRole.Studio.Lotteries.Count == 0)
                        {
                            var studio = myRole.Studio;
                            var ava = studio.Option.Ava;
                            var cover = studio.Option.Cover;
                            db.StudiosTeam.RemoveRange(studio.Team);
                            db.LinkStudios.RemoveRange(studio.Option.Link);
                            db.OptionStudios.Remove(studio.Option);

                            if (ava != "defaultAva.jpg")
                            {
                                if (System.IO.File.Exists(Server.MapPath($"~/Content/style/images/Studios/Avatars/Normal/{ava}.jpg")))
                                    System.IO.File.Delete(Server.MapPath($"~/Content/style/images/Studios/Avatars/Normal/{ava}.jpg"));
                                if (System.IO.File.Exists(Server.MapPath($"~/Content/style/images/Studios/Avatars/Reduced/{ava}.jpg")))
                                    System.IO.File.Delete(Server.MapPath($"~/Content/style/images/Studios/Avatars/Reduced/{ava}.jpg"));
                            }
                            if (cover != "defaultCover.jpg")
                            {
                                if (System.IO.File.Exists(Server.MapPath($"~/Content/style/images/Studios/Covers/{cover}.jpg")))
                                    System.IO.File.Delete(Server.MapPath($"~/Content/style/images/Studios/Covers/{cover}.jpg"));
                            }

                            db.Studios.Remove(studio);
                        }
                        else
                            return 0;
                    }
                    else if (Type == "GoOut" && myRole.Role.FirstOrDefault(r => (r.Name == "Member" || r.Name == "Sub") && r.Name != "Creator") != null)
                        db.StudiosTeam.Remove(myRole);
                }
                else
                {
                    if (Type == "Sub")
                        db.StudiosTeam.Add(new StudioTeam()
                        {
                            Member = user,
                            Studio = await db.Studios.FindAsync(StudioId),
                            Role = await db.StudioRolesMember.Where(r => r.Name == "Sub").ToListAsync()
                        });
                }

                await db.SaveChangesAsync();
                return 1;
            }
            return 0;
        }

        // Изменение контактной информации
        [HttpPost]
        public async Task EditContactInfo(int Id, EditContact NewContact)
        {
            if (Id > 0)
            {
                var id = User.Identity.GetUserId<int>();
                var user = await db.Users.FindAsync(id);
                var myRole = user.Studios.FirstOrDefault(s => s.Studio.Id == Id && s.Member.Id == id);

                if (myRole != null && myRole.Role.FirstOrDefault(r => r.Name == "Creator" || r.Name == "Admin" || r.Name == "EditInfo") != null)
                {
                    if (NewContact.SiteLink != null && NewContact.SiteName != null)
                    {
                        myRole.Studio.Option.LinkSite = NewContact.SiteLink;
                        myRole.Studio.Option.NameLinkSite = NewContact.SiteName;
                    }
                    else if (NewContact.Address != null)
                        myRole.Studio.Option.Adress = NewContact.Address;
                    else if (NewContact.Phone != null)
                        myRole.Studio.Option.Phone = NewContact.Phone;
                    else if (NewContact.Email != null)
                        myRole.Studio.Option.Email = NewContact.Email;
                    else
                    {
                        db.LinkStudios.RemoveRange(myRole.Studio.Option.Link);
                        myRole.Studio.Option.Link = NewContact.Link;
                    }

                    await db.SaveChangesAsync();
                }
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

                pathSave = Server.MapPath(pathSave);

                WebImage imgSave = new WebImage("~/Content/style/images/teamp/" + img.FileName);
                imgSave.Resize(maxW, maxH, true, true);
                imgSave.Save($"{pathSave}{imgName}", "JPG", true);

                System.IO.File.Delete(Server.MapPath("~/Content/style/images/teamp/") + img.FileName);

                return true;
            }
            else
                return false;
        } // end FramResImg

        //// Функция проверки совпадения имени картинки
        ////-------------------------------------------------------------------------
        //// fileNameImg - имя фала, pathDirectory - Путь сохранения
        ////-------------------------------------------------------------------------
        //public string InspectionPathImg(string fileNameImg, string pathDirectory)                                              // имя картинки, директория
        //{
        //    if (fileNameImg != null && pathDirectory != null)                                                                   // значения присутствуют
        //    {
        //        bool fileFlag = false;                                                                                          // флаг для выхода из цикла
        //        while (!fileFlag)
        //        {
        //            if (System.IO.File.Exists(Server.MapPath(pathDirectory + fileNameImg)))                                     // проверяем наличие картинки
        //            {                                                                                                           // если есть
        //                string newFileName = "J";                                                                               // добавляем в начало названия картинки "v"
        //                newFileName += fileNameImg;                                                                             //
        //                fileNameImg = newFileName;                                                                              //
        //            }
        //            else
        //                fileFlag = true;                                                                                        // иначе флаг = true
        //        }
        //        return fileNameImg;                                                                                             // возвращаем имя файла
        //    }
        //    else
        //        return null;
        //}// end inspectionPathImg

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