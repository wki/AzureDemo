﻿using DddSkeleton.Domain;
using System;

namespace StatisticsCollector.Common
{
    public class SensorId: ValueObject
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
    }
}
