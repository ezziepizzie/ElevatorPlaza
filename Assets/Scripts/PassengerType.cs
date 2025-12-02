using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewPassengerType", menuName = "Scriptable Objects/Passenger Type")]
public class PassengerType : ScriptableObject
{
    public string passengerType;
    public Sprite passengerSprite;
    public float patienceLevel;

    public int minFloor;
    public int maxFloor;
}
