using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GoodJoker.Models
{
    public class OptionUser
    {
        [Key, ForeignKey("User")]
        public int Id { get; set; }
        
        public string Ava { get; set; }                 // Аватарка

        [DefaultValue(null)]
        public string FirstName { get; set; }           // Фамилия

        [DefaultValue(null)]
        public string MiddleName { get; set; }          // Имя

        [DefaultValue(null)]
        public string LastName { get; set; }            // Отчество

        [DefaultValue(null)]
        public string Phone { get; set; }               // Телефон

        [DefaultValue(0)]
        public byte Gender { get; set; }                // Пол

        public DateTime Birthday { get; set; }          // День рождения

        [DefaultValue(null)]
        public string GeoMap { get; set; }              // Координаты местонахождения человека

        [DefaultValue(0)]
        public bool HaveReviews { get; set; }           // Возможность отзыва

        [DefaultValue(0)]
        public int CountComments { get; set; }          // Количество комментариев

        [DefaultValue(0)]
        public int CountShareLottery { get; set; }      // Количество розыгрышей, в которых пользователь принимал участие
        
        [DefaultValue(0)]
        public byte Warnings { get; set; }              // Предупреждения

        [DefaultValue(false)]
        public bool Ban { get; set; }                   // Бан

        [DefaultValue(null)]
        public string СauseBan { get; set; }            // Причина бана

        [DefaultValue(null)]
        public string ExactAddress { get; set; }        // Точный адресс

        [DefaultValue(null)]
        public string Index { get; set; }               // Индекс

        public int? BigCityID { get; set; }
        public int? CityID { get; set; }

        [ForeignKey("BigCityID")]
        [InverseProperty("UserBigCity")]
        public virtual City BigCity { get; set; }       // Город

        [ForeignKey("CityID")]
        [InverseProperty("UserCity")]
        public virtual City City { get; set; }          // Населённый пункт

        public virtual User User { get; set; }          // Пользователь

        public class OptionUserMapping : EntityTypeConfiguration<OptionUser>
        {
            public OptionUserMapping()
            {
                HasRequired(u => u.City).WithMany(c => c.UserCity).HasForeignKey(u => u.CityID).WillCascadeOnDelete(false);
                HasRequired(u => u.BigCity).WithMany(c => c.UserBigCity).HasForeignKey(u => u.BigCityID).WillCascadeOnDelete(false);
            }
        }
    }

    // РЕАЛИЗОВАТЬ
    // ---------------------------
    // - Бан и его описание
    // - VIP студия

    public class OptionStudio
    {
        [Key, ForeignKey("Studio")]
        public int Id { get; set; }                     // Идентификатор

        public string Ava { get; set; }                 // Аватарка

        public string Cover { get; set; }               // Обложка

        [DefaultValue(0)]
        public int Statistics { get; set; }             // Статистика

        [DefaultValue(null)]
        public string Quote { get; set; }               // Цитата

        [DefaultValue(null)]
        public string AuthorQuote { get; set; }         // Автор цитаты

        [DefaultValue(null)]
        public string About { get; set; }               // О нас

        [DefaultValue(null)]
        public string NameLinkSite { get; set; }        // Надпись ссылки

        [DefaultValue(null)]
        public string LinkSite { get; set; }            // Ссылка на сайт

        [DefaultValue(null)]
        public string Adress { get; set; }              // Адресс

        [DefaultValue(null)]
        public string Phone { get; set; }               // Телефон

        [DefaultValue(null)]
        public string Email { get; set; }               // Почта

        [DefaultValue(false)]
        public bool IsBan { get; set; }                 // Бан

        [DefaultValue(null)]
        public string СauseBan { get; set; }            // Причина бана

        [DefaultValue(10)]
        public int CountLottery { get; set; }           // Количество возможных розыгрышей

        [DefaultValue(false)]
        public bool IsVIP { get; set; }                 // VIP
        
        public DateTime BeginVIP { get; set; }          // Дата начала оказания VIP услуг
        
        public DateTime EndVIP { get; set; }            // Дата окончания оказания VIP услуг

        public virtual Studio Studio { get; set; }      // Студия

        public virtual ICollection<LinkStudio> Link { get; set; }
    }

    // Ссылки студии
    public class LinkStudio
    {
        [Key]
        public int Id { get; set; }
        
        public string Icon { get; set; }

        public string Link { get; set; }

        public virtual OptionStudio Studio { get; set; }
    }
}