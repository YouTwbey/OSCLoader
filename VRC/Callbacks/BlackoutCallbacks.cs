namespace VRC.Callbacks
{
    /// <summary>
    /// A script that allows for additional callbacks when in Blackout
    /// </summary>
    public abstract class BlackoutCallbacks
    {
        /// <summary>
        /// Blackout Roles
        /// </summary>
        public enum Roles
        {
            /// <summary>
            /// Murderer Role
            /// </summary>
            Murderer,
            /// <summary>
            /// Detective Role
            /// </summary>
            Detective,
            /// <summary>
            /// Bystander Role
            /// </summary>
            Bystander
        }

        /// <summary>
        /// Runs whenever the game starts.
        /// </summary>
        /// <param name="assignedRole">The role assigned to the client.</param>
        public virtual void OnGameStarted(Roles assignedRole) { }
        /// <summary>
        /// Runs whenever the client recieves damage.
        /// </summary>
        /// <param name="damage">The damage recieved.</param>
        public virtual void OnDamageRecieved(int damage) { }
        /// <summary>
        /// Runs whenever the client dies.
        /// </summary>
        public virtual void OnDeath() { }
        /// <summary>
        /// Runs whenever the game ends.
        /// </summary>
        /// <param name="winningRole">The role that won the game.</param>
        public virtual void OnGameEnded(Roles winningRole) { }

        internal static List<BlackoutCallbacks> All = new List<BlackoutCallbacks>();
        protected BlackoutCallbacks()
        {
            All.Add(this);
        }
        ~BlackoutCallbacks()
        {
            All.Remove(this);
        }

        internal static void CheckLog(string log)
        {
            if (log.StartsWith("[SPAWN (INNOCENT)]"))
            {
                foreach (BlackoutCallbacks callback in All)
                {
                    callback.OnGameStarted(Roles.Bystander);
                }
            }
            if (log.StartsWith("[SPAWN (MURDERER)]"))
            {
                foreach (BlackoutCallbacks callback in All)
                {
                    callback.OnGameStarted(Roles.Murderer);
                }
            }
            if (log.StartsWith("[SPAWN (DETECTIVE)]"))
            {
                foreach (BlackoutCallbacks callback in All)
                {
                    callback.OnGameStarted(Roles.Detective);
                }
            }
            if (log.StartsWith("[DAMAGED "))
            {
                int damage = int.Parse(log.Replace("[DAMAGED ", "").Replace(")]", ""));
                foreach (BlackoutCallbacks callback in All)
                {
                    callback.OnDamageRecieved(damage);
                }
            }
            if (log.StartsWith("[DEATH]"))
            {
                foreach (BlackoutCallbacks callback in All)
                {
                    callback.OnDeath();
                }
            }
        }
    }
}
