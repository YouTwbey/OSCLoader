using System.Text.RegularExpressions;

namespace VRC
{
    /// <summary>
    /// Some settings may not update correctly unless a player changes them. Use with caution.
    /// </summary>
    public static class VRCSettings
    {
        public enum Languages
        {
            English,
            Français,
            Español,
            Italiano,
            한국어,
            Deutsch,
            日本語,
            Polski,
            Русский,
            Português_Brasil,
            Chinese_Simplified,
            Chinese_Traditional,
            עברית,
            toki_pona,
            Українська
        }
        public enum MicState
        {
            AlwaysOn,
            AlwaysOff,
            Toggle,
            PushToTalk
        }
        public enum WorldJoinMicState
        {
            DefaultOn,
            DefaultOff,
            KeepLastState
        }
        public enum MicVisibility
        {
            AlwaysOn,
            OnActivity,
            OnActivityWhenMutedOnly
        }
        public enum ComfortModeEnum
        {
            Standard,
            TunnellingLow,
            TunnellingHigh
        }
        public enum InstanceType
        {
            Public,
            FriendsOfGuests,
            FriendsOnly,
            InvitePlus,
            InviteOnly,
            Everyone
        }
        public enum RegionType
        {
            Automatic,
            US_West,
            US_East,
            Europe,
            Japan
        }
        public enum AAEnum
        {
            Disabled,
            x2,
            x4,
            x8
        }
        public enum MirrorQuality
        {
            Quarter,
            Half,
            Full,
            Unlimited
        }
        public enum LMH
        {
            Low,
            Medium,
            High
        }
        public enum OLMH
        {
            Off,
            Low,
            Medium,
            High
        }
        public enum Clipping
        {
            Off,
            Forced,
            Dynamic
        }
        public enum PlayerRestriction
        {
            None,
            Friends,
            Everyone
        }
        public enum AvatarPerformance
        {
            Medium,
            Poor,
            VeryPoor
        }

        public enum WorldTooltipsState
        {
            HighlightOnly,
            Tether,
            Tooltip,
            Controller
        }
        public enum VisibilityState
        {
            Standard,
            Icons,
            Hidden
        }
        public enum SizeState
        {
            Tiny,
            Small,
            Normal,
            Medium,
            Large
        }
        public enum DetailLevel
        {
            Off,
            Minimal,
            Verbose
        }
        public enum UIPosition
        {
            Center,
            Right,
            Left
        }
        public enum ChatPosition
        {
            Above,
            Forward
        }
        public enum GestureState
        {
            Grab = 1,
            Pinch,
            Combined
        }
        public enum ColorFilers
        {
            NoFilter,
            Protranopia,
            Deuteranopia,
            Tritanopia,
            Grayscale,
            RetroLUT
        }

        // General
        public static bool ClearCacheOnStart { get; internal set; } = false;
        public static bool ShowCommunityWorldsInSearch { get; internal set; } = false;
        public static bool HideNotificationPhotos { get; internal set; } = false;
        public static RegionType SelectedNetworkRegion { get; internal set; } = RegionType.Automatic;
        public static bool OSCEnabled { get; internal set; } = false;

        // Audio & Voice
        public static float MasterVolume { get; internal set; } = 100.00f;
        public static float UIVolume { get; internal set; } = 100.00f;
        public static float WorldVolume { get; internal set; } = 100.00f;
        public static float VoiceVolume { get; internal set; } = 100.00f;
        public static float AvatarVolume { get; internal set; } = 100.00f;
        public static bool MasterVolumeEnabled { get; internal set; } = true;
        public static bool UIVolumeEnabled { get; internal set; } = true;
        public static bool WorldVolumeEnabled { get; internal set; } = true;
        public static bool VoiceVolumeEnabled { get; internal set; } = true;
        public static bool AvatarVolumeEnabled { get; internal set; } = true;
        public static string MicDevice { get; internal set; } = "System Default";
        public static float MicVolume { get; internal set; } = 100.00f;
        public static bool NoiseSuppression { get; internal set; } = true;
        public static float MicActivationThreshold { get; internal set; } = 0.01f;
        public static MicState MicMode { get; internal set; } = MicState.Toggle;
        public static WorldJoinMicState MicBehaviour { get; internal set; } = WorldJoinMicState.KeepLastState;
        public static MicVisibility MicIconVisibilityMode { get; internal set; } = MicVisibility.OnActivity;
        public static bool EarmuffMode { get; internal set; } = false;
        public static float EarmuffDistance { get; internal set; } = 3.00f;
        public static float EarmuffFalloff { get; internal set; } = 1.00f;
        public static float EarmuffOutsideVolume { get; internal set; } = 20.00f;
        public static bool EarmuffModeAlwaysShowVisualAid { get; internal set; } = true;
        public static bool EarmuffModeAvatars { get; internal set; } = true;
        public static float EarmuffConeValue { get; internal set; } = 0.00f;
        public static float EarmuffOffsetValue { get; internal set; } = 100.00f;
        public static bool EarmuffModeFollowingCamera { get; internal set; } = true;
        public static bool EarmuffShapeRotationLocked { get; internal set; } = false;
        public static bool LegacyTalkToggle { get; internal set; } = true;
        public static bool LegacyTalkDefaultOn { get; internal set; } = true;
        public static bool LegacyDisableMicButton { get; internal set; } = false;
        public static float MicLevelVR { get; internal set; } = 1;
        public static float MicLevelDesktop { get; internal set; } = 0.65f;
        public static float NoiseGate { get; internal set; } = 0.0001f;

        // Comfort & Safety
        public static bool ComfortTurning { get; internal set; } = false;
        public static bool PersonalSpace { get; internal set; } = false;
        public static bool AFKEnabled { get; internal set; } = false;
        public static bool HoloportLocomotion { get; internal set; } = false;
        public static bool ThirdPersonRotation { get; internal set; } = false;
        public static ComfortModeEnum ComfortMode { get; internal set; } = ComfortModeEnum.Standard;
        public static int FingerTrackingLocomotion { get; internal set; } = 3;
        public static bool AllowDirectShares { get; internal set; } = true;
        public static bool OnlyAllowSharingFromFriends { get; internal set; } = true;
        public static bool AllowPedistalShares { get; internal set; } = true;
        public static bool OnlyAllowPedistalSharesFromFriends { get; internal set; } = true;
        public static bool AllowPrints { get; internal set; } = true;
        public static bool OnlyShowPrintsToFriends { get; internal set; } = false;
        public static bool StreamerMode { get; internal set; } = false;
        public static bool ShowCommunityLabs { get; internal set; } = true;
        public static bool AllowUntrustedURLs { get; internal set; } = true;
        public static bool ChatBubbleProfanityFilter { get; internal set; } = false;
        public static bool PortalPrompt { get; internal set; } = true;
        public static bool PlacePortalManually { get; internal set; } = true;
        public static InstanceType HomeInstanceType { get; internal set; } = InstanceType.InviteOnly;
        public static RegionType HomeRegion { get; internal set; } = RegionType.Automatic;
        public static bool VoicePrioritization { get; internal set; } = true;
        public static bool HeadLookWalking { get; internal set; } = true;
        public static string LocomotionMethod { get; internal set; } = "Gamelike";
        public static bool ViveAdvanced { get; internal set; } = false;
        public static string SafetyLevel { get; internal set; } = "None";

        // Graphics
        public static string GraphicsQuality { get; internal set; } = "Custom";
        public static AAEnum AntiAliasing { get; internal set; } = AAEnum.x8;
        public static MirrorQuality MirrorResolution { get; internal set; } = MirrorQuality.Unlimited;
        public static LMH Shadows { get; internal set; } = LMH.High;
        public static LMH LevelOfDetail { get; internal set; } = LMH.High;
        public static LMH ParticlePhysicsQuality { get; internal set; } = LMH.High;
        public static bool ParticleLimiter { get; internal set; } = false;
        public static OLMH PixelLightCount { get; internal set; } = OLMH.High;
        public static Clipping ForcedCameraNearDIstance { get; internal set; } = Clipping.Off;

        // Avatars
        public static bool AllowAvatarCloning { get; internal set; } = false;
        public static bool AutoDisableAvatarCloning { get; internal set; } = true;
        public static bool PauseAvatarInteractions { get; internal set; } = false;
        public static PlayerRestriction AvatarAllowedToInteract { get; internal set; } = PlayerRestriction.Everyone;
        public static bool AvatarSelfInteract { get; internal set; } = true;
        public static AvatarPerformance BlockPoorlyOptimizedAvatars { get; internal set; } = AvatarPerformance.VeryPoor;
        public static int MaximumDownloadSize { get; internal set; } = 200;
        public static int MaximumUncompressedSize { get; internal set; } = 0;
        public static bool HideAvatarsBeyond { get; internal set; } = false;
        public static float AvatarProimityShowRange { get; internal set; } = 50.00f;
        public static bool LimitShownAvatars { get; internal set; } = false;
        public static int MaximumShownAvatars { get; internal set; } = 30;
        public static bool AlwaysShowFriendAvatars { get; internal set; } = true;
        public static bool AllowOverrideWithShowAvatar { get; internal set; } = true;
        public static bool PrioritizeAvatarDownloadByDistance { get; internal set; } = true;
        public static int DownloadPrioritizeDistance { get; internal set; } = 20;
        public static bool PrioritizeManuallyShownAvatars { get; internal set; } = true;
        public static bool PrioritizeFriendsAvatars { get; internal set; } = true;
        public static bool FallbackHidden { get; internal set; } = false;

        // Mirrors

        // User Interface
        public static bool MenuTooltips { get; internal set; } = true;
        public static WorldTooltipsState InteractiveObjectTooltips { get; internal set; } = WorldTooltipsState.Controller;
        public static bool ShowReticl { get; internal set; } = true;
        public static bool ShowGoButtonOnLoad { get; internal set; } = false;
        public static bool SliderSnapping { get; internal set; } = true;
        public static bool HeaderClickScrollsToTopOfPage { get; internal set; } = true;
        public static Languages PreferredLanguage { get; internal set; } = Languages.English;
        public static VisibilityState NameplateVisibility { get; internal set; } = VisibilityState.Standard;
        public static SizeState NameplateScale { get; internal set; } = SizeState.Normal;
        public static float NameplateOpacity { get; internal set; } = 80.00f;
        public static bool ShowAdditionalInfoOnNameplates { get; internal set; } = true;
        public static bool ShowFallbackIcon { get; internal set; } = true;
        public static bool ShowEarmuffsIcon { get; internal set; } = true;
        public static DetailLevel HUDDetailLevel { get; internal set; } = DetailLevel.Verbose;
        public static bool PlayNotificationAudio { get; internal set; } = true;
        public static bool ShowGestureIcons { get; internal set; } = false;
        public static float HUDOpacity { get; internal set; } = 50.00f;
        public static float MicOpacity { get; internal set; } = 50.00f;
        public static float MicToggleVolume { get; internal set; } = 50.00f;
        public static bool ShowJoinNotifications { get; internal set; } = false;
        public static bool ShowLeaveNotifications { get; internal set; } = false;
        public static bool ShowPortalNotifications { get; internal set; } = false;
        public static bool OnlyShowFriendJoinLeavePortalNotifications { get; internal set; } = true;
        public static bool ShowInvites { get; internal set; } = true;
        public static bool ShowFriendRequests { get; internal set; } = true;
        public static UIPosition NotificationPosition { get; internal set; } = UIPosition.Center;
        public static PlayerRestriction LocalChatboxVisibility { get; internal set; } = PlayerRestriction.Friends;
        public static ChatPosition ChatboxPosition { get; internal set; } = ChatPosition.Above;
        public static float ChatboxHeight { get; internal set; } = 0.5f;
        public static float ChatboxSize { get; internal set; } = 150.00f;
        public static float ChatboxOpacity { get; internal set; } = 100.00f;
        public static float ChatboxDisplayDuration { get; internal set; } = 30f;
        public static bool ChatboxNotificationSound { get; internal set; } = false;
        public static float ChatboxNotificationVolume { get; internal set; } = 1f;
        public static bool ShowOwnChatbox { get; internal set; } = true;
        public static bool SendTextAutomatically { get; internal set; } = true;
        public static bool ShowTooltips { get; internal set; } = true;
        public static bool DesktopReticle { get; internal set; } = true;
        public static bool ShowSocialRank { get; internal set; } = true;
        public static bool MainMenuFreePlacement { get; internal set; } = false;
        public static bool QMInfo { get; internal set; } = true;
        public static string StatusMode { get; internal set; } = "ShowOnMenu";

        // Controls
        public static float MouseSensitivity { get; internal set; } = 1;
        public static bool InvertedMouse { get; internal set; } = false;
        public static bool UIHapticsEnabled { get; internal set; } = false;

        // Tracking & IK
        public static bool FingerJumpGesture { get; internal set; } = true;
        public static GestureState FingerGrabGesture { get; internal set; } = GestureState.Grab;
        public static bool IKDebugLogging { get; internal set; } = false;
        public static bool PerAvatarCalibrationAdjustment { get; internal set; } = false;

        // Accessibility
        public static float ScreenBrightness { get; internal set; } = 100.00f;
        public static float BloomIntensity { get; internal set; } = 100.00f;
        public static bool ColorFilters { get; internal set; } = false;
        public static float ColorFilterIntensity { get; internal set; } = 30.00f;
        public static bool ApplyColorFilterToWorld { get; internal set; } = false;
        public static ColorFilers ColorFilter { get; internal set; } = ColorFilers.NoFilter;

        // Debug
        static float RemovePercent(string value)
        {
            string removePercent = value.Replace("%", "");
            float newVal = float.Parse(removePercent);
            return newVal;
        }
        static bool ToBool(string value)
        {
            bool val = bool.Parse(value);
            return val;
        }

        internal static void HandleSetting(string key, string value)
        {
            // Settings On Opening Game //
            if (key == "Clear cache on start")
            {
                ClearCacheOnStart = bool.Parse(value);
            }
            if (key == "Show go button on load")
            {
                ShowGoButtonOnLoad = bool.Parse(value);
            }
            if (key == "Show community labs")
            {
                ShowCommunityLabs = bool.Parse(value);
            }
            if (key == "Show community worlds in search")
            {
                ShowCommunityWorldsInSearch = bool.Parse(value);
            }
            if (key == "Hide notification photos")
            {
                HideNotificationPhotos = bool.Parse(value);
            }
            if (key == "Selected network region")
            {
                if (value == "Europe")
                {
                    SelectedNetworkRegion = RegionType.Europe;
                }
                if (value == "Japan")
                {
                    SelectedNetworkRegion = RegionType.Japan;
                }
                if (value == "US_East")
                {
                    SelectedNetworkRegion = RegionType.US_East;
                }
                if (value == "US_West")
                {
                    SelectedNetworkRegion = RegionType.US_West;
                }
                if (value == "Automatic")
                {
                    SelectedNetworkRegion = RegionType.Automatic;
                }
            }
            if (key == "OSC enabled")
            {
                OSCEnabled = bool.Parse(value);
            }
            if (key == "AFK enabled")
            {
                AFKEnabled = bool.Parse(value);
            }
            if (key == "Quality")
            {
                GraphicsQuality = value;
            }
            if (key == "Antianilasing")
            {
                AntiAliasing = Enum.Parse<AAEnum>(value);
            }
            if (key == "Mouse sensitivity")
            {
                MouseSensitivity = float.Parse(value);
            }
            if (key == "Inverted mouse")
            {
                InvertedMouse = bool.Parse(value);
            }
            if (key == "UI Haptics enabled")
            {
                UIHapticsEnabled = bool.Parse(value);
            }
            if (key == "Legacy Talk toggle")
            {
                LegacyTalkToggle = bool.Parse(value);
            }
            if (key == "Legacy Talk default on")
            {
                LegacyTalkDefaultOn = bool.Parse(value);
            }
            if (key == "Legacy Disable mic button")
            {
                LegacyDisableMicButton = bool.Parse(value);
            }
            if (key == "Mic Mode")
            {
                MicMode = Enum.Parse<MicState>(value);
            }
            if (key == "Mic Behaviour On Join")
            {
                MicBehaviour = Enum.Parse<WorldJoinMicState>(value);
            }
            if (key == "Mic level (VR)")
            {
                MicLevelVR = float.Parse(value);
            }
            if (key == "Mic level (Desktop)")
            {
                MicLevelDesktop = float.Parse(value);
            }
            if (key == "Noise suppression")
            {
                NoiseSuppression = bool.Parse(value);
            }
            if (key == "Noise gate")
            {
                NoiseGate = float.Parse(value);
            }
            if (key == "Show tooltips")
            {
                ShowTooltips = bool.Parse(value);
            }
            if (key == "Desktop reticle")
            {
                DesktopReticle = bool.Parse(value);
            }
            if (key == "Show social rank")
            {
                ShowSocialRank = bool.Parse(value);
            }
            if (key == "Main Menu free placement")
            {
                NoiseSuppression = bool.Parse(value);
            }
            if (key == "Nameplate display")
            {
                NameplateVisibility = Enum.Parse<VisibilityState>(value);
            }
            if (key == "QM Info")
            {
                if (value == "on")
                {
                    QMInfo = true;
                }
                else
                {
                    QMInfo = false;
                }
            }
            if (key == "Fallback Icon")
            {
                if (value == "visible")
                {
                    ShowFallbackIcon = true;
                }
                else
                {
                    ShowFallbackIcon = false;
                }
            }
            if (key == "Status mode")
            {
                StatusMode = value;
            }
            if (key == "Nameplate opacity")
            {
                string val = value.Replace("%", "");
                NameplateOpacity = float.Parse(val);
            }
            if (key == "Fallback hidden")
            {
                FallbackHidden = bool.Parse(value);
            }
            if (key == "Maximum download size")
            {
                string val = value.Split(' ')[0];
                MaximumDownloadSize = int.Parse(val);
            }
            if (key == "Performance rating minimum to display")
            {
                BlockPoorlyOptimizedAvatars = Enum.Parse<AvatarPerformance>(value);
            }
            if (key == "Allow avatar copying")
            {
                AllowAvatarCloning = bool.Parse(value);
            }
            if (key == "Disable avatar cloning on enter world")
            {
                AutoDisableAvatarCloning = bool.Parse(value);
            }
            if (key == "Avatar interaction level")
            {
                if (value == "All")
                {
                    AvatarAllowedToInteract = PlayerRestriction.Everyone;
                }
                else
                {
                    AvatarAllowedToInteract = Enum.Parse<PlayerRestriction>(value);
                }
            }
            if (key == "Avatar self-ineraction")
            {
                AvatarSelfInteract = bool.Parse(value);
            }
            if (key == "Gesture bar enabled")
            {
                ShowGestureIcons = bool.Parse(value);
            }
            if (key == "Earmuff mode")
            {
                EarmuffMode = bool.Parse(value);
            }
            if (key == "Earmuff mode (avatars)")
            {
                EarmuffModeAvatars = bool.Parse(value);
            }
            if (key == "Earmuff mode, Always show visual aid")
            {
                EarmuffModeAlwaysShowVisualAid = bool.Parse(value);
            }
            if (key == "Earmuff mode radius")
            {
                EarmuffDistance = float.Parse(value);
            }
            if (key == "Earmuff mode falloff")
            {
                EarmuffFalloff = float.Parse(value);
            }
            if (key == "Earmuff mode reduced volume")
            {
                EarmuffOutsideVolume = float.Parse(value);
            }
            if (key == "Chat bubble visibility")
            {
                if (value == "All")
                {
                    LocalChatboxVisibility = PlayerRestriction.Everyone;
                }
                else
                {
                    LocalChatboxVisibility = Enum.Parse<PlayerRestriction>(value);
                }
            }
            if (key == "Chat bubble scale")
            {
                ChatboxSize = float.Parse(value);
            }
            if (key == "Chat bubble opacity")
            {
                ChatboxOpacity = float.Parse(value);
            }
            if (key == "Chat bubble timeout")
            {
                ChatboxDisplayDuration = float.Parse(value);
            }
            if (key == "Personal space")
            {
                PersonalSpace = bool.Parse(value);
            }
            if (key == "Voice prioritization")
            {
                VoicePrioritization = bool.Parse(value);
            }
            if (key == "Third person rotation")
            {
                ThirdPersonRotation = bool.Parse(value);
            }
            if (key == "Comfort turning")
            {
                ComfortTurning = bool.Parse(value);
            }
            if (key == "Head look walking")
            {
                HeadLookWalking = bool.Parse(value);
            }
            if (key == "Locomotion method")
            {
                LocomotionMethod = value;
            }
            if (key == "Vive advanced")
            {
                ViveAdvanced = bool.Parse(value);
            }
            if (key == "Streamer mode enabled")
            {
                StreamerMode = bool.Parse(value);
            }
            if (key == "Limit particle systems")
            {
                ParticleLimiter = bool.Parse(value);
            }
            if (key == "Safety level")
            {
                SafetyLevel = value;
            }
            if (key == "Allow untrusted URLs")
            {
                AllowUntrustedURLs = bool.Parse(value);
            }
            if (key == "Ask to portal")
            {
                PortalPrompt = bool.Parse(value);
            }
            if (key == "Mirror resolution")
            {
                MirrorResolution = Enum.Parse<MirrorQuality>(value);
            }
            if (key == "Portal Mode")
            {
                if (value == "PlaceManually")
                {
                    PlacePortalManually = true;
                }
                else
                {
                    PlacePortalManually = false;
                }
            }

            // Settings from VRCInputManager //
            if (key == "Master volume")
            {
                MasterVolume = RemovePercent(value);
            }
            if (key == "Master volume enabled")
            {
                MasterVolumeEnabled = ToBool(value);
            }
            if (key == "UI volume")
            {
                UIVolume = RemovePercent(value);
            }
            if (key == "UI volume enabled")
            {
                UIVolumeEnabled = ToBool(value);
            }
            if (key == "World volume")
            {
                WorldVolume = RemovePercent(value);
            }
            if (key == "World volume enabled")
            {
                WorldVolumeEnabled = ToBool(value);
            }
            if (key == "Voice volume")
            {
                VoiceVolume = RemovePercent(value);
            }
            if (key == "Voice volume enabled")
            {
                VoiceVolumeEnabled = ToBool(value);
            }
            if (key == "Avatar volume")
            {
                AvatarVolume = RemovePercent(value);
            }
            if (key == "Avatar volume enabled")
            {
                AvatarVolumeEnabled = ToBool(value);
            }
            if (key == "Mic volume")
            {
                MicVolume = RemovePercent(value);
            }
            if (key == "Noise suppression")
            {
                NoiseSuppression = ToBool(value);
            }
            if (key == "Mic activation threshold")
            {
                MicActivationThreshold = RemovePercent(value);
            }
            if (key == "Mic mode")
            {
                if (value == "AlwaysOn")
                {
                    MicMode = MicState.AlwaysOn;
                }
                else if (value == "AlwaysOff")
                {
                    MicMode = MicState.AlwaysOff;
                }
                else if (value == "Toggle")
                {
                    MicMode = MicState.Toggle;
                }
                else if (value == "PushToTalk")
                {
                    MicMode = MicState.PushToTalk;
                }
            }
        }

        internal static void TrackLogForSetting(string log)
        {
            var keyValueRegex = new Regex(@"^\s+(?<key>.+?):\s+(?<value>.+)$");

            var match = keyValueRegex.Match(log);
            if (match.Success)
            {
                string key = match.Groups["key"].Value.Trim();
                string value = match.Groups["value"].Value.Trim();

                HandleSetting(key, value);
            }
        }
    }
}