using RLSApi.Net.Models;
using UnityEngine.UI;
using UnityEngine;

public class PlaystyleView : PlayerViewChild {
	[SerializeField]
	private Image goalsCircle;
	[SerializeField]
	private Image savesCircle;
	[SerializeField]
	private Image assistsCircle;

	public override void Set(Player player) {
		var stats = player.Stats;
	
		float goals = stats.Goals;
		float saves = stats.Saves;
		float assists = stats.Assists;

		float total = goals + saves + assists;

		var goalPercentage = goals / total;
		var savesPercentage = saves / total;
		var assistPercentage = assists / total;

		Debug.Log(goalPercentage);
		Debug.Log(savesPercentage);
		Debug.Log(assistPercentage);

		goalsCircle.fillAmount = goalPercentage;

		savesCircle.transform.localEulerAngles = new Vector3(0, 0, goalPercentage * -360f);
		savesCircle.fillAmount = savesPercentage;

		assistsCircle.transform.localEulerAngles = new Vector3(0, 0, (goalPercentage + savesPercentage) * -360f);
		assistsCircle.fillAmount = assistPercentage;
	}
}
