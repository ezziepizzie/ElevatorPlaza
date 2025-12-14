using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip backgroundMenu;
    public AudioClip backgroundGame;
    public AudioClip elevetorButtonPress;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            musicSource.clip = backgroundMenu;
        } 
        
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            musicSource.clip = backgroundGame;
        }

        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
