using System;
using RLSApi.Util;

namespace RLSApi.Models {
	[Serializable]
	public class Season {
		public int seasonId;
		public long startedOn;
		public long? endedOn;

		public DateTimeOffset GetStartDate() {
			return TimeUtil.UnixTimeStampToDateTime(startedOn);
		}

		public DateTimeOffset GetEndDate() {
			return endedOn.HasValue ? TimeUtil.UnixTimeStampToDateTime(endedOn.Value) : default(DateTimeOffset);
		}
	}
}
