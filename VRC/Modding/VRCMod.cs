using OSCLoader.Debug;
using Photon.Realtime;
using VRC.Managers;

namespace VRC.Modding
{
    /// <summary>
    /// The root mod attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class VRCMod : Attribute
    {
        /// <summary>
        /// Name of the VRCMod
        /// </summary>
        public string ModName { get; internal set; } = "";
        /// <summary>
        /// Version of the VRCMod
        /// </summary>
        public string ModVersion { get; internal set; } = "";
        /// <summary>
        /// Author of the VRCMod
        /// </summary>
        public string ModAuthor { get; internal set; } = "";
        /// <summary>
        /// File path of the VRCMod
        /// </summary>
        public string FilePath { get; internal set; } = "";
        /// <summary>
        /// Mod's personal storage path
        /// </summary>
        public string ModStoragePath { get; internal set; } = "";

        /// <summary>
        /// Runs every 60fps
        /// </summary>
        public virtual void Update() { }
        /// <summary>
        /// Runs before Update
        /// </summary>
        public virtual void EarlyUpdate() { }
        /// <summary>
        /// Runs after Update
        /// </summary>
        public virtual void LateUpdate() { }
        /// <summary>
        /// Runs when the mod has started
        /// </summary>
        public virtual void OnModStart() { }
        /// <summary>
        /// Runs before OnModStart
        /// </summary>
        public virtual void OnModEarlyStart() { }
        /// <summary>
        /// Runs after OnModStart
        /// </summary>
        public virtual void OnModLateStart() { }
        /// <summary>
        /// Runs when UdonManager.OnSceneLoaded has been called
        /// </summary>
        public virtual void OnSceneLoaded() { }
        /// <summary>
        /// Runs when a player joins the Photon room
        /// </summary>
        /// <param name="photonPlayer">The player that joined</param>
        public virtual void OnPlayerJoined(Player photonPlayer) { }
        /// <summary>
        /// Runs when a player leaves the Photon room
        /// </summary>
        /// <param name="photonPlayer">The player that left</param>
        public virtual void OnPlayerLeft(Player photonPlayer) { }
        /// <summary>
        /// Runs when the client picks up an interactable
        /// </summary>
        /// <param name="objectName">The GameObject's name</param>
        /// <param name="equipped">Interactable's equipped value</param>
        /// <param name="autoEquipType">Interactable's auto equip type value</param>
        /// <param name="inputMethod">Client's input method</param>
        /// <param name="autoHold">Interactable's auto hold value</param>
        public virtual void OnGrabInteractable(string objectName, bool equipped, bool autoEquipType, string inputMethod, bool autoHold) { }
        /// <summary>
        /// Runs when the client drops an interactable
        /// </summary>
        /// <param name="objectName">The GameObject's name</param>
        /// <param name="wasEquipped">Interactable's wasEquipped value</param>
        /// <param name="reasonForDrop">The reason why the interactable was dropped</param>
        /// <param name="inputMethod">Client's input method</param>
        public virtual void OnDropInteractable(string objectName, bool wasEquipped, string reasonForDrop, string inputMethod) { }
        /// <summary>
        /// Runs when a player spawns a prop
        /// </summary>
        /// <param name="photonPlayer">The player</param>
        /// <param name="propId">The PropID</param>
        public virtual void OnSpawnedProp(Player photonPlayer, string propId) { }
        /// <summary>
        /// Runs when a player spawns a sticker
        /// </summary>
        /// <param name="photonPlayer">The player</param>
        /// <param name="stickerId">The StickerID</param>
        public virtual void OnSpawnedSticker(Player photonPlayer, string stickerId) { }
        /// <summary>
        /// Runs when VRChat logs a message using UnityEngine.Debug
        /// </summary>
        /// <param name="log">The contents of the log</param>
        public virtual void OnVRChatLog(string log) { }
        /// <summary>
        /// Runs when the client opens a menu in VRChat
        /// </summary>
        /// <param name="pageType">The type of menu.</param>
        public virtual void OnPageShown(LocalPlayerBehaviour.PageType pageType) { }
        /// <summary>
        /// Runs when the client closes/changes from a menu in VRChat
        /// </summary>
        /// <param name="pageType">The type of menu.</param>
        public virtual void OnPageHidden(LocalPlayerBehaviour.PageType pageType) { }
        /// <summary>
        /// Runs when VRChat's API attempts to load a string from a URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        public virtual void OnStringDownload(string url) { }
        /// <summary>
        /// Runs when VRChat's AFK status has changed
        /// </summary>
        /// <param name="isReturning">Set to true if returning to the game</param>
        public virtual void OnAFKStatus(bool isReturning) { }
        /// <summary>
        /// Runs when a player's avatar loads
        /// </summary>
        /// <param name="photonPlayer">The player</param>
        /// <param name="avatarName">The avatar</param>
        public virtual void OnAvatarLoad(Player photonPlayer, string avatarName) { }
        /// <summary>
        /// Runs when a game event is sent from the world (REQUIRES WORLD TO HAVE OSCLOADER SUPPORT)
        /// </summary>
        /// <param name="eventName">The event's name</param>
        /// <param name="data">Any relevent event data</param>
        public virtual void OnGameEvent(string eventName, string data) { }
        /// <summary>
        /// Runs when game data is sent from the world (REQUIRES WORLD TO HAVE OSCLOADER SUPPORT)
        /// </summary>
        /// <param name="data">The data sent</param>
        public virtual void OnGameData(string data) { }
        /// <summary>
        /// Runs whenever an OSC message has been recieved
        /// </summary>
        /// <param name="address">The address of the message</param>
        /// <param name="value">The value of the message</param>
        public virtual void OnOSCMessage(string address, string value) { }
        /// <summary>
        /// Runs when the client joins an instance
        /// </summary>
        /// <param name="worldId">The instance's worldId</param>
        /// <param name="instanceName">The instance's name</param>
        /// <param name="instanceType">The instance's type</param>
        /// <param name="instanceCreator">The instance's creator</param>
        /// <param name="region">The instance's region</param>
        [Obsolete("This method is outdated, please use OnInstanceChanged instead.")]
        public virtual void OnJoinedInstance(string worldId, string instanceName, string instanceType, string instanceCreator, string region) { }
        /// <summary>
        /// Runs when the client changes worlds
        /// </summary>
        /// <param name="oldWorld">The old world info</param>
        /// <param name="newWorld">The new world info</param>
        public virtual void OnWorldChanged(WorldManager.WorldInfo oldWorld, WorldManager.WorldInfo newWorld) { }
        /// <summary>
        /// Runs when the client changes instances
        /// </summary>
        /// <param name="oldInstance">The old instance info</param>
        /// <param name="newInstance">The new instance info</param>
        public virtual void OnInstanceChanged(WorldManager.InstanceInfo oldInstance, WorldManager.InstanceInfo newInstance) { }
        /// <summary>
        /// Runs when Photon's custom authentication process completes
        /// </summary>
        public virtual void OnCustomAuthenticationResponse() { }
        /// <summary>
        /// Runs when the client connects to the Photon Master Server
        /// </summary>
        public virtual void OnConnectedToMaster() { }
        /// <summary>
        /// Runs when the client disconnects from Photon servers
        /// </summary>
        public virtual void OnDisconnected(string disconnectionReason) { }
        /// <summary>
        /// Runs when the region list updates in Photon
        /// </summary>
        public virtual void OnRegionListReceived() { }
        /// <summary>
        /// Runs when the client leaves the room
        /// </summary>
        public virtual void OnLeftRoom() { }
        /// <summary>
        /// Runs before the ImGui hook is added, used to create UI elements
        /// </summary>
        public virtual void OnGUI() { }
        /// <summary>
        /// Runs whenever the client recieves a notification in VRChat
        /// </summary>
        /// <param name="id">The notification ID</param>
        /// <param name="type">The type of notification</param>
        /// <param name="creationDate">The time the notification was created</param>
        /// <param name="message">The notification's message</param>
        public virtual void OnNotificationRecieved(int id, string type, DateTime creationDate, string message) { }
        /// <summary>
        /// Runs whenever the client clears a notification in VRChat
        /// </summary>
        /// <param name="id">The notification ID</param>
        /// <param name="category">Where the notification was located</param>
        /// <param name="type">The type of notification</param>
        /// <param name="creationDate">The time the notification was created</param>
        /// <param name="message">The notification's message</param>
        public virtual void OnNotificationRemoved(int id, string category, string type, DateTime creationDate, string message) { }
        /// <summary>
        /// Runs whenever OSCLoader or VRChat exits
        /// </summary>
        /// <param name="fromVRChat">A value that states if the close request came from VRChat or OSCLoader itself</param>
        public virtual void OnApplicationQuit(bool fromVRChat) { }

        /// <summary>
        /// Send a message to the console
        /// </summary>
        /// <param name="message">Message content</param>
        public void Log(string message)
        {
            Logging.ModLog(this, message);
        }
        /// <summary>
        /// Send a warning message to the console
        /// </summary>
        /// <param name="message">Message content</param>
        public void Warning(string message)
        {
            Logging.ModWarn(this, message);
        }
        /// <summary>
        /// Send a error message to the console
        /// </summary>
        /// <param name="message">Message content</param>
        public void Error(string message)
        {
            Logging.ModError(this, message);
        }
    }
}
