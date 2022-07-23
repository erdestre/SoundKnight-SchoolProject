using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dragon : Entity

{
	void Start(){
		health = 2;
		numOfActions = 5;
	} 


	public override void chooseActions(){
		Vector2 playerPos = board.player.getGridPos();
		int distToPlayer = (int)(Math.Abs(playerPos.x - gridPos.x) + Math.Abs(playerPos.y - gridPos.y));
		
		int mod = (int)UnityEngine.Random.Range(0,1);
		
		for(int i = 0; i < 3-mod; i++){
			addMoveCard(playerPos);
		}
		for(int i = 0; i < 2+mod; i++){
			addAttackCard();
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

	private void addAttackCard(){
		Vector2[] dirs = {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
		GameObject command = Instantiate(attackCmdPrefab, Vector3.zero, Quaternion.identity);
		command.GetComponent<AttackCommand>().direction = dirs[(int)UnityEngine.Random.Range(0,3)];
		command.GetComponent<AttackCommand>().thickness = 2;
		pendingActions.Add(command);
	}


}
