using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
	
	public Player playerPrefab;
	
	
	Grid grid;
	const int boardWidth = 7;
	const int boardHeight = 7;

	TextMeshProUGUI KillCounterText;
	int KillCounter = 0;
	//[HideInInspector]
	public Player player;
	Snake snake;
	
	bool[] tiles;
	
	List<Entity> enemies;
	
	//timer variables
	int amountOfCardsToPlay = 0;
	public int _playerAmountOfCardsToPlay
    {
		get => player.pendingActions.Count;
    }
	bool timerRunning = false;
	float startTime = 0;
	float interval = 0.5f;

	RoundManager roundManager;
	
	int currentRound = 1;
	
	void Awake() {
		Button GoButton = GameObject.Find("Go").GetComponent<Button>();
		GoButton.onClick.AddListener(() => { EndTurn(); });

		KillCounterText = GameObject.Find("KillCounter").gameObject.GetComponent<TextMeshProUGUI>();
		
		tiles = new bool[boardWidth*boardHeight];
		grid = GameObject.Find("GridAnchor").GetComponent(typeof(Grid)) as Grid;
		
		player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
		player.setGridPos(new Vector2(3,3));
		
		enemies = new List<Entity>();
		
		roundManager = GameObject.Find("RoundManager").GetComponent<RoundManager>();
		
	}
	
	void Start(){
		roundManager.spawnEnemies(currentRound);
		newTurn();
	}
	
	void Update(){
		if(timerRunning){
			if(startTime + interval <= Time.time){
				timerRunning = false;
				PlayCards();
			}
		}
	}
	
	public Entity getEntityByTile(Vector2 vec){
		for(int i = 0; i < enemies.Count; i++){
			if(player.getGridPos() == vec){
				return player;
			}
			if(enemies[i].getGridPos() == vec){
				return enemies[i];
			}
		}
		return null;
	}
	
	public void addEnemy(Entity e, Vector2 pos){
		int index = (int)pos.y * boardWidth + (int)pos.x;
		if(tiles[index] == false){
			enemies.Add(e);
			e.setGridPos(pos);
			tiles[index] = true;
		}
	}
	
	public void setTileOccupied(Vector2 pos, bool b){
		tiles[(int)pos.y * boardWidth + (int)pos.x] = b;
	}

	public bool getTileOccupied(Vector2 pos){
		return tiles[(int)pos.y * boardWidth + (int)pos.x];
	}
	
	public Grid getGrid(){return grid;}

	public void EndTurn(){
		amountOfCardsToPlay = player.pendingActions.Count;
		for(int i = 0; i < enemies.Count; i++){
			if(enemies[i].pendingActions.Count > amountOfCardsToPlay){
				amountOfCardsToPlay = enemies[i].pendingActions.Count;
			}
		}
		PlayCards();
	}

	private void StartTimer(){
		timerRunning = true;
		startTime = Time.time;
	}

	private void PlayCards(){
		amountOfCardsToPlay--;
		List<Entity> attackingEnemies = new List<Entity>();
		for(int i = 0; i < enemies.Count; i++){
			if(enemies[i].pendingActions.Count > 0){
				if(enemies[i].pendingActions[0].GetComponent<MoveCommand>() != null){
					enemies[i].playNextCard();
				}else if(enemies[i].pendingActions[0].GetComponent<AttackCommand>() != null){
					attackingEnemies.Add(enemies[i]);
				}
			}
		}		
		
		player.playNextCard();
		
		for(int i = 0; i < attackingEnemies.Count; i++){
			attackingEnemies[i].playNextCard();
		}
		attackingEnemies.Clear();
		
		for(int i = 0; i < enemies.Count; i++){
			if(enemies[i].markedForDeath){
				enemies[i].die();
			}
		}
		if(player.markedForDeath){
			player.die();
		}
		
		if(amountOfCardsToPlay > 0){
		
			StartTimer();
			
		} else {
			
			newTurn();
			
		}
	}

	public void entityDied(Entity entity){
		setTileOccupied(entity.getGridPos(), false);
		enemies.Remove(entity);
		if (!player.markedForDeath)
        {
			KillCounter++;
			KillCounterText.SetText("Kill Counter: " + KillCounter);
		}
		if(enemies.Count == 0){
			currentRound++;
			roundManager.spawnEnemies(currentRound);
		}
	}
	
	private void newTurn(){
		for(int i = 0; i < enemies.Count; i++){
			enemies[i].chooseActions();
		}
	}
	
}
