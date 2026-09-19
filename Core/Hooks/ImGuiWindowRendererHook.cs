using ClickableTransparentOverlay;
using ImGuiNET;
using OSCLoader.Core.Handlers;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static VRC.Core;

namespace OSCLoader.Core.Hooks
{
    internal class ImGuiWindowRendererHook : Overlay
    {
        internal static List<GUI.GUIData> windowsToRender = new List<GUI.GUIData>();
        const uint SWP_NOZORDER = 0x0004;
        const uint SWP_SHOWWINDOW = 0x0040;

        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int Left, Top, Right, Bottom;
        }
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        bool firstTime = true;

        const int GWL_EXSTYLE = -20;
        const int WS_EX_TOOLWINDOW = 0x00000080;
        const int WS_EX_APPWINDOW = 0x00040000;

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public static void HideFromTaskbar(IntPtr hWnd)
        {
            int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);

            exStyle &= ~WS_EX_APPWINDOW;
            exStyle |= WS_EX_TOOLWINDOW;

            SetWindowLong(hWnd, GWL_EXSTYLE, exStyle);
        }

        public void UpdateUIPosition()
        {
            if (ImGuiProcess == null) ImGuiProcess = Process.GetCurrentProcess();

            if (firstTime)
            {
                ForceHideWindow();
            }

            if (VRChat == null || VRChat.MainWindowHandle == IntPtr.Zero) return;
            if (!GetWindowRect(VRChat.MainWindowHandle, out RECT rect)) return;

            int x = rect.Left;
            int y = rect.Top;
            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;

            IntPtr foreground = GetForegroundWindow();

            if (foreground == VRChat.MainWindowHandle || foreground == ImGuiProcess.MainWindowHandle)
            {
                ShowWindow(ImGuiProcess.MainWindowHandle, SW_SHOW);
                HideFromTaskbar(ImGuiProcess.MainWindowHandle);
                SetWindowPos(ImGuiProcess.MainWindowHandle, VRChat.MainWindowHandle, x, y, width, height, SWP_NOZORDER | SWP_SHOWWINDOW);
            }
            else
            {
                ShowWindow(ImGuiProcess.MainWindowHandle, SW_HIDE);
            }
        }

        public void ForceHideWindow()
        {
            if (ImGuiProcess == null) ImGuiProcess = Process.GetCurrentProcess();
            if (ImGuiProcess == null) return;
            if (GetForegroundWindow() != ImGuiProcess.MainWindowHandle) return;

            HideFromTaskbar(ImGuiProcess.MainWindowHandle);
            ShowWindow(ImGuiProcess.MainWindowHandle, SW_HIDE);
            firstTime = false;
        }

        protected override void Render()
        {
            foreach (GUI.GUIData window in windowsToRender)
            {
                ImGui.Begin(window.Title, ref window.Open, (ImGuiWindowFlags)window.Flags);

                foreach (GUI.GUIData.Element element in window.Elements)
                {
                    switch (element.Type)
                    {
                        case GUI.GUIData.Element.ElementType.Text:
                            ImGui.Text(element.Content);
                            break;
                        case GUI.GUIData.Element.ElementType.Button:
                            if (ImGui.Button(element.Content))
                            {
                                element.Callback.Invoke();
                            }
                            break;
                        case GUI.GUIData.Element.ElementType.Checkbox:
                            ImGui.Checkbox(element.Content, ref element.BoolValue);
                            break;
                        case GUI.GUIData.Element.ElementType.InputText:
                            ImGui.InputText(element.Content, ref element.StringValue, (uint)element.InternalIntValues[0]);
                            break;
                        case GUI.GUIData.Element.ElementType.InputInt:
                            ImGui.InputInt(element.Content, ref element.IntValue, element.InternalIntValues[0]);
                            break;
                        case GUI.GUIData.Element.ElementType.InputFloat:
                            ImGui.InputFloat(element.Content, ref element.FloatValue, element.InternalFloatValues[0]);
                            break;
                        case GUI.GUIData.Element.ElementType.SliderInt:
                            ImGui.SliderInt(element.Content, ref element.IntValue, element.InternalIntValues[0], element.InternalIntValues[1]);
                            break;
                        case GUI.GUIData.Element.ElementType.SliderFloat:
                            ImGui.SliderFloat(element.Content, ref element.FloatValue, element.InternalFloatValues[0], element.InternalFloatValues[1]);
                            break;
                        case GUI.GUIData.Element.ElementType.ProgressBar:
                            ImGui.ProgressBar(element.FloatValue);
                            break;

                    }
                }

                ImGui.End();
            }
        }
    }
}
