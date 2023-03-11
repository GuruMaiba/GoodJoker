using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GoodJoker.Models
{
    public class LotteryCreate
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string ShortDescription { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime DateLottery { get; set; }

        public int GMT { get; set; }

        [DefaultValue(0)]
        public int CountUserForLottery { get; set; }

        [DefaultValue(0)]
        public int BeginAge { get; set; }

        [DefaultValue(0)]
        public int EndAge { get; set; }

        [DefaultValue(0)]
        public byte Gender { get; set; }

        [DefaultValue(null)]
        public string YouTubeId { get; set; }

        [DefaultValue(null)]
        public string CitiesId { get; set; }

        [DefaultValue(null)]
        public string RegionsId { get; set; }

        public List<string> Conditions { get; set; } = new List<string>();
        public List<Prize> Prizes { get; set; } = new List<Prize>();
        public ICollection<MandatoryInfo> MandatoryInfo { get; set; } = new List<MandatoryInfo>();
        public ICollection<LinkLottery> LinksLottery { get; set; } = new List<LinkLottery>();
    }
}