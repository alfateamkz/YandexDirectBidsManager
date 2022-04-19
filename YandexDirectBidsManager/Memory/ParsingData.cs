using API_Yandex_Direct.Model;

namespace YandexDirectBidsManager.Memory
{
    public static class ParsingData
    {
        public static KeywordClass[] LastHourParsing { get; private set; }
        public static KeywordClass[] ActualParsing { get; private set; }

        public static void SetParsedData(KeywordClass[] data)
        {
            if (ActualParsing == null)
            {
                ActualParsing = data;
            }
            else
            {
                LastHourParsing = ActualParsing.ToArray();
                ActualParsing = data;
            }
        }
    }
}
