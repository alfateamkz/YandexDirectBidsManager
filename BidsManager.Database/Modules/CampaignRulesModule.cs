using BidsManager.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidsManager.Database.Modules
{
    public static class CampaignRulesModule
    {
        public static async Task<int> CreateCampaignRule(YandexDirectAccount account, CampaignRule rule)
        {
            using (var db = new DatabaseContext())
            {
                account.CampaignRules.Add(rule);
                db.YandexDirectAccounts.Update(account);
                db.SaveChanges();
                return rule.Id;
            }
        }
        public static async Task CreateCampaignRules(YandexDirectAccount account, List<CampaignRule> rules)
        {
            using (var db = new DatabaseContext())
            {
                account.CampaignRules.AddRange(rules);
                db.YandexDirectAccounts.Update(account);
                db.SaveChanges();
            }
        }
        public static async Task UpdateCampaignRule(CampaignRule rule)
        {
            using (var db = new DatabaseContext())
            {
                try
                {
                    db.CampaignRules.Update(rule);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                }
            }
        }
        public static async Task DeleteCampaignRule(YandexDirectAccount account, CampaignRule rule)
        {
            using (var db = new DatabaseContext())
            {
                account.CampaignRules.Remove(rule);
                db.YandexDirectAccounts.Update(account);
                db.CampaignRules.Remove(rule);
                db.SaveChanges();
            }
        }
        public static async Task ClearCampaignRules(YandexDirectAccount account)
        {
            using (var db = new DatabaseContext())
            {
                db.CampaignRules.RemoveRange(account.CampaignRules);
                account.CampaignRules.Clear();
                db.YandexDirectAccounts.Update(account);
                db.SaveChanges();
            }
        }
    }
}
