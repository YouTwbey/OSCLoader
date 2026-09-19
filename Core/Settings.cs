using OSCLoader.Debug;

namespace OSCLoader.Core
{
    internal static class Settings
    {
        public static Dictionary<string, string> StringPrefs = new Dictionary<string, string>();
        public static Dictionary<string, int> IntPrefs = new Dictionary<string, int>();
        public static Dictionary<string, float> FloatPrefs = new Dictionary<string, float>();

        public static void LoadSettings()
        {
            Logging.DebugLog($"Loading settings..");
            string settingsFile = Main.GetSettingsFile();
            Logging.DebugLog($"Reading settings from file: {settingsFile}");
            string[] allSettings = File.ReadAllLines(settingsFile);
            foreach (string setting in allSettings)
            {
                if (string.IsNullOrEmpty(setting)) continue;
                string[] parts = setting.Split("|||");
                string key = parts[0];
                string value = parts[1];

                if (value.Contains("."))
                {
                    if (float.TryParse(value, out float f))
                    {
                        FloatPrefs.Add(key, f);
                    }
                }
                else
                {
                    if (int.TryParse(value, out int i))
                    {
                        IntPrefs.Add(key, i);
                    }
                    else
                    {
                        StringPrefs.Add(key, value);
                    }
                }
            }
            Logging.DebugLog($"Settings loaded!");
        }

        public static void Save()
        {
            File.WriteAllText(Main.GetSettingsFile(), ConvertToString());
        }

        public static string ConvertToString()
        {
            string result = "";

            foreach (string i in IntPrefs.Keys)
            {
                result += $"{i}|||{IntPrefs[i]}\n";
            }
            foreach (string f in FloatPrefs.Keys)
            {
                result += $"{f}|||{FloatPrefs[f]}\n";
            }
            foreach (string s in StringPrefs.Keys)
            {
                result += $"{s}|||{StringPrefs[s]}\n";
            }

            return result;
        }
    }
}