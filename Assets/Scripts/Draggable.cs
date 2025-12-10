using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public GameObject passengerSpritePrefab;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Passenger passenger;

    [HideInInspector] public GameObject passengerSprite;
    [HideInInspector] public RectTransform passengerSpriteTransform;
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag started");
        passengerSprite = Instantiate(passengerSpritePrefab, transform.root);
        passengerSpriteTransform = passengerSprite.GetComponent<RectTransform>();
        passenger = GetComponent<Passenger>();

        passengerSprite.GetComponent<Image>().sprite = passenger.passengerType.passengerSprite;
        passengerSprite.GetComponent<Image>().raycastTarget = false;

        image.color = new Color(1, 1, 1, 0.4f);

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

        if (passengerSprite != null)
            Destroy(passengerSprite);
    }
}
