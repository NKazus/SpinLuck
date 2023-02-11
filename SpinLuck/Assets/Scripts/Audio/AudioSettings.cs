using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle vibroToggle;

    [Inject] private readonly SoundManager soundManager;

    #region MONO
    private void OnEnable()
    {
        musicToggle.onValueChanged.AddListener(TurnMisic);
        vibroToggle.onValueChanged.AddListener(TurnVibro);

        if (!PlayerPrefs.HasKey("_MusicEnabled"))
        {
            PlayerPrefs.SetInt("_MusicEnabled", 1);
            PlayerPrefs.SetInt("_VibroEnabled", 1);
        }
        musicToggle.isOn = (PlayerPrefs.GetInt("_MusicEnabled") == 1);
        vibroToggle.isOn = (PlayerPrefs.GetInt("_VibroEnabled") == 1);
    }

    private void OnDisable()
    {
        musicToggle.onValueChanged.RemoveListener(TurnMisic);
        vibroToggle.onValueChanged.RemoveListener(TurnVibro);

        PlayerPrefs.SetInt("_MusicEnabled", musicToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("_VibroEnabled", vibroToggle.isOn ? 1 : 0);
    }
    #endregion

    private void TurnMisic(bool isMusicOn)
    {
        soundManager.TurnMusic(isMusicOn);
    }

    private void TurnVibro(bool isMusicOn)
    {
        soundManager.TurnVibro(isMusicOn);
    }
}
