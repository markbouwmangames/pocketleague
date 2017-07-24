using RLSApi.Data;
using UnityEngine;
using UnityEngine.UI;

public class PlaylistPopulationDisplay : MonoBehaviour {
    [SerializeField]
    private Text _titleTextfield;
    [SerializeField]
    private Text _populationTextfield;
    [SerializeField]
    private Image _fillImage;

    public void Set(RlsPlaylist playlist, int population, int totalPopulation) {
        _titleTextfield.text = CopyDictionary.Get("PLAYLIST_" + playlist.ToString().ToUpper());
        _populationTextfield.text = population.ToString();

        var percentage = ((float)(population)) / ((float)(totalPopulation));
        _fillImage.fillAmount = percentage;
    }
}
