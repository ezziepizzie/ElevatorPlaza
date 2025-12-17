using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Elevator : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public PassengerSpawner spawner;
    [SerializeField] private TextMeshProUGUI capacityText;
    [SerializeField] private TextMeshProUGUI floorText;
    [SerializeField] private Animator animator;

    [Header("Floor List UI")]
    public Transform floorListGridParent;
    public GameObject floorTextPrefab;

    [Header("Elevator Settings")]
    public int MaxCapacity = 4;
    public float travelTime = 1.0f;
    public float unloadingTime = 0.5f;

    [Header("Broken Elevator Settings")]
    public bool isBroken = false;
    public TextMeshProUGUI brokenText;
    public Slider fixMeter;
    public int fixTapsRequired;

    [HideInInspector] public bool isActive = true;
    [HideInInspector] public int currentCapacity;
    [HideInInspector] public int currentFloor = 0;
    [HideInInspector] public List<Passenger> passengerList = new List<Passenger>();

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedPassenger = eventData.pointerDrag;
        Passenger passenger = droppedPassenger.GetComponent<Passenger>();
        Draggable draggable = droppedPassenger.GetComponent<Draggable>();

        if (draggable != null)
            Destroy(draggable.passengerSprite);

        if (passenger == null || !isActive) return;

        // VIP logic
        if (passenger.passengerType.isVIP)
        {
            if(currentCapacity > 0)
            {
                return;
            }
        }

        else
        {
            if(passengerList.Any(p => p.passengerType.isVIP))
            {
                return;
            }
        }

        if (currentCapacity + passenger.passengerType.passengerAmount > MaxCapacity)
        {
            return;
        }

        // Successfully dropped passenger into elevator
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

        fixMeter.gameObject.SetActive(false);
        brokenText.gameObject.SetActive(false);
    }

    public void UpdateFloorTextUI()
    {
        foreach (Transform child in floorListGridParent)
            Destroy(child.gameObject);


        foreach (Passenger passenger in passengerList)
        {
            GameObject floorUI = Instantiate(floorTextPrefab, floorListGridParent);
            floorUI.GetComponent<FloorUI>().SetText(passenger);

            /*if (passenger.passengerType.isKid)
            {
                floorText.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            }

            //else
            //{
                floorUI.GetComponentInChildren<TextMeshProUGUI>().text = passenger.targetFloor + "F";
            //}*/
        }
    }

    private IEnumerator MoveElevatorUp()
    {
        isActive = false;

        animator.SetTrigger("doorClosing");
        yield return new WaitForSeconds(0.3f);

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

        animator.SetTrigger("doorOpening");
        yield return new WaitForSeconds(0.3f);
        isActive = true;
    }

    public void BreakElevator()
    {
        if (isBroken == false)
        {
            isBroken = true;
            isActive = false;
            fixMeter.gameObject.SetActive(true);
            brokenText.gameObject.SetActive(true);
            fixMeter.value = 0;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isBroken)
        {
            fixMeter.value += fixMeter.maxValue / fixTapsRequired;
            Debug.Log("Fix meter: " + fixMeter.value);

            if (fixMeter.value >= 1f)
            {
                isBroken = false;
                isActive = true;
                fixMeter.gameObject.SetActive(false);
                brokenText.gameObject.SetActive(false);
            }
        }
    }
}
