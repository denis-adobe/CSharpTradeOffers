﻿using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSharpTradeOffers.Trading
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Handles Steam Economy related tasks, like retrieving class info
    /// </summary>
    public class ISteamEconomyHandler
    {
        private const string BaseUrl = "https://api.steampowered.com/ISteamEconomy/";

        /// <summary>
        /// Gets the asset class info of an item, must provide ClassID/InstanceID in IDs
        /// </summary>
        /// <param name="apiKey">Your Steam API key</param>
        /// <param name="appid">Uint32 number that represents the game to retrieve item data from.</param>
        /// <param name="IDs">Dictionary MUST contain ClassID/InstanceID of item.</param>
        /// <returns></returns>
        public AssetClassInfo GetAssetClassInfo(string apiKey, uint appid, Dictionary<string, string> IDs)
        {
            const string url = BaseUrl + "GetAssetClassInfo/v0001/";
            var data = new Dictionary<string, string>
            {
                {"key", apiKey},
                {"appid", appid.ToString()},
                {"class_count", IDs.Count.ToString()}
            };
            int currentClass = 0;
            foreach (var key in IDs) //make only request per appid at a time
            {
                data.Add("classid" + currentClass, key.Key);
                data.Add("instanceid" + currentClass, key.Value);
                currentClass++;
            }

            dynamic dynamicinfo = JsonConvert.DeserializeObject<dynamic>(Web.Fetch(url, "GET", data, null, false)).result;

            Dictionary<string,dynamic> desrDictionary =
                JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(dynamicinfo.ToString());

            desrDictionary.Remove("success");

            return JsonConvert.DeserializeObject<AssetClassInfo>(desrDictionary.Values.First().ToString());

            //var desrDictionary =
            //    JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(dynamicinfo.ToString());
            //
            //desrDictionary.Remove("success");
            //
            //return JsonConvert.DeserializeObject<AssetClassInfo>(desrDictionary.Values.ToString());
        }

        #region old
        
        ///// <summary>
        ///// Partially converts the dynamic GetAssetClassInfo into an AssetClassInfo object.
        ///// Does not convert filter_data because its complicated and I'm lazy.
        ///// Very poorly done, possibly optimize/fix?
        ///// </summary>
        ///// <param name="dynamicinfo">The return variable from GetAssetClassInfo</param>
        ///// <returns>An AssetClassInfo object.</returns>
        //// ReSharper disable once FunctionComplexityOverflow
        //public AssetClassInfo ToAssetClassInfo(dynamic dynamicinfo)
        //{
        //    Dictionary<string, dynamic> desrDictionary =
        //        JsonConvert.DeserializeObject <Dictionary<string, dynamic>>(dynamicinfo.ToString());
        //    desrDictionary.Remove("success");
        //    return JsonConvert.DeserializeObject<AssetClassInfo>(desrDictionary.Values.First().ToString());
        //}
        
        #endregion
    }
}