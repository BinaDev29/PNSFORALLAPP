// File Path: Persistence/Converters/EmailAddressConverter.cs
using Domain.ValueObjects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Persistence.Converters
{
    public class EmailAddressConverter : JsonConverter<EmailAddress>
    {
        public override EmailAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("EmailAddress expected a string value.");
            }
            var email = reader.GetString();
            return EmailAddress.Create(email!);
        }

        public override void Write(Utf8JsonWriter writer, EmailAddress value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}