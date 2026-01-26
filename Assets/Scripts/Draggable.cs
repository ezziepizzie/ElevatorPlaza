using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public GameObject passengerSpritePrefab;
    AudioManager audioManager;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Passenger passenger;

    [HideInInspector] public GameObject passengerSprite;
    [HideInInspector] public RectTransform passengerSpriteTransform;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag started");
        passengerSprite = Instantiate(passengerSpritePrefab, transform.root);
        passengerSpriteTransform = passengerSprite.GetComponent<RectTransform>();
        passenger = GetComponent<Passenger>();

        passengerSprite.GetComponent<Image>().sprite = passenger.passengerType.passengerSprite;
        passengerSprite.GetComponent<Image>().raycastTarget = false;

        image.color = new Color(1, 1, 1, 0.4f);

        audioManager.PlaySFX(audioManager.passengerDrag);

        /*parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;*/
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
        //transform.position = eventData.position;

        passengerSpriteTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag ended");
        //transform.SetParent(parentAfterDrag);
        //image.raycastTarget = true;

        passengerSpriteTransform.SetParent(parentAfterDrag);
        passengerSprite.GetComponent<Image>().raycastTarget = true;
        image.color = new Color(1, 1, 1, 1);
        DestroySprite();
    }

    public void DestroySprite()
    {
        if (passengerSprite != null)
            Destroy(passengerSprite);
    }
}
