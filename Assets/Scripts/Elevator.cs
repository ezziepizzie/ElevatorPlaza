using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Elevator : MonoBehaviour, IDropHandler
{
    public PassengerSpawner spawner;

    [Header("Floor List UI")]
    public Transform floorListGridParent;
    public GameObject floorTextPrefab;

    [Header("Elevator Settings")]
    public int MaxCapacity = 4;
    public float travelTime = 1.0f;
    public float unloadingTime = 0.5f;

    [HideInInspector] public bool isActive = true;
    [HideInInspector] public int currentCapacity;
    [HideInInspector] public int currentFloor = 0;
    [HideInInspector] public List<Passenger> passengerList = new List<Passenger>();
    [SerializeField] private TextMeshProUGUI capacityText;
    [SerializeField] private TextMeshProUGUI floorText;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedPassenger = eventData.pointerDrag;
        Passenger passenger = droppedPassenger.GetComponent<Passenger>();
        Draggable draggable = droppedPassenger.GetComponent<Draggable>();

        if (draggable != null)
            Destroy(draggable.passengerSprite);

        if (passenger == null || !isActive) return;

        if (currentCapacity + passenger.passengerType.passengerAmount > MaxCapacity)
        {
            return;
        }

        draggable.transform.SetParent(null);

        passengerList.Add(passenger);
        passengerList = passengerList.OrderBy(p => p.targetFloor).ToList();

        spawner.RemovePassenger(droppedPassenger);

        currentCapacity += passenger.passengerType.passengerAmount;

        capacityText.text = currentCapacity + " / " + MaxCapacity;

        UpdateFloorTextUI();
    }

    void Start()
    {
        capacityText.text = currentCapacity + " / " + MaxCapacity;
    }

    public void UpdateFloorTextUI()
    {
        foreach (Transform child in floorListGridParent)
            Destroy(child.gameObject);


        foreach (Passenger passenger in passengerList)
        {
            GameObject floorText = Instantiate(floorTextPrefab, floorListGridParent);
            floorText.GetComponentInChildren<TextMeshProUGUI>().text = passenger.targetFloor + "F";
        }
    }

    private IEnumerator MoveElevatorUp()
    {
        isActive = false;

        yield return new WaitForSeconds(travelTime);

        while (passengerList.Count > 0)
        {
            Passenger nextPassenger = passengerList[0];

            currentFloor++;
            floorText.text = currentFloor + "F";

            yield return new WaitForSeconds(travelTime);

            var passengersToUnload = passengerList.Where(p => p.targetFloor == currentFloor).ToList();

            foreach (Passenger passenger in passengersToUnload)
            {
                currentCapacity -= passenger.passengerType.passengerAmount;
                capacityText.text = currentCapacity + " / " + MaxCapacity;

                passengerList.Remove(passenger);
                Destroy(passenger.gameObject);
                UpdateFloorTextUI();

                yield return new WaitForSeconds(unloadingTime);
            }

            // Debug.Log(passengerList.Count);
        }

        StartCoroutine(MoveElevatorDown());
    }

    public IEnumerator MoveElevatorDown()
    {
        while (currentFloor != 0)
        {
            currentFloor--;

            if (currentFloor == 0)
                floorText.text = "GF";
            else
                floorText.text = currentFloor + "F";

            yield return new WaitForSeconds(travelTime);
        }

        isActive = true;
    }
}
