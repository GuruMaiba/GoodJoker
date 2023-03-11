using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoodJoker.Models
{
    // Видео
    public class Video
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        // Идентификатор youtube video
        [Required]
        public string VideoId { get; set; }

        // Студия
        public virtual Studio Studio { get; set; }

        // Тип
        public virtual ICollection<TypeVideo> Type { get; set; }
    }

    // Тип видео
    public class TypeVideo
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        // Имя типа
        [Required]
        public string Name { get; set; }

        // Видео
        public virtual ICollection<Video> Videos { get; set; }
    }
}