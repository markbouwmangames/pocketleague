using UnityEngine;
using RLSApi.Data;

[CreateAssetMenu(fileName = "PlatformData", menuName = "Data/PlatformData", order = 1)]
public class PlatformData : ScriptableObject {
	public string NameKey;
	public RlsPlatform Platform;
	public Sprite Icon;
}
