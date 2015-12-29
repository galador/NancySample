using System;

namespace NancySample.Modules.Application
{
    //Some extension methods to convert Unix time
    public static class DateTimeExtensions
    {
        public static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTime(this long unixTime)
        {
            return UnixEpoch.AddSeconds(unixTime);
        }
        public static DateTime FromUnixTime(this string unixTimeAsString)
        {
            var unixTime = Convert.ToInt64(unixTimeAsString);
            return FromUnixTime(unixTime);
        }
    }
}
