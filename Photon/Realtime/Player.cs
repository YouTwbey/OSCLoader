namespace Photon.Realtime
{
    /// <summary>
    /// Read-only version of VRChat's Photon.Realtime.Player component. Only contains information that is possible to retrieve
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Non-unique nickname of this player. Synced automatically in a room.<br /><br />
        /// A player might change his own playername in a room (it's only a property). Settings this value updates the server and other players (using an operation).
        /// </summary>
        public string NickName { get; internal set; } = "";
        /// <summary>
        /// UserId of the player, available when the room got created with RoomOptions.PublishUserId = true.<br /><br />
        /// Useful for LoadBalancingClient.OpFindFriends and blocking slots in a room for expected players (e.g. in LoadBalancingClient.OpCreateRoom).
        /// </summary>
        public string UserID { get; internal set; } = "";
        /// <summary>
        /// Only one player is controlled by each client. Others are not local.
        /// </summary>
        public bool IsLocal { get; internal set; }
        /// <summary>
        /// Identifier of this player in current room. Also known as: actorNumber or actorNumber. It's -1 outside of rooms.<br /><br />
        /// The ID is assigned per room and only valid in that context. It will change even on leave and re-join. IDs are never re-used per room.
        /// </summary>
        public int ActorNumber { get; internal set; }
    }
}
