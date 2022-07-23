using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity {

	SetGame setgame;

	public new void Awake(){
		base.Awake();
		setgame = GameObject.Find("Canvas").GetComponent<SetGame>();
	}

	void Start(){
		playerProgress = GameObject.Find("PlayerProgress").GetComponent<PlayerProgress>();
		pendingActions = new List<GameObject>(playerProgress.playerHolderSize);
		gameObject.transform.name = "Player";
	}
	  

	public override void playNextCard(){
		if(pendingActions.Count == 0){ return; }
		Vector2 dir = pendingActions[0].GetComponent<CardDisplay>().card.direction;
		pendingActions[0].GetComponent<CardDisplay>().command.executeCommand(this, dir);
		Destroy(pendingActions[0]);
		pendingActions.RemoveAt(0);
		
		if(board._playerAmountOfCardsToPlay == 0) {
			setgame.ResetCardDock();
			setgame.CardDockSpawner();
		}

	}
}