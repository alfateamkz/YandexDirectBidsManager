

using API_Yandex_Direct.ApiConnect;
using API_Yandex_Direct.ApiConnect.Infrastructure;
using API_Yandex_Direct.Get;
using API_Yandex_Direct.Model;
using API_Yandex_Direct.Set;
using API_Yandex_Direct.Set.KeywordBids;

namespace YandexDirectAPI
{
    public static class YandexDirect
    {
        public const string CLIENT_ID = "1a09a053d0634a528de4782b67152761";
        public static Result5 SetBid(string login,string token,long campaignId,long keywordId,long price)
        {
            ApiConnect connect = new ApiConnect(login, token, AccepLanguage.ru);
            KeywordBidSetItem item = new KeywordBidSetItem()
            {
                CampaignId = campaignId,
                KeywordId = keywordId,
                NetworkBid = price
            };
            return new SetKeywordBids().SetBidsRequest(new[] { item }, connect);
        }

        public static KeywordBid[] GetCampaignBids(string login, string token, long[] campaignIds)
        {
            ApiConnect connect = new ApiConnect(login, token, AccepLanguage.ru);
            return new GetKeywordBid().GetKeywordBids(campaignIds, connect);
        }
        public static KeywordClass[] GetCampaignKeywords(string login, string token, long[] campaignIds)
        {
            ApiConnect connect = new ApiConnect(login, token, AccepLanguage.ru);

            var adGroups = new GetAdGroup().GetAdGroups(campaignIds, connect);
            var adGroupIds = adGroups.Select(o => o.Id);

            return new GetKeyword().GetKeywords(adGroupIds.ToArray(), connect);
        }

    }
}