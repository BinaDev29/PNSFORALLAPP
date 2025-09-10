// File Path: Domain/ValueObjects/PhoneNumber.cs
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        public string Value { get; private set; }

        private PhoneNumber(string value)
        {
            Value = value;
        }

        public static PhoneNumber Create(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be empty.", nameof(phoneNumber));

            // የስልክ ቁጥሩን ለማረጋገጥ የሚረዳ ቀላል Regex (በአገር ላይ ተመስርቶ ይበልጥ ውስብስብ ሊሆን ይችላል)
            var normalizedNumber = Regex.Replace(phoneNumber, @"[^\d]", ""); // ቁጥሮችን ብቻ ያስቀምጣል

            if (!Regex.IsMatch(normalizedNumber, @"^\d{10,15}$"))
                throw new ArgumentException($"'{phoneNumber}' is not a valid phone number format.", nameof(phoneNumber));

            return new PhoneNumber(normalizedNumber);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;

        // ከ string ወደ PhoneNumber የሚቀይር Implicit operator
        public static implicit operator PhoneNumber(string value) => Create(value);

        // ከ PhoneNumber ወደ string የሚቀይር Explicit operator
        public static explicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;
    }
}