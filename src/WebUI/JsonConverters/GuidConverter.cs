using Newtonsoft.Json;

namespace CleanArchitecture.WebUI.JsonConverters;

 class GuidConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Guid) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    return Guid.Empty;
                case JsonToken.String:
                    string? str = reader.Value as string;
                    if (string.IsNullOrEmpty(str))
                    {
                        return Guid.Empty;
                    }
                    else
                    {
                        return new Guid(str);
                    }
                default:
                    throw new ArgumentException("Invalid token type");
            }
        }
        public override void WriteJson(JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if ( value==null || Guid.Empty.Equals(value))
            {
                writer.WriteValue("");
            }
            else
            {
                writer.WriteValue((Guid)value);
            }
        }
    }
