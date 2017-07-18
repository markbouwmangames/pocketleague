using System;

namespace RLSApi.Models {
	[Serializable]
	public class PlayerRank {
		public int rankPoints;
		public int? matchesPlayed;
		public int? tier;
		public int? division;
	}
}
