using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : ScriptableObject
{
    public string CardName;
    public string cardDescription;
    public Sprite artwork;
	
  	public Vector2 direction;  
		public GameObject command;
}
