using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GoodJoker.Models
{
    public class Studio
    {
        // Идентификатор
        [Key]
        public int Id { get; set; }

        // Название
        [Required]
        public string Name { get; set; }
        
        // Дата создания
        [Required]
        public DateTime DateCreate { get; set; }

        // Настройки
        public virtual OptionStudio Option { get; set; }

        // Архив
        public virtual ICollection<ArchiveLottery> Archive { get; set; }

        // Розыгрыш
        public virtual ICollection<Lottery> Lotteries { get; set; }

        // Команда
        public virtual ICollection<StudioTeam> Team { get; set; }
    }

    public class StudioTeam
    {
        [Key]
        public int Id { get; set; }

        [DefaultValue(false)]
        public bool ConfirmMember { get; set; }

        [MaxLength(300)]
        [DefaultValue(null)]
        public string HelloMessage { get; set; }

        public virtual Studio Studio { get; set; }

        public virtual User Member { get; set; }

        public virtual ICollection<StudioRoleMember> Role { get; set; }
    }

    public class StudioRoleMember
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<StudioTeam> User { get; set; }
    }
}