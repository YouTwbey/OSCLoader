namespace VRC.Modding
{
    /// <summary>
    /// The root mod attribute that contains your mod's information
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class VRCModInfo : Attribute
    {
        /// <summary>
        /// Platform Support Types
        /// </summary>
        public enum PlatformSupport
        {
            /// <summary>
            /// Supports PC Only
            /// </summary>
            PC,
            /// <summary>
            /// Supports Quest Only
            /// </summary>
            Quest,
            /// <summary>
            /// Supports both PC and Quest
            /// </summary>
            Both
        }

        internal string name;
        internal string version;
        internal string author;
        internal PlatformSupport supportedDevice;
        
        /// <summary>
        /// Method to register your mod's information
        /// </summary>
        /// <param name="name">The mod's name</param>
        /// <param name="version">The mod's version</param>
        /// <param name="author">The mod's author</param>
        /// <param name="supportedDevice">The mod's supported platform</param>
        public VRCModInfo(string name, string version, string author, PlatformSupport supportedDevice = PlatformSupport.Both)
        {
            this.name = name;
            this.version = version;
            this.author = author;
            this.supportedDevice = supportedDevice;
        }
    }
}
