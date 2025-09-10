// File Path: Domain/ValueObjects/EmailAddress.cs
using Domain.Common; // ValueObject ከዚህ ነው የሚወርሰው
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public class EmailAddress : ValueObject
    {
        public string Value { get; private set; }

        private EmailAddress(string value)
        {
            Value = value;
        }

        public static EmailAddress Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email address cannot be empty.", nameof(email));

            if (!IsValidEmail(email))
                throw new ArgumentException($"'{email}' is not a valid email address format.", nameof(email));

            return new EmailAddress(email.ToLowerInvariant()); // ወደ lower case ይቀይራል
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                // RFC 5322 compliant regex
                return Regex.IsMatch(email,
                    @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                    + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)"
                    + @"(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;

        // ከ string ወደ EmailAddress የሚቀይር Implicit operator
        public static implicit operator EmailAddress(string value) => Create(value);

        // Explicit operator for converting EmailAddress to string
        // ይህም ግልጽ ያደርገዋል እንጂ ግራ አያጋባም
        public static explicit operator string(EmailAddress emailAddress) => emailAddress.Value;
    }
}