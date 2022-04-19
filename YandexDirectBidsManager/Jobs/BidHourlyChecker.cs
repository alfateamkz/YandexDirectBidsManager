using BidsManager.Database;
using BidsManager.Database.Models;
using Microsoft.EntityFrameworkCore;
using YandexDirectAPI;
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
            var tokens = _db.YandexDirectAccounts.Include(o => o.CampaignRules).ToList();
            foreach (var token in tokens)
            {
                var campaignIds = token.CampaignRules.GroupBy(o => o.CampaignId).Select(o => o.Key).ToArray();
                var keywords = YandexDirect.GetCampaignKeywords("elama-14885256", token.OAuthToken, campaignIds);

                ParsingData.SetParsedData(keywords);
                if (ParsingData.LastHourParsing == null) break;


                foreach (var rule in token.CampaignRules.Where(o => !o.IsInvalid))
                {
                    HandleBid(rule, token);
                }
            }
           
        }

        private void HandleBid(CampaignRule rule,YandexDirectAccount token)
        {
            var keywordActual = ParsingData.ActualParsing
                       .FirstOrDefault(o =>
                       o.CampaignId == rule.CampaignId
                       && o.Id == rule.KeyPhraseId);

            var keywordLast = ParsingData.LastHourParsing
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

            long showsForLastHour = keywordActual.StatisticsSearch.Impressions
                                    - keywordLast.StatisticsSearch.Impressions;

            double percent = showsForLastHour / rule.IdealShowingsPerHour * 100;



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
                YandexDirect.SetBid("elama-14885256", token.OAuthToken, rule.CampaignId, rule.KeyPhraseId, price);
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
