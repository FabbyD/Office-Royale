using System;

namespace OfficeRoyale.Utilities.ORTime
{
    public class ORTime
    {
        // Return unix timestamp in seconds
        public static double UtcTimestamp()
        {
            return (double)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000;
        }
    }
}
