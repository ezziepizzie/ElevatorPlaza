using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    public GameObject passengerPrefab;
    public Transform gridParent;
    public int maxSlots = 5;

    public PassengerType[] passengerTypes;

    [Header("Spawn Rate Settings")]
    public float minSpawnTime = 3f;
    public float maxSpawnTime = 9f;
    public float spawnRateMultiplier = 1f;

    private List<GameObject> activePassengers = new List<GameObject>();
    private Dictionary<PassengerType, int> spawnCounters = new Dictionary<PassengerType, int>();

    private Coroutine spawnRoutine;

    void Start()
    {
        InitializeCounters();
        StartSpawning();
    }

    public void InitializeCounters()
    {
        foreach (PassengerType type in passengerTypes)
        {
            spawnCounters[type] = type.isSpecialType
                ? Random.Range(0, type.maxPassengersBeforeSpawn) // randomize start
                : 0;
        }
    }

    public void StartSpawning()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        spawnRoutine = null;
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            float delay = Random.Range(minSpawnTime, maxSpawnTime) / spawnRateMultiplier;
            yield return new WaitForSeconds(delay);

            SpawnPassenger();
        }
    }

    public void SpawnPassenger()
    {
        if (activePassengers.Count >= maxSlots)
            return;

        PassengerType chosenType = ChoosePassengerType();

        GameObject newPassenger = Instantiate(passengerPrefab, gridParent);
        Passenger passenger = newPassenger.GetComponent<Passenger>();

        passenger.passengerType = chosenType;
        passenger.spawner = this;

        activePassengers.Add(newPassenger);

        IncrementCounters(chosenType);

        Debug.Log($"[SPAWN] {chosenType.passengerName}");
    }

    PassengerType ChoosePassengerType()
    {
        // 1️⃣ Guaranteed special spawn (max reached)
        foreach (PassengerType type in passengerTypes)
        {
            if (!type.isSpecialType) continue;

            if (spawnCounters[type] >= type.maxPassengersBeforeSpawn)
            {
                ResetCounter(type);
                return type;
            }
        }

        // 2️⃣ Eligible pool
        List<PassengerType> validTypes = new List<PassengerType>();

        foreach (PassengerType type in passengerTypes)
        {
            if (!type.isSpecialType)
            {
                validTypes.Add(type);
                continue;
            }

            if (spawnCounters[type] >= type.minPassengersBeforeSpawn)
            {
                validTypes.Add(type);
            }
        }

        PassengerType selected = validTypes[Random.Range(0, validTypes.Count)];

        if (selected.isSpecialType)
            ResetCounter(selected);

        return selected;
    }

    void IncrementCounters(PassengerType spawnedType)
    {
        foreach (PassengerType type in passengerTypes)
        {
            if (type != spawnedType)
                spawnCounters[type]++;
        }
    }

    void ResetCounter(PassengerType type)
    {
        spawnCounters[type] = 0;
    }

    public void RemovePassenger(GameObject passenger)
    {
        if (!activePassengers.Contains(passenger)) return;

        activePassengers.Remove(passenger);
        passenger.SetActive(false);

        for (int i = 0; i < activePassengers.Count; i++)
            activePassengers[i].transform.SetSiblingIndex(i);
    }

    public void SetRushHour(bool active)
    {
        spawnRateMultiplier = active ? 2f : 1f;
        StartSpawning();
    }

    public void ResetSpawner()
    {
        StopSpawning();
        foreach (GameObject passenger in activePassengers)
        {
            Destroy(passenger);
        }
        activePassengers.Clear();
    }
}
