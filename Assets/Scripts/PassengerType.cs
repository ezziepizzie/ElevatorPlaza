using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewPassengerType", menuName = "Scriptable Objects/Passenger Type")]
public class PassengerType : ScriptableObject
{
    public string passengerName;
    public Sprite passengerIcon;
    public Sprite passengerSprite;
    public int passengerAmount;
    public float patienceLevel;

    public int minFloor;
    public int maxFloor;

    [Header("Special Rules")]
    public bool isVIP;
    public bool isKid;
    public bool isCourier;
    public int minDeliveryFloors;
    public int maxDeliveryFloors;
    //public int courierFloorsCount;
}
