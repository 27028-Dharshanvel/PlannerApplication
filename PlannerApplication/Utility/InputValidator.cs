using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PlannerApplication.Utility
{
    internal static class InputValidator
    {
        public static bool IsValidInt(string input, out int result)
        {
            return int.TryParse(input, out result);
        }
        public static bool IsValidDouble(string input, out double result)
        {
            return double.TryParse(input, out result);
        }
        public static bool IsValidDecimal(string input, out decimal result)
        {
            return decimal.TryParse(input, out result);
        }
        public static bool IsValidString(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        //To check values are within range and under maxcharacters

        public static bool IsIntWithinRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        public static bool IsDoubleWithinRange(double value, double min, double max)
        {
            return value >= min && value <= max;
        }

        public static bool IsStringWithinMaxLength(string input, int maxLength)
        {
            return input.Length <= maxLength;
        }

        //To check other types

        public static bool IsValidDate(string input, out DateOnly date)
        {
            return DateOnly.TryParse(input, out date);
        }

        public static bool IsValidPastDate(DateOnly date, int maxYears)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            DateOnly cutOffDate = today.AddYears(-maxYears);
            return date <= today && date >= cutOffDate;
        }

        public static bool IsValidFutureDate(DateOnly date, int maxYears)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            DateOnly cutOffDate = today.AddYears(maxYears);
            return date >= today && date <= cutOffDate;
        }
    }
}