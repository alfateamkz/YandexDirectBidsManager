using API_Yandex_Direct.Model;

namespace YandexDirectBidsManager.Memory
{
    public static class ParsingData
    {
        public static Dictionary<string, KeywordClass[]> LastHourParsing { get; private set; } = new Dictionary<string, KeywordClass[]>();
        public static Dictionary<string, KeywordClass[]> ActualParsing { get; private set; } = new Dictionary<string, KeywordClass[]>();

        public static void SetParsedData(string token,KeywordClass[] data)
        {
            if (!ActualParsing.ContainsKey(token))
            {
                ActualParsing.Add(token,data);
            }
            else
            {
                var found = ActualParsing[token];
                LastHourParsing[token] = found;
                ActualParsing[token] = data;
            }
        }
    }
}
