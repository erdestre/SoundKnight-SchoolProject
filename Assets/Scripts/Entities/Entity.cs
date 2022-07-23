using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class Entity : MonoBehaviour
{
	
	protected Board board;
	Grid grid;
	Vector3 gridAnchor;
	
	protected Vector2 gridPos;
	
	protected int health;
	
	SpriteRenderer sprtRenderer;
	
	protected int numOfActions;
	
	[HideInInspector]
	public List<GameObject> pendingActions;

	public PlayerProgress playerProgress;
	
	public GameObject attackCmdPrefab;
	public GameObject moveCmdPrefab;

	[HideInInspector]
	public AudioSource audioSource;
	public BoxCollider2D boxCollider;
	
	public AudioClip [] moveSounds;
	public AudioClip [] attackSounds;

	[HideInInspector]
	public bool markedForDeath = false;

	public GameObject AttackIndicator;
	private List<GameObject> animationObjects;

	public GameObject heartPrefab;

	[HideInInspector]
	public List<GameObject> hearts;

	private bool heartsVisible = false;
	
	public void Awake(){
		audioSource = this.gameObject.AddComponent<AudioSource>();
		boxCollider = gameObject.AddComponent<BoxCollider2D>();
		
		boxCollider.size = gameObject.GetComponentInChildren<SpriteRenderer>().sprite.rect.size;
		boxCollider.offset = boxCollider.size/2;

		animationObjects = new List<GameObject>();
	
		hearts = new List<GameObject>();
	
		numOfActions = 1;
		health = 1;
		pendingActions = new List<GameObject>();
		board = GameObject.Find("Board").GetComponent<Board>();
		grid = board.getGrid();
		sprtRenderer = transform.Find("SpriteObject").GetComponent<SpriteRenderer>();
		
		gridAnchor = GameObject.Find("GridAnchor").transform.position;
		gridPos = new Vector2();
	}

	public void hurt(){
		health--;
		if(health == 0){
			markedForDeath = true;
		}
	}

	public void die(){
		board.entityDied(this);
		destroyAnimationObjects();
		Destroy(this.gameObject);
	}

	public virtual void playNextCard(){
		if(pendingActions.Count == 0){ return; }
		pendingActions[0].GetComponent<Command>().executeCommand(this);
		Destroy(pendingActions[0]);
		pendingActions.RemoveAt(0);
	}
	
	public Vector2 getGridPos(){return gridPos;}
	
	public void setGridPos(Vector2 vec){
		if(board.getTileOccupied(vec)){
			return;
		}
		Vector2 oldPos = gridPos;
		gridPos = vec;
		transform.localPosition = gridAnchor + grid.CellToLocal(new Vector3Int((int)gridPos.x, (int)gridPos.y, 0));
		board.setTileOccupied(oldPos, false);
		board.setTileOccupied(gridPos, true);
		sprtRenderer.sortingOrder = 10-(int)gridPos.y;
	}
	
	public void moveBy(Vector2 vec){
		Vector2 endPos = new Vector2(gridPos.x + vec.x, gridPos.y + vec.y);
		if(endPos.x >= 0 && endPos.x <= 6 && endPos.y >= 0 && endPos.y <= 6){
			setGridPos(endPos);
		}
	}

	public virtual void attack(Vector2 dir){
		Vector2 endPos = new Vector2(gridPos.x + dir.x, gridPos.y + dir.y);
		//Debug.Log("Attacking Tile " + endPos);
		Entity target = board.getEntityByTile(endPos);
		if(target != null){
			target.hurt();
		}
		StartCoroutine(AttackAnimation(dir));
	}
		
	IEnumerator AttackAnimation(Vector2 animationObjectOffset)
	{
		animationObjects.Add(Instantiate(AttackIndicator));
		animationObjects.Last().transform.position = new Vector2(gameObject.transform.position.x + animationObjectOffset.x * 32 + 16,
														gameObject.transform.position.y + animationObjectOffset.y * 24 + 12);
		yield return new WaitForSeconds(0.2f);
		destroyAnimationObjects();
	}

	private void destroyAnimationObjects(){
		for(int i = 0; i < animationObjects.Count; i++){
			if(animationObjects[i] != null){
				Destroy(animationObjects[i]);
			}
		}
		animationObjects.Clear();
	}

	public virtual void chooseActions(){}

	void OnMouseDown()
	{
		StartCoroutine(ClickEntity());
	}
	IEnumerator ClickEntity()
	{
		
		for (int i = 0; i < pendingActions.Count; i++)
		{
			Color DefaultColor = gameObject.GetComponentInChildren<SpriteRenderer>().color;
			pendingActions[i].GetComponent<Command>().playSound(this, pendingActions[i].GetComponent<Command>().direction);
			gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
			yield return new WaitForSeconds(1);
			gameObject.GetComponentInChildren<SpriteRenderer>().color = DefaultColor;
		}
	}



	void OnMouseOver() {
		if(heartsVisible == false){
			for(int i = 0; i < health; i++){
				hearts.Add(Instantiate(heartPrefab));
			}
		
			for(int i = 0; i < hearts.Count; i++){
				hearts[i].GetComponent<SpriteRenderer>().sortingOrder = 30;
				hearts[i].transform.position = transform.position;
				hearts[i].transform.Translate(15 + (hearts.Count-i-hearts.Count/2f)*10, 20, 0);
			}
			heartsVisible = true;
		}
	}

	void OnMouseExit()	{ //The mouse is no longer hovering over the GameObject so output this message each frame
		for(int i = 0; i < hearts.Count; i++){
				Destroy(hearts[i]);
		}
		hearts.Clear();
		heartsVisible = false;
	}
}
