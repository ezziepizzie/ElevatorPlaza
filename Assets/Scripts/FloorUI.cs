using TMPro;
using UnityEngine;

public class FloorUI : MonoBehaviour
{
    public TextMeshProUGUI floorText;
    public TextMeshProUGUI nameText;
    public void SetText(Passenger passenger)
    {
        nameText.text = passenger.passengerType.passengerName;
        floorText.text = passenger.targetFloor + "F";
    }
}
