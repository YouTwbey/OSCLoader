using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Windows.Media.Control;

namespace OSCLoader.Core.Handlers
{
    /// <summary>
    /// Get easy information about the client's device
    /// </summary>
    public static class Device
    {
        public enum PlatformType
        {
            PC,
            Quest,
            Unknown
        }

        [DllImport("kernel32.dll")]
        static extern bool GetSystemPowerStatus(out SYSTEM_POWER_STATUS sps);

        internal static MediaSnapshot snapshot = MediaSnapshot.Null;
        internal static SYSTEM_POWER_STATUS GetStatus()
        {
            GetSystemPowerStatus(out var status);
            return status;
        }

        internal struct SYSTEM_POWER_STATUS
        {
            public byte ACLineStatus;      // 0 = Offline, 1 = Online, 255 = Unknown
            public byte BatteryFlag;       // Charging flags
            public byte BatteryLifePercent;// 0–100, 255 = Unknown
            public int BatteryLifeTime;    // Seconds
            public int BatteryFullLifeTime;
        }

        /// <summary>
        /// Gets the client's device's battery percentage
        /// </summary>
        /// <returns>The battery percentage as an int</returns>
        public static int GetBatteryPercent()
        {
            return GetStatus().BatteryLifePercent;
        }

        /// <summary>
        /// Checks if the client's device's battery is being charged
        /// </summary>
        /// <returns>True if the battery is currently charging</returns>
        public static bool IsCharging()
        {
            return GetStatus().ACLineStatus == 1;
        }

        /// <summary>
        /// Checks the device the user is playing on
        /// </summary>
        /// <returns>The device the user is playing on</returns>
        public static PlatformType GetPlatform()
        {
            return PlatformType.PC;
        }

        /// <summary>
        /// A media's snapshot
        /// </summary>
        public struct MediaSnapshot
        {
            /// <summary>
            /// Title of the media
            /// </summary>
            public string Title = "null";
            /// <summary>
            /// Artist of the media
            /// </summary>
            public string Artist = "null";
            /// <summary>
            /// Media's position when the snapshot was taken
            /// </summary>
            public float MediaPosition
            {
                get
                {
                    float pos = LastUpdateMediaPosition;

                    if (IsPlaying)
                    {
                        pos += (float)(DateTime.UtcNow - LastUpdate).TotalSeconds;
                    }

                    return pos;
                }
            }
            internal float LastUpdateMediaPosition = 0;
            internal DateTime LastUpdate = DateTime.UtcNow;
            /// <summary>
            /// Media's length
            /// </summary>
            public float MediaLength = 0;
            /// <summary>
            /// If it is currently playing
            /// </summary>
            public bool IsPlaying = false;

            /// <summary>
            /// Creates a Media Snapshot instance
            /// </summary>
            public MediaSnapshot() { }
            /// <summary>
            /// Null reference to MediaSnapshot
            /// </summary>
            public static MediaSnapshot Null = new MediaSnapshot();
        }

        static PerformanceCounterCategory _gpuCategory = new("GPU Engine");
        static void RefreshGpuCounters()
        {
            foreach (var counter in _gpuCounters)
                counter.Dispose();

            _gpuCounters.Clear();

            foreach (string instance in _gpuCategory.GetInstanceNames())
            {
                if (instance.Contains("engtype_3D"))
                {
                    var counter = new PerformanceCounter(
                        "GPU Engine",
                        "Utilization Percentage",
                        instance);

                    counter.NextValue();
                    _gpuCounters.Add(counter);
                }
            }
        }

        internal static async Task RetrieveMediaSnapshot()
        {
            var manager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
            _cpuCounter.NextValue();

            foreach (var instance in _gpuCategory.GetInstanceNames())
            {
                if (instance.Contains("engtype_3D"))
                {
                    var counter = new PerformanceCounter(
                        "GPU Engine",
                        "Utilization Percentage",
                        instance,
                        true);

                    counter.NextValue();
                    _gpuCounters.Add(counter);
                }
            }

            await Task.Delay(1000);

            while (true)
            {
                try
                {
                    var session = manager.GetCurrentSession();

                    if (session != null)
                    {
                        var mediaTask = session.TryGetMediaPropertiesAsync();
                        var timeline = session.GetTimelineProperties();
                        var playback = session.GetPlaybackInfo();

                        await Task.WhenAll(mediaTask.AsTask());

                        var media = await mediaTask;

                        if (media != null && timeline != null && playback != null)
                        {
                            DateTime lastUpdate = snapshot.LastUpdate;
                            float previous = snapshot.LastUpdateMediaPosition;
                            float current = (float)timeline.Position.TotalSeconds;

                            bool isRoughlySame = Math.Abs(current - previous) < 0.5f;

                            if (!isRoughlySame)
                            {
                                lastUpdate = DateTime.UtcNow;
                            }

                            snapshot = new MediaSnapshot
                            {
                                Title = media.Title,
                                Artist = media.Artist,
                                MediaLength = (float)timeline.EndTime.TotalSeconds,
                                IsPlaying = playback.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing,
                                LastUpdate = lastUpdate,
                                LastUpdateMediaPosition = (float)timeline.Position.TotalSeconds
                            };
                        }
                        else
                        {
                            snapshot = MediaSnapshot.Null;
                        }
                    }
                    else
                    {
                        snapshot = MediaSnapshot.Null;
                    }

                    if (_cpuCounter != null)
                    {
                        RefreshGpuCounters();

                        CpuUsage = _cpuCounter.NextValue();

                        float gpu = 0;

                        foreach (var counter in _gpuCounters)
                        {
                            gpu += counter.NextValue();
                        }

                        GpuUsage = Math.Min(gpu, 100);
                    }
                }
                catch
                {
                    snapshot = MediaSnapshot.Null;
                }

                await Task.Delay(1000);
            }
        }

        /// <summary>
        /// Gets the current media as a snapshot<br/>Not supported on Quest
        /// </summary>
        /// <returns>The media snapshot retrieved</returns>
        public static MediaSnapshot GetMediaSnapshot()
        {
            return snapshot;
        }

        /// <summary>
        /// Returns the amount of RAM being used as a percentage
        /// </summary>
        /// <returns>The RAM being used as a percentage</returns>
        public static int GetRAMUsage()
        {
            try
            {
                float available = new PerformanceCounter("Memory", "Available MBytes").NextValue();
                float cpuUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total").NextValue();
                ComputerInfo computerInfo = new ComputerInfo();
                ulong total = computerInfo.TotalPhysicalMemory / 1024 / 1024;
                float usedPercent = (float)(total - available) * 100f / total;

                return (int)usedPercent;
            }
            catch
            {
                return 0;
            }
        }

        static PerformanceCounter? _cpuCounter;
        static List<PerformanceCounter> _gpuCounters = new();
        static float CpuUsage;
        static float GpuUsage;

        /// <summary>
        /// Returns the amount of CPU being used as a percentage<br/>Not supported on Quest
        /// </summary>
        /// <returns>The CPU being used as a percentage</returns>
        public static int GetCPUUsage()
        {
            return (int)CpuUsage;
        }

        /// <summary>
        /// Returns the amount of GPU being used as a percentage<br/>Not supported on Quest
        /// </summary>
        /// <returns>The GPU being used as a percentage</returns>
        public static int GetGPUUsage()
        {
            return (int)GpuUsage;
        }

        /// <summary>
        /// Gets the name of this device
        /// </summary>
        /// <returns>The name of this device</returns>
        public static string MachineName
        {
            get
            {
                return Environment.MachineName;
            }
        }
    }
}
