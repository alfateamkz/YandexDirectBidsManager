using API_Yandex_Direct.ApiConnect.Infrastructure;
using API_Yandex_Direct.Get.Bids;
using API_Yandex_Direct.Set.KeywordBids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Yandex_Direct.Set
{
    public class SetKeywordBids
    {
        public Result5 SetBidsRequest(KeywordBidSetItem[] keywordBidSetItems, ApiConnect.ApiConnect apiConnect)
        {
            var paramsRequest = new Set.KeywordBids.ParamsRequest()
            {
                KeywordBids = keywordBidSetItems,
            };
            string[] Headers = new string[] { };
            var a = apiConnect.SetResult5(paramsRequest, "keywordbids", "set", ref Headers);
            return a;
        }
    }
}
