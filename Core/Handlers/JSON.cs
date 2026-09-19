using System.Text.Json;

namespace OSCLoader.Core.Handlers
{
    internal static class JSON
    {
        public static string GetString(JsonElement element, string property)
        {
            if (element.TryGetProperty(property, out JsonElement value))
            {
                if (value.ValueKind == JsonValueKind.String)
                {
                    string? val = value.GetString();

                    if (val != null)
                    {
                        return val;
                    }
                }
            }

            return "null";
        }

        public static string GetRawString(JsonElement element, string property)
        {
            if (element.TryGetProperty(property, out JsonElement value))
            {
                return value.GetRawText();
            }

            return "null";
        }

        public static int GetInt32(JsonElement element, string property)
        {
            if (element.TryGetProperty(property, out JsonElement value))
            {
                if (value.TryGetInt32(out int val))
                {
                    return val;
                }
            }

            return 0;
        }

        public static DateTime GetDateTime(JsonElement element, string property)
        {
            if (element.TryGetProperty(property, out JsonElement value))
            {
                if (value.TryGetDateTime(out DateTime val))
                {
                    return val;
                }
            }

            return DateTime.MinValue;
        }

        public static bool GetBool(JsonElement element, string property)
        {
            if (element.TryGetProperty(property, out JsonElement value))
            {
                return value.GetBoolean();
            }

            return false;
        }
    }
}
