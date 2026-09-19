using Photon.Pun;
using Photon.Realtime;

namespace VRC.Callbacks
{
    /// <summary>
    /// A script that allows for additional callbacks when in Prison Escape.
    /// </summary>
    public abstract class PrisonEscapeCallbacks
    {
        /// <summary>
        /// Prison Escape teams.
        /// </summary>
        public enum Teams
        {
            /// <summary>
            /// Guard Team
            /// </summary>
            Guard,
            /// <summary>
            /// Prisoner Team
            /// </summary>
            Prisoner,
            /// <summary>
            /// Unknown Team
            /// </summary>
            Unknown
        }

        /// <summary>
        /// Runs whenever the game starts.
        /// </summary>
        /// <param name="assignedTeam">The team assigned to the client.</param>
        public virtual void OnGameStarted(Teams assignedTeam) { }
        /// <summary>
        /// Runs whenever the client recieves damage.
        /// </summary>
        /// <param name="damage">The damage recieved.</param>
        public virtual void OnDamageRecieved(int damage) { }
        /// <summary>
        /// Runs whenever the client kills another player.
        /// </summary>
        /// <param name="photonPlayer">The player the client has killed.</param>
        public virtual void OnPlayerKilled(Player photonPlayer) { }
        /// <summary>
        /// Runs whenever the game has ended.
        /// </summary>
        /// <param name="winningTeam">The winning team.</param>
        public virtual void OnGameEnd(Teams winningTeam) { }

        internal static List<PrisonEscapeCallbacks> All = new List<PrisonEscapeCallbacks>();
        protected PrisonEscapeCallbacks()
        {
            All.Add(this);
        }
        ~PrisonEscapeCallbacks()
        {
            All.Remove(this);
        }

        internal static void CheckLog(string log)
        {
            if (log.StartsWith("[SPAWN (PRISONER)]"))
            {
                foreach (PrisonEscapeCallbacks callback in All)
                {
                    callback.OnGameStarted(Teams.Prisoner);
                }
            }
            if (log.StartsWith("[SPAWN (GUARD)]"))
            {
                foreach (PrisonEscapeCallbacks callback in All)
                {
                    callback.OnGameStarted(Teams.Guard);
                }
            }
            if (log.StartsWith("[DAMAGED "))
            {
                int damage = int.Parse(log.Replace("[DAMAGED ", "").Replace(")]", ""));
                foreach (PrisonEscapeCallbacks callback in All)
                {
                    callback.OnDamageRecieved(damage);
                }
            }
            if (log.StartsWith("[KILLED ("))
            {
                Player photonPlr = PhotonNetwork.GetPlayerByNickName(log.Replace("[KILLED (", "").Replace(")]", ""));
                foreach (PrisonEscapeCallbacks callback in All)
                {
                    callback.OnPlayerKilled(photonPlr);
                }
            }
            if (log.StartsWith("[PE] Guards win!"))
            {
                foreach (PrisonEscapeCallbacks callback in All)
                {
                    callback.OnGameEnd(Teams.Guard);
                }
            }
            if (log.StartsWith("[PE] Prisoners win!"))
            {
                foreach (PrisonEscapeCallbacks callback in All)
                {
                    callback.OnGameEnd(Teams.Prisoner);
                }
            }
        }
    }
}