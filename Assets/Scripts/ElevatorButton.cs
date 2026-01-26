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
        ToolType currentTool = GameManager.instance.currentTool;

        if (currentTool != ToolType.Hand || elevator.isActive == false || elevator.currentCapacity == 0)
            return;

        elevator.StartCoroutine("MoveElevatorUp");
        audioManager.PlaySFX(audioManager.elevatorButtonPress);
        elevatorButtonAnim.SetTrigger("buttonClicked");
    }
}
