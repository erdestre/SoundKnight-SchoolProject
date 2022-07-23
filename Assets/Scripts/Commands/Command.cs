using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command : MonoBehaviour {

		public Vector2 direction;
    public abstract void executeCommand(Entity e, Vector2 dir);
    public virtual void executeCommand(Entity e){
    	executeCommand(e, direction);
    }
    public abstract void playSound(Entity e, Vector2 dir);
    
}
