using BidsManager.Database.Models;
using BidsManager.Database.Modules;
using Microsoft.AspNetCore.Mvc;

namespace YandexDirectBidsManager.Extensions
{
    public static class ControllerExtensions
    {
        public static async Task<UserModel> GetAuthorizedUser(this Controller controller)
        {
            int id = Convert.ToInt32(controller.User.Identity.Name);
            return UsersModule.GetUserById(id);
        }
    }
}
