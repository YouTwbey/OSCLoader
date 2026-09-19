using OSCLoader.Debug;

namespace VRC.Modding
{
    /// <summary>
    /// World Interaction for worlds that support Mod to VRChat communication.
    /// </summary>
    public static class WorldInteraction
    {
        internal static void StartConnection(string url)
        {

        }

        internal static void Internal_SendData(string function, string data = "")
        {
            if (!Supported)
            {
                Logging.Error("World does not support Interaction.");
                return;
            }
        }

        /// <summary>
        /// States whether the world supports Interaction.
        /// </summary>
        public static bool Supported { get; internal set; }

        /// <summary>
        /// Allows you to change the walk speed of the local client.
        /// </summary>
        /// <param name="speed">The new speed.</param>
        public static void SetWalkSpeed(float speed = 2) => Internal_SendData("INTERNAL_WalkSpeed", speed.ToString());
        /// <summary>
        /// Allows you to change the run speed of the local client.
        /// </summary>
        /// <param name="speed">The new speed.</param>
        public static void SetRunSpeed(float speed = 4) => Internal_SendData("INTERNAL_RunSpeed", speed.ToString());
        /// <summary>
        /// Allows you to change the strafe speed of the local client.
        /// </summary>
        /// <param name="speed">The new speed.</param>
        public static void SetStrafeSpeed(float speed = 3) => Internal_SendData("INTERNAL_StrafeSpeed", speed.ToString());
        /// <summary>
        /// Allows you to change the jump impulse of the local client.
        /// </summary>
        /// <param name="jump">The new jump impulse.</param>
        public static void SetJumpImpulse(float jump = 0) => Internal_SendData("INTERNAL_JumpImp", jump.ToString());
        /// <summary>
        /// Allows you to change the gravity of the local client.
        /// </summary>
        /// <param name="gravity">The new gravity.</param>
        public static void SetGravityStrength(float gravity = 1) => Internal_SendData("INTERNAL_Gravity", gravity.ToString());
        /// <summary>
        /// Allows you to call a function depending if the world supports and contains it.
        /// </summary>
        /// <param name="function">The function name.</param>
        /// <param name="data">Optional data.</param>
        public static void SendData(string function, string data = "")
        {
            if (function.StartsWith("INTERNAL_"))
            {
                Logging.Error("Cannot call internal functions directly!");
                return;
            }

            SendData(function, data);
        }
    }
}
