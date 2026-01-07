using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;  

public class Passenger : MonoBehaviour
{
    public PassengerSpawner spawner;
    public PassengerType passengerType;
    public Image passengerImage;
    public float maxPatienceLevel;
    public float currentPatienceLevel;
    //public int targetFloor;
    public List<int> targetFloors = new List<int>();

    public Slider patienceMeter;
    public TextMeshProUGUI floorText;
    private Draggable draggable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        passengerImage.sprite = passengerType.passengerIcon;
        maxPatienceLevel = passengerType.patienceLevel;
        currentPatienceLevel = passengerType.patienceLevel;
        draggable = GetComponent<Draggable>();

        /*targetFloor = Random.Range(passengerType.minFloor, passengerType.maxFloor + 1); 

        if (passengerType.isKid)
        {
            floorText.text = "?";
        }
        else
        {
            floorText.text = targetFloor.ToString() + "F";
        }*/

        GetPassengerFloors();
        UpdateFloorText();

        patienceMeter.maxValue = maxPatienceLevel;
        patienceMeter.value = currentPatienceLevel;
    }

    // Update is called once per frame
    void Update()
    {
        currentPatienceLevel -= Time.deltaTime;
        patienceMeter.value = currentPatienceLevel;

        UpdatePatienceMeterColor();

        if (currentPatienceLevel <= 0)
        {
            GameManager.instance.LosePassengerScore(this);
            spawner.RemovePassenger(gameObject);
            draggable.DestroySprite();
        }

    }

    public void UpdatePatienceMeterColor()
    {
        float patienceRatio = currentPatienceLevel / maxPatienceLevel;

        if (patienceRatio > 0.5f)
        {
            patienceMeter.fillRect.GetComponent<Image>().color = Color.green;
        }
        else if (patienceRatio > 0.2f)
        {
            patienceMeter.fillRect.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            patienceMeter.fillRect.GetComponent<Image>().color = Color.red;
        }
    }

    public void GetPassengerFloors()
    {
        targetFloors.Clear();

        int stopCount = 1;
        //int stopCount = passengerType.isCourier? passengerType.courierFloorsCount: 1;

        if (passengerType.isCourier)
        {
            stopCount = Random.Range(passengerType.minDeliveryFloors, passengerType.maxDeliveryFloors + 1);
        }

        while (targetFloors.Count < stopCount)
        {
            int floor = Random.Range(passengerType.minFloor, passengerType.maxFloor + 1);
            if (!targetFloors.Contains(floor))
            {
                targetFloors.Add(floor);
            }
        }

        targetFloors.Sort();
    }

    public void UpdateFloorText()
    {
        if (passengerType.isKid)
        {
            floorText.text = "?";
            return;
        }

        if (targetFloors.Count == 1)
        {
            floorText.text = targetFloors[0].ToString() + "F";
        }
        else
        {
            floorText.text = string.Join(", ", targetFloors.Select(f => f + "F"));
        }
    }
}
