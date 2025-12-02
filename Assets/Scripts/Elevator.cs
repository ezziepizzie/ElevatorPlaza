using UnityEngine;
using UnityEngine.EventSystems; 
using TMPro;

public class Elevator : MonoBehaviour, IDropHandler
{
    public PassengerSpawner spawner;
    public int MaxCapacity = 4;
    [HideInInspector] public int CurrentCapacity;
    [SerializeField] private TextMeshProUGUI capacityText;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Draggable draggable = dropped.GetComponent<Draggable>();

        if(CurrentCapacity < MaxCapacity)
        {
            CurrentCapacity++;
            draggable.transform.SetParent(null);
            spawner.RemovePassenger(dropped);
            capacityText.text = CurrentCapacity + " / " + MaxCapacity;
            Debug.Log("Current capacity: " + CurrentCapacity);
        }
    }

    public void ResetElevator()
    {
        CurrentCapacity = 0;
        capacityText.text = "0 / " + MaxCapacity;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        capacityText.text = "0 / " + MaxCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
