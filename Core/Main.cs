using OSCLoader.Core.Handlers;
using OSCLoader.Debug;
using Photon.Pun;
using System.Diagnostics;
using System.Text;
using VRC.Managers;

namespace OSCLoader.Core
{
    internal static class Main
    {
        public static string CurrentVRCLogFile = "";

        public static void Init()
        {
            Logging.SetupLogPrinter();
            Settings.LoadSettings();
            AppDomain.CurrentDomain.ProcessExit += (_, __) => { OnProcessExit(); };
            Logging.timeOnOpenApp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
            Console.Title = LoaderInfo.DebugMode ? $"{LoaderInfo.Name} [DEBUG] | {LoaderInfo.Version}" : $"{LoaderInfo.Name} | {LoaderInfo.Version}";
            Logging.Log($"{LoaderInfo.Name} | {LoaderInfo.Version}");
            Logging.Log($"Written by {LoaderInfo.Author}");
            Logging.Log("Loading libs...");
            VRCModsManager.LoadAllLibs();
            Logging.Log($"{VRCModsManager.Libs.Count} libs loaded!");
            Logging.Log("Loading mods...");
            VRCModsManager.LoadAllMods();
            Logging.Log($"{VRCModsManager.VRChatMods.Count} mods loaded!");
            LaunchVRChat();
            HandleUpdate();
        }

        public static void OnProcessExit(bool isFromVRChat = false)
        {
            VRCModsManager.OnApplicationQuitAll(isFromVRChat);

            if (isFromVRChat)
            {
                Environment.Exit(0);
            }
            else
            {
                if (VRC.Core.EasyAntiCheat_Installer != null) VRC.Core.EasyAntiCheat_Installer.Kill();
                if (VRC.Core.EasyAntiCheat_Startup != null) VRC.Core.EasyAntiCheat_Startup.Kill();
                if (VRC.Core.VRChat != null) VRC.Core.VRChat.Kill();
            }
        }

        static void HandleUpdate()
        {
            while (true)
            {
                OSCManager.HandleOSC();

                if (VRC.Core.VRChat == null)
                {
                    Process[] processes = Process.GetProcessesByName("VRChat");

                    if (processes.Length != 0)
                    {
                        Logging.Log("Attempting hook onto VRChat's process...");
                        VRC.Core.PerformHook(processes[0]);
                    }
                }
                else
                {
                    if (VRC.Core.VRChat.MainWindowTitle != $"VRChat [{LoaderInfo.Name} | v{LoaderInfo.Version}]" && PhotonNetwork.LocalPlayer == null)
                    {
                        VRC.Core.ChangeWindowTitle($"VRChat [{LoaderInfo.Name} | v{LoaderInfo.Version}]");
                    }
                    VRC.Core.UpdateImGuiPosition();
                }

                Update();
                Thread.Sleep(17);
            }
        }

        static void Update()
        {
            VRCModsManager.EarlyUpdateAll();
            VRCModsManager.UpdateAll();
            VRCModsManager.LateUpdateAll();
            Input.Update();
        }

        public static string GetModsPath()
        {
            string exeDir = AppContext.BaseDirectory;
            string path = Path.Combine(exeDir, "Mods");
            if (!Path.Exists(path)) Directory.CreateDirectory(path);

            return path + "/";
        }

        public static string GetLibsPath()
        {
            string exeDir = AppContext.BaseDirectory;
            string path = Path.Combine(exeDir, "Libs");
            if (!Path.Exists(path)) Directory.CreateDirectory(path);

            return path + "/";
        }

        public static string GetLogsPath()
        {
            string exeDir = AppContext.BaseDirectory;
            string path = Path.Combine(exeDir, "Logs");
            if (!Path.Exists(path)) Directory.CreateDirectory(path);

            if (Directory.GetFiles(path).Count() <= 10)
            {
                foreach (FileInfo file in new DirectoryInfo(path).GetFiles().OrderByDescending(f => f.LastWriteTimeUtc).ToList().Skip(9))
                {
                    file.Delete();
                }
            }

            return path + "/";
        }

        public static string GetVRChatLogsPath()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).Replace("Local", "LocalLow"), "VRChat", "VRChat");
            return path + "/";
        }

        public static string GetSettingsFile()
        {
            string exeDir = AppContext.BaseDirectory;
            string path = Path.Combine(exeDir, "settings.cfg");
            if (!File.Exists(path)) File.WriteAllText(path, "");

            return path;
        }

        public static string GetGlobalStoragePath()
        {
            string exeDir = AppContext.BaseDirectory;
            string path = Path.Combine(exeDir, "ModStorage");

            return path + "/";
        }

        public static void LaunchVRChat()
        {
            VRC.Bootstrap.launch.Program.Main(Environment.GetCommandLineArgs());

            Task.Run(() => GetLogFile());
        }

        static async Task GetLogFile()
        {
            string logFolder = GetVRChatLogsPath();

            string[] existingFiles = Directory.GetFiles(logFolder);
            bool searching = true;

            while (searching)
            {
                await Task.Delay(100);

                string[] currentFiles = Directory.GetFiles(logFolder);
                foreach (string file in currentFiles)
                {
                    if (!existingFiles.Contains(file))
                    {
                        CurrentVRCLogFile = file;
                        searching = false;
                    }
                }
            }

            using var fs = new FileStream(CurrentVRCLogFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fs, Encoding.UTF8);
            fs.Seek(0, SeekOrigin.End);

            while (true)
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line)) VRCModsManager.HandleLog(line);
                }
                await Task.Delay(100);
            }
        }
    }
}
