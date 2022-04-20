using System.Text;

namespace YandexDirectBidsManager.Helpers
{
    public static class LoggerHelper
    {
        public static void LogToTxt(long keyId,
                                    long showsBefore,
                                    long showsNow,
                                    double priceBefore,
                                    double priceAfter)
        {
            string filepath = @$"C:\Users\Administrator\Desktop\logs\{DateTime.Now.Date.ToString("ddMMyyyy")}.txt";
            
            if(!File.Exists(filepath))
                File.Create(filepath);

            using (StreamWriter sw = new StreamWriter(filepath,true,Encoding.UTF8))
            {
                string line = $"{DateTime.Now}  ID:{keyId}  Показов до:{showsBefore} После:{showsNow} Ставка до: {priceBefore}   После: {priceAfter}";
                sw.WriteLine(line);      
            }

        }
    }
}
