using OSCLoader.Core.Hooks;
using OSCLoader.Debug;
using System.Diagnostics;
using VRC.Managers;

namespace OSCLoader.Core.Handlers
{
    /// <summary>
    /// Handles creating UI elements for VRChat using ImGui
    /// </summary>
    public static class GUI
    {
        static bool WasCalledFromOnGUI()
        {
            var stack = new StackTrace();

            foreach (var frame in stack.GetFrames())
            {
                var method = frame.GetMethod();
                if (method == null) continue;

                if (method.Name == "OnGUIAll" && method.DeclaringType == typeof(VRCModsManager))
                {
                    return true;
                }
            }

            return false;
        }

        internal static bool imGuiHookHappened;
        /// <summary>
        /// Creates a UI window using specific parameters<br />
        /// Must be called in OnGUI
        /// </summary>
        public static void Create(ref GUIData data)
        {
            if (ImGuiWindowRendererHook.windowsToRender.Contains(data))
            {
                Logging.Error($"Failed to add GUI: {data.Title}], instance has already been registered!");
                return;
            }

            if (imGuiHookHappened)
            {
                Logging.Error($"Failed to add GUI: {data.Title}], ImGui hook has already happened!");
                return;
            }

            if (!WasCalledFromOnGUI())
            {
                Logging.Error($"Failed to add GUI: {data.Title}], GUI.Create can only be called in OnGUI!");
                return;
            }

            ImGuiWindowRendererHook.windowsToRender.Add(data);
        }

        /// <summary>
        /// GUI data used for creating UI windows
        /// </summary>
        public class GUIData
        {
            /// <summary>
            /// A UI element created with GUIData
            /// </summary>
            public class Element
            {
                internal enum ElementType
                {
                    Text,
                    Button,
                    Checkbox,
                    InputText,
                    InputInt,
                    InputFloat,
                    SliderInt,
                    SliderFloat,
                    ProgressBar,
                }

                internal ElementType Type;
                /// <summary>
                /// The content associated with this element
                /// </summary>
                public string Content = "";
                /// <summary>
                /// The callback associated with this element
                /// </summary>
                public Action Callback = NoMethod;
                /// <summary>
                /// The string value associated with this element
                /// </summary>
                public string StringValue = "";
                internal List<string> InternalStringValues = new List<string>();
                /// <summary>
                /// The float value assocciated with this element
                /// </summary>
                public float FloatValue;
                internal List<float> InternalFloatValues = new List<float>();
                /// <summary>
                /// The int value associated with this element
                /// </summary>
                public int IntValue;
                internal List<int> InternalIntValues = new List<int>();
                /// <summary>
                /// The bool value associated with this element
                /// </summary>
                public bool BoolValue;
                internal List<bool> InternalBoolValues = new List<bool>();

                internal static void NoMethod() { }
            }

            /// <summary>
            /// Creates text visible to the client
            /// </summary>
            /// <param name="content">The text's content</param>
            /// <returns>The element instance</returns>
            public Element Text(string content)
            {
                Element element = new Element
                {
                    Type = Element.ElementType.Text,
                    Content = content
                };

                Elements.Add(element);
                return element;
            }

            /// <summary>
            /// Creates a button that can be pressed
            /// </summary>
            /// <param name="content">Text that appears on the button</param>
            /// <param name="callback">The method callback once this button is pressed</param>
            /// <returns>The element instance</returns>
            public Element Button(string content, Action callback)
            {
                Element element = new Element
                {
                    Type = Element.ElementType.Button,
                    Content = content,
                    Callback = callback
                };

                Elements.Add(element);
                return element;
            }

            /// <summary>
            /// Creates a checkbox that can be toggled and changes BoolValue
            /// </summary>
            /// <param name="content">Text alongside the input</param>
            /// <returns>The element instance</returns>
            public Element Checkbox(string content)
            {
                Element element = new Element
                {
                    Type = Element.ElementType.Checkbox,
                    Content = content,
                };

                Elements.Add(element);
                return element;
            }

            /// <summary>
            /// Creates an input field that changes StringValue
            /// </summary>
            /// <param name="label">Text alongside the input</param>
            /// <param name="value">The default value of the input</param>
            /// <param name="maxCharacters">How many characters can be set</param>
            /// <returns>The element instance</returns>
            public Element InputText(string label, string value = "", int maxCharacters = 256)
            {
                Element element = new Element
                {
                    Type = Element.ElementType.InputText,
                    Content = label,
                    StringValue = value,
                };

                element.InternalIntValues.Add(maxCharacters);
                Elements.Add(element);
                return element;
            }

            /// <summary>
            /// Creates an input field that changes IntValue
            /// </summary>
            /// <param name="label">Text alongside the input</param>
            /// <param name="value">The default value of the input</param>
            /// <param name="step">How much the input changes by</param>
            /// <returns>The element instance</returns>
            public Element InputInt(string label, int value = 0, int step = 1)
            {
                Element element = new Element
                {
                    Type = Element.ElementType.InputInt,
                    Content = label,
                    IntValue = value,
                };

                element.InternalIntValues.Add(step);
                Elements.Add(element);
                return element;
            }

            /// <summary>
            /// Creates an input field that changes FloatValue
            /// </summary>
            /// <param name="label">Text alongside the input</param>
            /// <param name="value">The default value of the input</param>
            /// <param name="step">How much the input changes by</param>
            /// <returns>The element instance</returns>
            public Element InputFloat(string label, float value = 0, float step = 1)
            {
                Element element = new Element
                {
                    Type = Element.ElementType.InputFloat,
                    Content = label,
                    FloatValue = value,
                };

                element.InternalFloatValues.Add(step);
                Elements.Add(element);
                return element;
            }

            /// <summary>
            /// Creates a slider that changes IntValue
            /// </summary>
            /// <param name="label">Text alongside the slider</param>
            /// <param name="value">The default value of the slider</param>
            /// <param name="min">The minimum value the slider can go to</param>
            /// <param name="max">The max value the slider can go to</param>
            /// <returns>The element instance</returns>
            public Element SliderInt(string label, int value, int min, int max)
            {
                Element element = new Element
                {
                    Type = Element.ElementType.SliderInt,
                    Content = label,
                    IntValue = value,
                };

                element.InternalIntValues.Add(min);
                element.InternalIntValues.Add(max);
                Elements.Add(element);
                return element;
            }

            /// <summary>
            /// Creates a slider that changes FloatValue
            /// </summary>
            /// <param name="label">Text alongside the slider</param>
            /// <param name="value">The default value of the slider</param>
            /// <param name="min">The minimum value the slider can go to</param>
            /// <param name="max">The max value the slider can go to</param>
            /// <returns>The element instance</returns>
            public Element SliderFloat(string label, float value, float min, float max)
            {
                Element element = new Element
                {
                    Type = Element.ElementType.SliderFloat,
                    Content = label,
                    FloatValue = value,
                };

                element.InternalFloatValues.Add(min);
                element.InternalFloatValues.Add(max);
                Elements.Add(element);
                return element;
            }

            /// <summary>
            /// Creates a progress bar
            /// </summary>
            /// <param name="value">The default value of the progress bar</param>
            /// <returns>The element instance</returns>
            public Element ProgressBar(float value = 0)
            {
                Element element = new Element
                {
                    Type = Element.ElementType.SliderFloat,
                    FloatValue = value,
                };

                Elements.Add(element);
                return element;
            }

            /// <summary>
            /// Creates an instance of GUIData
            /// </summary>
            /// <param name="title">The title of your UI</param>
            /// <param name="flags">Optional flags for the UI</param>
            public GUIData(string title, WindowFlags flags = WindowFlags.None)
            {
                Title = title;
                Flags = flags;
            }

            /// <summary>
            /// The title of the UI
            /// </summary>
            public string Title;
            /// <summary>
            /// Value that states if the UI is open
            /// </summary>
            public bool Open = true;
            /// <summary>
            /// Window flags that were given upon creating the data
            /// </summary>
            public WindowFlags Flags { get; internal set; }
            /// <summary>
            /// List of elements your UI data contains
            /// </summary>
            public List<Element> Elements = new List<Element>();

            /// <summary>
            /// Flags for your UI window
            /// </summary>
            [Flags]
            public enum WindowFlags
            {
                /// <summary>
                /// No effect
                /// </summary>
                None = 0,
                /// <summary>
                /// Hides the title bar
                /// </summary>
                NoTitleBar = 1,
                /// <summary>
                /// Does not allow resizing
                /// </summary>
                NoResize = 2,
                /// <summary>
                /// Does not allow movement
                /// </summary>
                NoMove = 4,
                /// <summary>
                /// Does not have a scrollbar
                /// </summary>
                NoScrollbar = 8,
                /// <summary>
                /// Disables scrolling with mouse
                /// </summary>
                NoScrollWithMouse = 0x10,
                /// <summary>
                /// Disables collapsing
                /// </summary>
                NoCollapse = 0x20,
                /// <summary>
                /// Always auto-resizes
                /// </summary>
                AlwaysAutoResize = 0x40,
                /// <summary>
                /// Hides background
                /// </summary>
                NoBackground = 0x80,
                /// <summary>
                /// Disables saving to settings
                /// </summary>
                NoSavedSettings = 0x100,
                /// <summary>
                /// Disables mouse inputs
                /// </summary>
                NoMouseInputs = 0x200,
                /// <summary>
                /// Adds menu bar
                /// </summary>
                MenuBar = 0x400,
                /// <summary>
                /// Adds horizontal scrollbar
                /// </summary>
                HorizontalScrollbar = 0x800,
                /// <summary>
                /// Doesn't focus on appearing
                /// </summary>
                NoFocusOnAppearing = 0x1000,
                /// <summary>
                /// Doesn't bring to front on focused
                /// </summary>
                NoBringToFrontOnFocus = 0x2000,
                /// <summary>
                /// Always uses vertical scrollbar
                /// </summary>
                AlwaysVerticalScrollbar = 0x4000,
                /// <summary>
                /// Always uses horizontal scrollbar
                /// </summary>
                AlwaysHorizontalScrollbar = 0x8000,
                /// <summary>
                /// Disables navigation inputs
                /// </summary>
                NoNavInputs = 0x10000,
                /// <summary>
                /// Disables navigation focus
                /// </summary>
                NoNavFocus = 0x20000,
                /// <summary>
                /// Doesn't save document
                /// </summary>
                UnsavedDocument = 0x40000,
                /// <summary>
                /// Doesn't dock
                /// </summary>
                NoDocking = 0x80000,
                /// <summary>
                /// Disables navigation
                /// </summary>
                NoNav = 0x30000,
                /// <summary>
                /// Disables decoration
                /// </summary>
                NoDecoration = 0x2B,
                /// <summary>
                /// Disables inputs
                /// </summary>
                NoInputs = 0x30200,
                /// <summary>
                /// Navigation is flattened
                /// </summary>
                NavFlattened = 0x800000,
                /// <summary>
                /// Sets UI a child window
                /// </summary>
                ChildWindow = 0x1000000,
                /// <summary>
                /// Has tooltip
                /// </summary>
                Tooltip = 0x2000000,
                /// <summary>
                /// Function as a Popup
                /// </summary>
                Popup = 0x4000000,
                /// <summary>
                /// Sets UI to modal
                /// </summary>
                Modal = 0x8000000,
                /// <summary>
                /// Sets UI to a child menu
                /// </summary>
                ChildMenu = 0x10000000,
                /// <summary>
                /// Dock Node is host
                /// </summary>
                DockNodeHost = 0x20000000
            }
        }
    }
}
