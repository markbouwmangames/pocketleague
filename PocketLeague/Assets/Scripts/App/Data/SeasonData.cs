using UnityEngine;
using RLSApi.Data;

[CreateAssetMenu(fileName = "SeasonData", menuName = "Data/SeasonData", order = 1)]
public class SeasonData : ScriptableObject {
	public RlsSeason Season;
	public RankData[] Ranks;
	public bool HasDivisions;
}