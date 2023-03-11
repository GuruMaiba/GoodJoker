using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GoodJoker.Models
{
    // Статистические данные удалённых лоттерей
    public class ArchiveLottery
    {
        // Идентификатор
        public int Id { get; set; }

        // Заголовок
        public string Title { get; set; }

        // Количество просмотров
        public int View { get; set; }

        // Количество комментариев
        public int Comment { get; set; }

        // Разница пальцев вверх и вниз
        public int OddsFingers { get; set; }

        // Дата создания лотереи
        public DateTime CreateLottery { get; set; }

        // Дата проведения лотереи
        public DateTime HoldingLottery { get; set; }

        // Количество пользователей для лоттереи
        public int CountParticipient { get; set; }

        // Студия
        public virtual Studio Studio { get; set; }

        // Победители
        public virtual ICollection<ArchiveWinner> Winner { get; set; }
    }

    // Статистические данные победителей
    public class ArchiveWinner
    {
        public int Id { get; set; }

        public int Place { get; set; }

        public string Prize { get; set; }

        [DefaultValue(false)]
        public bool NoWin { get; set; }

        public string RefreshId { get; set; }

        public virtual User Winner { get; set; }

        public virtual ArchiveLottery Lottery { get; set; }
    }

    // Статистические данные победителей ежедневок
    public class ArchiveWinnerEveryday
    {
        public int Id { get; set; }

        public string FromWhom { get; set; }

        public string Prize { get; set; }
        
        public DateTime Date { get; set; }

        public virtual User Winner { get; set; }
    }
}