namespace VRC
{
    /// <summary>
    /// References to the API
    /// </summary>
    public static class API
    {
        /// <summary>
        /// Info about a world unity package
        /// </summary>
        public struct PackageInfo
        {
            /// <summary>
            /// The asset's url
            /// </summary>
            public string AssetUrl { get; internal set; }
            /// <summary>
            /// The asset's version
            /// </summary>
            public int AssetVersion { get; internal set; }
            /// <summary>
            /// The asset's creation date
            /// </summary>
            public DateTime CreatedAt { get; internal set; }
            /// <summary>
            /// The asset's id
            /// </summary>
            public string Id { get; internal set; }
            /// <summary>
            /// The asset's supported platform
            /// </summary>
            public string Platform { get; internal set; }
            /// <summary>
            /// The asset's scan status
            /// </summary>
            public string ScanStatus { get; internal set; }
            /// <summary>
            /// The asset's sort number in Unity
            /// </summary>
            public string UnitySortNumber { get; internal set; }
            /// <summary>
            /// The unity version the asset was built with
            /// </summary>
            public string UnityVersion { get; internal set; }
            /// <summary>
            /// The asset's variant
            /// </summary>
            public string Variant { get; internal set; }
            /// <summary>
            /// The asset's world signiture
            /// </summary>
            public string WorldSignature { get; internal set; }

            internal PackageInfo(string url, int version, DateTime creation, string id, string platform, string scan, string sort, string unityVer, string variant, string signature)
            {
                AssetUrl = url;
                AssetVersion = version;
                CreatedAt = creation;
                Id = id;
                Platform = platform;
                ScanStatus = scan;
                UnitySortNumber = sort;
                UnityVersion = unityVer;
                Variant = variant;
                WorldSignature = signature;
            }
        }
    }
}
