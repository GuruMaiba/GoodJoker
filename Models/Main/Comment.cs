using System;
using System.ComponentModel.DataAnnotations;

namespace GoodJoker.Models
{
    public class CommentNews
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        // Текст комментария
        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        
        // Дата создания
        public DateTime DateCreate { get; set; }

        // Идентификатор комментария на который отвечают
        public int ReplyId { get; set; }

        // Автор
        public virtual User Author { get; set; }

        // Статья
        public virtual News News { get; set; }
    }

    public class CommentLottery
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        // Текст комментария
        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        // Дата создания
        public DateTime DateCreate { get; set; }

        // Идентификатор комментария на который отвечают
        public int ReplyId { get; set; }

        // Автор
        public virtual User Author { get; set; }

        // Розыгрыш
        public virtual Lottery Lottery { get; set; }
    }

    public class CommentChatLottery
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        // Текст комментария
        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        // Дата создания
        public DateTime DateCreate { get; set; }

        // Автор
        public virtual User Author { get; set; }

        // Розыгрыш
        public virtual Lottery Lottery { get; set; }
    }
}