using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : Command
{

    public override void executeCommand(Entity e, Vector2 dir){
    	e.moveBy(dir);
			playSound(e, dir);
    }

    public override void playSound(Entity e, Vector2 dir){
    	int index = (int)((180 - Vector2.SignedAngle(Vector2.up, dir))/90);
    	AudioClip clip;
    	if(index < e.moveSounds.Length){
    		clip = e.moveSounds[index];
	    	if(clip != null){
	    		//Debug.Log("Playing clip" + clip);
	    		e.audioSource.PlayOneShot(clip); 
	    	}
    	}
    }
}
