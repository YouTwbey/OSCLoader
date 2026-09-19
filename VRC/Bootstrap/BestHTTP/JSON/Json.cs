using System.Collections;
using System.Globalization;
using System.Text;

namespace VRC.Bootstrap.BestHTTP.JSON
{
    internal static class Json
    {
        public static object Decode(string json)
        {
            bool flag = true;
            return Json.Decode(json, ref flag);
        }

        public static object Decode(string json, ref bool success)
        {
            success = true;
            if (json != null)
            {
                char[] json2 = json.ToCharArray();
                int num = 0;
                return Json.ParseValue(json2, ref num, ref success);
            }
            return new object();
        }

        public static string? Encode(object json)
        {
            StringBuilder stringBuilder = new StringBuilder(2000);
            if (!Json.SerializeValue(json, stringBuilder))
            {
                return null;
            }
            return stringBuilder.ToString();
        }

        public static Dictionary<string, object> ParseObject(char[] json, ref int index, ref bool success)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            Json.NextToken(json, ref index);
            bool flag = false;
            while (!flag)
            {
                int num = Json.LookAhead(json, index);
                if (num == 0)
                {
                    success = false;
                    return new Dictionary<string, object>();
                }
                if (num == 6)
                {
                    Json.NextToken(json, ref index);
                }
                else
                {
                    if (num == 2)
                    {
                        Json.NextToken(json, ref index);
                        return dictionary;
                    }
                    string key = Json.ParseString(json, ref index, ref success);
                    if (!success)
                    {
                        success = false;
                        return new Dictionary<string, object>();
                    }
                    num = Json.NextToken(json, ref index);
                    if (num != 5)
                    {
                        success = false;
                        return new Dictionary<string, object>();
                    }
                    object value = Json.ParseValue(json, ref index, ref success);
                    if (!success)
                    {
                        success = false;
                        return new Dictionary<string, object>();
                    }
                    dictionary[key] = value;
                }
            }
            return dictionary;
        }

        public static List<object> ParseArray(char[] json, ref int index, ref bool success)
        {
            List<object> list = new List<object>();
            Json.NextToken(json, ref index);
            bool flag = false;
            while (!flag)
            {
                int num = Json.LookAhead(json, index);
                if (num == 0)
                {
                    success = false;
                    return new List<object>();
                }
                if (num == 6)
                {
                    Json.NextToken(json, ref index);
                }
                else
                {
                    if (num == 4)
                    {
                        Json.NextToken(json, ref index);
                        break;
                    }
                    object item = Json.ParseValue(json, ref index, ref success);
                    if (!success)
                    {
                        return new List<object>();
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        public static object ParseValue(char[] json, ref int index, ref bool success)
        {
            switch (Json.LookAhead(json, index))
            {
                case 1:
                    return Json.ParseObject(json, ref index, ref success);
                case 3:
                    return Json.ParseArray(json, ref index, ref success);
                case 7:
                    return Json.ParseString(json, ref index, ref success);
                case 8:
                    return Json.ParseNumber(json, ref index, ref success);
                case 9:
                    Json.NextToken(json, ref index);
                    return true;
                case 10:
                    Json.NextToken(json, ref index);
                    return false;
                case 11:
                    Json.NextToken(json, ref index);
                    return new object();
            }
            success = false;
            return new object();
        }

        public static string ParseString(char[] json, ref int index, ref bool success)
        {
            StringBuilder stringBuilder = new StringBuilder(2000);
            Json.EatWhitespace(json, ref index);
            int num = index;
            index = num + 1;
            char c = json[num];
            bool flag = false;
            while (!flag && index != json.Length)
            {
                num = index;
                index = num + 1;
                c = json[num];
                if (c == '"')
                {
                    flag = true;
                    break;
                }
                if (c == '\\')
                {
                    if (index == json.Length)
                    {
                        break;
                    }
                    num = index;
                    index = num + 1;
                    c = json[num];
                    if (c == '"')
                    {
                        stringBuilder.Append('"');
                    }
                    else if (c == '\\')
                    {
                        stringBuilder.Append('\\');
                    }
                    else if (c == '/')
                    {
                        stringBuilder.Append('/');
                    }
                    else if (c == 'b')
                    {
                        stringBuilder.Append('\b');
                    }
                    else if (c == 'f')
                    {
                        stringBuilder.Append('\f');
                    }
                    else if (c == 'n')
                    {
                        stringBuilder.Append('\n');
                    }
                    else if (c == 'r')
                    {
                        stringBuilder.Append('\r');
                    }
                    else if (c == 't')
                    {
                        stringBuilder.Append('\t');
                    }
                    else if (c == 'u')
                    {
                        if (json.Length - index < 4)
                        {
                            break;
                        }
                        uint utf;
                        if (!(success = uint.TryParse(new string(json, index, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out utf)))
                        {
                            return "";
                        }
                        stringBuilder.Append(char.ConvertFromUtf32((int)utf));
                        index += 4;
                    }
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }
            if (!flag)
            {
                success = false;
                return "";
            }
            return stringBuilder.ToString();
        }

        public static double ParseNumber(char[] json, ref int index, ref bool success)
        {
            Json.EatWhitespace(json, ref index);
            int lastIndexOfNumber = Json.GetLastIndexOfNumber(json, index);
            int length = lastIndexOfNumber - index + 1;
            double result;
            success = double.TryParse(new string(json, index, length), NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            index = lastIndexOfNumber + 1;
            return result;
        }

        public static int GetLastIndexOfNumber(char[] json, int index)
        {
            int num = index;
            while (num < json.Length && "0123456789+-.eE".IndexOf(json[num]) != -1)
            {
                num++;
            }
            return num - 1;
        }

        public static void EatWhitespace(char[] json, ref int index)
        {
            while (index < json.Length && " \t\n\r".IndexOf(json[index]) != -1)
            {
                index++;
            }
        }

        public static int LookAhead(char[] json, int index)
        {
            int num = index;
            return Json.NextToken(json, ref num);
        }

        public static int NextToken(char[] json, ref int index)
        {
            Json.EatWhitespace(json, ref index);
            if (index == json.Length)
            {
                return 0;
            }
            char c = json[index];
            index++;
            if (c <= '[')
            {
                switch (c)
                {
                    case '"':
                        return 7;
                    case '#':
                    case '$':
                    case '%':
                    case '&':
                    case '\'':
                    case '(':
                    case ')':
                    case '*':
                    case '+':
                    case '.':
                    case '/':
                        break;
                    case ',':
                        return 6;
                    case '-':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        return 8;
                    case ':':
                        return 5;
                    default:
                        if (c == '[')
                        {
                            return 3;
                        }
                        break;
                }
            }
            else
            {
                if (c == ']')
                {
                    return 4;
                }
                if (c == '{')
                {
                    return 1;
                }
                if (c == '}')
                {
                    return 2;
                }
            }
            index--;
            int num = json.Length - index;
            if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
            {
                index += 5;
                return 10;
            }
            if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
            {
                index += 4;
                return 9;
            }
            if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
            {
                index += 4;
                return 11;
            }
            return 0;
        }

        public static bool SerializeValue(object value, StringBuilder builder)
        {
            bool result = true;

            if (value is string)
            {
                result = Json.SerializeString((string)value, builder);
            }
            else if (value is IDictionary)
            {
                result = Json.SerializeObject((IDictionary)value, builder);
            }
            else if (value is IList)
            {
                result = Json.SerializeArray(value as IList, builder);
            }
            else if (value is bool && (bool)value)
            {
                builder.Append("true");
            }
            else if (value is bool && !(bool)value)
            {
                builder.Append("false");
            }
            else if (value is ValueType)
            {
                result = Json.SerializeNumber(Convert.ToDouble(value), builder);
            }
            else if (value == null)
            {
                builder.Append("null");
            }
            else
            {
                result = false;
            }
            return result;
        }

        public static bool SerializeObject(IDictionary anObject, StringBuilder builder)
        {
            builder.Append("{");
            IDictionaryEnumerator enumerator = anObject.GetEnumerator();
            bool flag = true;
            while (enumerator.MoveNext())
            {
                string aString = enumerator.Key.ToString();
                object value = enumerator.Value;
                if (!flag)
                {
                    builder.Append(", ");
                }
                Json.SerializeString(aString, builder);
                builder.Append(":");
                if (!Json.SerializeValue(value, builder))
                {
                    return false;
                }
                flag = false;
            }
            builder.Append("}");
            return true;
        }

        public static bool SerializeArray(IList anArray, StringBuilder builder)
        {
            builder.Append("[");
            bool flag = true;
            for (int i = 0; i < anArray.Count; i++)
            {
                object value = anArray[i];
                if (!flag)
                {
                    builder.Append(", ");
                }
                if (!Json.SerializeValue(value, builder))
                {
                    return false;
                }
                flag = false;
            }
            builder.Append("]");
            return true;
        }

        public static bool SerializeString(string aString, StringBuilder builder)
        {
            builder.Append("\"");
            foreach (char c in aString.ToCharArray())
            {
                if (c == '"')
                {
                    builder.Append("\\\"");
                }
                else if (c == '\\')
                {
                    builder.Append("\\\\");
                }
                else if (c == '\b')
                {
                    builder.Append("\\b");
                }
                else if (c == '\f')
                {
                    builder.Append("\\f");
                }
                else if (c == '\n')
                {
                    builder.Append("\\n");
                }
                else if (c == '\r')
                {
                    builder.Append("\\r");
                }
                else if (c == '\t')
                {
                    builder.Append("\\t");
                }
                else
                {
                    int num = Convert.ToInt32(c);
                    if (num >= 32 && num <= 126)
                    {
                        builder.Append(c);
                    }
                    else
                    {
                        builder.Append("\\u" + Convert.ToString(num, 16).PadLeft(4, '0'));
                    }
                }
            }
            builder.Append("\"");
            return true;
        }

        public static bool SerializeNumber(double number, StringBuilder builder)
        {
            builder.Append(Convert.ToString(number, CultureInfo.InvariantCulture));
            return true;
        }

        const int TOKEN_NONE = 0;
        const int TOKEN_CURLY_OPEN = 1;
        const int TOKEN_CURLY_CLOSE = 2;
        const int TOKEN_SQUARED_OPEN = 3;
        const int TOKEN_SQUARED_CLOSE = 4;
        const int TOKEN_COLON = 5;
        const int TOKEN_COMMA = 6;
        const int TOKEN_STRING = 7;
        const int TOKEN_NUMBER = 8;
        const int TOKEN_TRUE = 9;
        const int TOKEN_FALSE = 10;
        const int TOKEN_NULL = 11;
        const int BUILDER_CAPACITY = 2000;
    }
}
