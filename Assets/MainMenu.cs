using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    public void SettingsApply()
    {
        // change settings here
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
