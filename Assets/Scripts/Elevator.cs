using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems; 

public class Elevator : MonoBehaviour, IDropHandler
{
    public PassengerSpawner spawner;
    public int MaxCapacity = 4;
    [HideInInspector] public int currentCapacity;
    [HideInInspector] public List<Passenger> passengerList = new List<Passenger>();
    [HideInInspector] public List<int> floorsList = new List<int>();
    [SerializeField] private TextMeshProUGUI capacityText;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Passenger passenger = dropped.GetComponent<Passenger>();
        Draggable draggable = dropped.GetComponent<Draggable>();

        if (passenger == null) return;

        if (currentCapacity + passenger.passengerType.passengerAmount > MaxCapacity)
        {
            return;
        }

        draggable.transform.SetParent(null);

        passengerList.Add(passenger);
        floorsList.Add((int)passenger.targetFloor);
        floorsList.Sort();
        Debug.Log("Floors in elevator: " + string.Join(", ", floorsList));
        spawner.RemovePassenger(dropped);

        currentCapacity += passenger.passengerType.passengerAmount;

        capacityText.text = currentCapacity + " / " + MaxCapacity;
        //Debug.Log("Current capacity: " + currentCapacity);
    }

    public void ResetElevator()
    {
        currentCapacity = 0;
        capacityText.text = "0 / " + MaxCapacity;
        passengerList.Clear();
        floorsList.Clear();
        //Destroy(passenger);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        capacityText.text = "0 / " + MaxCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
