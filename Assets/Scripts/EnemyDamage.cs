/*
	Chris Finley
	CS485 Project
	Kill State San Marcos
	2015.05.04

	This script controls the collider to determin when
	the player is close enough for the enemy to hurt the player
*/

using UnityEngine;
using System.Collections;

public class EnemyDamage : MonoBehaviour {

	public float AttackDamage;

	// Triggers while collider 
	void OnTriggerStay(Collider c){
		if (c.tag == "Player") {
			c.SendMessage("TakeDamage", AttackDamage);
		}
	}
}
