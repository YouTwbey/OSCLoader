using OSCLoader.Debug;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;

namespace VRC.Managers
{
    /// <summary>
    /// Gives you access to OSC requests
    /// </summary>
    public static class OSCManager
    {
        /// <summary>
        /// Updates the client's chatbox
        /// </summary>
        /// <param name="chatMessage">The chatbox's content</param>
        /// <param name="instantSend">Value to instantly send the message</param>
        /// <param name="playNotification">Value to play a notification sound</param>
        public static void RPC_UpdateChatbox(string chatMessage, bool instantSend, bool playNotification)
        {
            byte[] message = CreateChatboxOSCMessage(chatMessage, instantSend, playNotification);

            if (sender == null)
            {
                Logging.Warn("OSC is not connected!");
                return;
            }

            sender.Send(message);
        }

        /// <summary>
        /// Sends an OSC request to VRChat
        /// </summary>
        /// <param name="address">The address for the OSC request</param>
        /// <param name="value">The value for the OSC request</param>
        public static void SendOSC(string address, float value)
        {
            byte[] message = CreateOSCMessage(address, value);

            if (sender == null)
            {
                Logging.Warn("OSC is not connected!");
                return;
            }

            sender.Send(message);
        }

        /// <summary>
        /// Sends an OSC request to VRChat
        /// </summary>
        /// <param name="addressX">The address for the OSC request for the X value</param>
        /// <param name="addressY">The address for the OSC request for the Y value</param>
        /// <param name="value">The value for the OSC request</param>
        public static void SendOSC(string addressX, string addressY, Vector2 value)
        {
            byte[] message1 = CreateOSCMessage(addressX, value.X);
            byte[] message2 = CreateOSCMessage(addressY, value.Y);

            if (sender == null)
            {
                Logging.Warn("OSC is not connected!");
                return;
            }

            sender.Send(message1);
            sender.Send(message2);
        }

        /// <summary>
        /// Sends an OSC request to VRChat
        /// </summary>
        /// <param name="address">The address for the OSC request</param>
        /// <param name="value">The value for the OSC request</param>
        public static void SendOSC(string address, Vector3 value)
        {
            byte[] message = CreateOSCMessage(address, ",fff", value);

            if (sender == null)
            {
                Logging.Warn("OSC is not connected!");
                return;
            }

            sender.Send(message);
        }

        /// <summary>
        /// Sends an OSC request to VRChat
        /// </summary>
        /// <param name="address">The address for the OSC request</param>
        /// <param name="value">The value for the OSC request</param>
        public static void SendOSC(string address, Vector4 value)
        {
            byte[] message = CreateOSCMessage(address, ",ffff", value);

            if (sender == null)
            {
                Logging.Warn("OSC is not connected!");
                return;
            }

            sender.Send(message);
        }

        /// <summary>
        /// Sends an OSC request to VRChat
        /// </summary>
        /// <param name="address">The address for the OSC request</param>
        /// <param name="value">The value for the OSC request</param>
        public static void SendOSC(string address, bool value)
        {
            byte[] message = CreateOSCMessage(address, value);

            if (sender == null)
            {
                Logging.Warn("OSC is not connected!");
                return;
            }

            sender.Send(message);
        }

        /// <summary>
        /// Sends an OSC request to VRChat
        /// </summary>
        /// <param name="address">The address for the OSC request</param>
        /// <param name="value">The value for the OSC request</param>
        public static void SendOSC(string address, int value)
        {
            byte[] message = CreateOSCMessage(address, value);

            if (sender == null)
            {
                Logging.Warn("OSC is not connected!");
                return;
            }

            sender.Send(message);
        }

        /// <summary>
        /// Sends an OSC request to VRChat
        /// </summary>
        /// <param name="address">The address for the OSC request</param>
        /// <param name="types">The value types for the OSC request</param>
        /// <param name="values">Any values to be passed along with the OSC request, must be in the same order as the value types</param>
        public static void SendOSC(string address = "", string types = "", params object[] values)
        {
            byte[] message = CreateOSCMessage(address, types, values);

            if (sender == null)
            {
                Logging.Warn("OSC is not connected!");
                return;
            }

            sender.Send(message);
        }

        static int sendPort = 9000;
        static int receivePort = 9001;

        static UdpClient sender;
        static UdpClient reader;

        internal static void HandleOSC()
        {
            if (reader == null)
            {
                reader = new UdpClient(receivePort);
            }

            if (sender == null)
            {
                sender = new UdpClient();
                sender.Connect("127.0.0.1", sendPort);
            }

            try
            {
                if (reader.Available > 0)
                {
                    IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = reader.Receive(ref remote);

                    if (data.Length > 0)
                    {
                        ProcessOSCMessage(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Error($"Unexpected OSC error: {ex}");
            }
        }

        static void RunOSCMessageEvents(string address, string value)
        {
            VRCModsManager.OnOSCRecievedAll(address, value);
            bool formatted = value != "0";

            if (address.EndsWith("VelocityX"))
            {
                LocalPlayerBehaviour.VelocityX = float.Parse(value);
            }
            if (address.EndsWith("VelocityY"))
            {
                LocalPlayerBehaviour.VelocityY = float.Parse(value);
            }
            if (address.EndsWith("VelocityZ"))
            {
                LocalPlayerBehaviour.VelocityZ = float.Parse(value);
            }
            if (address.EndsWith("AngularY"))
            {
                LocalPlayerBehaviour.AngularY = float.Parse(value);
            }
            if (address.EndsWith("Grounded"))
            {
                LocalPlayerBehaviour.Grounded = formatted;
            }
            if (address.EndsWith("AFK"))
            {
                LocalPlayerBehaviour.AFK = formatted;
            }
            if (address.EndsWith("VRMode"))
            {
                LocalPlayerBehaviour.VRMode = formatted;
            }
            if (address.EndsWith("MuteSelf"))
            {
                LocalPlayerBehaviour.MuteSelf = formatted;
            }
            if (address.EndsWith("Voice"))
            {
                LocalPlayerBehaviour.Voice = float.Parse(value);
            }
            if (address.EndsWith("Earmuffs"))
            {
                LocalPlayerBehaviour.Earmuffs = formatted;
            }
            if (address.EndsWith("VelocityMagnitude"))
            {
                LocalPlayerBehaviour.VelocityMagnitude = float.Parse(value);
            }
            if (address.EndsWith("ScaleFactor"))
            {
                LocalPlayerBehaviour.ScaleFactor = float.Parse(value);
            }
            if (address.EndsWith("ScaleFactorInverse"))
            {
                LocalPlayerBehaviour.ScaleFactorInverse = float.Parse(value);
            }
            if (address.EndsWith("ScaleModified"))
            {
                LocalPlayerBehaviour.ScaleModified = formatted;
            }
            if (address.EndsWith("EyeHeightAsPercent"))
            {
                LocalPlayerBehaviour.EyeHeightAsPercent = float.Parse(value);
            }
            if (address.EndsWith("EyeHeightAsMeters"))
            {
                LocalPlayerBehaviour.EyeHeightAsMeters = float.Parse(value);
            }
            if (address.EndsWith("GestureLeft"))
            {
                LocalPlayerBehaviour.GestureLeft = int.Parse(value);
            }
            if (address.EndsWith("GestureRight"))
            {
                LocalPlayerBehaviour.GestureRight = int.Parse(value);
            }
            if (address.EndsWith("GestureLeftWeight"))
            {
                LocalPlayerBehaviour.GestureLeftWeight = float.Parse(value);
            }
            if (address.EndsWith("GestureRightWeight"))
            {
                LocalPlayerBehaviour.GestureRightWeight = float.Parse(value);
            }
            if (address.EndsWith("Seated"))
            {
                LocalPlayerBehaviour.Seated = formatted;
            }
            if (address.EndsWith("InStation"))
            {
                LocalPlayerBehaviour.InStation = formatted;
            }
            if (address.EndsWith("VRCEmote"))
            {
                LocalPlayerBehaviour.VRCEmote = int.Parse(value);
            }
            if (address.EndsWith("VRCFaceBlendH"))
            {
                LocalPlayerBehaviour.VRCFaceBlendH = float.Parse(value);
            }
            if (address.EndsWith("VRCFaceBlendV"))
            {
                LocalPlayerBehaviour.VRCFaceBlendV = float.Parse(value);
            }
        }

        static void ProcessOSCMessage(byte[] data)
        {
            int index = 0;
            string address = DecodeString(data, index, out index);
            string typeTag = DecodeString(data, index, out index);


            switch (typeTag)
            {
                case ",f": // float
                    float flt = DecodeFloat(data, index);
                    RunOSCMessageEvents(address, flt.ToString());
                    break;
                case ",i": // int
                    int num = DecodeInt(data, index);
                    RunOSCMessageEvents(address, num.ToString());
                    break;
                case ",T": // true
                    RunOSCMessageEvents(address, "true");
                    break;
                case ",F": // false
                    RunOSCMessageEvents(address, "false");
                    break;
            }
        }

        static byte[] CreateOSCMessage(string address, string types, params object[] values)
        {
            var addressBytes = EncodeString(address);
            var typeTagBytes = EncodeString("," + types);

            List<byte> valueBytesList = new List<byte>();
            foreach (var value in values)
            {
                if (value is int intValue)
                {
                    var bytes = BitConverter.GetBytes(intValue);
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(bytes);
                    }
                    valueBytesList.AddRange(bytes);
                }
                else if (value is float floatValue)
                {
                    var bytes = BitConverter.GetBytes(floatValue);
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(bytes);
                    }
                    valueBytesList.AddRange(bytes);
                }
                else if (value is string stringValue)
                {
                    var bytes = EncodeString(stringValue);
                    valueBytesList.AddRange(bytes);
                }
                else if (value is bool boolValue)
                {
                    var bytes = BitConverter.GetBytes(boolValue);
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(bytes);
                    }
                    valueBytesList.AddRange(bytes);
                }
                else if (value is Vector2 v2)
                {
                    var X2bytes = BitConverter.GetBytes(v2.X);
                    var Y2bytes = BitConverter.GetBytes(v2.Y);
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(X2bytes);
                        Array.Reverse(Y2bytes);
                    }
                    valueBytesList.AddRange(X2bytes);
                    valueBytesList.AddRange(Y2bytes);
                }
                else if (value is Vector3 v3)
                {
                    var X3bytes = BitConverter.GetBytes(v3.X);
                    var Y3bytes = BitConverter.GetBytes(v3.Y);
                    var Z3bytes = BitConverter.GetBytes(v3.Z);
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(X3bytes);
                        Array.Reverse(Y3bytes);
                        Array.Reverse(Z3bytes);
                    }
                    valueBytesList.AddRange(X3bytes);
                    valueBytesList.AddRange(Y3bytes);
                    valueBytesList.AddRange(Z3bytes);
                }
                else if (value is Vector4 v4)
                {
                    var X4bytes = BitConverter.GetBytes(v4.X);
                    var Y4bytes = BitConverter.GetBytes(v4.Y);
                    var Z4bytes = BitConverter.GetBytes(v4.Z);
                    var W4bytes = BitConverter.GetBytes(v4.Z);
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(X4bytes);
                        Array.Reverse(Y4bytes);
                        Array.Reverse(Z4bytes);
                        Array.Reverse(W4bytes);
                    }
                    valueBytesList.AddRange(X4bytes);
                    valueBytesList.AddRange(Y4bytes);
                    valueBytesList.AddRange(Z4bytes);
                    valueBytesList.AddRange(W4bytes);
                }
                else
                {
                    Logging.Error($"Unsupported value type: {value.GetType()}");
                }
            }

            var valueBytes = valueBytesList.ToArray();
            byte[] message = new byte[addressBytes.Length + typeTagBytes.Length + valueBytes.Length];
            Buffer.BlockCopy(addressBytes, 0, message, 0, addressBytes.Length);
            Buffer.BlockCopy(typeTagBytes, 0, message, addressBytes.Length, typeTagBytes.Length);
            Buffer.BlockCopy(valueBytes, 0, message, addressBytes.Length + typeTagBytes.Length, valueBytes.Length);

            return message;
        }

        static byte[] CreateOSCMessage(string address, Vector4 value)
        {
            var addressBytes = EncodeString(address);
            var typeTagBytes = EncodeString(",ffff");
            byte[] xBytes = BitConverter.GetBytes(value.X);
            byte[] yBytes = BitConverter.GetBytes(value.Y);
            byte[] zBytes = BitConverter.GetBytes(value.Z);
            byte[] wBytes = BitConverter.GetBytes(value.W);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(xBytes);
                Array.Reverse(yBytes);
                Array.Reverse(zBytes);
                Array.Reverse(wBytes);
            }

            byte[] message = new byte[addressBytes.Length + typeTagBytes.Length + xBytes.Length + yBytes.Length + zBytes.Length];
            int offset = 0;

            Buffer.BlockCopy(addressBytes, 0, message, offset, addressBytes.Length);
            offset += addressBytes.Length;

            Buffer.BlockCopy(typeTagBytes, 0, message, offset, typeTagBytes.Length);
            offset += typeTagBytes.Length;

            Buffer.BlockCopy(xBytes, 0, message, offset, xBytes.Length);
            offset += xBytes.Length;

            Buffer.BlockCopy(yBytes, 0, message, offset, yBytes.Length);
            offset += yBytes.Length;

            Buffer.BlockCopy(zBytes, 0, message, offset, zBytes.Length);
            offset += zBytes.Length;

            Buffer.BlockCopy(zBytes, 0, message, offset, wBytes.Length);

            return message;
        }

        static byte[] CreateOSCMessage(string address, Vector3 value)
        {
            var addressBytes = EncodeString(address);
            var typeTagBytes = EncodeString(",fff");
            byte[] xBytes = BitConverter.GetBytes(value.X);
            byte[] yBytes = BitConverter.GetBytes(value.Y);
            byte[] zBytes = BitConverter.GetBytes(value.Z);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(xBytes);
                Array.Reverse(yBytes);
                Array.Reverse(zBytes);
            }

            byte[] message = new byte[addressBytes.Length + typeTagBytes.Length + xBytes.Length + yBytes.Length + zBytes.Length];
            int offset = 0;

            Buffer.BlockCopy(addressBytes, 0, message, offset, addressBytes.Length);
            offset += addressBytes.Length;

            Buffer.BlockCopy(typeTagBytes, 0, message, offset, typeTagBytes.Length);
            offset += typeTagBytes.Length;

            Buffer.BlockCopy(xBytes, 0, message, offset, xBytes.Length);
            offset += xBytes.Length;

            Buffer.BlockCopy(yBytes, 0, message, offset, yBytes.Length);
            offset += yBytes.Length;

            Buffer.BlockCopy(zBytes, 0, message, offset, zBytes.Length);

            return message;
        }

        static byte[] CreateOSCMessage(string address, float value)
        {
            var addressBytes = EncodeString(address);
            var typeTagBytes = EncodeString(",f");
            var valueBytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(valueBytes);
            }

            byte[] message = new byte[addressBytes.Length + typeTagBytes.Length + valueBytes.Length];
            Buffer.BlockCopy(addressBytes, 0, message, 0, addressBytes.Length);
            Buffer.BlockCopy(typeTagBytes, 0, message, addressBytes.Length, typeTagBytes.Length);
            Buffer.BlockCopy(valueBytes, 0, message, addressBytes.Length + typeTagBytes.Length, valueBytes.Length);

            return message;
        }

        static byte[] CreateOSCMessage(string address, int value)
        {
            var addressBytes = EncodeString(address);
            var typeTagBytes = EncodeString(",i");
            var valueBytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(valueBytes);
            }

            byte[] message = new byte[addressBytes.Length + typeTagBytes.Length + valueBytes.Length];
            Buffer.BlockCopy(addressBytes, 0, message, 0, addressBytes.Length);
            Buffer.BlockCopy(typeTagBytes, 0, message, addressBytes.Length, typeTagBytes.Length);
            Buffer.BlockCopy(valueBytes, 0, message, addressBytes.Length + typeTagBytes.Length, valueBytes.Length);

            return message;
        }

        static byte[] CreateChatboxOSCMessage(string message, bool instantSend, bool playNotification)
        {
            // Encode the OSC address for the chatbox input
            var addressBytes = EncodeString("/chatbox/input");

            string tags = ",s";
            if (instantSend)
            {
                tags += "T";
            }
            else
            {
                tags += "F";
            }
            if (playNotification)
            {
                tags += "T";
            }
            else
            {
                tags += "F";
            }
            // Prepare type tags for the parameters (message string, instantSend, playNotification)
            var typeTagBytes = EncodeString(tags); // "s" = string, "f" = float (1.0 for true, 0.0 for false)

            // Encode the string message
            var messageBytes = EncodeString(message);

            // Convert boolean flags to float values (1.0 for true, 0.0 for false)
            var instantSendValue = BitConverter.GetBytes(instantSend ? 1.0f : 0.0f);
            var playNotificationValue = BitConverter.GetBytes(playNotification ? 1.0f : 0.0f);

            // Ensure correct endianness
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(instantSendValue);
                Array.Reverse(playNotificationValue);
            }

            // Calculate total message size
            var totalSize = addressBytes.Length + typeTagBytes.Length + messageBytes.Length +
                            instantSendValue.Length + playNotificationValue.Length;

            // Allocate the byte array for the complete OSC message
            var oscMessage = new byte[totalSize];

            // Copy data into the OSC message byte array
            int offset = 0;
            Buffer.BlockCopy(addressBytes, 0, oscMessage, offset, addressBytes.Length);
            offset += addressBytes.Length;

            Buffer.BlockCopy(typeTagBytes, 0, oscMessage, offset, typeTagBytes.Length);
            offset += typeTagBytes.Length;

            Buffer.BlockCopy(messageBytes, 0, oscMessage, offset, messageBytes.Length);
            offset += messageBytes.Length;

            Buffer.BlockCopy(instantSendValue, 0, oscMessage, offset, instantSendValue.Length);
            offset += instantSendValue.Length;

            Buffer.BlockCopy(playNotificationValue, 0, oscMessage, offset, playNotificationValue.Length);

            return oscMessage;
        }

        static byte[] CreateOSCMessage(string address, bool value)
        {
            var addressBytes = EncodeString(address);
            byte[] typeTagBytes;
            if (value == true)
            {
                typeTagBytes = EncodeString(",T");
            }
            else
            {
                typeTagBytes = EncodeString(",F");
            }
            var valueBytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(valueBytes);
            }

            byte[] message = new byte[addressBytes.Length + typeTagBytes.Length + valueBytes.Length];
            Buffer.BlockCopy(addressBytes, 0, message, 0, addressBytes.Length);
            Buffer.BlockCopy(typeTagBytes, 0, message, addressBytes.Length, typeTagBytes.Length);
            Buffer.BlockCopy(valueBytes, 0, message, addressBytes.Length + typeTagBytes.Length, valueBytes.Length);

            return message;
        }

        static byte[] CreateOSCMessage(string raw)
        {
            var addressBytes = EncodeString(raw);

            byte[] message = new byte[addressBytes.Length];
            Buffer.BlockCopy(addressBytes, 0, message, 0, addressBytes.Length);

            return message;
        }

        static byte[] EncodeString(string value)
        {
            byte[] stringBytes = Encoding.UTF8.GetBytes(value);
            int paddedLength = (stringBytes.Length + 4) & ~3;
            byte[] paddedBytes = new byte[paddedLength];
            Array.Copy(stringBytes, paddedBytes, stringBytes.Length);
            return paddedBytes;
        }

        static string DecodeString(byte[] data, int startIndex, out int nextIndex)
        {
            int length = Array.IndexOf(data, (byte)0, startIndex) - startIndex;
            if (length < 0) length = data.Length - startIndex;
            string result = Encoding.UTF8.GetString(data, startIndex, length);
            nextIndex = (startIndex + length + 4) & ~3;
            return result;
        }


        static float DecodeFloat(byte[] data, int startIndex)
        {
            byte[] floatBytes = new byte[4];
            Array.Copy(data, startIndex, floatBytes, 0, 4);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(floatBytes);
            }

            return BitConverter.ToSingle(floatBytes, 0);
        }

        static int DecodeInt(byte[] data, int startIndex)
        {
            byte[] intBytes = new byte[4];
            Array.Copy(data, startIndex, intBytes, 0, 4);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(intBytes);
            }

            return BitConverter.ToInt32(intBytes, 0);
        }
    }
}
