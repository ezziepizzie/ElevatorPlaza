using TMPro;
using UnityEngine;
using UnityEngine.UI;  

public class Passenger : MonoBehaviour
{
    public PassengerSpawner spawner;
    public PassengerType passengerType;
    public Image passengerImage;
    public float currentPatienceLevel;
    public int targetFloor;

    public TextMeshProUGUI floorText;
    public TextMeshProUGUI patienceText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        passengerImage.sprite = passengerType.passengerSprite;
        currentPatienceLevel = passengerType.patienceLevel;
        targetFloor = Random.Range(passengerType.minFloor, passengerType.maxFloor + 1); 
        floorText.text = targetFloor.ToString() + "F";
        patienceText.text = Mathf.CeilToInt(currentPatienceLevel).ToString() + "s";
    }

    // Update is called once per frame
    void Update()
    {
        currentPatienceLevel -= Time.deltaTime;
        patienceText.text = Mathf.CeilToInt(Mathf.Max(currentPatienceLevel, 0)).ToString() + "s"; 

        if (currentPatienceLevel <= 0)
        {
            spawner.RemovePassenger(gameObject);
        }
    }
}
