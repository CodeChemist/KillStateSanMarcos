/*                  //depricated due to namespace error: Already contains definition for EnemyAttack 
 *                      (Chase --- 5/2/15)
 * 

	Chris Finley
	CS485 Assignment 1
	2D Platformer
	2015.02.17

	This script controls the collider to determin when
	the player is close enough for the enemy to play
	it's attack sound and animations


using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

	// Triggers when the player comes within "Biting Distance"
	void OnTriggerEnter(Collider c){
		if (c.tag == "Player") {
			transform.parent.SendMessage ("Attack", c);
		}
	}
	
	// Triggers when the player leaves "Biting Distance"
	void OnTriggerExit(Collider c){
		if (c.tag == "Player") {
			transform.parent.SendMessage ("Walk", c);
		}
	}
}

*/