using OSCLoader.Core;
using OSCLoader.Core.Handlers;
using OSCLoader.Debug;
using Photon.Pun;
using Photon.Realtime;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using VRC.Callbacks;
using VRC.Modding;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static VRC.LocalPlayerBehaviour;
using static VRC.VRCSettings;

namespace VRC.Managers
{
    internal static class VRCModsManager
    {
        public static List<VRCMod> VRChatMods = new List<VRCMod>();
        public static Dictionary<Assembly, VRCMod> AssemblyToVRCMod = new Dictionary<Assembly, VRCMod>();
        public static List<Assembly> Libs = new List<Assembly>();

        public static void LoadAllMods()
        {
            foreach (string file in Directory.GetFiles(Main.GetModsPath(), "*.dll"))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);

                    foreach (Type type in assembly.GetTypes())
                    {
                        if (!typeof(VRCMod).IsAssignableFrom(type) || type.IsAbstract)
                            continue;

                        VRCModInfo? info = assembly.GetCustomAttribute<VRCModInfo>();
                        if (info == null)
                        {
                            Logging.Error($"VRCMod \"{assembly.FullName}\" has failed to load. Please make sure it contains a VRCModInfo attribute in the assembly.");
                            continue;
                        }

                        if (info.supportedDevice != VRCModInfo.PlatformSupport.Both && info.supportedDevice.ToString() != Device.GetPlatform().ToString())
                        {
                            Logging.Error($"VRCMod \"{assembly.FullName}\" has failed to load. It is not supported on this platform!");
                            continue;
                        }

                        VRCMod mod = (VRCMod)Activator.CreateInstance(type)!;

                        mod.FilePath = file.Replace("/", "\\");
                        mod.ModName = info.name;
                        mod.ModVersion = info.version;
                        mod.ModAuthor = info.author;
                        mod.ModStoragePath = Path.Combine(Main.GetGlobalStoragePath(), mod.ModName) + "\\";

                        if (!Directory.Exists(mod.ModStoragePath)) Directory.CreateDirectory(mod.ModStoragePath);

                        VRChatMods.Add(mod);

                        Logging.LogLoadedMod(mod);

                        AssemblyToVRCMod.Add(assembly, mod);

                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logging.Error($"Citical error when trying to load [{file}]: {ex}");
                    continue;
                }
            }

            if (VRChatMods.Count != 0) Logging.Print("<color=blue>----------------------------------------");
            
            foreach (VRCMod mod in VRChatMods)
            {
                mod.OnModEarlyStart();
                mod.OnModStart();
                mod.OnModLateStart();
            }
        }

        public static void LoadAllLibs()
        {
            foreach (string file in Directory.GetFiles(Main.GetLibsPath(), "*.dll"))
            {
                Assembly assembly = Assembly.LoadFrom(file);
                if (assembly != null)
                {
                    Libs.Add(assembly);
                }
                else
                {
                    Logging.Error("Assembly failed to load for: " + file);
                    continue;
                }
            }
        }

        public static void ParseException(Exception e, VRCMod mod = null)
        {
            var lines = e.StackTrace?.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (lines != null && lines.Count > 0)
            {
                lines.RemoveAt(lines.Count - 1);
                if (mod != null)
                {
                    Logging.ModError(mod, $"{e.GetType()}: {e.Message}\n{string.Join(Environment.NewLine, lines)}");
                }
                else
                {
                    Logging.Error($"{e.GetType()}: {e.Message}\n{string.Join(Environment.NewLine, lines)}");
                }
                return;
            }

            if (mod != null)
            {
                Logging.ModError(mod, $"{e.GetType()}: {e.Message}");
            }
            else
            {
                Logging.Error($"{e.GetType()}: {e.Message}");
            }
        }

        public static void OnVRChatLogAll(string message)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnVRChatLog(message);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnPageShownAll(LocalPlayerBehaviour.PageType pageType)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnPageShown(pageType);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnPageHiddenAll(LocalPlayerBehaviour.PageType pageType)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnPageHidden(pageType);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnStringDownloadAll(string url)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnStringDownload(url);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnSceneLoadedAll()
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnSceneLoaded();
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnPlayerJoinedAll(Player player)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnPlayerJoined(player);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnPlayerLeftAll(Player player)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnPlayerLeft(player);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnGrabInteractableAll(string name, bool equip, bool autoEq, string input, bool autoHold)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnGrabInteractable(name, equip, autoEq, input, autoHold);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnDropInteractableAll(string name, bool equip, string reasonForDrop, string input)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnDropInteractable(name, equip, reasonForDrop, input);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnSpawnedStickerAll(Player plr, string id)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnSpawnedSticker(plr, id);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnSpawnedPropAll(Player plr, string id)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnSpawnedProp(plr, id);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnAFKStatusAll(bool ret)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnAFKStatus(ret);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnAvatarLoadAll(Player plr, string avtr)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnAvatarLoad(plr, avtr);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnGameEventAll(string name, string data)
        {
            if (name.StartsWith("INTERNAL_"))
            {
                name = name.Substring(6);

                switch (name)
                {
                    case "StartInteractionConnection":
                        WorldInteraction.StartConnection(data);
                        break;
                }

                return;
            }
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnGameEvent(name, data);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnGameDataAll(string data)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnGameData(data);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnOSCRecievedAll(string address, string value)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnOSCMessage(address, value);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnJoinedInstanceAll(string worldId, string instanceName, string instanceType, string instanceCreator, string region)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnJoinedInstance(worldId, instanceName, instanceType, instanceCreator, region);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnWorldChangedAll(WorldManager.WorldInfo old, WorldManager.WorldInfo _new)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnWorldChanged(old, _new);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnInstanceChangedAll(WorldManager.InstanceInfo old, WorldManager.InstanceInfo _new)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnInstanceChanged(old, _new);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnConnectedToMasterAll()
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnConnectedToMaster();
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnCustomAuthenticationResponseAll()
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnCustomAuthenticationResponse();
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnRegionListReceivedAll()
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                { 
                    mod.OnRegionListReceived();
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnDisconnectedAll(string reason)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnDisconnected(reason);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnLeftRoomAll()
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnLeftRoom();
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void UpdateAll()
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.Update();
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void EarlyUpdateAll()
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.EarlyUpdate();
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void LateUpdateAll()
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.LateUpdate();
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnGUIAll()
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnGUI();
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnNotificationRecievedAll(int id, string type, DateTime creationDate, string message)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnNotificationRecieved(id, type, creationDate, message);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnNotificationRemovedAll(int id, string category, string type, DateTime creationDate, string message)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnNotificationRemoved(id, category, type, creationDate, message);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        public static void OnApplicationQuitAll(bool isFromVRChat)
        {
            foreach (VRCMod mod in VRChatMods)
            {
                try
                {
                    mod.OnApplicationQuit(isFromVRChat);
                }
                catch (Exception e)
                {
                    ParseException(e, mod);
                }
            }
        }

        static (string LogType, string Message) ParseVRChatLog(string str)
        {
            if (string.IsNullOrEmpty(str) || !str.Contains(" ") || !str.Contains(" - "))
            {
                return ("", str);
            }

            int firstSpaceIndex = str.IndexOf(' ');
            int secondSpaceIndex = str.IndexOf(' ', firstSpaceIndex + 1);
            int dashIndex = str.IndexOf(" - ", secondSpaceIndex + 1);

            string logType = str.Substring(secondSpaceIndex + 1, dashIndex - secondSpaceIndex - 1).Trim();
            string message = str.Substring(dashIndex + 3).Trim();

            if (message.Contains("inner exception"))
            {
                logType = "Error";
            }

            return (logType, message);
        }

        static void CreateVRChatLog(string log)
        {
            try
            {
                var (Type, Message) = ParseVRChatLog(log);

                if (Message.StartsWith("OSCLOADER_"))
                {
                    string function = Message.Replace("OSCLOADER_", "");

                    switch (function)
                    {
                        case string s when s.StartsWith("SendGameEvent"):
                            string[] parts = s.Substring("SendGameEvent".Length).TrimStart('(').TrimEnd(')').Split(", ");

                            List<string> result = new List<string>();

                            foreach (string part in parts)
                            {
                                result.Add(part.TrimStart('"').TrimEnd('"'));
                            }

                            if (parts.Length == 2) OnGameEventAll(result[0], result[1]);
                            break;

                        case string s when s.StartsWith("SendGameData"):
                            OnGameDataAll(s.Substring("SendGameData".Length).TrimStart('(', '"').TrimEnd(')', '"'));
                            break;

                        default:
                            Logging.Warn($"Unknown OSCLoader function: {function}");
                            break;
                    }

                    return;
                }

                VRCSettings.TrackLogForSetting(Message);
                OnVRChatLogAll(Message);

                if (!Core.AllowLogging) return;

                if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Message))
                {
                    if (Type.ToLower() == "error")
                    {
                        Logging.ModError(null, $"[VRCHAT] {Message}");
                    }
                    else if (Type.ToLower() == "warning")
                    {
                        Logging.ModWarn(null, $"[VRCHAT] {Message}");
                    }
                    else
                    {
                        Logging.ModLog(null, $"[<color=cyan>VRCHAT</color>] {Message}");
                    }
                }
            }
            catch (Exception e)
            {
                ParseException(e);
            }
        }

        static string newWorldIdCache = "";
        static string newInstanceIdCache = "";
        static string newInstanceVisibilityCache = "";
        static string newInstanceRegionCache = "";

        public static void HandleLog(string log)
        {
            CreateVRChatLog(log);
            var (Type, Message) = ParseVRChatLog(log);

            foreach (string worldIdOrName in WorldManager.worldCallbacks.Keys)
            {
                if (WorldManager.currentWorld.Id == worldIdOrName || WorldManager.currentWorld.Name == worldIdOrName)
                {
                    string listenFor = WorldManager.worldCallbacks[worldIdOrName].Keys.ToList()[0];
                    if (log.Contains(listenFor))
                    {
                        foreach (Action action in WorldManager.worldCallbacks[worldIdOrName][listenFor])
                        {
                            action.Invoke();
                        }
                    }
                }
            }

            try
            {
                if (Message.Contains("UdonManager.OnSceneLoaded"))
                {
                    OnSceneLoadedAll();
                }
                else if (Message.Contains("OnPlayerJoined"))
                {
                    Regex regex = new Regex(@"OnPlayerJoined\s+(.+?)\s+\((usr_[a-zA-Z0-9\-]+)\)");
                    Match match = regex.Match(Message);

                    if (match.Success)
                    {
                        string playerName = match.Groups[1].Value;
                        string userId = match.Groups[2].Value;

                        PhotonNetwork.AddPlayer(playerName, userId);
                        OnPlayerJoinedAll(PhotonNetwork.GetPlayerByUserID(userId));
                    }
                    else
                    {
                        OnPlayerJoinedAll(PhotonNetwork.GetNullPlayer());
                    }
                }
                else if (Message.Contains("OnPlayerLeft"))
                {
                    Regex regex = new Regex(@"OnPlayerLeft\s+(.+?)\s+\((usr_[a-zA-Z0-9\-]+)\)");
                    Match match = regex.Match(Message);

                    if (match.Success)
                    {
                        string playerName = match.Groups[1].Value;
                        string userId = match.Groups[2].Value;

                        OnPlayerLeftAll(PhotonNetwork.GetPlayerByUserID(userId));
                        PhotonNetwork.RemovePlayer(userId);
                    }
                    else
                    {
                        OnPlayerLeftAll(PhotonNetwork.GetNullPlayer());
                    }
                }
                else if (Message.Contains("[Behaviour] Pickup object: "))
                {
                    Match match = Regex.Match(Message, @"Pickup object: '([^']+)' equipped = (\w+), is AutoEquipType Pickup = (\w+), last input method = (\w+), is AutoHold is enabled for this controller type = (\w+)");

                    if (match.Success)
                    {
                        string objectName = match.Groups[1].Value;
                        bool equipped = bool.Parse(match.Groups[2].Value);
                        bool autoEquipType = bool.Parse(match.Groups[3].Value);
                        string inputMethod = match.Groups[4].Value;
                        bool autoHold = bool.Parse(match.Groups[5].Value);

                        OnGrabInteractableAll(objectName, equipped, autoEquipType, inputMethod, autoHold);
                    }
                    else
                    {
                        OnGrabInteractableAll("null", false, false, "null", false);
                    }
                }
                else if (Message.Contains("[Behaviour] Drop object: "))
                {
                    Match match = Regex.Match(Message, @"Drop object: '([^,]+), was equipped = (True|False)'\s*(.*?),\s*last input method = (\w+)");

                    if (match.Success)
                    {
                        string objectName = match.Groups[1].Value;
                        bool wasEquipped = bool.Parse(match.Groups[2].Value);
                        string reason = match.Groups[3].Value;
                        string inputMethod = match.Groups[4].Value;

                        OnDropInteractableAll(objectName, wasEquipped, reason, inputMethod);
                    }
                    else
                    {
                        OnDropInteractableAll("null", false, "null", "null");
                    }
                }
                else if (Message.Contains("[StickersManager] User "))
                {
                    Match match = Regex.Match(Message, @"User\s+(usr_[a-f0-9\-]+)\s+\(([^)]+)\)\s+spawned\s+sticker\s+(inv_[a-f0-9\-]+)");

                    if (match.Success)
                    {
                        string userId = match.Groups[1].Value;
                        string username = match.Groups[2].Value;
                        string stickerId = match.Groups[3].Value;

                        OnSpawnedStickerAll(PhotonNetwork.GetPlayerByUserID(userId), stickerId);
                    }
                    else
                    {
                        OnSpawnedStickerAll(PhotonNetwork.GetNullPlayer(), "null");
                    }
                }
                else if (Message.Contains("[VRCItems] Item "))
                {
                    Match match = Regex.Match(Message, @"Item\s+(prop_[a-f0-9\-]+)\s+spawned\s+by\s+(usr_[a-f0-9\-]+)", RegexOptions.IgnoreCase);

                    if (match.Success)
                    {
                        string propId = match.Groups[1].Value;
                        string userId = match.Groups[2].Value;

                        OnSpawnedPropAll(PhotonNetwork.GetPlayerByUserID(userId), propId);
                    }
                    else
                    {
                        OnSpawnedPropAll(PhotonNetwork.GetNullPlayer(), "null");
                    }
                }
                else if (Message.Contains("[OVRManager] OnApplicationFocus("))
                {
                    bool value = bool.Parse(Message.Replace("[OVRManager] OnApplicationFocus(", "").Replace(")", ""));
                    OnAFKStatusAll(value);
                }
                else if (Message.Contains("[Behaviour] Switching ") && Message.Contains("to avatar "))
                {
                    Match match = Regex.Match(Message, @"\[Behaviour\]\s+Switching\s+(.+?)\s+to\s+avatar\s+(.+)", RegexOptions.IgnoreCase);

                    if (match.Success)
                    {
                        string playerName = match.Groups[1].Value.Trim();
                        string avatarName = match.Groups[2].Value.Trim();

                        if (playerName == PhotonNetwork.NickName)
                        {
                            AvatarManager.currentAvatarName = avatarName;
                        }

                        OnAvatarLoadAll(PhotonNetwork.GetPlayerByNickName(playerName), avatarName);
                    }
                    else
                    {
                        OnAvatarLoadAll(PhotonNetwork.GetNullPlayer(), "null");
                    }
                }
                else if (Message.Contains("[Behaviour] Joining wrld_"))
                {
                    PhotonNetwork.InLobby = false;
                    PhotonNetwork.InRoom = true;

                    Match m = Regex.Match(log, @"(?<world>wrld_[^:]+):(?<name>\d+)~(?<type>\w+)(?:\((?<creator>usr_[^)]+)\))?~region\((?<region>[^)]+)\)", RegexOptions.IgnoreCase);
                    Match groupMatch = Regex.Match(log, @"(?<world>wrld_[^:]+):(?<instance>\d+)~group\((?<group>grp_[^)]+)\)~groupAccessType\((?<type>[^)]+)\)~region\((?<region>[^)]+)\)", RegexOptions.IgnoreCase);
                    if (m.Success)
                    {
                        string worldId = m.Groups["world"].Value;
                        string instanceId = m.Groups["name"].Value;
                        string visibility = m.Groups["type"].Value;
                        string creator = m.Groups["creator"].Value;
                        string region = m.Groups["region"].Value;

                        newWorldIdCache = worldId;
                        newInstanceIdCache = instanceId;
                        newInstanceVisibilityCache = visibility;
                        newInstanceRegionCache = region;
                        WorldManager.UpdateCurrentInfoValues(newWorldIdCache, newInstanceIdCache, newInstanceVisibilityCache, newInstanceRegionCache);

                        OnJoinedInstanceAll(
                            worldId,
                            instanceId,
                            visibility,
                            m.Groups["creator"].Success ? creator : "null",
                            region
                        );
                    }
                    else if (groupMatch.Success)
                    {
                        string worldId = groupMatch.Groups["world"].Value;
                        string instanceId = groupMatch.Groups["instance"].Value;
                        string groupId = groupMatch.Groups["group"].Value;
                        string visibility = groupMatch.Groups["type"].Value;
                        string region = groupMatch.Groups["region"].Value;

                        newWorldIdCache = worldId;
                        newInstanceIdCache = instanceId;
                        newInstanceVisibilityCache = visibility;
                        newInstanceRegionCache = region;
                        WorldManager.UpdateCurrentInfoValues(newWorldIdCache, newInstanceIdCache, newInstanceVisibilityCache, newInstanceRegionCache);

                        OnJoinedInstanceAll(
                            worldId,
                            instanceId,
                            visibility,
                            m.Groups["group"].Success ? groupId : "null",
                            region
                        );
                    }
                    else
                    {
                        WorldManager.UpdateCurrentInfoValues("null", "null", "null", "null");
                        OnJoinedInstanceAll("null", "null", "null", "null", "null");
                    }
                }
                else if (Message.Contains("[Behaviour] OnDisconnected: "))
                {
                    OnDisconnectedAll(Message.Replace("[Behaviour] OnDisconnected: ", ""));
                }
                else if (Message.Contains("[Behaviour] OnRegionListReceived"))
                {
                    OnRegionListReceivedAll();
                }
                else if (Message.Contains("[Behaviour] OnCustomAuthenticationResponse"))
                {
                    OnCustomAuthenticationResponseAll();
                }
                else if (Message.Contains("[Behaviour] OnConnectedToMaster"))
                {
                    PhotonNetwork.InRoom = false;
                    PhotonNetwork.CurrentRoom = null;
                    if (PhotonNetwork.LocalPlayer != null) PhotonNetwork.LocalPlayer.ActorNumber = -1;
                    PhotonNetwork.InLobby = true;

                    OnConnectedToMasterAll();
                }
                else if (Message.Contains("User Authenticated: "))
                {
                    Match match = Regex.Match(Message, @"User Authenticated:\s+(.+?)\s+\((usr_[^)]+)\)", RegexOptions.IgnoreCase);

                    if (match.Success)
                    {
                        Player localPlr = new Player
                        {
                            ActorNumber = -1,
                            IsLocal = true,
                            NickName = match.Groups[1].Value,
                            UserID = match.Groups[2].Value
                        };

                        PhotonNetwork.LocalPlayer = localPlr;
                    }
                    else
                    {
                        Player localPlr = new Player
                        {
                            ActorNumber = -1,
                            IsLocal = true,
                            NickName = "null",
                            UserID = "null"
                        };

                        PhotonNetwork.LocalPlayer = localPlr;
                    }
                }
                else if (Message.Contains($"[Behaviour] Switching {PhotonNetwork.NickName} to avatar "))
                {
                    AvatarManager.currentAvatarName = Message.Replace($"[Behaviour] Switching {PhotonNetwork.NickName} to avatar ", "");
                }
                else if (Message.Contains("VRCApplication: HandleApplicationQuit"))
                {
                    Main.OnProcessExit(true);
                }
                else if (Message.Contains("OnPageShown()"))
                {
                    string menuType = Message.Replace("VP MainMenu", "").Replace(" OnPageShown()", "");
                    bool enumConvert = Enum.TryParse(menuType, out PageType type);

                    if (enumConvert)
                    {
                        OnPageShownAll(type);
                    }
                    else
                    {
                        OnPageShownAll(PageType.Null);
                    }
                }
                else if (Message.Contains("OnPageHidden()"))
                {
                    string menuType = Message.Replace("VP MainMenu", "").Replace(" OnPageHidden()", "");
                    bool enumConvert = Enum.TryParse(menuType, out PageType type);

                    if (enumConvert)
                    {
                        OnPageHiddenAll(type);
                    }
                    else
                    {
                        OnPageHiddenAll(PageType.Null);
                    }
                }
                else if (Message.Contains("[String Download] Attempting to load String from URL "))
                {
                    string url = Message.Replace("[String Download] Attempting to load String from URL ", "").Replace("'", "");

                    OnStringDownloadAll(url);
                }
                else if (Message.Contains("AnswerNotification for notification:"))
                {
                    Match match = Regex.Match(Message, @"sender user id:(?<senderId>\S+).*? id:\s*(?<notificationId>\d+).*? created at:\s*(?<created>.*?UTC).*?" + @"type:(?<type>[^,]+).*? seen:(?<seen>True|False).*? message:\s*""(?<message>.*?)""");

                    if (match.Success)
                    {
                        int id = int.Parse(match.Groups["notificationId"].Value);
                        string type = match.Groups["type"].Value;
                        string message = match.Groups["message"].Value;

                        if (!DateTime.TryParseExact(match.Groups["created"].Value, "MM/dd/yyyy HH:mm:ss 'UTC'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out DateTime creationDate))
                        {
                            OnNotificationRecievedAll(id, type, DateTime.Now, message);
                        }
                        else
                        {
                            OnNotificationRecievedAll(id, type, creationDate, message);
                        }
                    }
                    else
                    {
                        OnNotificationRecievedAll(-1, "null", DateTime.Now, "null");
                    }
                }
                else if (Message.Contains("Remove notification from "))
                {
                    Match match = Regex.Match(Message, @"sender user id:(?<senderId>\S+).*? id:\s*(?<notificationId>\d+).*? created at:\s*(?<created>.*?UTC).*?" + @"type:(?<type>[^,]+).*? seen:(?<seen>True|False).*? message:\s*""(?<message>.*?)""");

                    if (match.Success)
                    {
                        int id = int.Parse(match.Groups["notificationId"].Value);
                        string type = match.Groups["type"].Value;
                        string message = match.Groups["message"].Value;
                        string category = Message.Replace("Remove notification from ", "").Split(' ')[0];

                        if (!DateTime.TryParseExact(match.Groups["created"].Value, "MM/dd/yyyy HH:mm:ss 'UTC'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out DateTime creationDate))
                        {
                            OnNotificationRemovedAll(id, category, type, DateTime.Now, message);
                        }
                        else
                        {
                            OnNotificationRemovedAll(id, category, type, creationDate, message);
                        }
                    }
                    else
                    {
                        OnNotificationRemovedAll(-1, "null", "null", DateTime.Now, "null");
                    }
                }

                switch (WorldManager.currentWorld.Id)
                {
                    case "wrld_ff8b4a6e-4268-4783-bc16-3103067a4be6":
                        SuperVRBallCallbacks.CheckLog(Message);
                        break;
                    case "wrld_14750dd6-26a1-4edb-ae67-cac5bcd9ed6a":
                        PrisonEscapeCallbacks.CheckLog(Message);
                        break;
                    case "wrld_858dfdfc-1b48-4e1e-8a43-f0edc611e5fe":
                        Murder4Callbacks.CheckLog(Message);
                        break;
                    case "wrld_e9093447-5e69-4eec-8262-358a4ad72db9":
                        BlackoutCallbacks.CheckLog(Message);
                        break;
                }
            }
            catch (Exception e)
            {
                ParseException(e);
            }
        }
    }
}
