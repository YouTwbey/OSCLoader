using System.Runtime.InteropServices;
using System.Diagnostics;
using OSCLoader.Core.Hooks;
using OSCLoader.Debug;

namespace OSCLoader.Core.Handlers
{
    /// <summary>
    /// Track keyboard inputs when tabbed into VRChat
    /// </summary>
    public static class Input
    {
        static readonly HashSet<int> _current = new();
        static readonly HashSet<int> _previous = new();

        internal static void Start()
        {
            KeyboardHook.KeyEvent += (key, down) =>
            {
                lock (_current)
                {
                    if (down)
                    {
                        if (_current.Contains(key))
                            return;

                        _current.Add(key);
                    }
                    else
                    {
                        if (!_current.Contains(key))
                            return;

                        _current.Remove(key);
                    }
                }
            };
        }

        internal static void Update()
        {
            lock (_current)
            {
                _previous.Clear();
                foreach (var k in _current)
                    _previous.Add(k);
            }
        }

        /// <summary>
        /// Gets the current state of a key given
        /// </summary>
        /// <param name="key">The KeyCode</param>
        /// <returns>True if the key is held down</returns>
        public static bool GetKey(KeyCode key)
        {
            if (!VRC.Core.IsFocused()) return false;

            return _current.Contains((int)key);
        }

        /// <summary>
        /// Checks if the key given was pressed this frame
        /// </summary>
        /// <param name="key">The KeyCode</param>
        /// <returns>True if the key was pressed this frame</returns>
        public static bool GetKeyDown(KeyCode key)
        {
            if (!VRC.Core.IsFocused()) return false;

            return _current.Contains((int)key) && !_previous.Contains((int)key);
        }

        /// <summary>
        /// Checks if the key given was released this frame
        /// </summary>
        /// <param name="key">The KeyCode</param>
        /// <returns>True if the key was released this frame</returns>
        public static bool GetKeyUp(KeyCode key)
        {
            if (!VRC.Core.IsFocused()) return false;

            return !_current.Contains((int)key) && _previous.Contains((int)key);
        }
    }

    public enum KeyCode
    {
        None = 0,

        // Letters
        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,

        // Numbers (top row)
        Alpha0 = 0x30,
        Alpha1 = 0x31,
        Alpha2 = 0x32,
        Alpha3 = 0x33,
        Alpha4 = 0x34,
        Alpha5 = 0x35,
        Alpha6 = 0x36,
        Alpha7 = 0x37,
        Alpha8 = 0x38,
        Alpha9 = 0x39,

        // Function keys
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,

        // Arrows
        UpArrow = 0x26,
        DownArrow = 0x28,
        LeftArrow = 0x25,
        RightArrow = 0x27,

        // Modifiers
        LeftShift = 0xA0,
        RightShift = 0xA1,
        LeftControl = 0xA2,
        RightControl = 0xA3,
        LeftAlt = 0xA4,
        RightAlt = 0xA5,

        // Common keys
        Space = 0x20,
        Enter = 0x0D,
        Escape = 0x1B,
        Tab = 0x09,
        Backspace = 0x08,
        CapsLock = 0x14,

        // Navigation
        Insert = 0x2D,
        Delete = 0x2E,
        Home = 0x24,
        End = 0x23,
        PageUp = 0x21,
        PageDown = 0x22,

        // Numpad
        Keypad0 = 0x60,
        Keypad1 = 0x61,
        Keypad2 = 0x62,
        Keypad3 = 0x63,
        Keypad4 = 0x64,
        Keypad5 = 0x65,
        Keypad6 = 0x66,
        Keypad7 = 0x67,
        Keypad8 = 0x68,
        Keypad9 = 0x69,

        KeypadMultiply = 0x6A,
        KeypadAdd = 0x6B,
        KeypadSubtract = 0x6D,
        KeypadDecimal = 0x6E,
        KeypadDivide = 0x6F,
    }
}
