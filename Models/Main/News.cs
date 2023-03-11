using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GoodJoker.Models
{
    public class News
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
        [DisplayName("Короткое описание новости")]
        public string ShortDescription { get; set; }

        // Статья
        //[AllowHtml]
        //[DisplayName("Контент")]
        //public string Text { get; set; }

        // Список картинок на всю ширину статьи
        //public string ImagesBig { get; set; }

        // Список картинок левых или правых
        //public string ImagesSmall { get; set; }

        // Идентификатор автора
        [DefaultValue(0)]
        public int AuthorId { get; set; }

        // Имя автора
        [DefaultValue("GoodJoker")]
        public string AuthorName { get; set; }

        // Количество просмотров
        [DefaultValue(0)]
        public int View { get; set; }

        // Дата создания статьи
        public DateTime DateCreate { get; set; }

        // Опубликованная или нет
        [DefaultValue(false)]
        public bool Publish { get; set; }

        // Статья
        public virtual ICollection<ContentNews> Content { get; set; }

        // Комментарии
        public virtual ICollection<CommentNews> Comments { get; set; }

        // Лайки новостям
        public virtual ICollection<User> UsersLike { get; set; }

        // Теги
        public virtual ICollection<Tag> Tags { get; set; }

        public News()
        {
            UsersLike = new List<User>();
            Tags = new List<Tag>();
        }
    }

    public class ContentNews
    {
        [Key]
        public int Id { get; set; }

        [AllowHtml]
        public string Text { get; set; }

        public int Place { get; set; }

        public string Type { get; set; }

        public virtual News News { get; set; }

        public virtual ICollection<ImageNews> Images { get; set; }
    }

    public class ImageNews
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public virtual ContentNews Content { get; set; }
    }

    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<News> News { get; set; }

        public Tag()
        {
            News = new List<News>();
        }
    }
}