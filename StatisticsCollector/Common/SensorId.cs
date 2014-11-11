using DddSkeleton.Domain;
using System;
using System.ComponentModel;
using System.Linq;

namespace StatisticsCollector.Common
{
    // type converter needed for JSON deserialization
    [TypeConverter(typeof(SensorIdConverter))]
    public class SensorId: ValueObject, IEquatable<SensorId>
    {
        private string[] parts;

        public SensorId(string name): this(name, "/") {}

        public SensorId(string name, string delimiter)
        {
            parts = name.Split(delimiter.ToCharArray(), 3);

            if (parts.Length != 3)
                throw new ArgumentException("not three parts in name");
        }

        // this one will be serialized by NewtonSoft.JSON
        public string Name {
            get { return DelimitedBy("/"); }
        }

        public string DelimitedBy(string delimiter)
        {
            return String.Join(delimiter, parts);
        }

        public bool MatchesMask(string mask)
        {
            return MatchesMask(mask ?? "", "/");
        }

        public bool MatchesMask(string mask, string delimiter)
        {
            return MatchesMask(mask.Split(delimiter.ToCharArray(), 3));
        }

        private bool MatchesMask(string[] maskParts)
        {
            var matches = true;

            for (var i = 0; i < parts.Length; i++)
            {
                var maskPart = i < maskParts.Length ? maskParts[i] : null;
                matches = matches && PartMatches(parts[i], maskPart);
            }

            return matches;
        }

        public bool Equals(SensorId other)
        {
            return other != null
                && this.ToString() == other.ToString();
        }

        public override bool Equals(object other)
        {
            return other is SensorId
                ? Equals((SensorId) other)
                : false;
        }

        public static bool operator ==(SensorId s1, SensorId s2)
        {
            if ((object)s1 == null && (object)s2 == null) return true;
            if ((object)s1 == null || (object)s2 == null) return false;

            return s1.ToString() == s2.ToString();
        }

        public static bool operator !=(SensorId s1, SensorId s2)
        {
            return !(s1 == s2);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /*
        private bool MatchesMask(string firstMask, string middleMask, string lastMask)
        {
            return PartMatches(parts[0], firstMask)
                && PartMatches(parts[1], middleMask)
                && PartMatches(parts[2], lastMask);
        }
        */

        private bool PartMatches(string part, string mask)
        {
            return String.IsNullOrEmpty(mask) || mask == "*" || part == mask;
        }

        public override string ToString()
        {
            return DelimitedBy("/");
        }
    
    }
}
