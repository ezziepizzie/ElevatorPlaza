using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    public GameObject returnToMenuButton;

    private void Start()
    {
        HandleReturnToMenuVisibility();
    }

    void HandleReturnToMenuVisibility()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Game")
        {
            returnToMenuButton.SetActive(true);
        }
        else
        {
            returnToMenuButton.SetActive(false);
        }
    }

    public void OnApplyPressed()
    {
        CloseMenu();
    }

    public void OnCancelPressed()
    {
        CloseMenu();
    }

    public void OnMainMenuPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    void CloseMenu()
    {
        GameManager.instance.UpdateGameState(GameState.Active);
        FindObjectOfType<MenuManager>().ForceCloseState();
        Destroy(gameObject);
    }
}
