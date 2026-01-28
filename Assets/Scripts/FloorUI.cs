using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class FloorUI : MonoBehaviour
{
    public TextMeshProUGUI floorText;
    //public TextMeshProUGUI nameText;
    public Image image;

    public void UpdateUI(Passenger passenger)
    {
        //nameText.text = passenger.passengerType.passengerName;
        image.sprite = passenger.passengerType.passengerIcon;

        if (passenger.targetFloors.Count == 1)
        {
            floorText.text = passenger.targetFloors[0] + "F";
        }
        else
        {
            floorText.text = string.Join(", ", passenger.targetFloors.Select(f => f + "F"));
        }
    }
}
