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
    public AudioClip elevatorButtonPress;
    public AudioClip elevatorDirtSplat;
    public AudioClip elevatorCleaning;
    public AudioClip elevatorCleanDing;
    public AudioClip elevatorBreakdown;
    public AudioClip elevatorFixing;
    public AudioClip elevatorActive;
    public AudioClip passengerDrag;

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
    public void PlayLoopSFX(AudioClip clip)
    {
        if (SFXSource.clip == clip && SFXSource.isPlaying)
            return; 

        SFXSource.clip = clip;
        SFXSource.loop = true;
        SFXSource.Play();
    }

    public void StopLoopSFX()
    {
        SFXSource.loop = false;
        SFXSource.Stop();
    }
}
