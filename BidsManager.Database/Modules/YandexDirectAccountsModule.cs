using BidsManager.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidsManager.Database.Modules
{
    public static class YandexDirectAccountsModule
    {
        public static async Task CreateAccount(UserModel owner,YandexDirectAccount account)
        {
            using (var db = new DatabaseContext())
            {
                owner.YandexDirectAccounts.Add(account);
                db.Users.Update(owner);
                db.SaveChanges();
            }
        }
        public static async Task UpdateAccount(YandexDirectAccount account)
        {
            using (var db = new DatabaseContext())
            {
                db.YandexDirectAccounts.Update(account);
                db.SaveChanges();
            }
        }
        public static async Task DeleteAccount(UserModel owner, int accountId)
        {
            using (var db = new DatabaseContext())
            {
                YandexDirectAccount account = owner.YandexDirectAccounts.FirstOrDefault(o => o.Id == accountId);
                owner.YandexDirectAccounts.Remove(account);
                db.YandexDirectAccounts.Remove(account);
                db.Users.Update(owner);
                db.SaveChanges();
            }
        }
        public static async Task SetActiveAccount(UserModel owner, int accountId)
        {
            using (var db = new DatabaseContext())
            {
                YandexDirectAccount account = owner.YandexDirectAccounts.FirstOrDefault(o => o.Id == accountId);
                owner.UserData.SelectedYandexDirectAccount = account;
                db.Users.Update(owner);
                db.SaveChanges();
            }
        }
    }
}
