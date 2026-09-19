namespace Photon.Realtime
{
    /// <summary>
    /// Read-only version of VRChat's Photon.Realtime.Room component. Only contains information that is possible to retrieve
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Sets a limit of players to this room. This property is synced and shown in lobby, too. If the room is full (players count == maxplayers), joining this room will fail.<br /><br />
        /// As part of RoomInfo this can't be set. As part of a Room (which the player joined), the setter will update the server and all clients.
        /// </summary>
        public int MaxPlayers { get; internal set; }
        /// <summary>
        /// The name of a room. Unique identifier (per region and virtual appid) for a room/match.<br /> <br />
        /// The name can't be changed once it's set by the server.
        /// </summary>
        public string Name { get; internal set; } = "";
        /// <summary>
        /// Defines if the room can be joined.<br /><br />
        /// This does not affect listing in a lobby but joining the room will fail if not open. If not open, the room is excluded from random matchmaking.<br />
        /// Due to racing conditions, found matches might become closed while users are trying to join. Simply re-connect to master and find another. Use property "IsVisible" to not list the room.<br /><br />
        /// As part of RoomInfo this can't be set. As part of a Room (which the player joined), the setter will update the server and all clients.
        /// </summary>
        public bool IsOpen { get { return true; } }
        /// <summary>
        /// Defines if the room is listed in its lobby.<br /><br />
        /// Rooms can be created invisible, or changed to invisible. To change if a room can be joined, use property: open.
        /// As part of RoomInfo this can't be set. As part of a Room (which the player joined), the setter will update the server and all clients.
        /// </summary>
        public bool IsVisible { get; internal set; }
        /// <summary>
        /// While insided a Room, this is the list of players who are also in that room.
        /// </summary>
        public Dictionary<int, Player> Players { get; internal set; } = new Dictionary<int, Player>();
        /// <summary>
        /// Define if UserIds of the players are broadcast in the room. Useful for FindFriends and reserving slots for expected users.
        /// </summary>
        public bool PublishUserId { get { return true; } }
        /// <summary>
        /// The count of players in this Room (using this.Players.Count).
        /// </summary>
        public int PlayerCount { get { return Players.Count; } }
        /// <summary>
        /// Tries to find the player with given ActorNumber (a.k.a. ID). Only useful when in a Room, as IDs are only valid per Room.
        /// </summary>
        /// <param name="id">ID to look for.</param>
        /// <returns>The player with the ID or null.</returns>
        public Player? GetPlayer(int id)
        {
            foreach (Player player in Players.Values)
            {
                if (player.ActorNumber == id)
                {
                    return player;
                }
            }

            return null;
        }

        internal int nextActorNumber = 0;
    }
}
