using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetttings : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    private void Start()
    {
        // Load saved values or default to full volume
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.value = musicVol;
        SFXSlider.value = sfxVol;

        setMusicVolume(musicVol);
        setSFXVolume(sfxVol);

        musicSlider.onValueChanged.AddListener(setMusicVolume);
        SFXSlider.onValueChanged.AddListener(setSFXVolume);
    }

    public void setMusicVolume(float value)
    {
        if (value <= 0f)
            value = 0.0001f;

        float dB = Mathf.Log10(value) * 20f;
        myMixer.SetFloat("music", dB);

        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void setSFXVolume(float value)
    {
        if (value <= 0f)
            value = 0.0001f;

        float dB = Mathf.Log10(value) * 20f;
        myMixer.SetFloat("sfx", dB);

        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}
