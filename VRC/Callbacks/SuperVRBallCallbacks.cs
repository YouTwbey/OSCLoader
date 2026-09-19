using Photon.Pun;
using Photon.Realtime;
using System.Text.RegularExpressions;

namespace VRC.Callbacks
{
    /// <summary>
    /// A script that allows for additional callbacks when in Super VR Ball.
    /// </summary>
    public abstract class SuperVRBallCallbacks
    {
        /// <summary>
        /// Runs whenever a level has been loaded.
        /// </summary>
        /// <param name="levelId">The ID of the level.</param>
        public virtual void OnLevelLoaded(int levelId) { }
        /// <summary>
        /// Runs whenever a player falls and respawns.
        /// </summary>
        /// <param name="photonPlayer">The player who fell.</param>
        public virtual void OnPlayerFell(Player photonPlayer) { }
        /// <summary>
        /// Runs whenever a player's stats is updated.
        /// </summary>
        /// <param name="photonPlayer">The player who stats are updating.</param>
        /// <param name="totalTime">Total race time.</param>
        /// <param name="levelsBeaten">The amount of levels the client has beaten in the session.</param>
        public virtual void OnPlayerStatsUpdated(Player photonPlayer, float totalTime, int levelsBeaten) { }
        /// <summary>
        /// Runs whenever the client reaches the goal.
        /// </summary>
        public virtual void OnClientBeatLevel() { }

        internal static List<SuperVRBallCallbacks> All = new List<SuperVRBallCallbacks>();
        protected SuperVRBallCallbacks()
        {
            All.Add(this);
        }
        ~SuperVRBallCallbacks()
        {
            All.Remove(this);
        }

        internal static void CheckLog(string log)
        {
            if (log.StartsWith("Super VR Ball: [DeathMarker] Player "))
            {
                string name = log.Replace("Super VR Ball: [DeathMarker] Player ", "").Split(' ')[0];
                Player? plr = null;

                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    if (player.NickName == name)
                    {
                        plr = player;
                        break;
                    }
                }

                if (plr == null) return;
                foreach (SuperVRBallCallbacks callback in All)
                {
                    callback.OnPlayerFell(plr);
                }
            }
            if (log.StartsWith("Super VR Ball: Index "))
            {
                Regex regex = new Regex(@"Index \d+: (?<Username>.+?)\. Time (?<Time>\d+(?:\.\d+)?), levels: (?<Levels>\d+)");

                Match match = regex.Match(log);

                if (match.Success)
                {
                    string username = match.Groups["Username"].Value;
                    float time = float.Parse(match.Groups["Time"].Value);
                    int levels = int.Parse(match.Groups["Levels"].Value);

                    Player? plr2 = null;

                    foreach (Player player in PhotonNetwork.PlayerList)
                    {
                        if (player.NickName == username)
                        {
                            plr2 = player;
                            break;
                        }
                    }

                    if (plr2 == null) return;
                    foreach (SuperVRBallCallbacks callback in All)
                    {
                        callback.OnPlayerStatsUpdated(plr2, time, levels);
                    }
                }
            }
            if (log.StartsWith("Super VR Ball: Master has set LevelIndex and loaded level "))
            {
                string data = log.Replace("Super VR Ball: Master has set LevelIndex and loaded level ", "").Split('.')[0];
                int val = int.Parse(data);

                foreach (SuperVRBallCallbacks callback in All)
                {
                    callback.OnLevelLoaded(val);
                }
            }
            if (log.Contains("Super VR Ball: Local player has reached the goal! Congrats."))
            {
                foreach (SuperVRBallCallbacks callback in All)
                {
                    callback.OnClientBeatLevel();
                }
            }
        }
    }
}