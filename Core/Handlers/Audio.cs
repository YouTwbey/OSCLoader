using NAudio.Wave;

namespace OSCLoader.Core.Handlers
{
    /// <summary>
    /// Handle playing audio through your mod
    /// </summary>
    public static class Audio
    {
        /// <summary>
        /// A stuct audio player for specific files
        /// </summary>
        public struct AudioPlayer
        {
            /// <summary>
            /// The audio file's reader
            /// </summary>
            public AudioFileReader fileReader;
            /// <summary>
            /// The audio's output
            /// </summary>
            public WaveOutEvent output;

            /// <summary>
            /// Plays the audio
            /// </summary>
            public void Play() { output.Play(); }

            /// <summary>
            /// Pauses the audio
            /// </summary>
            public void Pause() { output.Pause(); }
            
            /// <summary>
            /// Stops and resets the audio
            /// </summary>
            public void Stop() { output.Stop(); }

            public AudioPlayer(AudioFileReader fileReader, WaveOutEvent output)
            {
                this.fileReader = fileReader;
                this.output = output;
            }

        }
        /// <summary>
        /// Creates a NAudio.Wave.WaveOutEvent based on the requested file
        /// </summary>
        /// <param name="filePath">The audio file</param>
        /// <returns>WaveOutEvent for audio playing usage</returns>
        public static AudioPlayer Create(string filePath)
        {
            AudioFileReader audioFile = new AudioFileReader(filePath);
            WaveOutEvent output = new WaveOutEvent();

            output.Init(audioFile);
            return new AudioPlayer(audioFile, output);            
        }
    }
}
