using OSCLoader.Debug;
using System;
using System.IO.Pipes;
using System.Text;

namespace VRC.Bootstrap.launch
{
    /// <summary>
    /// VRChat's NP Client pipeline
    /// </summary>
    public class VRCNPClient : IDisposable
    {
        /// <summary>
        /// Tries to connect to the pipeline
        /// </summary>
        /// <returns>Returns true if the connect is successful</returns>
        public bool TryConnect()
        {
            bool result;
            try
            {
                pipeClientStream = new NamedPipeClientStream(".", "VRChatURLLaunchPipe", PipeDirection.InOut, PipeOptions.Asynchronous);
                pipeClientStream.Connect(0);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Tries to send a URL to the pipeline
        /// </summary>
        /// <param name="url">The URL to be sent</param>
        /// <returns>True if the send is successful</returns>
        public bool TrySendURL(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            if (pipeClientStream == null)
            {
                return false;
            }
            if (!pipeClientStream.IsConnected)
            {
                return false;
            }
            bool result;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(url);
                pipeClientStream.Write(bytes, 0, bytes.Length);
                byte[] array = new byte[1];
                result = (pipeClientStream.ReadAsync(array, 0, 1).Wait(1000) && array[0] == 1);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        internal bool AttemptConnect()
        {
            bool result;
            try
            {
                Logging.ModLog(null, $"[<color=orange>VRChat Bootstrap</color>] Connecting to VRCNPClient by the name: \"VRChatURLLaunchPipe\".");
                pipeClientStream = new NamedPipeClientStream(".", "VRChatURLLaunchPipe", PipeDirection.InOut, PipeOptions.Asynchronous);
                pipeClientStream.Connect(0);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        internal bool AttemptSendURL(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            if (pipeClientStream == null)
            {
                return false;
            }
            if (!pipeClientStream.IsConnected)
            {
                return false;
            }
            bool result;
            try
            {
                Logging.ModLog(null, $"[<color=orange>VRChat Bootstrap</color>] Reaching out to VRCNPClient: {url}");
                byte[] bytes = Encoding.UTF8.GetBytes(url);
                pipeClientStream.Write(bytes, 0, bytes.Length);
                byte[] array = new byte[1];
                result = (pipeClientStream.ReadAsync(array, 0, 1).Wait(1000) && array[0] == 1);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public void Dispose()
        {
            NamedPipeClientStream namedPipeClientStream = pipeClientStream;
            if (namedPipeClientStream == null)
            {
                return;
            }
            namedPipeClientStream.Dispose();
        }

        const string VRC_PIPE_NAME = "VRChatURLLaunchPipe";
        const byte RET_OK = 1;
        const byte RET_FAIL = 2;
        NamedPipeClientStream pipeClientStream;
    }
}
