using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoodJoker.Models
{
    // Реклама
    public class Advert
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        // Изображение
        public string Ava { get; set; }

        // Рекламодатель
        public int AdvertiserId { get; set; }
        public string Advertiser { get; set; }

        // Стоимость
        public int Cost { get; set; }
        
        // Ссылка
        public string Link { get; set; }

        // Заголовок
        public string Title { get; set; }

        // Текст
        public string Text { get; set; }

        // Видео
        public string Video { get; set; }

        // Количество кликов по рекламе
        public int CountClick { get; set; }

        // Расположение главного контента
        public bool MainAva { get; set; }

        // Дата начала рекламы
        public DateTime DateBeginAdvert { get; set; }

        // Дата окончания
        public DateTime DateEndAdvert { get; set; }

        // Город
        public virtual ICollection<City> Cities { get; set; }

        // Город
        public virtual ICollection<RegionCity> Regions { get; set; }

        // Место
        public virtual ICollection<PlaceAdvert> Place { get; set; }

        public Advert()
        {
            Place = new List<PlaceAdvert>();
            Cities = new List<City>();
            Regions = new List<RegionCity>();
        }
    }

    // Место рекламы
    public class PlaceAdvert
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        // Имя
        [Required]
        public string Name { get; set; }

        // Реклама
        public virtual ICollection<Advert> Adverts { get; set; }

        public PlaceAdvert()
        {
            Adverts = new List<Advert>();
        }
    }

    // Архив рекламных заказов
    public class ArchiveAdvert
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        // Рекламодатель
        public int AdvertiserId { get; set; }
        public string Advertiser { get; set; }

        // Стоимость
        public int Cost { get; set; }

        // Ссылка
        public string Link { get; set; }

        // Количество кликов по рекламе
        public int CountClick { get; set; }

        // Дата начала рекламы
        public DateTime DateBeginAdvert { get; set; }

        // Дата окончания
        public DateTime DateEndAdvert { get; set; }
    }
}