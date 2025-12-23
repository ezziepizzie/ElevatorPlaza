using TMPro;
using UnityEngine;
using System.Linq;

public class FloorUI : MonoBehaviour
{
    public TextMeshProUGUI floorText;
    public TextMeshProUGUI nameText;
    public void SetText(Passenger passenger)
    {
        nameText.text = passenger.passengerType.passengerName;

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
