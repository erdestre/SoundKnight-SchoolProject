using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IDropHandler
{
    Player player;
    SetGame setgame;
    public CardDisplay card;
    [HideInInspector]

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        setgame = GameObject.Find("Canvas").GetComponent<SetGame>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("Bi raycast");
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
            //player.pendingActions.Add(eventData.pointerDrag.gameObject);
            //Debug.Log(player.pendingActions.Count);
            eventData.pointerDrag.GetComponent<CardDisplay>().shouldbeinDock = false;
            card = eventData.pointerDrag.GetComponent<CardDisplay>();
            card.currentSlot = this;
            setgame.updatePendingActions();
            /*for (int i = 0; i < player.pendingActions.Count; i++)
            {
                if(player.pendingActions[i] != null)
                {
                    Debug.Log(player.pendingActions[i].name);
                }
                else {
                    Debug.Log("Bo�ke Varke");
                }
            }*/
        }
    }
    public void updatePendingActions()
    {
        setgame.updatePendingActions();
    }
}
