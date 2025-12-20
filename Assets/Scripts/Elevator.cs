using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Elevator : MonoBehaviour, IDropHandler, IPointerClickHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public PassengerSpawner spawner;
    [SerializeField] private TextMeshProUGUI capacityText;
    [SerializeField] private TextMeshProUGUI floorText;
    [SerializeField] private Animator elevatorDoorAnim;

    [Header("Floor List UI")]
    public Transform floorListGridParent;
    public GameObject floorTextPrefab;

    [Header("Elevator Settings")]
    public int MaxCapacity = 4;
    public float travelTime = 1.0f;
    public float unloadingTime = 0.5f;

    [Header("Broken Elevator Settings")]
    public bool isBroken = false;
    public GameObject brokenSign;
    public Slider fixMeter;
    public int fixTapsRequired;

    [Header("Cleaning Settings")]
    public bool isScrubbing = false;
    public Slider dirtyMeter;
    public bool isDirty => dirtyMeter.value > 0f;
    [SerializeField] private float cleanRate = 0.25f;

    [HideInInspector] public bool isActive = true;
    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public int currentCapacity;
    [HideInInspector] public int currentFloor = 0;
    [HideInInspector] public List<Passenger> passengerList = new List<Passenger>();


    public void OnDrop(PointerEventData eventData)
    {
        ToolType currentTool = GameManager.instance.currentTool;

        if (currentTool != ToolType.Hand)
            return;

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
        //passengerList = passengerList.OrderBy(p => p.targetFloor).ToList();
        passengerList = passengerList.OrderBy(p => p.targetFloors.Min()).ToList();

        spawner.RemovePassenger(droppedPassenger);

        currentCapacity += passenger.passengerType.passengerAmount;

        capacityText.text = currentCapacity + " / " + MaxCapacity;

        UpdateFloorTextUI();
    }

    void Start()
    {
        capacityText.text = currentCapacity + " / " + MaxCapacity;

        fixMeter.gameObject.SetActive(false);
        brokenSign.gameObject.SetActive(false);

        dirtyMeter.value = 0f;
    }

    public void UpdateFloorTextUI()
    {
        foreach (Transform child in floorListGridParent)
            Destroy(child.gameObject);


        foreach (Passenger passenger in passengerList)
        {
            GameObject floorUI = Instantiate(floorTextPrefab, floorListGridParent);
            floorUI.GetComponent<FloorUI>().SetText(passenger);

            // uncomment if we wanna hide the kid's target floor

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
        isMoving = true;

        elevatorDoorAnim.SetTrigger("doorClosing");
        yield return new WaitForSeconds(0.5f);

        while (passengerList.Count > 0)
        {
            while (isBroken)
            {
                yield return null;
            }

            yield return new WaitForSeconds(travelTime);

            currentFloor++;
            floorText.text = currentFloor + "F";

            //var passengersToUnload = passengerList.Where(p => p.targetFloor == currentFloor).ToList();
            var passengersToUnload = passengerList.Where(p => p.targetFloors.Contains(currentFloor)).ToList();

            foreach (Passenger passenger in passengersToUnload)
            {
                while (isBroken)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(unloadingTime);

                /*currentCapacity -= passenger.passengerType.passengerAmount;
                capacityText.text = currentCapacity + " / " + MaxCapacity;

                passengerList.Remove(passenger);
                Destroy(passenger.gameObject);
                UpdateFloorTextUI();*/

                passenger.targetFloors.Remove(currentFloor);
                
                if (passenger.targetFloors.Count > 0)
                {
                    UpdateFloorTextUI();
                    continue;
                }

                currentCapacity -= passenger.passengerType.passengerAmount;
                capacityText.text = currentCapacity + " / " + MaxCapacity;
                passengerList.Remove(passenger);
                Destroy(passenger.gameObject);
                UpdateFloorTextUI();
            }
        }

        while (isBroken)
        {
            yield return null;
        }

        StartCoroutine(MoveElevatorDown());
    }

    public IEnumerator MoveElevatorDown()
    {
        while (currentFloor != 0)
        {
            yield return new WaitForSeconds(travelTime);

            while (isBroken)
            {
                yield return null;
            }

            currentFloor--;

            if (currentFloor == 0)
                floorText.text = "GF";
            else
                floorText.text = currentFloor + "F";

            //yield return new WaitForSeconds(travelTime);
        }

        if(!isBroken)
            elevatorDoorAnim.SetTrigger("doorOpening");

        yield return new WaitForSeconds(0.5f);
        isActive = true;
        isMoving = false;
    }

    public void BreakElevator()
    {
        if (isBroken == false)
        {
            isBroken = true;
            isActive = false;
            brokenSign.gameObject.SetActive(true);
            fixMeter.value = 0;

            if(!isMoving)
                elevatorDoorAnim.SetTrigger("doorClosing");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.instance.currentTool != ToolType.Hammer)
            return;

        if (isBroken)
            FixElevator();
    }

    public void FixElevator()
    {
       fixMeter.value += fixMeter.maxValue / fixTapsRequired;
       Debug.Log("Fix meter: " + fixMeter.value);
       fixMeter.gameObject.SetActive(true);

       if (fixMeter.value >= 1f)
       {
            isBroken = false;
            isActive = true;
            fixMeter.gameObject.SetActive(false);
            brokenSign.gameObject.SetActive(false);

            if (!isMoving)
                elevatorDoorAnim.SetTrigger("doorOpening");
       }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.instance.currentTool != ToolType.Sponge)
            return;

        isScrubbing = true;

        Debug.Log("Scrubbing Started");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isScrubbing)
            return;

        if (GameManager.instance.currentTool != ToolType.Sponge)
            return;

        dirtyMeter.value -= cleanRate * Time.deltaTime;
        dirtyMeter.value = Mathf.Clamp01(dirtyMeter.value);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isScrubbing)
            return;

        isScrubbing = false;
    }

    public void AddDirtiness(float amount)
    {
        dirtyMeter.value = Mathf.Clamp01(dirtyMeter.value + amount);
    }    
}
