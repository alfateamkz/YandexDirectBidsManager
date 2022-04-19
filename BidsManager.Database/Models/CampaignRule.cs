using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidsManager.Database.Models
{
    public class CampaignRule
    {
        [Key]
        public int Id { get; set; }
        public long CampaignId { get; set; }
        public long KeyPhraseId { get; set; }
        public int IdealShowingsPerHour { get; set; }

        public YandexDirectAccount? Owner { get; set; }
        public int? OwnerId { get; set; }

        public bool IsInvalid { get; set; }
    }
}
