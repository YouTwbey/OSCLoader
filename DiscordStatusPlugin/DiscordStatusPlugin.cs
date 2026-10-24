using Discord;
using OSCLoader.Debug;
using Photon.Pun;
using VRC.Managers;

namespace DiscordStatus
{
    public class DiscordStatusPlugin
    {
        public const long AppId = 1542090939336499251;
        public Discord.Discord discordClient;
        public ActivityManager activityManager;
        public Activity desiredActivity;


        private bool gameClosing;
        public bool GameStarted { get; private set; }

        public void OnStartup()
        {
            desiredActivity = new Activity();
            desiredActivity.Name = "VRChat [OSCLoader]";
            DiscordLibraryLoader.LoadLibrary();
            InitializeDiscord();
            new Thread(DiscordLoopThread).Start();
            GameStarted = true;
        }

        public void OnApplicationQuit()
        {
            gameClosing = true;
        }

        public void DiscordLoopThread()
        {
            var lastUpdate = DateTime.UtcNow;

            while (!gameClosing)
            {
                try
                {
                    if (discordClient != null)
                    {
                        discordClient.RunCallbacks();
                    }
                }
                catch (ResultException ex)
                {
                    Logging.DebugError($"Discord error: {ex.Message}");
                    break;
                }

                Thread.Sleep(200);
            }
        }

        public bool InitializeDiscord()
        {
            try
            {
                discordClient = new Discord.Discord(AppId, (ulong)CreateFlags.NoRequireDiscord);
                discordClient.SetLogHook(LogLevel.Debug, DiscordLogHandler);

                activityManager = discordClient.GetActivityManager();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void DiscordLogHandler(LogLevel level, string message)
        {
            switch (level)
            {
                case LogLevel.Info:
                case LogLevel.Debug:
                    Logging.Log(message);
                    break;

                case LogLevel.Warn:
                    Logging.Warn(message);
                    break;

                case LogLevel.Error:
                    Logging.Error(message);
                    break;
            }
        }

        long previousTime;
        public void UpdateActivity(string state, bool updateTime = false)
        {
            if (discordClient == null) return;

            desiredActivity.State = state;

            if (updateTime)
            {
                previousTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            }

            desiredActivity.Timestamps.Start = previousTime;
            activityManager.UpdateActivity(desiredActivity, ResultHandler);
        }

        public void UdonUpdateActivity(string state, bool updateTime = false)
        {
            if (PhotonNetwork.CurrentRoom != null)
            {
                if (!PhotonNetwork.CurrentRoom.IsVisible)
                {
                    UpdateActivity(state, updateTime);
                }
            }
        }

        internal void UpdateInstanceActivity()
        {
            if (discordClient == null) return;

            desiredActivity = new Activity
            {
                Details = "Playing " + WorldManager.currentWorld.Name,
                Party =
                {
                    Id = "InstanceSize",
                    Size =
                    {
                        CurrentSize = PhotonNetwork.CurrentRoom.PlayerCount,
                        MaxSize = PhotonNetwork.CurrentRoom.MaxPlayers
                    }
                },
                Assets =
                {
                    LargeText = WorldManager.currentWorld.Name,
                    LargeImage = WorldManager.currentWorld.ImageUrl,
                    SmallImage = "oscloader_icon",
                    SmallText = "Powered using OSCLoader"
                }
            };

            if (PhotonNetwork.CurrentRoom != null)
            {
                if (!PhotonNetwork.CurrentRoom.IsVisible)
                {
                    desiredActivity = new Activity
                    {
                        Details = "Playing in a Private World",
                        Party =
                        {
                            Id = "InstanceSize",
                            Size =
                            {
                                CurrentSize = 1,
                                MaxSize = 1
                            }
                        },
                        Assets =
                        {
                            LargeText = "Private World",
                            LargeImage = "private_world",
                            SmallImage = "oscloader_icon",
                            SmallText = "Powered using OSCLoader"
                        }
                    };
                }
            }

            previousTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

            desiredActivity.Timestamps.Start = previousTime;
            activityManager.UpdateActivity(desiredActivity, ResultHandler);
        }

        public void ResultHandler(Result result)
        {

        }
    }
}
