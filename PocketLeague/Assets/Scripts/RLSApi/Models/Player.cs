using System;
using UnityEngine;
using RLSApi.Data;
using System.Collections.Generic;

namespace RLSApi.Models {
	[Serializable]
	public class Player {
		public string uniqueId;
		public string displayName;
		public Platform platform;
		public string avatar;
		public string profileUrl;
		public string signatureUrl;
		public PlayerStats stats;
		public Dictionary<RlsSeason, Dictionary<RlsPlaylistRanked, PlayerRank>> rankedSeasons;
		public uint lastRequested;
		public uint createdAt;
		public uint updatedAt;
		public uint nextUpdateAt;
	}
}
