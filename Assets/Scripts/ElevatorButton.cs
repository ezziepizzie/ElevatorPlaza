using UnityEngine;
using UnityEngine.EventSystems;

public class ElevatorButton : MonoBehaviour, IPointerClickHandler
{
    public Elevator elevator;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (elevator.isActive == false) return;

        elevator.StartCoroutine("MoveElevatorUp");
    }
}
