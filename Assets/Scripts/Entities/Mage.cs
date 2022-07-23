using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mage : Entity
{
    // Start is called before the first frame update
    void Start()
    {
    	health = 1;
    	numOfActions = 3;
    }

    
	public override void chooseActions(){
		Vector2 playerPos = board.player.getGridPos();
		int distToPlayer = (int)(Math.Abs(playerPos.x - gridPos.x) + Math.Abs(playerPos.y - gridPos.y));

		for(int i = 0; i < Math.Min(distToPlayer-1, numOfActions); i++){
			addMoveCard(playerPos);
		}
		for(int i = 0; i < numOfActions-distToPlayer+2; i++){
			addAttackCard(playerPos);
		}
	}


	private void addMoveCard(Vector2 playerPos){
		Vector2 dir = new Vector2();
		if(Math.Abs(playerPos.x - gridPos.x) > Math.Abs(playerPos.y - gridPos.y)){
			dir.x = Math.Sign(playerPos.x - gridPos.x);
			dir.y = 0;
		}else{
			dir.x = 0;
			dir.y = Math.Sign(playerPos.y - gridPos.y);
		}
		GameObject command = Instantiate(moveCmdPrefab, Vector3.zero, Quaternion.identity);
		command.GetComponent<MoveCommand>().direction = dir;
		pendingActions.Add(command);
	}

	private void addAttackCard(Vector2 playerPos){
		Vector2 dir = new Vector2();
		if(Math.Abs(playerPos.x - gridPos.x) > Math.Abs(playerPos.y - gridPos.y)){
			dir.x = Math.Sign(playerPos.x - gridPos.x);
			dir.y = 0;
		}else{
			dir.x = 0;
			dir.y = Math.Sign(playerPos.y - gridPos.y);
		}
		GameObject command = Instantiate(attackCmdPrefab, Vector3.zero, Quaternion.identity);
		command.GetComponent<AttackCommand>().direction = dir;
		command.GetComponent<AttackCommand>().range = 2;
		//Debug.Log("Attack direction " + dir);
		pendingActions.Add(command);
	}
}
