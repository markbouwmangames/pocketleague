using System;
using RLSApi.Data;

namespace RLSApi.Models {
	[Serializable]
	public class Playlist {
		public RlsPlaylist id;
		public RlsPlatform platformId;
		public string name;
		public PlaylistPopulation population;
	}
}
