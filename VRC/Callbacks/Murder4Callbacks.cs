using System.Text.RegularExpressions;

namespace VRC.Callbacks
{
    /// <summary>
    /// A script that allows for additional callbacks when in Murder 4
    /// </summary>
    public abstract class Murder4Callbacks
    {
        /// <summary>
        /// A list of item pickups in Murder 4
        /// </summary>
        public enum Items
        {
            /// <summary>
            /// Knife murderers can use
            /// </summary>
            Knife,
            /// <summary>
            /// Revolver that's given to Detectives
            /// </summary>
            Revolver,
            /// <summary>
            /// Camera that is unlocked by finding all the photographs
            /// </summary>
            Camera,
            /// <summary>
            /// Shotgun that is unlocked by finding 5 clues
            /// </summary>
            Shotgun,
            /// <summary>
            /// Grenade that is unlocked by finding 5 clues
            /// </summary>
            Grenade,
            /// <summary>
            /// Smoke Grenade that is unlocked by finding 5 clues
            /// </summary>
            SmokeGrenade,
            /// <summary>
            /// Luger that is unlocked by finding 5 clues
            /// </summary>
            Luger,
            /// <summary>
            /// Photographs that are used to unlock the camera
            /// </summary>
            Photograph,
            /// <summary>
            /// An item that isn't listed
            /// </summary>
            Other
        }

        /// <summary>
        /// Runs when the client picks up an item
        /// </summary>
        /// <param name="item">The item that was picked up</param>
        public virtual void OnItemPickup(Items item) { }

        internal static List<Murder4Callbacks> All = new List<Murder4Callbacks>();
        protected Murder4Callbacks()
        {
            All.Add(this);
        }
        ~Murder4Callbacks()
        {
            All.Remove(this);
        }

        internal static void CheckLog(string log)
        {
            if (log.Contains("[Behaviour] Pickup object: "))
            {
                Match match = Regex.Match(log, @"Pickup object: '([^']+)' equipped = (\w+), is AutoEquipType Pickup = (\w+), last input method = (\w+), is AutoHold is enabled for this controller type = (\w+)");

                if (match.Success)
                {
                    string objectName = match.Groups[1].Value;
                    Items item = Items.Other;

                    if (objectName.Contains("GamePickup Knife"))
                    {
                        item = Items.Knife;
                    }
                    else if (objectName.Contains("GamePickup Revolver"))
                    {
                        item = Items.Revolver;
                    }

                    foreach (Murder4Callbacks callback in All)
                    {
                        callback.OnItemPickup(item);
                    }
                }
                else
                {
                    foreach (Murder4Callbacks callback in All)
                    {
                        callback.OnItemPickup(Items.Other);
                    }
                }
            }
        }
    }
}
