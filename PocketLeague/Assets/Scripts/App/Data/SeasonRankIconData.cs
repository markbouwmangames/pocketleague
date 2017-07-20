using UnityEngine;
using RLSApi.Data;

[CreateAssetMenu(fileName = "SeasonRankIconData", menuName = "Data/SeasonRankIconData", order = 1)]
public class SeasonRankIconData : ScriptableObject {
	public Sprite[] RankIcons;
}