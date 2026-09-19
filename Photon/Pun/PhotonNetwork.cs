using Photon.Realtime;

namespace Photon.Pun
{
    /// <summary>
    /// Read-only version of VRChat's Photon.Pun.PhotonNetwork component. Only contains information that is possible to retrieve
    /// </summary>
    public static class PhotonNetwork
    {
        /// <summary>
        /// Set to synchronize the player's nickname with everyone in the room(s) you enter.<br /><br />
        /// This sets PhotonNetwork.player.NickName.
        /// </summary>
        public static string NickName
        {
            get
            {
                if (LocalPlayer != null) return LocalPlayer.NickName;
                return "";
            }
        }
        /// <summary>
        /// This client's Player instance is always available, unless the app shuts down.
        /// </summary>
        public static Player? LocalPlayer { get; internal set; }
        /// <summary>
        /// A sorted copy of the players-list of the current room. This is using Linq, so better cache this value. Update when players join / leave.
        /// </summary>
        public static Player[] PlayerList
        {
            get
            {
                Player[]? list = CurrentRoom?.Players.Values.ToArray();
                if (list == null) return new List<Player>().ToArray();
                return list;
            }
        }
        /// <summary>
        /// A sorted copy of the players-list of the current room, excluding this client. This is using Linq, so better cache this value. Update when players join / leave.
        /// </summary>
        public static Player[] PlayerListOthers
        {
            get
            {
                List<Player>? players = CurrentRoom?.Players.Values.ToList();

                if (players != null)
                {
                    if (LocalPlayer != null)
                    {
                        players.Remove(LocalPlayer);
                        return players.ToArray();
                    }
                }

                return new List<Player>().ToArray();
            }
        }
        /// <summary>
        /// True when this client is in a lobby.<br /><br />
        /// Implement IPunCallbacks.OnRoomListUpdate() for a notification when the list of rooms becomes available or updated.<br /><br />
        /// You are automatically leaving any lobby when you join a room! Lobbies only exist on the Master Server (whereas rooms are handled by Game Servers).
        /// </summary>
        public static bool InLobby;
        /// <summary>
        /// Is true when being in a room (NetworkSlientState == ClientState.Joined).<br /><br />
        /// Aside from polling this value, game logic should implement IMatchmakingCallbacks in some class and react when that gets called.<br /><br />
        /// Many actions can only be executed in a room, like Instantiate or Leave, etc.<br />
        /// A client can join a room in offline mode. In that case, don't use LoadBalancingClient.InRoom, which does not cover offline mode.
        /// </summary>
        public static bool InRoom;
        /// <summary>
        /// Get the room we're currently in (also when in OfflineMode). Null if we aren't in any room.<br /><br />
        /// LoadBalancing Client is not aware of the Photon Offline Mode, so never use PhotonNetwork.NetworkingClient.CurrentRoom will be null if you are in OffLine Mode, while PhotonNetwork.CurrentRoom will be set when offlineModed is true
        /// </summary>
        public static Room? CurrentRoom;

        internal static void AddPlayer(string playerName, string userId)
        {
            if (CurrentRoom == null) return;

            bool alreadyExists = false;
            foreach (Player player in CurrentRoom.Players.Values)
            {
                if (player.UserID == userId)
                {
                    alreadyExists = true;
                    break;
                }
            }
            if (alreadyExists) return;

            int newActorNum = CurrentRoom.nextActorNumber;
            CurrentRoom.nextActorNumber++;

            if (userId == LocalPlayer?.UserID)
            {
                LocalPlayer.ActorNumber = newActorNum;
                CurrentRoom.Players.Add(newActorNum, LocalPlayer);
                return;
            }

            Player plr = new Player
            {
                UserID = userId,
                NickName = playerName,
                ActorNumber = newActorNum
            }; 
            CurrentRoom.Players.Add(plr.ActorNumber, plr);
        }
        internal static void RemovePlayer(string userId)
        {
            if (CurrentRoom == null) return;

            foreach (Player player in CurrentRoom.Players.Values)
            {
                if (player.UserID == userId)
                {
                    CurrentRoom.Players.Remove(player.ActorNumber);
                    break;
                }
            }
        }

        internal static Player GetNullPlayer()
        {
            return new Player
            {
                ActorNumber = -1,
                NickName = "null",
                UserID = "null",
                IsLocal = false
            };
        }

        internal static Player GetPlayerByNickName(string nickname)
        {
            if (CurrentRoom != null)
            {
                foreach (Player player in CurrentRoom.Players.Values)
                {
                    if (player.NickName == nickname)
                    {
                        return player;
                    }
                }
            }
            else
            {
                if (LocalPlayer?.NickName == nickname)
                {
                    return LocalPlayer;
                }
            }

            return new Player
            {
                ActorNumber = -1,
                NickName = "null",
                UserID = "null",
                IsLocal = false
            };
        }

        internal static Player GetPlayerByUserID(string userId)
        {
            if (CurrentRoom != null)
            {
                foreach (Player player in CurrentRoom.Players.Values)
                {
                    if (player.UserID == userId)
                    {
                        return player;
                    }
                }
            }
            else
            {
                if (LocalPlayer?.UserID == userId)
                {
                    return LocalPlayer;
                }
            }

            return new Player
            {
                ActorNumber = -1,
                NickName = "null",
                UserID = "null",
                IsLocal = false
            };
        }
    }
}
