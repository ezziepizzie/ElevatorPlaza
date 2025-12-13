using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewPassengerType", menuName = "Scriptable Objects/Passenger Type")]
public class PassengerType : ScriptableObject
{
    public string passengerType;
    public Sprite passengerIcon;
    public Sprite passengerSprite;
    public int passengerAmount;
    public float patienceLevel;

    public int minFloor;
    public int maxFloor;

    [Header("Special Rules")]
    public bool isVIP;
    public bool isKid;
}
