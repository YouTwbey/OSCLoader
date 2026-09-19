using Microsoft.Win32;
using OSCLoader.Debug;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.ServiceProcess;
using VRC.Bootstrap.BestHTTP.JSON;

namespace VRC.Bootstrap.launch
{
    internal enum PriorityArg
    {
        Idle = -2,
        BelowNormal,
        Normal,
        AboveNormal,
        High
    }

    internal static class Program
    {
        internal static PriorityArg? FindPriorityArgument(string argumentPrefix, ref List<string> argsList)
        {
            Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] Fetching PriorityArg...");
            if (!string.IsNullOrEmpty(argumentPrefix))
            {
                for (int i = argsList.Count - 1; i >= 0; i--)
                {
                    string text = argsList[i];
                    if (text.StartsWith(argumentPrefix))
                    {
                        string[] array = text.Split(new char[] { '=' });
                        if (array.Length == 2 && int.TryParse(array[1].Trim(new char[] { '"' }).Trim(), out int num) && Enum.IsDefined(typeof(PriorityArg), num))
                        {
                            Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] Found PriorityArg: {num}!");
                            return new PriorityArg?((PriorityArg)num);
                        }
                        argsList.RemoveAt(i);
                    }
                }
            }

            Logging.ModError(null, $"[VRChat Bootstrap] Failed to find PriorityArg.");
            return null;
        }


        static object? FetchAmplitudeExperimentValue(HttpClient httpClient, string amplitudeUserId, string flagKey, int millisecondsTimeout)
        {
            if (amplitudeUserId == null)
            {
                Logging.ModError(null, $"[VRChat Bootstrap] amplitudeUserId is null!");
                return null;
            }

            string requestUrl = "https://api.lab.amplitude.com/v1/vardata?&user_id=" + amplitudeUserId + "&flag_key=" + flagKey;
            Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] Fetching Amplitude Experiment Value from [{requestUrl}]...");
            HttpResponseMessage? httpResponseMessage = null;
            try
            {
                Task<HttpResponseMessage> async = httpClient.GetAsync(requestUrl);
                if (!async.Wait(millisecondsTimeout))
                {
                    Logging.ModError(null, $"[VRChat Bootstrap] Fetch timed out!");
                    return null;
                }
                httpResponseMessage = async.Result;
            }
            catch (Exception e)
            {
                Logging.ModError(null, $"[VRChat Bootstrap] FetchAmplitudeExperimentValue ran into a critical error! {e.ToString()}");
                return null;
            }

            object? result;
            try
            {
                if (httpResponseMessage == null)
                {
                    Logging.ModError(null, $"[VRChat Bootstrap] httpResponseMessage is null!");
                    result = null;
                }
                else if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    Logging.ModError(null, $"[VRChat Bootstrap] Fetch failed!");
                    result = null;
                }
                else
                {
                    HttpContent content = httpResponseMessage.Content;
                    if (content == null)
                    {
                        Logging.ModError(null, $"[VRChat Bootstrap] content is null!");
                        result = null;
                    }
                    else
                    {
                        object obj = Json.Decode(content.ReadAsStringAsync().Result);
                        if (obj == null)
                        {
                            Logging.ModError(null, $"[VRChat Bootstrap] Json is null!");
                            result = null;
                        }
                        else
                        {
                            Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
                            if (dictionary == null)
                            {
                                Logging.ModError(null, $"[VRChat Bootstrap] dictionary is null!");
                                result = null;
                            }
                            else if (!dictionary.TryGetValue(flagKey, out object? obj2))
                            {
                                Logging.ModError(null, $"[VRChat Bootstrap] obj2 is null!");
                                result = null;
                            }
                            else
                            {
                                result = obj2;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logging.ModError(null, $"[VRChat Bootstrap] FetchAmplitudeExperimentValue ran into a critical error! {e.ToString()}");
                result = null;
            }

            return result;
        }

        static string GetAmplitudeUserId()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp", "VRChat", "VRChat");
            string result = "";
            Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] Fetching client UserID from [{path}]...");

            try
            {
                if (File.Exists(Path.Combine(path, "settings_com.amplitude")))
                {
                    result = (string)((Dictionary<string, object>)Json.Decode(File.ReadAllText(Path.Combine(path, "settings_com.amplitude"))))["com.amplitude_userId"];
                }
            }
            catch (Exception e)
            {
                Logging.ModError(null, $"[VRChat Bootstrap] GetAmplitudeUserId ran into a critical error! {e.ToString()}");
            }
            
            if (result != "")
            {
                Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] Found UserID: {result}!");
            }
            else
            {
                Logging.ModLog(null, $"[VRChat Bootstrap] Failed to find UserID!");
            }

            return result;
        }

        public static void Main(string[] args)
        {
            List<string> argsList = args != null ? args.ToList() : new List<string>();
            foreach (string text in argsList)
	        {
                string text2 = text.ToLower();
                if (text2.StartsWith("vrchat://") && text2.Contains("&attach=1"))
                {
                    Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] Creating VRCNPClient...");

                    using (VRCNPClient vrcnpclient = new VRCNPClient())
                    {
                        if (vrcnpclient.AttemptConnect() && vrcnpclient.AttemptSendURL(text))
                        {
                            return;
                        }
                    }
                }
            }

            string baseDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..")) + "\\";
            string? text3 = (string?)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\EasyAntiCheat_EOS", "ProductsInstalled", "null");

            if (text3 == null)
            {
                text3 = "null";
            }

            if (((Dictionary<string, object>)Json.Decode(File.ReadAllText(baseDirectory + "\\EasyAntiCheat\\Settings.json"))).TryGetValue("productid", out object? obj))
            {
                string? text4 = null;

                if (obj != null)
                {
                    text4 = obj as string;
                }

                if (text4 != null)
                {
                    Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] Checking to see if EasyAntiCheat is installed...");
                    try
                    {
                        if (!ServiceController.GetServices().Any((ServiceController x) => x.ServiceName == "EasyAntiCheat_EOS") || text3 == null || !text3.Contains(text4))
                        {
                            Logging.ModWarn(null, $"[VRChat Bootstrap] EasyAntiCheat_EOS is not installed! Installing...");
                            Core.EasyAntiCheat_Installer = new Process();
                            Core.EasyAntiCheat_Installer.StartInfo.FileName = baseDirectory + "\\EasyAntiCheat\\EasyAntiCheat_EOS_Setup.exe";
                            Core.EasyAntiCheat_Installer.StartInfo.WorkingDirectory = baseDirectory;
                            Core.EasyAntiCheat_Installer.StartInfo.Arguments = "install " + text4;
                            Core.EasyAntiCheat_Installer.Start();
                            Core.EasyAntiCheat_Installer.WaitForExit();
                        }
                        else
                        {
                            Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] EasyAntiCheat already installed!");
                        }
                    }
                    catch (Exception e)
                    {
                        Logging.ModError(null, $"[VRChat Bootstrap] Error when trying to validate EasyAntiCheat's installation! [{e.ToString()}] Trying to continue anyways...");
                    }

                    HttpClient httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Api-Key client-bD50OiwphEUxwPTBQbUUIwhSTTDfCGdB");
                    nint? intPtr = null;
                    bool flag = false;
                    bool flag2 = false;
                    bool flag3 = false;
                    foreach (string text5 in argsList)
			        {
                        if (text5.StartsWith("--affinity="))
                        {
                            string[] array = text5.Split(new char[] { '=' });
                            if (array.Length == 2)
                            {
                                Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] Enabling affinity...");

                                string text6 = array[1].Trim(new char[] { '"' }).Trim();
                                flag3 = true;
                                if (text6.StartsWith("0x"))
                                {
                                    text6 = text6.Substring(2);
                                }
                                ulong value;
                                if (ulong.TryParse(text6, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
                                {
                                    intPtr = new nint?((nint)(long)value);
                                    break;
                                }
                            }
                        }
                    }

                    string amplitudeUserId = GetAmplitudeUserId();

                    if (!flag3)
                    {
                        object? obj2 = FetchAmplitudeExperimentValue(httpClient, amplitudeUserId, "ct-processor-affinity", 2000);
                        if (obj2 != null)
                        {
                            try
                            {
                                flag = (string)((Dictionary<string, object>)obj2)["key"] != "control";
                                flag2 = (string)((Dictionary<string, object>)obj2)["key"] == "second-ccx-affinity";
                            }
                            catch (Exception e)
                            {
                                Logging.ModError(null, $"[VRChat Bootstrap] Main rain into a critical error! {e.ToString()}");
                            }
                        }
                        RegistryKey? registryKey = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0\\");
                        if (registryKey != null)
                        {
                            Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] Fetching processor name...");
                            object? obj3 = registryKey != null ? registryKey.GetValue("ProcessorNameString") : null;

                            if (obj3 != null && flag)
                            {
                                Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] Processor name found: [{obj3.ToString()}]!");

                                if (obj3.ToString().Contains("X3D"))
                                {
                                    flag2 = false;
                                }

                                if (obj3.ToString().Contains("AMD Ryzen 5 1600 ") || obj3.ToString().Contains("AMD Ryzen 5 1600X ") || obj3.ToString().Contains("AMD Ryzen Threadripper 1920X ") || obj3.ToString().Contains("AMD Ryzen 5 1600 (AF) ") || obj3.ToString().Contains("AMD Ryzen 5 2600E ") || obj3.ToString().Contains("AMD Ryzen 5 2600 ") || obj3.ToString().Contains("AMD Ryzen 5 2600X ") || obj3.ToString().Contains("AMD Ryzen Threadripper 2920X ") || obj3.ToString().Contains("AMD Ryzen Threadripper 2970WX ") || obj3.ToString().Contains("AMD Ryzen 5 3500 ") || obj3.ToString().Contains("AMD Ryzen 5 3500X ") || obj3.ToString().Contains("AMD Ryzen 5 3600 ") || obj3.ToString().Contains("AMD Ryzen 5 3600X ") || obj3.ToString().Contains("AMD Ryzen 5 3600XT ") || obj3.ToString().Contains("AMD Ryzen 9 3900 ") || obj3.ToString().Contains("AMD Ryzen 9 3900X ") || obj3.ToString().Contains("AMD Ryzen 9 3900XT ") || obj3.ToString().Contains("AMD Ryzen Threadripper 3960X ") || obj3.ToString().Contains("AMD Ryzen Threadripper PRO 3945WX ") || obj3.ToString().Contains("AMD Ryzen 5 4600GE ") || obj3.ToString().Contains("AMD Ryzen 5 4600G ") || obj3.ToString().Contains("AMD Ryzen 5 4500U ") || obj3.ToString().Contains("AMD Ryzen 5 4600U ") || obj3.ToString().Contains("AMD Ryzen 5 4680U ") || obj3.ToString().Contains("AMD Ryzen 5 4600HS ") || obj3.ToString().Contains("AMD Ryzen 5 4600H ") || obj3.ToString().Contains("AMD Ryzen 5 5500U "))
                                {
                                    intPtr = new nint?(new nint(63));
                                    if (flag2)
                                    {
                                        intPtr = new nint?(new nint(intPtr.Value.ToInt64() << 6));
                                    }
                                }
                                else if (obj3.ToString().Contains("AMD Ryzen 7 1700 ") || obj3.ToString().Contains("AMD Ryzen 7 1700X ") || obj3.ToString().Contains("AMD Ryzen 7 1800X ") || obj3.ToString().Contains("AMD Ryzen Threadripper 1900X ") || obj3.ToString().Contains("AMD Ryzen Threadripper 1950X ") || obj3.ToString().Contains("AMD Ryzen 7 2700E ") || obj3.ToString().Contains("AMD Ryzen 7 2700 ") || obj3.ToString().Contains("AMD Ryzen 7 2700X ") || obj3.ToString().Contains("AMD Ryzen Threadripper 2950X ") || obj3.ToString().Contains("AMD Ryzen Threadripper 2990WX ") || obj3.ToString().Contains("AMD Ryzen 7 3700X ") || obj3.ToString().Contains("AMD Ryzen 7 3800X ") || obj3.ToString().Contains("AMD Ryzen 7 3800XT ") || obj3.ToString().Contains("AMD Ryzen 9 3950X ") || obj3.ToString().Contains("AMD Ryzen Threadripper 3970X ") || obj3.ToString().Contains("AMD Ryzen Threadripper 3990X ") || obj3.ToString().Contains("AMD Ryzen Threadripper PRO 3955WX ") || obj3.ToString().Contains("AMD Ryzen Threadripper PRO 3975WX ") || obj3.ToString().Contains("AMD Ryzen Threadripper PRO 3995WX ") || obj3.ToString().Contains("AMD Ryzen 7 4700GE ") || obj3.ToString().Contains("AMD Ryzen 7 4700G ") || obj3.ToString().Contains("AMD Ryzen 7 4700U ") || obj3.ToString().Contains("AMD Ryzen 7 4800U ") || obj3.ToString().Contains("AMD Ryzen 7 4800H ") || obj3.ToString().Contains("AMD Ryzen 7 4800HS ") || obj3.ToString().Contains("AMD Ryzen 7 4980U ") || obj3.ToString().Contains("AMD Ryzen 9 4900HS ") || obj3.ToString().Contains("AMD Ryzen 9 4900H ") || obj3.ToString().Contains("AMD Ryzen 7 5700U ") || obj3.ToString().Contains("AMD Ryzen 3 7330U ") || obj3.ToString().Contains("AMD Ryzen 3 7335U "))
                                {
                                    intPtr = new nint?(new nint(255));
                                    if (flag2)
                                    {
                                        intPtr = new nint?(new nint(intPtr.Value.ToInt64() << 8));
                                    }
                                }
                                else if (obj3.ToString().Contains("AMD Ryzen 9 5900 ") || obj3.ToString().Contains("AMD Ryzen 9 5900X ") || obj3.ToString().Contains("AMD Ryzen Threadripper PRO 5945WX ") || obj3.ToString().Contains("AMD Ryzen Threadripper PRO 5965WX ") || obj3.ToString().Contains("AMD Ryzen 9 7900 ") || obj3.ToString().Contains("AMD Ryzen 9 7900X ") || obj3.ToString().Contains("AMD Ryzen 9 7900X3D ") || obj3.ToString().Contains("AMD Ryzen 9 7845HX "))
                                {
                                    intPtr = new nint?(new nint(4095));
                                    if (flag2)
                                    {
                                        intPtr = new nint?(new nint(intPtr.Value.ToInt64() << 12));
                                    }
                                }
                                else if (obj3.ToString().Contains("AMD Ryzen 9 5950X ") || obj3.ToString().Contains("AMD Ryzen Threadripper PRO 5995WX ") || obj3.ToString().Contains("AMD Ryzen 9 7950X ") || obj3.ToString().Contains("AMD Ryzen 9 7950X3D ") || obj3.ToString().Contains("AMD Ryzen 9 7950HX ") || obj3.ToString().Contains("AMD Ryzen Threadripper PRO 5955WX ") || obj3.ToString().Contains("AMD Ryzen Threadripper PRO 5975WX "))
                                {
                                    intPtr = new nint?(new nint(65535));
                                    if (flag2)
                                    {
                                        intPtr = new nint?(new nint(intPtr.Value.ToInt64() << 16));
                                    }
                                }
                            }
                        }
                    }
                    if (flag3)
                    {
                        Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] Adding --user-affinity argument to launch...");
                        argsList.Add("--user-affinity");
                    }
                    else if (intPtr != null)
                    {
                        Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] Adding --affinity={intPtr.Value.ToString("X")}...");
                        argsList.Add("--affinity=" + intPtr.Value.ToString("X"));
                    }

                    PriorityArg? priorityArg = FindPriorityArgument("--process-priority=", ref argsList);
                    ProcessPriorityClass? processPriorityClass = null;
                    if (priorityArg != null)
                    {
                        Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] Setting process priority to {priorityArg.ToString()}...");
                        switch (priorityArg.Value)
                        {
                            case PriorityArg.Idle:
                                processPriorityClass = new ProcessPriorityClass?(ProcessPriorityClass.Idle);
                                break;
                            case PriorityArg.BelowNormal:
                                processPriorityClass = new ProcessPriorityClass?(ProcessPriorityClass.BelowNormal);
                                break;
                            case PriorityArg.Normal:
                                processPriorityClass = new ProcessPriorityClass?(ProcessPriorityClass.Normal);
                                break;
                            case PriorityArg.AboveNormal:
                                processPriorityClass = new ProcessPriorityClass?(ProcessPriorityClass.AboveNormal);
                                break;
                            case PriorityArg.High:
                                processPriorityClass = new ProcessPriorityClass?(ProcessPriorityClass.High);
                                break;
                        }
                    }

                    Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] Disposing HTTP Client..");
                    httpClient.Dispose();
                    long timestamp = Stopwatch.GetTimestamp();
                    Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] Adding --startup-begin-ts={timestamp.ToString()} to arguments...");
                    argsList.Add("--startup-begin-ts=" + timestamp.ToString());
                    Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] Adding --oscloader-enabled to arguments...");
                    argsList.Add("--oscloader-enabled");
                    string arguments = string.Join(" ", argsList);
                    Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] Bootstrap complete!");
                    Logging.ModLog(null, $"[<color=darkcyan>VRChat Bootstrap</color>] Launching VRChat...");
                    Core.EasyAntiCheat_Startup = new Process();
                    Core.EasyAntiCheat_Startup.StartInfo.FileName = Path.Combine(baseDirectory, "start_protected_game.exe");
                    Core.EasyAntiCheat_Startup.StartInfo.WorkingDirectory = baseDirectory;
                    Core.EasyAntiCheat_Startup.StartInfo.Arguments = arguments;
                    Core.EasyAntiCheat_Startup.Start();
                    try
                    {
                        if (intPtr != null)
                        {
                            Core.EasyAntiCheat_Startup.ProcessorAffinity = intPtr.Value;
                        }
                        if (processPriorityClass != null)
                        {
                            Core.EasyAntiCheat_Startup.PriorityClass = processPriorityClass.Value;
                        }
                    }
                    catch (Win32Exception e)
                    {
                        Logging.ModError(null, $"[VRChat Bootstrap] Main rain into a critical error! {e.ToString()}");
                    }
                    return;
                }
            }
            throw new Exception("ProductID parse error");
        }
    }
}
