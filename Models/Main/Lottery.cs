using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodJoker.Models
{
    // РЕАЛИЗОВАТЬ
    // ---------------------------------------
    // - Доступ по ссылке
    // - Бан розыгрыша
    // - Бан в комментариях и чате (преимущественно в чате)
    // - Добавление фото выйгранного приза пользователем

    // Розыгрыш
    public class Lottery
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        public string Cover { get; set; }

        // Заголовок
        [Required]
        [DisplayName("Заголовок")]
        public string Title { get; set; }

        // Короткое описание
        [Required]
        [MaxLength(300)]
        [DisplayName("Короткое описание")]
        public string ShortDescription { get; set; }

        // Текст
        [Required]
        [DisplayName("Описание")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        // Условия
        [DisplayName("Условия")]
        public string Conditions { get; set; }

        // Видео
        [DefaultValue(null)]
        public string YoutubeId { get; set; }

        // Прямая трансляция
        [DefaultValue(null)]
        public string LifeYoutubeId { get; set; }

        // Дата создания розыгрыша
        [Required]
        [DisplayName("Дата проведения")]
        public DateTime DateCreate { get; set; }

        // Дата проведения розыгрыша
        [Required]
        [DisplayName("Дата проведения")]
        public DateTime DateLottery { get; set; }

        // Возростные ограничения
        [DefaultValue(0)]
        public int BeginAgeLimit { get; set; }

        // Возростные ограничения
        [DefaultValue(0)]
        public int EndAgeLimit { get; set; }

        // Гендарное ограничение
        [DefaultValue(0)]
        public byte GenderLimit { get; set; }

        // Количество пользователей для лоттереи
        [DefaultValue(0)]
        [DisplayName("Нужное колчество пользователей")]
        public int CountUserForLottery { get; set; }

        // Место проведения
        [DefaultValue(null)]
        public string Place { get; set; }

        // Количество просмотров
        [DefaultValue(0)]
        public int View { get; set; }

        // Разница пальца вверх и вниз
        [DefaultValue(0)]
        public int OddsFingers { get; set; }

        // Автоматический розыгрыш
        [DefaultValue(false)]
        public bool Auto { get; set; }

        // Начало розыгрыша
        [DefaultValue(false)]
        public bool BeginLott { get; set; }

        // Розыгрыш с первого
        [DefaultValue(false)]
        public bool FromTheFirst { get; set; }

        // Флаг Бан
        [DefaultValue(false)]
        public bool IsBan { get; set; }

        // Флаг - доступ по ссылке
        [DefaultValue(false)]
        public bool ForLink { get; set; }

        // Студия создавшая Лоттерею
        public virtual Studio Studio { get; set; }

        // Участники
        [InverseProperty("Lotteries")]
        public virtual ICollection<User> Users { get; set; }

        // Бан в комментах
        [InverseProperty("BanCommentLotteries")]
        public virtual ICollection<User> BanComment { get; set; }

        // Призы
        public virtual ICollection<Prize> Prizes { get; set; }

        // Комментарии
        public virtual ICollection<CommentLottery> Comments { get; set; }

        // Чат при проведении лотереи
        public virtual ICollection<CommentChatLottery> Chat { get; set; }

        // Ссылки на сторонние сервисы
        public virtual ICollection<LinkLottery> Link { get; set; }

        // Пальцы вверх и вниз
        public virtual ICollection<Finger> Fingers { get; set; }

        // Обязательная информация о пользователе
        public virtual ICollection<MandatoryInfo> MandatoryInfo { get; set; }

        // Субъекты феедерации
        public virtual ICollection<RegionCity> Regions { get; set; }

        // Города
        public virtual ICollection<City> Cities { get; set; }

        public Lottery()
        {
            Users = new List<User>();
            BanComment = new List<User>();
            Cities = new List<City>();
            Regions = new List<RegionCity>();
        }
    }

    // Приз и победитель
    public class Prize
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        // Название
        [Required]
        public string Name { get; set; }

        // Место
        [Required]
        public int Place { get; set; }

        // Подтверждение победителя
        [DefaultValue(false)]
        public bool Confirm { get; set; }

        // Не розыгранный приз
        [DefaultValue(false)]
        public bool NoWin { get; set; }

        // Просмотр уведомления пользователем
        [DefaultValue(false)]
        public bool View { get; set; }

        // Картинка
        public string Photo { get; set; }

        // Победитель
        public virtual User Winner { get; set; }

        // Розыгрыш
        public virtual Lottery Lottery { get; set; }

        // Перевыбор победителя
        public virtual ICollection<RefreshWinner> RefreshWinners { get; set; }
    }

    public class RefreshWinner
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        // Объяснительная
        public string Explanatory { get; set; }

        // Просмотр уведомления пользователем
        [DefaultValue(false)]
        public bool View { get; set; }

        // Удалённый пользователь
        public virtual User DelWinner { get; set; }

        // Приз
        public virtual Prize Prize { get; set; }
    }

    // Ссылка
    public class LinkLottery
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        // Имя
        [Required]
        public string Name { get; set; }

        // Ссылка на внешний ресурс
        [Required]
        public string Link { get; set; }

        // тип ссылки
        [Required]
        public string Type { get; set; }

        // Розыгрыш
        public virtual Lottery Lottery { get; set; }
    }

    // Обязательная информация о пользователе
    public class MandatoryInfo
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        // Имя
        public string Name { get; set; }

        // Тип
        [Required]
        public string Type { get; set; }

        // Розыгрыш
        public virtual Lottery Lottery { get; set; }

        // Ответ пользователя
        public virtual ICollection<ReturnMandatoryInfo> Return { get; set; }
    }

    // Ответ на обязательную информацию
    public class ReturnMandatoryInfo
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        // Ответ
        [Required]
        public string Text { get; set; }

        // Информация на которую отвечает пользователь
        public virtual MandatoryInfo Info { get; set; }

        // Пользователь который даёт требуемую информацию
        public virtual User User { get; set; }
    }
}