using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public GameState state;

    public static event Action<GameState> OnGameStateChange;

    [Header("Spawning")]
    public PassengerSpawner passengerSpawner;

    [Header("Day Timer")]
    public float dayDuration = 60f; // SECONDS. 5 mins is kinda goated, has to be odd, 1 min rush hour
    private float dayTimer;

    [Header("Rush Hour")]
    public float rushHourStartPercent = 0.5f; // PERCENTAGE. halfway through day
    public float rushHourDuration = 60f; // SECONDS. how long rush hour lasts. 1 min
    private float rushHourTimer;
    private bool rushHourTriggered = false;

    [Header("Score")]
    public int currentScore = 0;
    public int targetScore = 100; // changer per level, log curve? or expo?

    [Header("Level")]
    public int currentLevel = 1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.Loading);
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.Loading:
                HandleLoading();
                break;
            case GameState.Active:
                HandleActive();
                break;
            case GameState.RushHour:
                HandleRushHour();
                break;
            case GameState.Paused:
                HandlePause();
                break;
            case GameState.DayEnd:
                HandleDayEnd();
                break;
            case GameState.DayWin:
                HandleDayWin();
                break;
            case GameState.DayLose:
                HandleDayLose();
                break;
            case GameState.Cleaning:
                HandleCleaning();
                break;
        }

        OnGameStateChange?.Invoke(newState);

    }

    private void HandleLoading()
    {
        Debug.Log("[STATE] Loading");
        Time.timeScale = 1;
        dayTimer = dayDuration;
        rushHourTimer = rushHourDuration;
        rushHourTriggered = false;
        currentScore = 0;
        passengerSpawner.StopSpawning();
        passengerSpawner.StartSpawning();

        // to add dirtyness



        UpdateGameState(GameState.Active);
    }

    private void HandleActive()
    {
        Debug.Log("[STATE] Active");
        Time.timeScale = 1;

        passengerSpawner.StartSpawning();
        passengerSpawner.SetRushHour(false);
    }

    private void HandleRushHour()
    {
        Debug.Log("[STATE] Rush Hour Start");
        passengerSpawner.SetRushHour(true);
    }

    private void HandlePause()
    {
        Debug.Log("[STATE] Pause");
        Time.timeScale = 0;
        passengerSpawner.StopSpawning();
    }

    private void HandleDayEnd()
    {
        Debug.Log("[STATE] Day End");
        Time.timeScale = 0;
        passengerSpawner.StopSpawning();

        if (currentScore >= targetScore)
            UpdateGameState(GameState.DayWin);
        else
            UpdateGameState(GameState.DayLose);
    }

    private void HandleDayWin()
    {
        Debug.Log("[STATE] Day Win");
        currentLevel++;
    }

    private void HandleDayLose()
    {
        Debug.Log("[STATE] Day Lose");
    }

    private void HandleCleaning()
    {
        Debug.Log("[STATE] Cleaning");
        // these will pause game and stop passenger spawning when toggled
        // Time.timeScale = 0;
        // passengerSpawner.CancelInvoke("SpawnPassenger");
    }

    private void Update()
    {
        // handle all the timers
        if (state == GameState.Active || state == GameState.RushHour)
            HandleTimers();
    }

    private void HandleTimers()
    {
        // Day Timer
        dayTimer -= Time.deltaTime;


        if (dayTimer <= 0)
        {
            UpdateGameState(GameState.DayEnd);
            return;
        }


        // RUSH HOUR SECTION
        float rushStart = dayDuration * rushHourStartPercent;

        if (!rushHourTriggered && dayTimer <= rushStart)
        {
            rushHourTriggered = true;
            UpdateGameState(GameState.RushHour);
        }

        // RUSH HOUR TIMER
        if (state == GameState.RushHour)
        {
            rushHourTimer -= Time.deltaTime;

            if (rushHourTimer <= 0)
            {
                passengerSpawner.SetRushHour(false);
                UpdateGameState(GameState.Active);
                Debug.Log("[STATE] Rush Hour End");
            }
        }
    }

    // score for future use
    public void AddScore(int amount)
    {
        currentScore += amount;
        Debug.Log("Score +" + amount + " | Total = " + currentScore);
    }

    public void AddCleaningScore(int amount)
    {
        AddScore(amount);
    }

    public void AddRepairScore(int amount)
    {
        AddScore(amount);
    }

}

public enum GameState
{
    Loading, // reset everything when progressing to new level
    Active, // normal gameplay
    RushHour, // set rush hour for ui and spawns
    Paused, // pause menu
    DayEnd, // pause game and show UI to calculate score 
    DayWin, // win ui, save progress, next level
    DayLose, // lose ui, restart, keep same level
    Cleaning // lock interactions for player convinience
}