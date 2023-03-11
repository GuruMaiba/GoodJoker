using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoodJoker.Models
{
    // Город
    public class City
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        // Город
        [Required]
        public string Name { get; set; }

        // Индекс населённого пункта
        [Required]
        public int Index { get; set; }

        // Регион
        public virtual RegionCity Region { get; set; }

        // Реклама
        public virtual ICollection<Advert> Adverts { get; set; }

        // Пользователи привязанные к маленькому городу
        public virtual ICollection<OptionUser> UserCity { get; set; }

        // Пользователи привязанные к большому городу
        public virtual ICollection<OptionUser> UserBigCity { get; set; }

        // Лотереи которые ограниченны населёнными пунктами
        public virtual ICollection<Lottery> Lotteries { get; set; }

        public City()
        {
            Adverts = new List<Advert>();
            UserCity = new List<OptionUser>();
            UserBigCity = new List<OptionUser>();
            Lotteries = new List<Lottery>();
        }
    }

    // Регион
    public class RegionCity
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        // Имя
        [Required]
        public string Name { get; set; }

        // Реклама
        public virtual ICollection<Advert> Adverts { get; set; }

        // Города
        public virtual ICollection<City> Cities { get; set; }

        // Лотереи ограниченные по регонам
        public virtual ICollection<Lottery> Lotteries { get; set; }

        public RegionCity()
        {
            Adverts = new List<Advert>();
            Cities = new List<City>();
            Lotteries = new List<Lottery>();
        }
    }
}