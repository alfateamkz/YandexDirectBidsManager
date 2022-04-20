using BidsManager.Database;
using BidsManager.Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using YandexDirectAPI;
using YandexDirectBidsManager.Helpers;
using YandexDirectBidsManager.Memory;

namespace YandexDirectBidsManager.Jobs
{
    public class BidHourlyChecker : IHostedService, IDisposable
    {
        private Timer _timer = null;
        private DatabaseContext _db;
        public BidHourlyChecker()
        {
            _db = new DatabaseContext();
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(60));
        }

        private async void DoWork(object state)
        {
            _db = new DatabaseContext();
            var tokens = _db.YandexDirectAccounts.Include(o => o.CampaignRules).ToList();
            foreach (var token in tokens)
            {
                var campaignIds = token.CampaignRules.GroupBy(o => o.CampaignId).Select(o => o.Key).ToArray();
                var keywords = YandexDirect.GetCampaignKeywords(token.Login, token.OAuthToken, campaignIds);

                ParsingData.SetParsedData(token.OAuthToken,keywords);

                if (!ParsingData.LastHourParsing.ContainsKey(token.OAuthToken)) continue;


                foreach (var rule in token.CampaignRules.Where(o => !o.IsInvalid))
                {
                    HandleBid(rule, token);
                }
            }
           
        }

        private void HandleBid(CampaignRule rule,YandexDirectAccount token)
        {
            try
            {

                var keywordActual = ParsingData.ActualParsing[token.OAuthToken]
                     .FirstOrDefault(o =>
                     o.CampaignId == rule.CampaignId
                     && o.Id == rule.KeyPhraseId);

                var keywordLast = ParsingData.LastHourParsing[token.OAuthToken]
                    .FirstOrDefault(o =>
                    o.CampaignId == rule.CampaignId
                    && o.Id == rule.KeyPhraseId);


                if (keywordActual == null || keywordLast == null)
                {
                    rule.IsInvalid = true;
                    _db.CampaignRules.Update(rule);
                    _db.SaveChanges();
                    return;
                }


                double correctionPercent = 0;
                long price = keywordActual.ContextBid;

                long showsForLastHour = keywordActual.StatisticsNetwork.Impressions
                                        - keywordLast.StatisticsNetwork.Impressions;


                if (rule.IdealShowingsPerHour == 0) return;
                double percent = (double)showsForLastHour / (double)rule.IdealShowingsPerHour * 100;



                if (percent < 100)
                {
                    int steps = (100 - (int)percent) / 10;
                    correctionPercent = steps * 2;

                    price += price / 100 * (long)correctionPercent;
                }
                else if (percent > 100)
                {
                    int steps = ((int)percent - 100) / 10;
                    correctionPercent = steps * 1;

                    if (correctionPercent > 50)
                        correctionPercent = 50;

                    price -= price / 100 * (long)correctionPercent;
                }

                if (correctionPercent != 0)
                {
                   // YandexDirect.SetBid(token.Login, token.OAuthToken, rule.CampaignId, rule.KeyPhraseId, price);

                    LoggerHelper.LogToTxt(rule.KeyPhraseId,
                          keywordLast.StatisticsNetwork.Impressions,
                          keywordActual.StatisticsNetwork.Impressions,
                          keywordActual.ContextBid / 1000000,
                          price / 1000000);
                }

            }
            catch (Exception ex) { 
            
            }
        }


        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
