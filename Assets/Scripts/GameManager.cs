using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public GameState state;

    public static event Action<GameState> OnGameStateChange;

    [Header("Spawning")]
    public PassengerSpawner passengerSpawner;

    [Header("Day Timer")]
    public float dayDuration = 120f; // SECONDS. 5 mins is kinda goated, has to be odd, 1 min rush hour
    private float dayTimer;
    private int startHour = 9;
    private int endHour = 17;
    private int currentHour;
    public TextMeshProUGUI dayTimerText;

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

    [Header("Elevators")]
    public List<Elevator> elevators = new List<Elevator>();
    public float minBreakTime = 15f;
    public float maxBreakTime = 25f;
    private float breakdownTimer;

    [Header("Player Tool")]
    public ToolType currentTool = ToolType.Hand;
    public TextMeshProUGUI currentToolText;

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
        currentHour = startHour;

        if (SceneManager.GetActiveScene().name == "Game")
        {
            passengerSpawner.StopSpawning();
            passengerSpawner.StartSpawning();
            SwitchToolCursor();
            breakdownTimer = GetRandomBreakTime();
        }

        // to add dirtyness

        UpdateGameState(GameState.Active);
    }

    private void HandleActive()
    {
        Debug.Log("[STATE] Active");
        Time.timeScale = 1;

        if (SceneManager.GetActiveScene().name == "Game")
        {
            passengerSpawner.StartSpawning();
            passengerSpawner.SetRushHour(false);
        }
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
        if (SceneManager.GetActiveScene().name == "Game")
        {
            passengerSpawner.StopSpawning();
        }
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
        if (SceneManager.GetActiveScene().name == "Game" && state != GameState.Paused)
            ScrollTools();

        // handle all the timers
        if (state == GameState.Active || state == GameState.RushHour)
            HandleTimers();
    }

    private void HandleTimers()
    {
        // Day Timer
        dayTimer -= Time.deltaTime;

        UpdateHour();

        if (dayTimer <= 0)
        {
            UpdateGameState(GameState.DayEnd);
            return;
        }

        HandleBreakdowns();

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

    // handle elevator breakdown timer cooldown
    private void HandleBreakdowns()
    {
        if (state != GameState.Active && state != GameState.RushHour)
            return;

        breakdownTimer -= Time.deltaTime;

        if (breakdownTimer <= 0f)
        {
            TryBreakRandomElevator();
            breakdownTimer = GetRandomBreakTime();
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

    private void UpdateHour()
    {
        float elapsedPercent = 1 - (dayTimer / dayDuration);
        int totalHours = endHour - startHour;
        int calculatedHour = startHour + Mathf.FloorToInt(elapsedPercent * totalHours);

        if (calculatedHour != currentHour)
        {
            currentHour = calculatedHour;
            //Debug.Log("Current Hour: " + currentHour);

            if(currentHour < 12)
            {
                dayTimerText.text = currentHour + " AM";
            }
            else if (currentHour == 12)
            {
                dayTimerText.text = "12 PM";
            }
            else
            {
                dayTimerText.text = (currentHour - 12) + " PM";
            }
        }
    }

    // get random time for next breakdown
    private float GetRandomBreakTime()
    {
        return UnityEngine.Random.Range(minBreakTime, maxBreakTime);
    }

    // attempt to break a random elevator
    private void TryBreakRandomElevator()
    {
        List<Elevator> validElevators = new List<Elevator>();

        foreach (Elevator e in elevators)
        {
            if (!e.isBroken)
            {
                validElevators.Add(e);
            }
            
        }

        if (validElevators.Count == 0)
            return;

        Elevator chosen = validElevators[UnityEngine.Random.Range(0, validElevators.Count)];
        chosen.BreakElevator();

        Debug.Log("[BREAKDOWN] Elevator broke!");
    }

    private void ScrollTools()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (scroll == 0) return;

        int toolCount = System.Enum.GetValues(typeof(ToolType)).Length;
        int currentIndex = (int)currentTool;

        if (scroll > 0)
            currentIndex--;
        else
            currentIndex++;

        currentIndex = (currentIndex + toolCount) % toolCount;

        currentTool = (ToolType)currentIndex;

        SwitchToolCursor();

        currentToolText.text = "Current Tool: " + currentTool.ToString();
        Debug.Log("Current Tool: " + currentTool);
    }

    public void SwitchToolCursor()
    {
        switch (currentTool)
        {
            case ToolType.Hand:
                CursorController.instance.ChangeCursor(CursorController.instance.handCursor);
                break;

            case ToolType.Hammer:
                CursorController.instance.ChangeCursor(CursorController.instance.hammerCursor);
                break;

            case ToolType.Sponge:
                CursorController.instance.ChangeCursor(CursorController.instance.spongeCursor);
                break;
            default:
                CursorController.instance.ChangeCursor(CursorController.instance.defaultCursor);
                break;
        }
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

public enum ToolType
{
    Hand,
    Hammer,
    Sponge
}