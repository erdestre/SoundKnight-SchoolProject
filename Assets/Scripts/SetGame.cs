using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetGame : MonoBehaviour
{
    PlayerProgress playerProgress;
    Player player;
    
    [SerializeField] GameObject Deck;
    [SerializeField] GameObject CardHolder;
    [SerializeField] GameObject CardHolderPanelPrefab;
    
    CardSlot[] cardSlots;

    private void Start()
    {
        playerProgress = GameObject.Find("PlayerProgress").GetComponent<PlayerProgress>();

        cardSlots = new CardSlot[playerProgress.playerHolderSize];
        CardHolderPanelSpawner();
        CardDockSpawner();
    }

    public void CardHolderPanelSpawner()
    {
        for (int i = 0; i < playerProgress.playerHolderSize; i++)
        {
            GameObject Panel = Instantiate(CardHolderPanelPrefab);
            Panel.transform.parent = CardHolder.transform;
            cardSlots[i] = Panel.GetComponent<CardSlot>();
        }
    }
    public void CardDockSpawner()
    {
        for (int i = 0; i < playerProgress.playerRandomCardNumber; i++)
        {
            int number = Random.Range(0, playerProgress.RandomCards.Length);
            GameObject card = Instantiate(playerProgress.RandomCards[number]);
            card.transform.parent = Deck.transform;
        }
    }
    public void ResetCardDock()
    {
        for (int i = Deck.transform.childCount-1; i >= 0; i--)
        {
            Destroy(Deck.transform.GetChild(i).gameObject);
        }
    }

    public void updatePendingActions()
    {
        if (!player)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }
        player.pendingActions.Clear();
        for(int i = 0; i < playerProgress.playerHolderSize; i++)
        {
            if (cardSlots[i].card != null)
            {
                player.pendingActions.Add(cardSlots[i].card.gameObject);
            }
        }
    }
}
