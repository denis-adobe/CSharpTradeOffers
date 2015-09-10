using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpTradeOffers.Configuration
{
    /// <summary>
    /// A generic RootConfig object containing configuration information.
    /// </summary>
    [JsonObject(Title = "RootObject")]
    public class Config
    {
        /// <summary>
        /// Username to automatically log in to.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Password to use to automatically log in.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Special bytes sent to Steam to prove the user is the account holder.
        /// </summary>"
        public string SteamMachineAuth { get; set; }
        /// <summary>
        /// A special key retrieved from https://steamcommunity.com/dev/apikey
        /// The key MUST be from the bot's account.
        /// </summary>
        public string ApiKey { get; set; }
                                                                                                           
        public List<uint> Inventories { get; set; }
    }
}