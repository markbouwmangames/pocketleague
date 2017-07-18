﻿using System;
using Newtonsoft.Json;
using RLSApi.Util;

namespace RLSApi.Net.Models {
	public class Season {
		[JsonProperty("seasonId", Required = Required.Always)]
		public int SeasonId { get; set; }

		[JsonProperty("startedOn", Required = Required.Always)]
		public long StartedOnUnix { get; set; }

		[JsonIgnore]
		public DateTimeOffset StartedOn { get { return TimeUtil.UnixTimeStampToDateTime(StartedOnUnix); } }

		/// <summary>
		///     <see cref="EndedOnUnix"/> is <code>null</code> if the season has not ended yet.
		/// </summary>
		[JsonProperty("endedOn", Required = Required.AllowNull)]
		public long? EndedOnUnix { get; set; }

		[JsonIgnore]
		public DateTimeOffset EndedOn { get { return EndedOnUnix.HasValue ? TimeUtil.UnixTimeStampToDateTime(EndedOnUnix.Value) : default(DateTimeOffset); } }
	}
}
