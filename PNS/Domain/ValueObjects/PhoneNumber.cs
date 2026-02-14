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

            if (!IsValid(phoneNumber))
                throw new ArgumentException($"'{phoneNumber}' is not a valid phone number format.", nameof(phoneNumber));

            var normalizedNumber = Regex.Replace(phoneNumber, @"[^\d]", "");
            return new PhoneNumber(normalizedNumber);
        }

        public static bool IsValid(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return false;
            var normalizedNumber = Regex.Replace(phoneNumber, @"[^\d]", "");
            return Regex.IsMatch(normalizedNumber, @"^\d{10,15}$");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;

        // change from string to PhoneNumber  Implicit operator
        public static implicit operator PhoneNumber(string value) => Create(value);

        // change from PhoneNumber to string  Explicit operator
        public static explicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;
    }
}