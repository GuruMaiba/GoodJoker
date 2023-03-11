using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace GoodJoker.Models
{
    public class YouTube
    {
        // Парсинг ID
        public static string ParseId(string YTid)
        {
            if (YTid != "" && YTid != null)
            {
                string link = "",
                       charDel = "";

                if (YTid.IndexOf("https://") > -1)
                    link = "https://";

                if (YTid.IndexOf("www.youtube.com/watch?v=") > -1)
                {
                    link += "www.youtube.com/watch?v=";
                    charDel = "&";
                }
                else if (YTid.IndexOf("youtu.be/") > -1)
                {
                    link += "youtu.be/";
                    charDel = "?";
                }
                else
                    return null;

                YTid = YTid.Replace(link, "");
                var inx = YTid.IndexOf(charDel);
                if (inx > -1)
                    YTid = YTid.Remove(inx, YTid.Length - inx);

                return YTid;
            }
            else
                return null;
        }
    }

    public class Hash
    {
        // Метод хеширования
        public static string Get(string s)
        {
            byte[] hash = Encoding.ASCII.GetBytes(s);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashenc = md5.ComputeHash(hash);
            string result = "";

            foreach (var b in hashenc)
                result += b.ToString("x2");

            return result;
        }
    }

    public class Token
    {
        // Метод генерации токена
        public static string GetToken()
        {
            var token = "";
            Random random = new Random();

            for (var i = 0; i < 10; i++)
                token += (i % 2 == 0) ? (char)(random.Next(10000, 99999)) : random.Next(100, 999);

            token = Hash.Get(token);

            return token;
        }
    }

    public class EmailMessage
    {
        // Метод отправки письма для подтвержения почты 
        public static void SendMessage(string Email, string Subject, string Body, string TypeService = "YA")
        {
            const string Pass = "GoodJokerCfvsqGjgekzhysqGhjtrn130595";
            int port = 587;
            string from, host;
            MailMessage msg = new MailMessage();
            SmtpClient client = new SmtpClient();

            if (TypeService == "GL")
            {
                from = "goodjokerteam@gmail.com";
                host = "smtp.gmail.com";
            }
            else
            {
                from = "team@goodjoker.ru";
                host = "smtp.yandex.ru";
            }

            msg.Subject = Subject;
            msg.Body = Body;

            msg.From = new MailAddress(from, "GoodJoker");
            msg.To.Add(Email);
            msg.IsBodyHtml = true;
            client.Host = host;
            System.Net.NetworkCredential basicauthenticationinfo = new System.Net.NetworkCredential(from, Pass);
            client.Port = port;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicauthenticationinfo;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(msg);
        }
    }

    public class PathImg
    {
        // Метод проверки имени картинки
        public static string Inspection(string fileNameImg, string pathDirectory)                                               // имя картинки, директория
        {
            if (fileNameImg != null && pathDirectory != null)                                                                   // значения присутствуют
            {
                bool fileFlag = false;                                                                                          // флаг для выхода из цикла
                var split = fileNameImg.Split('.');
                var name = split[0];
                while (!fileFlag)
                {
                    if (File.Exists(pathDirectory + name + ".jpg"))                                                             // проверяем наличие картинки
                        name += "GJ";
                    else                                                                                                        // иначе
                        fileFlag = true;                                                                                        // иначе флаг = true
                }
                return name + ".jpg";                                                                                           // возвращаем имя файла
            }
            else                                                                                                                // иначе null
                return null;
        }
    }
}