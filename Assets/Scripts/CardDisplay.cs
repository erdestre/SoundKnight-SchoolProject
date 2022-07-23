using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Card card;
    public TextMeshProUGUI cardDescription;
    public Sprite artwork;
    
    public Command command;

    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    [SerializeField] Canvas canvas;
    [HideInInspector]
    public bool shouldbeinDock = true;

    GameObject defaultParent;

    [HideInInspector]
    public CardSlot currentSlot;


    private void FixedUpdate()
    {
        /*
        if (!isCardSelected && !atDestination) {
            if (shouldbeinDock)
            {
                SetPosition(DockPosition);
            }
            else
            {
                SetPosition(CardHolderPosition);
            }
        }
        else if (Hover && shouldbeinDock)
        {
            SetPosition(DockPosition); // +Y
        }
        else if (isCardSelected)
        {
            //gameObject.transform.position = 
        }
        */
    }

    void SetPosition(Transform Pos)
    {

    }
    void Start()
    {
        cardDescription.text = card.cardDescription;
        artwork = card.artwork;
        gameObject.name = card.CardName;
        command = card.command.GetComponent<Command>();

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        defaultParent = gameObject.transform.parent.gameObject;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentSlot != null)
        {
            currentSlot.card = null;
            currentSlot.updatePendingActions();
        }
        //Debug.Log("Begin");
        shouldbeinDock = true;
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        gameObject.transform.parent = canvas.transform;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("End");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        if (shouldbeinDock)
        {
            transform.parent = defaultParent.transform;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Down");
    }
}
