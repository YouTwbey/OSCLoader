using DiscordStatus;

namespace OSCLoader.Core.Handlers
{
    /// <summary>
    /// Allows you to modify values in relation to Discord's RPC system.
    /// </summary>
    public static class DiscordRPC
    {
        internal static DiscordStatusPlugin discordStatus;

        internal static void Start()
        {
            discordStatus = new DiscordStatusPlugin();
            discordStatus.OnStartup();
            UpdateActivity("Starting game...", true);
        }

        /// <summary>
        /// Allows you to update the state value on a Discord Activity.
        /// </summary>
        /// <param name="state">The new state to be displayed.</param>
        /// <param name="updateTime">Whether to reset the game time or not.</param>
        public static void UpdateActivity(string state, bool updateTime = false)
        {
            discordStatus.UpdateActivity(state, updateTime);
        }
    }
}
