using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidsManager.Database.Models
{
    public class YandexDirectAccount
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Login { get; set; }
        public string OAuthToken { get; set; }

        public List<CampaignRule> CampaignRules { get; set; } = new List<CampaignRule>();

        public UserModel? Owner { get; set; }
        public int? OwnerId { get; set; }
    }
}
