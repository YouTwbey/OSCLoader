using OSCLoader.Core;

namespace UnityEngine
{
    /// <summary>
    /// PlayerPrefs is a class remade from Unity that stores Player preferences between game sessions. It can store string, float and integer values into the client's settings file.
    /// </summary>
    public static class PlayerPrefs
    {
        /// <summary>
        /// The PlayerPref value type
        /// </summary>
        public enum PrefType
        {
            /// <summary>
            /// String value
            /// </summary>
            String,
            /// <summary>
            /// Float value
            /// </summary>
            Float,
            /// <summary>
            /// Int value
            /// </summary>
            Int
        }

        /// <summary>
        /// Deletes all PlayerPref values stored on the client's device. Use with caution.
        /// </summary>
        public static void DeleteAll()
        {
            Settings.StringPrefs.Clear();
            Settings.FloatPrefs.Clear();
            Settings.IntPrefs.Clear();
        }

        /// <summary>
        /// Deletes a specific key from the client's PlayerPrefs if it exists.
        /// </summary>
        /// <param name="type">The type of value to delete</param>
        /// <param name="key">The name of the key</param>
        public static void DeleteKey(PrefType type, string key)
        {
            switch (type)
            {
                case PrefType.String:
                    if (Settings.StringPrefs.ContainsKey(key))
                    {
                        Settings.StringPrefs.Remove(key);
                    }
                    break;
                case PrefType.Int:
                    if (Settings.IntPrefs.ContainsKey(key))
                    {
                        Settings.IntPrefs.Remove(key);
                    }
                    break;
                case PrefType.Float:
                    if (Settings.FloatPrefs.ContainsKey(key))
                    {
                        Settings.FloatPrefs.Remove(key);
                    }
                    break;
            }
        }

        /// <summary>
        /// Gets the float pref of a specific key
        /// </summary>
        /// <param name="key">The key of the float pref</param>
        /// <returns>The value for which the key is assigned to. Returns 0 if it cannot be found</returns>
        public static float GetFloat(string key)
        {
            if (Settings.FloatPrefs.ContainsKey(key))
            {
                Settings.FloatPrefs.Remove(key);
            }

            return 0;
        }

        /// <summary>
        /// Gets the float pref of a specific key
        /// </summary>
        /// <param name="key">The key of the float pref</param>
        /// <param name="defaultValue">The value to return if it cannot be found</param>
        /// <returns>The value for which the key is assigned to. Returns defaultValue if it cannot be found</returns>
        public static float GetFloat(string key, float defaultValue)
        {
            if (Settings.FloatPrefs.ContainsKey(key))
            {
                Settings.FloatPrefs.Remove(key);
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the int pref of a specific key
        /// </summary>
        /// <param name="key">The key of the int pref</param>
        /// <returns>The value for which the key is assigned to. Returns 0 if it cannot be found</returns>
        public static int GetInt(string key)
        {
            if (Settings.IntPrefs.ContainsKey(key))
            {
                Settings.IntPrefs.Remove(key);
            }

            return 0;
        }

        /// <summary>
        /// Gets the int pref of a specific key
        /// </summary>
        /// <param name="key">The key of the int pref</param>
        /// <param name="defaultValue">The value to return if it cannot be found</param>
        /// <returns>The value for which the key is assigned to. Returns defaultValue if it cannot be found</returns>
        public static int GetInt(string key, int defaultValue)
        {
            if (Settings.IntPrefs.ContainsKey(key))
            {
                Settings.IntPrefs.Remove(key);
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the string pref of a specific key
        /// </summary>
        /// <param name="key">The key of the string pref</param>
        /// <returns>The value for which the key is assigned to. Returns string.Empty if it cannot be found</returns>
        public static string GetString(string key)
        {
            if (Settings.StringPrefs.ContainsKey(key))
            {
                Settings.StringPrefs.Remove(key);
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the string pref of a specific key
        /// </summary>
        /// <param name="key">The key of the string pref</param>
        /// <param name="defaultValue">The value to return if it cannot be found</param>
        /// <returns>The value for which the key is assigned to. Returns defaultValue if it cannot be found</returns>
        public static string GetString(string key, string defaultValue)
        {
            if (Settings.StringPrefs.ContainsKey(key))
            {
                Settings.StringPrefs.Remove(key);
            }

            return defaultValue;
        }

        /// <summary>
        /// Checks to see if a key exists in the requested value type
        /// </summary>
        /// <param name="type">The type of value to check</param>
        /// <param name="key">The name of the key</param>
        public static bool HasKey(PrefType type, string key)
        {
            switch (type)
            {
                case PrefType.String:
                    if (Settings.StringPrefs.ContainsKey(key))
                    {
                        return true;
                    }
                    return false;
                case PrefType.Int:
                    if (Settings.IntPrefs.ContainsKey(key))
                    {
                        return true;
                    }
                    return false;
                case PrefType.Float:
                    if (Settings.FloatPrefs.ContainsKey(key))
                    {
                        return true;
                    }
                    return false;
            }

            return false;
        }

        /// <summary>
        /// Saves all PlayerPrefs manually
        /// </summary>
        public static void Save()
        {
            Settings.Save();
        }

        /// <summary>
        /// Sets a float pref
        /// </summary>
        /// <param name="key">The name of the key</param>
        /// <param name="value">The float value</param>
        public static void SetFloat(string key, float value)
        {
            if (Settings.FloatPrefs.ContainsKey(key))
            {
                Settings.FloatPrefs[key] = value;
            }
            else
            {
                Settings.FloatPrefs.Add(key, value);
            }
        }

        /// <summary>
        /// Sets an int pref
        /// </summary>
        /// <param name="key">The name of the key</param>
        /// <param name="value">The int value</param>
        public static void SetInt(string key, int value)
        {
            if (Settings.IntPrefs.ContainsKey(key))
            {
                Settings.IntPrefs[key] = value;
            }
            else
            {
                Settings.IntPrefs.Add(key, value);
            }
        }

        /// <summary>
        /// Sets a string pref
        /// </summary>
        /// <param name="key">The name of the key</param>
        /// <param name="value">The string value</param>
        public static void SetString(string key, string value)
        {
            if (Settings.StringPrefs.ContainsKey(key))
            {
                Settings.StringPrefs[key] = value;
            }
            else
            {
                Settings.StringPrefs.Add(key, value);
            }
        }
    }
}
