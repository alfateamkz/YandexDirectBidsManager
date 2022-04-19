using BidsManager.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidsManager.Database.Modules
{
    public static class UsersModule
    {
        public static UserModel GetUserById(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Users
                    .Include(o => o.UserData)
                    .Include(o => o.YandexDirectAccounts).ThenInclude(o => o.CampaignRules)
                    .FirstOrDefault(o => o.Id == id);
            }
        }
        public static UserModel GetUserByCredentials(string email,string password)
        {
            using (var db = new DatabaseContext())
            {
                return db.Users
                    .Include(o => o.UserData)
                    .Include(o => o.YandexDirectAccounts).ThenInclude(o => o.CampaignRules)
                    .FirstOrDefault(o => o.Email == email && o.Password == password);
            }
        }
        public static UserModel GetUserByEmail(string email)
        {
            using (var db = new DatabaseContext())
            {
                return db.Users
                    .Include(o => o.UserData)
                    .Include(o => o.YandexDirectAccounts).ThenInclude(o => o.CampaignRules)
                    .FirstOrDefault(o => o.Email == email);
            }
        }

        public static void CreateUser(UserModel user)
        {
            using (var db = new DatabaseContext())
            {
             
                var userData = new UserData();
                db.SaveChanges();


                user.UserData = userData;
                db.Users.Add(user);
                db.SaveChanges();
            }
        }
    }
}
