using OSCLoader.Debug;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DiscordStatus
{
    public static class DiscordLibraryLoader
    {
        public static bool Loaded { get; private set; }
        public const string LibDestinationPath = "discord_game_sdk.dll";

        public static void LoadLibrary()
        {
            if (Loaded)
                return;

            if (!File.Exists(LibDestinationPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(LibDestinationPath));
                using (var libStr = Assembly.GetExecutingAssembly().GetManifestResourceStream(LibDestinationPath))
                {
                    if (libStr == null)
                        throw new NullReferenceException($"Could not find the SDK library resource.");

                    var data = new byte[libStr.Length];
                    libStr.Read(data, 0, data.Length);
                    File.WriteAllBytes(LibDestinationPath, data);
                }
            }

            if (LoadLibrary(LibDestinationPath) == IntPtr.Zero)
                throw new DllNotFoundException($"Could not load the Discord SDK library from '{LibDestinationPath}'.");

            Logging.DebugLog("Discord Library loaded.");
            Loaded = true;
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string path);
    }
}
