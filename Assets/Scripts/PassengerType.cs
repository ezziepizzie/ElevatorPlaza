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

    [Header("Spawn Control")]
    public bool isSpecialType;
    public int minPassengersBeforeSpawn = 3;
    public int maxPassengersBeforeSpawn = 8;

    [Header("Scoring")]
    public int baseScore = 10;
    public int patiencePenalty = 5;

}
