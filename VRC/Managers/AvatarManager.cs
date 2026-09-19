using OSCLoader.Debug;
using System.Text.Json;
using static VRC.API;

namespace VRC.Managers
{
    /// <summary>
    /// View info about VRChat avatars
    /// </summary>
    public static class AvatarManager
    {
        /// <summary>
        /// Info about an avatar's performance
        /// </summary>
        public struct PerformanceInfo
        {
            /// <summary>
            /// The avatar's android rating
            /// </summary>
            public string Android { get; internal set; }
            /// <summary>
            /// The avatar's android sort
            /// </summary>
            public int AndroidSort { get; internal set; }
            /// <summary>
            /// The avatar's IOS rating
            /// </summary>
            public string IOS { get; internal set; }
            /// <summary>
            /// The avatar's IOS sort
            /// </summary>
            public int IOSSort { get; internal set; }
            /// <summary>
            /// The avatar's standalone windows rating
            /// </summary>
            public string StandaloneWindows { get; internal set; }
            /// <summary>
            /// The avatar's standalone windows sort
            /// </summary>
            public int StandaloneWindowsSort { get; internal set; }

            internal PerformanceInfo(string and, int andSort, string i, int isort, string win, int winSort)
            {
                Android = and;
                AndroidSort = andSort;
                IOS = i;
                IOSSort = isort;
                StandaloneWindows = win;
                StandaloneWindowsSort = winSort;
            }
        }

        /// <summary>
        /// Info about an avatar
        /// </summary>
        public struct AvatarInfo
        {
            /// <summary>
            /// The avatar author's id
            /// </summary>
            public string AuthorId { get; internal set; }
            /// <summary>
            /// The avatar author's name
            /// </summary>
            public string AuthorName { get; internal set; }
            /// <summary>
            /// The avatar's creation date
            /// </summary>
            public DateTime CreatedAt { get; internal set; }
            /// <summary>
            /// The avatar's description
            /// </summary>
            public string Description { get; internal set; }
            /// <summary>
            /// True if the avatar is currently featured
            /// </summary>
            public bool Featured { get; internal set; }
            /// <summary>
            /// The avatar's id
            /// </summary>
            public string Id { get; internal set; }
            /// <summary>
            /// The avatar's image url
            /// </summary>
            public string ImageUrl { get; internal set; }
            /// <summary>
            /// The avatar's listing date
            /// </summary>
            public string ListingDate { get; internal set; }
            /// <summary>
            /// The avatar's name
            /// </summary>
            public string Name { get; internal set; }
            /// <summary>
            /// The avatar's performance info
            /// </summary>
            public PerformanceInfo Performance { get; internal set; }
            /// <summary>
            /// The avatar's release status
            /// </summary>
            public string ReleaseStatus { get; internal set; }
            /// <summary>
            /// True if the avatar is searchable
            /// </summary>
            public bool Searchable { get; internal set; }
            /// <summary>
            /// The avatar's tags
            /// </summary>
            public string[] Tags { get; internal set; }
            /// <summary>
            /// The avatar's thumbnail image url
            /// </summary>
            public string ThumbnailImageUrl { get; internal set; }
            /// <summary>
            /// The avatar's packages
            /// </summary>
            public PackageInfo[] UnityPackages { get; internal set; }
            /// <summary>
            /// The avatar's update time
            /// </summary>
            public DateTime UpdatedAt { get; internal set; }
            /// <summary>
            /// The avatar's version
            /// </summary>
            public int Version { get; internal set; }

            internal AvatarInfo(string convertFromJson)
            {
                // Basic Info
                using JsonDocument doc = JsonDocument.Parse(convertFromJson);
                JsonElement root = doc.RootElement;
                AuthorId = root.GetProperty("authorId").GetString();
                AuthorName = root.GetProperty("authorName").GetString();
                CreatedAt = root.GetProperty("created_at").GetDateTime();
                Description = root.GetProperty("description").GetString();
                Featured = root.GetProperty("featured").GetBoolean();
                Id = root.GetProperty("id").GetString();
                ImageUrl = root.GetProperty("imageUrl").GetString();
                ListingDate = root.GetProperty("listingDate").GetString();
                Name = root.GetProperty("name").GetString();
                ReleaseStatus = root.GetProperty("releaseStatus").GetString();
                Searchable = root.GetProperty("searchable").GetBoolean();
                ThumbnailImageUrl = root.GetProperty("thumbnailImageUrl").GetString();
                UpdatedAt = root.GetProperty("updated_at").GetDateTime();
                Version = root.GetProperty("version").GetInt32();

                // Tags
                JsonElement tagsEl = root.GetProperty("tags");
                List<string> tags = new List<string>();

                for (int i = 0; i < tagsEl.GetArrayLength(); i++)
                {
                    tags.Add(tagsEl[i].GetString());
                }
                Tags = tags.ToArray();

                // Performance
                JsonElement perfEl = root.GetProperty("instances");
                PerformanceInfo perf = new PerformanceInfo();
                perf.Android = perfEl.GetProperty("android").GetString();
                perf.AndroidSort = perfEl.GetProperty("android-sort").GetInt32();
                perf.IOS = perfEl.GetProperty("ios").GetString();
                perf.IOSSort = perfEl.GetProperty("ios-sort").GetInt32();
                perf.StandaloneWindows = perfEl.GetProperty("standalonewindows").GetString();
                perf.StandaloneWindowsSort = perfEl.GetProperty("stanalonewindows-sort").GetInt32();
                Performance = perf;

                // Packages
                JsonElement packagesEl = root.GetProperty("unityPackages");
                List<PackageInfo> packages = new List<PackageInfo>();

                for (int i = 0; i < packagesEl.GetArrayLength(); i++)
                {
                    JsonElement pkg = packagesEl[i];

                    packages.Add(new PackageInfo(
                        pkg.GetProperty("assetUrl").GetString(),
                        pkg.GetProperty("assetVersion").GetInt32(),
                        pkg.GetProperty("created_at").GetDateTime(),
                        pkg.GetProperty("id").GetString(),
                        pkg.GetProperty("platform").GetString(),
                        pkg.TryGetProperty("scanStatus", out var scan) ? scan.GetString() : null,
                        pkg.GetProperty("unitySortNumber").GetRawText(),
                        pkg.GetProperty("unityVersion").GetString(),
                        pkg.TryGetProperty("variant", out var varEl) ? varEl.GetString() : null,
                        pkg.GetProperty("worldSignature").GetString()
                    ));
                }
            }
        }

        /// <summary>
        /// The name of the avatar the client is currently wearing
        /// </summary>
        public static string currentAvatarName { get; internal set; } = "";

        /// <summary>
        /// Get info about an avatar by it's avatarId
        /// </summary>
        /// <param name="avatarId">The avatar's Id</param>
        /// <returns>Info about the avatar</returns>
        public static AvatarInfo GetAvatarInfo(string avatarId)
        {
            return GetAvatarInfoAsync(avatarId).Result;
        }

        /// <summary>
        /// Get info about a avatar by it's avatarId asynchronously
        /// </summary>
        /// <param name="avatarId">The avatar's Id</param>
        /// <returns>Info about the avatar</returns>
        public async static Task<AvatarInfo> GetAvatarInfoAsync(string avatarId)
        {
            string result;
            try
            {
                HttpClient http = new HttpClient();
                http.DefaultRequestHeaders.Add("User-Agent", "OSCLoader/1.0.0");

                string url = $"https://vrchat.com/api/1/worlds/{avatarId}";
                result = await http.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                Logging.Error("HTTP error when fetching for avatar info on \"" + avatarId + "\": " + ex.Message);
                return new AvatarInfo();
            }

            try
            {
                return new AvatarInfo(result);
            }
            catch
            {
                Logging.Error("Conversion error when fetching for avatar info on \"" + avatarId + "\": " + result);
                return new AvatarInfo();
            }
        }
    }
}
