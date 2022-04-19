using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidsManager.Database.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<YandexDirectAccount> YandexDirectAccounts { get; set; } = new List<YandexDirectAccount>();

        public UserData UserData { get; set; }
        public int UserDataId { get; set; }
    }
}
