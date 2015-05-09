using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour {

	public float AttackDamage;
	
	// Triggers while collider 
	void OnTriggerStay(Collider c){
		if (c.tag == "Zombie") {
			c.SendMessage("TakeDamage", AttackDamage);
		}
	}
}
