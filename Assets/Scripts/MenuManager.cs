using UnityEngine;

public class MenuManager : MonoBehaviour
{

    [Header("Settings")]
    public GameObject settingsMenuPrefab;
    public Canvas mainCanvas;

    private GameObject settingsMenuInstance;
    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleSettingsMenu();
        }
    }

    public void ToggleSettingsMenu()
    {
        if (!isOpen)
            OpenMenu();
        else
            CloseMenu();
    }

    void OpenMenu()
    {
        if (settingsMenuInstance != null) return;
        settingsMenuInstance = Instantiate(settingsMenuPrefab, mainCanvas.transform);
        settingsMenuInstance.transform.SetAsLastSibling();

        GameManager.instance.UpdateGameState(GameState.Paused);
        isOpen = true;
    }

    void CloseMenu()
    {
        if (settingsMenuInstance != null)
        {
            Destroy(settingsMenuInstance);
            settingsMenuInstance = null;
        }

        GameManager.instance.UpdateGameState(GameState.Active);
        isOpen = false;
    }

    public void ForceCloseState()
    {
        isOpen = false;
        settingsMenuInstance = null;
    }
}
