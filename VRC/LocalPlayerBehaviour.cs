using System.Numerics;
using VRC.Managers;

namespace VRC
{
    /// <summary>
    /// Shows information about the Local Player while also allowing to change avatar parameters
    /// </summary>
    public static class LocalPlayerBehaviour
    {
        /// <summary>
        /// Page Type on Menu.
        /// </summary>
        public enum PageType
        {
            /// <summary>
            /// No Menu.
            /// </summary>
            Null,
            /// <summary>
            /// VRChat Plus Purchase Menu.
            /// </summary>
            VRChatPlusSubscriptions,
            /// <summary>
            /// World Browser Menu.
            /// </summary>
            Worlds,
            /// <summary>
            /// Client's profile.
            /// </summary>
            Profile
        }

        /// <summary>
        /// The local player's X velocity
        /// </summary>
        public static float VelocityX { get; internal set; } = 0;
        /// <summary>
        /// The local player's Y velocity
        /// </summary>
        public static float VelocityY { get; internal set; } = 0;
        /// <summary>
        /// The local player's Z velocity
        /// </summary>
        public static float VelocityZ { get; internal set; } = 0;
        /// <summary>
        /// The local player's Y angular
        /// </summary>
        public static float AngularY { get; internal set; } = 0;
        /// <summary>
        /// Check to see if the local player is grounded
        /// </summary>
        public static bool Grounded { get; internal set; } = true;
        /// <summary>
        /// Check to see if the local player is AFK
        /// </summary>
        public static bool AFK { get; internal set; } = false;
        /// <summary>
        /// Check to see if the local player is in VR
        /// </summary>
        public static bool VRMode { get; internal set; } = false;
        /// <summary>
        /// Check to see if the local player has muted themself
        /// </summary>
        public static bool MuteSelf { get; internal set; } = false;
        /// <summary>
        /// The local player's voice
        /// </summary>
        public static float Voice { get; internal set; } = 0.00f;
        /// <summary>
        /// Check to see if the local player has earmuffs enabled
        /// </summary>
        public static bool Earmuffs { get; internal set; } = false;
        /// <summary>
        /// The local player's velocity magnitude
        /// </summary>
        public static float VelocityMagnitude { get; internal set; } = 0;
        /// <summary>
        /// The local player's scale factor
        /// </summary>
        public static float ScaleFactor { get; internal set; } = 1.00f;
        /// <summary>
        /// The local player's scale factor inverse
        /// </summary>
        public static float ScaleFactorInverse { get; internal set; } = 1.00f;
        /// <summary>
        /// Check to see if the local player's scale has been modified
        /// </summary>
        public static bool ScaleModified { get; internal set; } = false;
        /// <summary>
        /// The local player's eye height as a percentage
        /// </summary>
        public static float EyeHeightAsPercent { get; internal set; } = 0.05f;
        /// <summary>
        /// The local player's eye height in meters
        /// </summary>
        public static float EyeHeightAsMeters { get; internal set; } = 0.45f;
        /// <summary>
        /// The local player's left hand gesture
        /// </summary>
        public static int GestureLeft { get; internal set; } = 0;
        /// <summary>
        /// The local player's right hand gesture
        /// </summary>
        public static int GestureRight { get; internal set; } = 0;
        /// <summary>
        /// The local player's left hand gesture's weight
        /// </summary>
        public static float GestureLeftWeight { get; internal set; } = 0.00f;
        /// <summary>
        /// The local player's right hand gesture's weight
        /// </summary>
        public static float GestureRightWeight { get; internal set; } = 0.00f;
        /// <summary>
        /// Check to see if the local player is sitting
        /// </summary>
        public static bool Seated { get; internal set; } = false;
        /// <summary>
        /// Check to see if the local player is on a VRC Station
        /// </summary>
        public static bool InStation { get; internal set; } = false;
        /// <summary>
        /// The local player's emote
        /// </summary>
        public static int VRCEmote { get; internal set; } = 0;
        /// <summary>
        /// The local player's H face blend
        /// </summary>
        public static float VRCFaceBlendH { get; internal set; } = 0.00f;
        /// <summary>
        /// The local player's V face blend
        /// </summary>
        public static float VRCFaceBlendV { get; internal set; } = 0.00f;

        /// <summary>
        /// Sends a request to change your parameters on your current avatar
        /// </summary>
        /// <param name="paramater">The parameter's path</param>
        /// <param name="value">The new value for the parameter</param>
        public static void RPC_SetAvatarParameter(string paramater, float value)
        {
            OSCManager.SendOSC("/avatar/parameters/" + paramater, value);
        }

        /// <summary>
        /// Sends a request to change your parameters on your current avatar
        /// </summary>
        /// <param name="paramater">The parameter's path</param>
        /// <param name="value">The new value for the parameter</param>
        public static void RPC_SetAvatarParameter(string paramater, bool value)
        {
            OSCManager.SendOSC("/avatar/parameters/" + paramater, value);
        }

        /// <summary>
        /// Sends a request to change your parameters on your current avatar
        /// </summary>
        /// <param name="paramater">The parameter's path</param>
        /// <param name="value">The new value for the parameter</param>
        public static void RPC_SetAvatarParameter(string paramater, int value)
        {
            OSCManager.SendOSC("/avatar/parameters/" + paramater, value);
        }

        /// <summary>
        /// Sends a request to change your scale on your current avatar
        /// </summary>
        /// <param name="scale">The new scale</param>
        public static void RPC_SetScale(float scale)
        {
            float clampedScale = Math.Clamp(scale, 0.01f, 10000);
            OSCManager.SendOSC("/avatar/eyeheight", clampedScale);
        }

        /// <summary>
        /// Sendable inputs for OSC
        /// </summary>
        public enum Inputs
        {
            /// <summary>
            /// Move forwards (1) or Backwards (-1)
            /// </summary>
            Vertical,
            /// <summary>
            /// Move right (1) or left (-1)
            /// </summary>
            Horizontal,
            /// <summary>
            /// Look Left and Right. Smooth in Desktop, VR will do a snap-turn when the value is 1 if Comfort Turning is on.
            /// </summary>
            LookHorizontal,
            /// <summary>
            /// Use held item
            /// </summary>
            UseAxisRight,
            /// <summary>
            /// Grab item
            /// </summary>
            GrabAxisRight,
            /// <summary>
            /// Move a held object forwards (1) and backwards (-1)
            /// </summary>
            MoveHoldFB,
            /// <summary>
            /// Spin a held object Clockwise or Counter-Clockwise
            /// </summary>
            SpinHoldCwCcw,
            /// <summary>
            /// Spin a held object Up or Down
            /// </summary>
            SpinHoldUD,
            /// <summary>
            /// Spin a held object Left or Right
            /// </summary>
            SpinHoldLR,
            /// <summary>
            /// Move forward while this is 1.
            /// </summary>
            MoveForward,
            /// <summary>
            /// Move backwards while this is 1.
            /// </summary>
            MoveBackward,
            /// <summary>
            /// Strafe left while this is 1.
            /// </summary>
            MoveLeft,
            /// <summary>
            /// Strafe right while this is 1.
            /// </summary>
            MoveRight,
            /// <summary>
            /// Turn to the left while this is 1. Smooth in Desktop, VR will do a snap-turn if Comfort Turning is on.
            /// </summary>
            LookLeft,
            /// <summary>
            /// Turn to the right while this is 1. Smooth in Desktop, VR will do a snap-turn if Comfort Turning is on.
            /// </summary>
            LookRight,
            /// <summary>
            /// Jump if the world supports it.
            /// </summary>
            Jump,
            /// <summary>
            /// Walk faster if the world supports it.
            /// </summary>
            Run,
            /// <summary>
            /// Snap-Turn to the left - VR Only.
            /// </summary>
            ComfortLeft,
            /// <summary>
            /// Snap-Turn to the right - VR Only.
            /// </summary>
            ComfortRight,
            /// <summary>
            /// Drop the item held in your right hand - VR Only.
            /// </summary>
            DropRight,
            /// <summary>
            /// Use the item highlighted by your right hand - VR Only.
            /// </summary>
            UseRight,
            /// <summary>
            /// Grab the item highlighted by your right hand - VR Only.
            /// </summary>
            GrabRight,
            /// <summary>
            /// Drop the item held in your left hand - VR Only.
            /// </summary>
            DropLeft,
            /// <summary>
            /// Use the item highlighted by your left hand - VR Only.
            /// </summary>
            UseLeft,
            /// <summary>
            /// Grab the item highlighted by your left hand - VR Only.
            /// </summary>
            GrabLeft,
            /// <summary>
            /// Turn on Safe Mode.
            /// </summary>
            PanicButton,
            /// <summary>
            /// Toggle QuickMenu On/Off. Will toggle upon receiving '1' if it's currently '0'.
            /// </summary>
            QuickMenuToggleLeft,
            /// <summary>
            /// Toggle QuickMenu On/Off. Will toggle upon receiving '1' if it's currently '0'.
            /// </summary>
            QuickMenuToggleRight,
            /// <summary>
            /// Toggle Voice - the action will depend on whether "Toggle Voice" is turned on in your Settings.
            /// </summary>
            Voice
        }

        /// <summary>
        /// Eye tracking for OSC.
        /// </summary>
        public enum EyeTracking
        {
            EyesClosedAmount,
            CenterPitchYaw,
            CenterPitchYawDist,
            CenterVec,
            CenterVecFull,
            LeftRightPitchYaw,
            LeftRightVec
        }

        /// <summary>
        /// Eye tracking value types.
        /// </summary>
        public enum EyeTrackingTypes
        {
            Vector2,
            Vector3,
            Vector4,
            Vector3Vector3,
        }

        /// <summary>
        /// Sends a request to VRChat to invoke an input
        /// </summary>
        /// <param name="input">Input to invoke</param>
        /// <param name="val">Input value</param>
        public static void RPC_RunInput(Inputs input, float val)
        {
            val = Math.Clamp(val, -1, 1);
            OSCManager.SendOSC("/input/" + input.ToString(), val);
        }

        /// <summary>
        /// Sends a request to VRChat to update a tracker
        /// </summary>
        /// <param name="tracker">Tracker to update</param>
        /// <param name="updatingRotation">Bool whether to update the position or rotation</param>
        /// <param name="value">Position/Rotation Value</param>
        public static void RPC_UpdateTracker(int tracker, bool updatingRotation, Vector3 value)
        {
            string type = updatingRotation ? "rotation" : "position";
            string _tracker = Math.Clamp(tracker, 1, 9).ToString();
            if (tracker == 9) _tracker = "head";

            OSCManager.SendOSC($"/tracking/trackers/{_tracker}/{type}", value);
        }

        /// <summary>
        /// Sends a request to VRChat to update a tracker
        /// </summary>
        /// <param name="tracker">Tracker to update</param>
        /// <param name="type">The type of eye tracking value</param>
        /// <param name="values">Values to send matching the EyeTrackingTypes value given</param>
        public static void RPC_UpdateTracker(EyeTracking tracker, EyeTrackingTypes type, params object[] values)
        {
            string types = ",";

            switch (type)
            {
                case EyeTrackingTypes.Vector2:
                    types = ",ff";
                    break;
                case EyeTrackingTypes.Vector3:
                    types = ",fff";
                    break;
                case EyeTrackingTypes.Vector4:
                    types = ",ffff";
                    break;
                case EyeTrackingTypes.Vector3Vector3:
                    types = ",ffffff";
                    break;
            }

            OSCManager.SendOSC($"/tracking/eye/{tracker}", types, values);
        }
    }
}
