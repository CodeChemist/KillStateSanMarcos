/*
	Chris Finley
	CS485 Project
	Kill State San Marcos
	2015.05.04

	This script controls the collider to determin when
	the player is close enough for the enemy to play
	it's attack sound and animations
*/

using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

	// Triggers when the player comes within Attack Distance
	void OnTriggerEnter(Collider c){
		if (c.tag == "Player") {
			transform.parent.SendMessage ("Attack");
		}
	}
	
	// Triggers when the player leaves Attack Distance
	void OnTriggerExit(Collider c){
		if (c.tag == "Player") {
			transform.parent.SendMessage ("Walk");
		}
	}
}
