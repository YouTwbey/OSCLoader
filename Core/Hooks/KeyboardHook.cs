using OSCLoader.Debug;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OSCLoader.Core.Hooks
{
    internal static class KeyboardHook
    {
        public static event Action<int, bool>? KeyEvent;

        private static IntPtr _hook;
        private static HookProc _proc = HookCallback;

        public static void Install()
        {
            using var curProcess = Process.GetCurrentProcess();
            using var curModule = curProcess.MainModule!;
            _hook = SetWindowsHookEx(
                13, // WH_KEYBOARD_LL
                _proc,
                GetModuleHandle(curModule.ModuleName),
                0
            );
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                bool isDown = wParam == 0x0100 || wParam == 0x0104;
                bool isUp = wParam == 0x0101 || wParam == 0x0105;

                if (isDown || isUp)
                    KeyEvent?.Invoke(vkCode, isDown);
            }

            return CallNextHookEx(_hook, nCode, wParam, lParam);
        }

        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
