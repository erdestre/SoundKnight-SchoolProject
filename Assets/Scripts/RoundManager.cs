using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{

	Board board; 

	public Entity snake;
	public Entity mage;
	public Entity dragon;

	List<Entity> prefabs = new List<Entity>();

	void Start() {
	
		board = GameObject.Find("Board").GetComponent<Board>();
		prefabs.Add(snake);
		prefabs.Add(mage);
		prefabs.Add(dragon);
	}

	// Update is called once per frame
	void Update()
	{
	    
	}

	public void spawnEnemies(int roundNumber){
		int numOfEnemies = (int)Mathf.Sqrt(roundNumber+1)+1;
		
		for(int i = 0; i < numOfEnemies; i++){
			int enemyDifficulty = Mathf.Min((int)Random.Range(0, (float)numOfEnemies/2), 2);
			
			Entity entityInstance = Instantiate(prefabs[enemyDifficulty], Vector3.zero, Quaternion.identity).GetComponent<Entity>();
			Vector2 pos = new Vector2(Random.Range(0, 6), Random.Range(0, 6));
			board.addEnemy(entityInstance, pos);
		}
	}
}
