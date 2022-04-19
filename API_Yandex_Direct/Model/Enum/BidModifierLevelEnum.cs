﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace API_Yandex_Direct.Model.Enum
{
    /// <summary>
    /// Уровень корректировки
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BidModifierLevelEnum : byte
    {
        CAMPAIGN,
        AD_GROUP
    }
}
