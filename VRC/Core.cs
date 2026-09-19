using OSCLoader.Core.Handlers;
using OSCLoader.Core.Hooks;
using OSCLoader.Debug;
using System.Diagnostics;
using System.Runtime.InteropServices;
using VRC.Managers;

namespace VRC
{
    /// <summary>
    /// Modify root features of VRChat
    /// </summary>
    public static class Core
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool SetWindowText(IntPtr hWnd, string lpString);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        static readonly IntPtr HWND_TOP = IntPtr.Zero;
        const uint SWP_NOMOVE = 0x0002;
        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;
        const uint SWP_SHOWWINDOW = 0x0040;

        /// <summary>
        /// VRChat's Process
        /// </summary>
        public static Process? VRChat;
        internal static Process? EasyAntiCheat_Installer;
        internal static Process? EasyAntiCheat_Startup;
        internal static ImGuiWindowRendererHook? ImGuiWindow;
        internal static Process? ImGuiProcess;

        /// <summary>
        /// Bool to allow VRChat's logs to be put into OSCLoader's console.<br />
        /// True by default.
        /// </summary>
        public static bool AllowLogging = true;

        internal static void UpdateImGuiPosition()
        {
            if (ImGuiWindow == null)
            {
                return;
            }
        }

        public static void HandleUIUpdates()
        {
            while (true)
            {
                ImGuiWindow.UpdateUIPosition();
                Thread.Sleep(1);
            }
        }

        internal static void PerformHook(Process processToHook)
        {
            try
            {
                VRChat = processToHook;
                Logging.Log("Setting up UnityEngine.Input emulation...");
                KeyboardHook.Install();
                Input.Start();
                Logging.Log("UnityEngine.Input emulation setup complete!");
                Logging.Log("Setting up ImGui...");

                VRCModsManager.OnGUIAll();

                if (ImGuiWindowRendererHook.windowsToRender.Count != 0)
                {
                    ImGuiWindow = new ImGuiWindowRendererHook();
                    Thread ImGuiThread = new Thread(ImGuiWindow.Start().Wait);
                    ImGuiThread.SetApartmentState(ApartmentState.STA);
                    ImGuiThread.IsBackground = true;
                    ImGuiThread.Start();
                    new Thread(HandleUIUpdates).Start();
                }
                else
                {
                    Logging.Log("No UI to render! Skipping ImGui setup...");
                }

                Logging.Log("Setting up Media Listener...");
                _ = Device.RetrieveMediaSnapshot();
                Logging.Log("Media Listener set up!");

                Logging.Log("Hook successful!");
            }
            catch (Exception e)
            {
                Logging.Error($"Failed to hook onto VRChat process! {e.ToString()}");
            }
        }

        /// <summary>
        /// Change VRChat's window title
        /// </summary>
        /// <param name="newTitle">The new title that will be displayed</param>
        public static void ChangeWindowTitle(string newTitle)
        {
            if (VRChat == null)
            {
                Logging.Warn("Cannot call ChangeWindowTitle due to VRChat currently not being hooked!");
                return;
            }
            
            SetWindowText(VRChat.MainWindowHandle, newTitle);
        }

        /// <summary>
        /// Sets position of VRChat's window
        /// </summary>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        public static void MoveWindow(int x, int y)
        {
            if (VRChat == null)
            {
                Logging.Warn("Cannot call MoveWindow due to VRChat currently not being hooked!");
                return;
            }

            SetWindowPos(VRChat.Handle, HWND_TOP, x, y, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW);
        }

        /// <summary>
        /// Sets size of VRChat's window
        /// </summary>
        /// <param name="width">Width of Window</param>
        /// <param name="height">Height of Window</param>
        public static void ResizeWindow(int width, int height)
        {
            if (VRChat == null)
            {
                Logging.Warn("Cannot call ResizeWindow due to VRChat currently not being hooked!");
                return;
            }

            SetWindowPos(VRChat.Handle, HWND_TOP, 0, 0, width, height, SWP_NOMOVE | SWP_NOZORDER | SWP_SHOWWINDOW);
        }

        /// <summary>
        /// Sets position and size of VRChat's window
        /// </summary>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        /// <param name="width">Width of Window</param>
        /// <param name="height">Height of Window</param>
        public static void SetWindowBounds(int x, int y, int width, int height)
        {
            if (VRChat == null)
            {
                Logging.Warn("Cannot call SetWindowBounds due to VRChat currently not being hooked!");
                return;
            }

            MoveWindow(VRChat.Handle, x, y, width, height, true);
        }

        /// <summary>
        /// Brings VRChat window to front
        /// </summary>
        public static void BringWindowFront()
        {
            if (VRChat == null)
            {
                Logging.Warn("Cannot call BringWindowFront due to VRChat currently not being hooked!");
                return;
            }

            SetWindowPos(VRChat.Handle, HWND_TOP, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
        }

        /// <summary>
        /// States whether the VRChat is in focus
        /// </summary>
        /// <returns>True if in focus</returns>
        public static bool IsFocused()
        {
            if (VRChat == null) return false;

            IntPtr foreground = GetForegroundWindow();
            GetWindowThreadProcessId(foreground, out uint pid);

            if (pid == VRChat.Id) return true; else return false;
        }
    }
}
