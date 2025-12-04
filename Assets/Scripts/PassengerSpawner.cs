using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    public GameObject passengerPrefab;
    public Transform gridParent;
    public int maxSlots = 5;
    private List<GameObject> activePassengers = new List<GameObject>();

    public PassengerType[] passengerTypes;

    [Header("Spawn Rate Settings")]
    public float minSpawnTime = 3f;
    public float maxSpawnTime = 9f;

    public float spawnRateMultiplier = 1f;

    private Coroutine spawnRoutine;

    void Start()
    {
        StartSpawning();
    }

    // call this to START the random spawning
    public void StartSpawning()
    {
        if (spawnRoutine != null) StopCoroutine(spawnRoutine);
        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    // call this to STOP the random spawning
    public void StopSpawning() 
    {
        if (spawnRoutine != null) StopCoroutine(spawnRoutine);
        spawnRoutine = null;
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            float delay = Random.Range(minSpawnTime, maxSpawnTime);
            delay /= spawnRateMultiplier; // faster during Rush Hour

            yield return new WaitForSeconds(delay);

            SpawnPassenger();
        }
    }

    public void SpawnPassenger()
    {
        if (activePassengers.Count >= maxSlots) return;

        GameObject newPassenger = Instantiate(passengerPrefab, gridParent);

        Passenger passenger = newPassenger.GetComponent<Passenger>();
        passenger.passengerType = passengerTypes[Random.Range(0, passengerTypes.Length)];
        passenger.spawner = this;


        activePassengers.Add(newPassenger);
        Debug.Log("[SPAWNER] " + passenger.passengerType.name + " Spawned");
    }

    public void RemovePassenger(GameObject passenger)
    {
        if (activePassengers.Contains(passenger))
        {
            activePassengers.Remove(passenger);
            passenger.SetActive(false);
            //Destroy(passenger);

            // reorder UI
            for (int i = 0; i < activePassengers.Count; i++)
                activePassengers[i].transform.SetSiblingIndex(i);
        }
    }

    // rush hour controller
    public void SetRushHour(bool active) 
    {
        spawnRateMultiplier = active ? 2f : 1f;

        // restart spawn loop with new rate
        StartSpawning();
    }
}
