using UnityEngine;
using UnityEngine.EventSystems;

public class ElevatorButton : MonoBehaviour, IPointerClickHandler
{
    public Elevator elevator;
    AudioManager audioManager;
    [SerializeField] private Animator elevatorButtonAnim;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (elevator.isActive == false) return;

        if(elevator.currentCapacity == 0) return;

        elevator.StartCoroutine("MoveElevatorUp");
        audioManager.PlaySFX(audioManager.elevetorButtonPress);
        elevatorButtonAnim.SetTrigger("buttonClicked");
    }
}
