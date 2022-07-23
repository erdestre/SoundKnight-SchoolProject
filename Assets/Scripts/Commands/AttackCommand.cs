using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : Command {

	public int range = 1;
	public int thickness = 1;
	
	public override void executeCommand(Entity e, Vector2 dir){
		Vector2 normal = new Vector2(dir.y, dir.x);
		for(int j = 0-thickness+1; j < thickness; j++){
			Vector2 current = Vector2.zero;
			
			for(int i = 0; i < range; i++){
				current.x += dir.x + normal.x*j;
				current.y += dir.y + normal.y*j;
	    	e.attack(current);
			}
			
		}
    playSound(e, dir);
  }
    
	public override void playSound(Entity e, Vector2 dir){
    int index = (int)((180 - Vector2.SignedAngle(Vector2.up, dir))/45);
    	AudioClip clip;
    	if(index < e.attackSounds.Length){
    		clip = e.attackSounds[index];
	    	if(clip != null){
	    		e.audioSource.PlayOneShot(clip); 
	    	}
    	}
    }

}
