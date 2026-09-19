using OSCLoader.Debug;
using Valve.VR;

namespace SteamVR
{
    /// <summary>
    /// Contains information about any currently connected devices to SteamVR. Class only supported on PC
    /// </summary>
    public static class Devices
    {
        /// <summary>
        /// Lists out all devices connected based on when Devices.Refresh() was last called
        /// </summary>
        public static List<DeviceInfo> All = new List<DeviceInfo>();
        /// <summary>
        /// The info about a specific device
        /// </summary>
        public struct DeviceInfo
        {
            /// <summary>
            /// The name of the device
            /// </summary>
            public string Name { get; internal set; }
            /// <summary>
            /// The serial of the device
            /// </summary>
            public string Serial { get; internal set; }
            /// <summary>
            /// The battery percentage of the device
            /// </summary>
            public int BatteryPercent { get; internal set; }

            internal DeviceInfo(string name, string serial, int battery)
            {
                Name = name;
                Serial = serial;
                BatteryPercent = battery;
            }
        }

        /// <summary>
        /// Refreshes Devices.All
        /// </summary>
        public static void Refresh()
        {
            All.Clear();

            CVRSystem system = OpenVR.System;

            if (system == null)
            {
                EVRInitError error = EVRInitError.None;
                OpenVR.Init(ref error, EVRApplicationType.VRApplication_Background);

                if (error != EVRInitError.None)
                {
                    if (error != EVRInitError.Init_NoServerForBackgroundApp) Logging.DebugError(error.ToString());
                    return;
                }

                system = OpenVR.System;
                if (system == null)
                {
                    Logging.DebugError("System is null");
                    return;
                }
            }

            for (uint i = 0; i < OpenVR.k_unMaxTrackedDeviceCount; i++)
            {
                if (!system.IsTrackedDeviceConnected(i)) continue;

                ETrackedPropertyError error = ETrackedPropertyError.TrackedProp_Success;

                bool hasBattery = system.GetBoolTrackedDeviceProperty(i, ETrackedDeviceProperty.Prop_DeviceProvidesBatteryStatus_Bool, ref error);
                int percent = 100;
                if (hasBattery)
                {
                    percent = (int)(system.GetFloatTrackedDeviceProperty(i, ETrackedDeviceProperty.Prop_DeviceBatteryPercentage_Float, ref error) * 100f);
                }
                string name = GetDeviceName(system, i);
                string serial = GetDeviceSerial(system, i);

                DeviceInfo device = new DeviceInfo(name, serial, percent);

                if (!hasBattery) Logging.DebugError($"Failed to find battery for device: {device.Name} ({device.Serial})");

                All.Add(device);
            }
        }

        static string GetDeviceName(CVRSystem system, uint index)
        {
            return GetStringProperty(system, index, ETrackedDeviceProperty.Prop_ModelNumber_String);
        }

        static string GetDeviceSerial(CVRSystem system, uint index)
        {
            return GetStringProperty(system, index, ETrackedDeviceProperty.Prop_SerialNumber_String);
        }

        static string GetStringProperty(CVRSystem system, uint index, ETrackedDeviceProperty property)
        {
            ETrackedPropertyError error = ETrackedPropertyError.TrackedProp_Success;
            uint size = system.GetStringTrackedDeviceProperty( index, property, null, 0, ref error);
            if (size == 0) return "";
            var buffer = new System.Text.StringBuilder((int)size);
            system.GetStringTrackedDeviceProperty( index, property, buffer, size, ref error);
            return buffer.ToString();
        }
    }
}
