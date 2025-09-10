// File Path: Persistence/Converters/PhoneNumberConverter.cs
using Domain.ValueObjects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Persistence.Converters
{
    public class PhoneNumberConverter : JsonConverter<PhoneNumber>
    {
        public override PhoneNumber? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("PhoneNumber expected a string value.");
            }
            var phoneNumber = reader.GetString();
            return PhoneNumber.Create(phoneNumber!);
        }

        public override void Write(Utf8JsonWriter writer, PhoneNumber value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}