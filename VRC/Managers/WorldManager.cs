using OSCLoader.Core.Handlers;
using OSCLoader.Debug;
using Photon.Pun;
using Photon.Realtime;
using System.Text.Json;
using static VRC.API;

namespace VRC.Managers
{
    /// <summary>
    /// View info about VRChat worlds
    /// </summary>
    public static class WorldManager
    {
        /// <summary>
        /// Info about the current world the client is in
        /// </summary>
        public static WorldInfo currentWorld { get; internal set; } = new WorldInfo();
        /// <summary>
        /// Info about the current instance the client is in
        /// </summary>
        public static InstanceInfo currentInstance { get; internal set; } = new InstanceInfo();

        /// <summary>
        /// Info about a current instance
        /// </summary>
        public struct InstanceInfo
        {
            /// <summary>
            /// Name of the instance
            /// </summary>
            public string Name { get; internal set; }
            /// <summary>
            /// The instance's region
            /// </summary>
            public string Region { get; internal set; }
            /// <summary>
            /// Amount of players in the instance<br/>
            /// Value is set to -1 if it can't be found
            /// </summary>
            public int Players { get; internal set; }

            internal InstanceInfo(string name, string region, int players)
            {
                Name = name;
                Region = region;
                Players = players;
            }
        }

        /// <summary>
        /// Info about a world
        /// </summary>
        public struct WorldInfo
        {
            /// <summary>
            /// The world author's Id
            /// </summary>
            public string AuthorId { get; internal set; }
            /// <summary>
            /// The world author's name
            /// </summary>
            public string AuthorName { get; internal set; }
            /// <summary>
            /// The world's player limit
            /// </summary>
            public int Capacity { get; internal set; }
            /// <summary>
            /// The world's creation date
            /// </summary>
            public DateTime CreatedAt { get; internal set; }
            /// <summary>
            /// The world's description
            /// </summary>
            public string Description { get; internal set; }
            /// <summary>
            /// The amount of favourites the world has
            /// </summary>
            public int Favourites { get; internal set; }
            /// <summary>
            /// True if the world is currently featured
            /// </summary>
            public bool Featured { get; internal set; }
            /// <summary>
            /// The world's heat value
            /// </summary>
            public int Heat { get; internal set; }
            /// <summary>
            /// The world's Id
            /// </summary>
            public string Id { get; internal set; }
            /// <summary>
            /// The world's image url
            /// </summary>
            public string ImageUrl { get; internal set; }
            /// <summary>
            /// A list of all currently public active instances at the time of fetching this world's info
            /// </summary>
            public InstanceInfo[] Instances { get; internal set; }
            /// <summary>
            /// The world's publication date as a Lab world
            /// </summary>
            public DateTime LabsPublicationDate { get; internal set; }
            /// <summary>
            /// The world's name
            /// </summary>
            public string Name { get; internal set; }
            /// <summary>
            /// The world's player count
            /// </summary>
            public int Occupants { get; internal set; }
            /// <summary>
            /// The world's organization
            /// </summary>
            public string Organization { get; internal set; }
            /// <summary>
            /// The world's popularity
            /// </summary>
            public int Popularity { get; internal set; }
            /// <summary>
            /// The world's preview youtube Id
            /// </summary>
            public string PreviewYouTubeId { get; internal set; }
            /// <summary>
            /// The amount of players in private instances
            /// </summary>
            public int PrivateOccupants { get; internal set; }
            /// <summary>
            /// The amount of players in public instances
            /// </summary>
            public int PublicOccupants { get; internal set; }
            /// <summary>
            /// The world's publication date
            /// </summary>
            public DateTime PublicationDate { get; internal set; }
            /// <summary>
            /// The world's recommended capacity for players
            /// </summary>
            public int RecommendedCapacity { get; internal set; }
            /// <summary>
            /// The world's release status
            /// </summary>
            public string ReleaseStatus { get; internal set; }
            /// <summary>
            /// The world's StoreId
            /// </summary>
            public string StoreId { get; internal set; }
            /// <summary>
            /// The world's Tags
            /// </summary>
            public string[] Tags { get; internal set; }
            /// <summary>
            /// The world's thumbnail image url
            /// </summary>
            public string ThumbnailImageUrl { get; internal set; }
            /// <summary>
            /// The world's packages
            /// </summary>
            public PackageInfo[] UnityPackages { get; internal set; }
            /// <summary>
            /// The world's last updated in time
            /// </summary>
            public DateTime UpdatedAt { get; internal set; }
            /// <summary>
            /// The world's version
            /// </summary>
            public int Version { get; internal set; }
            /// <summary>
            /// The world's visits
            /// </summary>
            public int Visits { get; internal set; }

            internal WorldInfo(string convertFromJson)
            {
                string stack = "parse";
                try
                {
                    // Basic Info
                    using JsonDocument doc = JsonDocument.Parse(convertFromJson);
                    stack = "root";
                    JsonElement root = doc.RootElement;
                    stack = "basic";
                    AuthorId = JSON.GetString(root, "authorId");
                    AuthorName = JSON.GetString(root, "authorName");
                    Capacity = JSON.GetInt32(root, "capacity");
                    CreatedAt = JSON.GetDateTime(root, "created_at");
                    Description = JSON.GetString(root, "description");
                    Favourites = JSON.GetInt32(root, "favorites");
                    Featured = JSON.GetBool(root, "featured");
                    Heat = JSON.GetInt32(root, "heat");
                    Id = JSON.GetString(root, "id");
                    ImageUrl = JSON.GetString(root, "imageUrl");
                    Name = JSON.GetString(root, "name");
                    Occupants = JSON.GetInt32(root, "occupants");
                    Organization = JSON.GetString(root, "organization");
                    Popularity = JSON.GetInt32(root, "popularity");
                    PreviewYouTubeId = JSON.GetString(root, "previewYoutubeId");
                    PrivateOccupants = JSON.GetInt32(root, "privateOccupants");
                    PublicOccupants = JSON.GetInt32(root, "publicOccupants");
                    PublicationDate = JSON.GetDateTime(root, "publicationDate");
                    RecommendedCapacity = JSON.GetInt32(root, "recommendedCapacity");
                    ReleaseStatus = JSON.GetString(root, "releaseStatus");
                    StoreId = JSON.GetString(root, "storeId");
                    ThumbnailImageUrl = JSON.GetString(root, "thumbnailImageUrl");
                    UpdatedAt = JSON.GetDateTime(root, "updated_at");
                    Version = JSON.GetInt32(root, "version");
                    Visits = JSON.GetInt32(root, "visits");

                    stack = "tags";
                    // Tags
                    List<string> tags = new List<string>();

                    if (root.TryGetProperty("tags", out JsonElement tagsEl))
                    {
                        for (int i = 0; i < tagsEl.GetArrayLength(); i++)
                        {
                            string? val = tagsEl[i].GetString();
                            if (val == null) continue;

                            tags.Add(val);
                        }
                    }

                    Tags = tags.ToArray();

                    stack = "instances";
                    // Instances
                    List<InstanceInfo> instances = new List<InstanceInfo>();

                    if (root.TryGetProperty("instances", out JsonElement instancesEl))
                    {
                        for (int i = 0; i < instancesEl.GetArrayLength(); i++)
                        {
                            JsonElement inst = instancesEl[i];

                            string? raw = inst[0].GetString();
                            if (raw == null) continue;

                            int players = inst[1].GetInt32();

                            string name = raw.Split('~')[0];
                            string region = raw.Contains("region(")
                                ? raw.Split("region(")[1].TrimEnd(')')
                                : "unknown";

                            instances.Add(new InstanceInfo(name, region, players));
                        }
                    }

                    Instances = instances.ToArray();

                    stack = "packages";
                    // Packages
                    List<PackageInfo> packages = new List<PackageInfo>();

                    if (root.TryGetProperty("unityPackages", out JsonElement packagesEl))
                    {
                        for (int i = 0; i < packagesEl.GetArrayLength(); i++)
                        {
                            JsonElement pkg = packagesEl[i];

                            packages.Add(new PackageInfo(
                                JSON.GetString(pkg, "assetUrl"),
                                JSON.GetInt32(pkg, "assetVersion"),
                                JSON.GetDateTime(pkg, "created_a t"),
                                JSON.GetString(pkg, "id"),
                                JSON.GetString(pkg, "platform"),
                                JSON.GetString(pkg, "scanStatus"),
                                JSON.GetRawString(pkg, "unitySortNumber"),
                                JSON.GetString(pkg, "unityVersion"),
                                JSON.GetString(pkg, "variant"),
                                JSON.GetString(pkg, "worldSignature")
                            ));
                        }
                    }

                    UnityPackages = packages.ToArray();
                }
                catch (Exception ex)
                {
                    Logging.Error($"Failed to generate world info! Section: {stack} | Reason: {ex.ToString()}");
                }
            }
        }

        /// <summary>
        /// Get info about a world by it's worldId
        /// </summary>
        /// <param name="worldId">The world's Id</param>
        /// <returns>Info about the world</returns>
        public static WorldInfo GetWorldInfo(string worldId)
        {
            return GetWorldInfoAsync(worldId).Result;
        }

        /// <summary>
        /// Get info about a world by it's worldId asynchronously
        /// </summary>
        /// <param name="worldId">The world's Id</param>
        /// <returns>Info about the world</returns>
        public async static Task<WorldInfo> GetWorldInfoAsync(string worldId)
        {
            string result;
            try
            {
                HttpClient http = new HttpClient();
                http.DefaultRequestHeaders.Add("User-Agent", "OSCLoader/1.0.0");

                string url = $"https://vrchat.com/api/1/worlds/{worldId}";
                result = await http.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                Logging.Error("HTTP error when fetching for world info on \"" + worldId + "\": " + ex.Message);
                return new WorldInfo();
            }

            return new WorldInfo(result);
        }

        internal static void UpdateCurrentInfoValues(string newWorldId, string newInstanceName, string newInstanceVisibility, string newInstanceRegion)
        {
            WorldInfo worldInfo = GetWorldInfo(newWorldId);

            InstanceInfo instance = new InstanceInfo("invalid", "invalid", -1);

            foreach (InstanceInfo inst in worldInfo.Instances)
            {
                if (inst.Name == newInstanceName)
                {
                    instance = inst;
                    break;
                }
            }

            if (instance.Name == "invalid")
            {
                instance = new InstanceInfo(newInstanceName, newInstanceRegion, -1);
            }

            VRCModsManager.OnInstanceChangedAll(currentInstance, instance);
            currentInstance = instance;
            Room newRoom = new Room();
            newRoom.Name = newInstanceName;
            newRoom.IsVisible = newInstanceVisibility == "public";
            newRoom.MaxPlayers = worldInfo.Capacity;
            PhotonNetwork.CurrentRoom = newRoom;
            if (PhotonNetwork.LocalPlayer != null)
            {
                PhotonNetwork.AddPlayer(PhotonNetwork.LocalPlayer.NickName, PhotonNetwork.LocalPlayer.UserID);
            }

            VRCModsManager.OnWorldChangedAll(currentWorld, worldInfo);
            currentWorld = worldInfo;
        }

        internal static Dictionary<string, Dictionary<string, List<Action>>> worldCallbacks = new Dictionary<string, Dictionary<string, List<Action>>>();
        /// <summary>
        /// Adds a custom callback when a specific world logs a specific message 
        /// </summary>
        /// <param name="worldNameOrId">The world's name or Id</param>
        /// <param name="listeningFor">The message pattern it listens for</param>
        /// <param name="callback">Your method that will be invoked</param>
        public static void AddWorldCallback(string worldNameOrId, string listeningFor, Action callback)
        {
            if (worldCallbacks.ContainsKey(worldNameOrId))
            {
                if (worldCallbacks[worldNameOrId].ContainsKey(listeningFor))
                {
                    worldCallbacks[worldNameOrId][listeningFor].Add(callback);
                }
                else
                {
                    worldCallbacks[worldNameOrId].Add(listeningFor, new List<Action> { callback });
                }
            }
            else
            {
                worldCallbacks.Add(worldNameOrId, new Dictionary<string, List<Action>> { { listeningFor, new List<Action> { callback } } });
            }
        }
    }
}
