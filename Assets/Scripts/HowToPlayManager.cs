using UnityEngine;
using UnityEngine.UI;

public class HowToPlayManager : MonoBehaviour
{
    public Canvas howToPlayCanvas;
    public Image objectivesImage;
    public Image elevatorsImage;
    public Image passengersImage;
    public Image eventsImage;

    private void Awake()
    {
        howToPlayCanvas.gameObject.SetActive(false);

        objectivesImage.gameObject.SetActive(false);
        elevatorsImage.gameObject.SetActive(false);
        passengersImage.gameObject.SetActive(false);
        eventsImage.gameObject.SetActive(false);
    }
    public void ShowObjectivesScreen()
    {
        objectivesImage.gameObject.SetActive(true);
        elevatorsImage.gameObject.SetActive(false);
        passengersImage.gameObject.SetActive(false);
        eventsImage.gameObject.SetActive(false);
    }

    public void ShowElevatorsScreen()
    {
        objectivesImage.gameObject.SetActive(false);
        elevatorsImage.gameObject.SetActive(true);
        passengersImage.gameObject.SetActive(false);
        eventsImage.gameObject.SetActive(false);
    }

    public void ShowPassengersScreen()
    {
        objectivesImage.gameObject.SetActive(false);
        elevatorsImage.gameObject.SetActive(false);
        passengersImage.gameObject.SetActive(true);
        eventsImage.gameObject.SetActive(false);
    }

    public void ShowEventsScreen()
    {
        objectivesImage.gameObject.SetActive(false);
        elevatorsImage.gameObject.SetActive(false);
        passengersImage.gameObject.SetActive(false);
        eventsImage.gameObject.SetActive(true);
    }
    public void OpenHowToPlay()
    {
        howToPlayCanvas.gameObject.SetActive(true);
        ShowObjectivesScreen();
    }

    public void CloseHowToPlay()
    {
        howToPlayCanvas.gameObject.SetActive(false);
    }
}
