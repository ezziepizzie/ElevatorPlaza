using UnityEngine;
using UnityEngine.EventSystems; 

public class Elevator : MonoBehaviour, IDropHandler
{
    public PassengerSpawner spawner;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Draggable draggable = dropped.GetComponent<Draggable>();
        draggable.transform.SetParent(null);
        spawner.RemovePassenger(dropped);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
