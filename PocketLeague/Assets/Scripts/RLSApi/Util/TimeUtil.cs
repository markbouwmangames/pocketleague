using System;

namespace RLSApi.Util {
	internal static class TimeUtil {
		private static DateTimeOffset _epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);

		public static DateTimeOffset UnixTimeStampToDateTime(double unixTimeStamp) {
			return _epoch.AddSeconds(unixTimeStamp);
		}

		public static float Difference(DateTimeOffset a, DateTimeOffset b) {
			return (float)(b - a).TotalSeconds;
		}
	}
}
