using System.ComponentModel.DataAnnotations;

namespace GoodJoker.Models
{
    // Пальцы
    public class Finger
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        // Пользователь
        public virtual User User { get; set; }

        // Лоттерея
        public virtual Lottery Lottery { get; set; }

        // Вверх или вниз
        public bool UpOrDown { get; set; }
    }
}