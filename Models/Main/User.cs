using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodJoker.Models
{
    public class User
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        // Ник
        [Required]
        public string Nick { get; set; }

        // Почтовый адресс
        [DefaultValue(null)]
        public string Email { get; set; }

        // Подтверждение почты
        [DefaultValue(false)]
        public bool ConfirmEmail { get; set; }

        // Хеш пароля
        [DefaultValue(null)]
        public string HashPass { get; set; }

        // Токен
        [DefaultValue(null)]
        public string Token { get; set; }

        // Последний вход
        public DateTime LastOnline { get; set; }

        // Дата регистрации
        public DateTime DateRegistration { get; set; }

        // Роль
        public virtual Role Role { get; set; }

        // Опции
        public virtual OptionUser Option { get; set; }

        // Оценка GoodJoker`а
        public virtual Assess Assess { get; set; }

        // Розыгрыши
        [InverseProperty("Users")]
        public virtual ICollection<Lottery> Lotteries { get; set; }

        // Бан в комментариях к розыгрышу
        [InverseProperty("BanComment")]
        public virtual ICollection<Lottery> BanCommentLotteries { get; set; }

        // Ежедневные розыгрыши
        [InverseProperty("Participants")]
        public virtual ICollection<EverydayLott> Everyday { get; set; }

        // Лайки
        [InverseProperty("Likes")]
        public virtual ICollection<EverydayLott> LikesEveryday { get; set; }

        // Победы в ежедевных розыгрышах
        public virtual ICollection<WinnersEverydayLott> WinEveryday { get; set; }

        // Создатель диалога
        [InverseProperty("Author")]
        public virtual ICollection<Dialog> AuthorDialog { get; set; }

        // Приглашенный в диалог
        [InverseProperty("Invitation")]
        public virtual ICollection<Dialog> InvitationDialog { get; set; }

        // Подключенные социальные сети
        public virtual ICollection<SocialNetwork> SocialNetworks { get; set; }

        // Комментарии к новостям
        public virtual ICollection<CommentNews> CommentNews { get; set; }

        // Комментарии к розыгрышам
        public virtual ICollection<CommentLottery> CommentLottery { get; set; }

        // Лайки новостям
        public virtual ICollection<News> LikeNews { get; set; }

        // Призы
        public virtual ICollection<Prize> WinLott { get; set; }

        // Архивные места
        public virtual ICollection<ArchiveWinner> ArchiveWinLott { get; set; }

        // Архивные места
        public virtual ICollection<ArchiveWinnerEveryday> ArchiveWinEver { get; set; }

        // Лайк и дизлайк
        public virtual ICollection<Finger> Fingers { get; set; }

        // Информация
        public virtual ICollection<ReturnMandatoryInfo> ReturnMandatoryInfo { get; set; }

        // Студии
        public virtual ICollection<StudioTeam> Studios { get; set; }

        // Перевыбор победителя
        public virtual ICollection<RefreshWinner> RefreshWinner { get; set; }

        // Уведомления
        public virtual ICollection<Notice> Notices { get; set; }

        public User()
        {
            LikeNews = new List<News>();
            Lotteries = new List<Lottery>();
            BanCommentLotteries = new List<Lottery>();
            Everyday = new List<EverydayLott>();
            LikesEveryday = new List<EverydayLott>();
            Notices = new List<Notice>();
        }
    }

    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}