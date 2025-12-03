using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;   

public class PassengerSpawner : MonoBehaviour
{
    public GameObject passengerPrefab;
    public Transform gridParent;
    public int maxSlots = 5; 
    private List<GameObject> activePassengers = new List<GameObject>();
    public PassengerType[] passengerTypes;


    public void SpawnPassenger()
    {
        if (activePassengers.Count >= maxSlots) return;

        GameObject newPassenger = Instantiate(passengerPrefab, gridParent);

        Passenger passengerComponent = newPassenger.GetComponent<Passenger>();
        passengerComponent.passengerType = passengerTypes[Random.Range(0, passengerTypes.Length)];
        passengerComponent.spawner = this;

        // Get the Image component and set a random color
        /*Image img = newPassenger.GetComponent<Image>();

        if (img != null)
        {
            img.color = new Color(Random.value, Random.value, Random.value);
        }*/

        activePassengers.Add(newPassenger);
    }
    public void RemovePassenger(GameObject passenger)
    {
        if (activePassengers.Contains(passenger))
        {
            activePassengers.Remove(passenger);
            passenger.SetActive(false);
            //Destroy(passenger);

            // Reorder remaining passengers
            for (int i = 0; i < activePassengers.Count; i++)
            {
                activePassengers[i].transform.SetSiblingIndex(i);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       InvokeRepeating("SpawnPassenger", 1f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
