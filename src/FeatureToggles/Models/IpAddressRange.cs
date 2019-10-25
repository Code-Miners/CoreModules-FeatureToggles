

namespace FeatureToggles.Models
{
    using System;
    using System.Net;

    public class IPAddressRange
    {
        private readonly bool empty;

        public IPAddress Lower { get; }

        public IPAddress Upper { get; }


        public static IPAddressRange Empty => new IPAddressRange();

        private IPAddressRange()
        {
            empty = true;
        }


        public IPAddressRange(IPAddress lower, IPAddress upper)
        {
            Lower = lower;
            Upper = upper;
            empty = false;
        }

        public static bool IsNullOrEmpty(IPAddressRange range)
        {
            if (range == null)
            {
                return true;
            }

            return range.empty;
        }

        public static IPAddressRange FromCidrAddress(string candidateRange)
        {
            if (string.IsNullOrWhiteSpace(candidateRange))
            {
                return Empty;
            }

            int slashIndex = candidateRange.IndexOf('/');

            if (slashIndex < 0)
            {
                return Empty;
            }

            string[] parts = candidateRange.Split('.', '/');

            uint ipnum = (Convert.ToUInt32(parts[0]) << 24) |
                         (Convert.ToUInt32(parts[1]) << 16) |
                         (Convert.ToUInt32(parts[2]) << 8) |
                         Convert.ToUInt32(parts[3]);

            int maskbits = Convert.ToInt32(parts[4]);
            uint mask = 0xffffffff;
            mask <<= (32 - maskbits);

            uint ipstart = ipnum & mask;
            uint ipend = ipnum | (~mask);

            IPAddress start = new IPAddress(new [] {
                (byte)((ipstart >> 24) & 0xFF) ,
                (byte)((ipstart >> 16) & 0xFF) ,
                (byte)((ipstart >> 8)  & 0xFF) ,
                (byte)( ipstart & 0xFF)});

            IPAddress end = new IPAddress(new [] {
                (byte)((ipend >> 24) & 0xFF) ,
                (byte)((ipend >> 16) & 0xFF) ,
                (byte)((ipend >> 8)  & 0xFF) ,
                (byte)( ipend & 0xFF)});

            return new IPAddressRange(start, end);
        }

        public override string ToString()
        {
            string start = Lower.ToString();
            string end = Upper.ToString();

            return string.Concat(start, " - ", end);
        }

        public bool IPInRange(IPAddress candidate)
        {
            byte[] lowerBytes = Lower.GetAddressBytes();
            byte[] upperBytes = Upper.GetAddressBytes();
            byte[] addressBytes = candidate.GetAddressBytes();

            bool lowerBoundary = true, upperBoundary = true;

            for (int i = 0; i < lowerBytes.Length && (lowerBoundary || upperBoundary); i++)
            {
                if ((lowerBoundary && addressBytes[i] < lowerBytes[i]) ||
                    (upperBoundary && addressBytes[i] > upperBytes[i]))
                {
                    return false;
                }

                lowerBoundary &= (addressBytes[i] == lowerBytes[i]);
                upperBoundary &= (addressBytes[i] == upperBytes[i]);
            }

            return true;
        }
    }
}
