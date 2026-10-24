using OSCLoader.Debug;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;

namespace VRC.Modding
{
    /// <summary>
    /// World Interaction for worlds that support Mod to VRChat communication.
    /// </summary>
    public static class WorldInteraction
    {
        static HttpListener? listener;
        public static List<EventData> AllEvents { get; internal set; } = new List<EventData>();
        public struct EventData
        {
            public Guid Id { get; internal set; }
            public string Name { get; internal set; }
            public string Data { get; internal set; }
            
            public static EventData Null = new EventData();
        }

        internal static void StartConnection()
        {
            if (listener == null)
            {
                listener = new HttpListener();
                listener.Prefixes.Add("http://localhost:5000/");
                listener.Start();
            }

            Supported = true;
            Task.Run(LocalHostListener);
        }

        internal static void CloseConnection()
        {
            if (listener != null)
            {
                listener.Stop();
                listener = null;
            }

            Supported = false;
        }

        internal static async void LocalHostListener()
        {
            while (Supported)
            {
                HttpListenerContext context = await listener.GetContextAsync();

                Stopwatch stopwatch = Stopwatch.StartNew();

                string response;

                if (context.Request.Url?.AbsolutePath == "/oscloader")
                {
                    response = "";
                }
                else
                {
                    context.Response.StatusCode = 404;
                    response = "Not Found";
                }

                stopwatch.Stop();

                long responseTimeMs = stopwatch.ElapsedMilliseconds;

                string json = GetJsonData();

                byte[] buffer = Encoding.UTF8.GetBytes(json);

                context.Response.ContentType = "application/json";
                context.Response.ContentLength64 = buffer.Length;

                await context.Response.OutputStream.WriteAsync(buffer);
                context.Response.OutputStream.Close();
            }
        }

        public static string GetJsonData()
        {
            Dictionary<string, object> events = new Dictionary<string, object>();

            foreach (EventData _event in AllEvents)
            {
                events[_event.Id.ToString()] = new
                {
                    Name = _event.Name,
                    Data = _event.Data
                };
            }

            return JsonSerializer.Serialize(events, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        internal static void Internal_SendData(string function, string data = "")
        {
            if (!Supported)
            {
                Logging.Error("World does not support Interaction.");
                return;
            }

            EventData _event = new EventData
            {
                Id = Guid.NewGuid(),
                Name = function,
                Data = data
            };
            AllEvents.Add(_event);
        }

        internal static void RemoveEvent(Guid eventId)
        {
            foreach (EventData _event in AllEvents)
            {
                if (eventId == _event.Id)
                {
                    AllEvents.Remove(_event);
                    break;
                }
            }
        }

        /// <summary>
        /// States whether the world supports Interaction.
        /// </summary>
        public static bool Supported { get; internal set; }

        /// <summary>
        /// Allows you to respawn the local client.
        /// </summary>
        public static void Respawn() => Internal_SendData("INTERNAL_Respawn");
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
        /// Allows you to teleport the local client.
        /// </summary>
        /// <param name="x">The new X position.</param>
        /// <param name="y">The new Y position.</param>
        /// <param name="z">The new Z position.</param>
        public static void TeleportTo(float x, float y, float z) => Internal_SendData("INTERNAL_Teleport", $"{x},{y},{z}");
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
