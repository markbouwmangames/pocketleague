using System.Collections.Generic;
using Newtonsoft.Json;
using RLSApi.Data;
using RLSApi.Util;
using System;

namespace RLSApi.Net.Models
{
    public class Player
    {
        [JsonProperty("uniqueId", Required = Required.Always)]
        public string UniqueId { get; set; }

        [JsonProperty("displayName", Required = Required.Always)]
        public string DisplayName { get; set; }

        [JsonProperty("platform", Required = Required.Always)]
        public Platform Platform { get; set; }

        [JsonProperty("avatar", Required = Required.AllowNull)]
        public string Avatar { get; set; }

        [JsonProperty("profileUrl", Required = Required.Always)]
        public string ProfileUrl { get; set; }

        [JsonProperty("signatureUrl", Required = Required.Always)]
        public string SignatureUrl { get; set; }

        [JsonProperty("stats", Required = Required.Always)]
        public PlayerStats Stats { get; set; }

        [JsonProperty("rankedSeasons", Required = Required.Always)]
        public Dictionary<RlsSeason, Dictionary<RlsPlaylistRanked, PlayerRank>> RankedSeasons { get; set; }

        [JsonProperty("lastRequested", Required = Required.Always)]
        public long LastRequestedUnix { get; set; }
		[JsonIgnore]
		public DateTimeOffset LastRequested { get { return TimeUtil.UnixTimeStampToDateTime(LastRequestedUnix); } }

		[JsonProperty("createdAt", Required = Required.Always)]
        public long CreatedAtUnix { get; set; }
		[JsonIgnore]
		public DateTimeOffset CreatedAt { get { return TimeUtil.UnixTimeStampToDateTime(CreatedAtUnix); } }

		[JsonProperty("updatedAt", Required = Required.Always)]
        public long UpdatedAtUnix { get; set; }
		[JsonIgnore]
		public DateTimeOffset UpdatedAt { get { return TimeUtil.UnixTimeStampToDateTime(UpdatedAtUnix); } }

		[JsonProperty("nextUpdateAt", Required = Required.Always)]
        public long NextUpdateAtUnix { get; set; }
		[JsonIgnore]
		public DateTimeOffset NextUpdateAt { get { return TimeUtil.UnixTimeStampToDateTime(NextUpdateAtUnix); } }
	}
}
