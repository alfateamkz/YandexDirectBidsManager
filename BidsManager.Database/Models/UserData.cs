using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidsManager.Database.Models
{
    public class UserData
    {
        [Key]
        public int Id { get; set; }
        public YandexDirectAccount? SelectedYandexDirectAccount { get; set; }
        public int? SelectedYandexDirectAccountId { get; set; }

    }
}
