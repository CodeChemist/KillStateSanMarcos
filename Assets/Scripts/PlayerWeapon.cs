using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour {


	public float rotationDegreesPerSecond = 60f;
	public float AttackDamage;

	void Update() {
		if (transform.parent != null)
		transform.parent.SendMessage("SetSwingInfo",rotationDegreesPerSecond);
	}

	// Triggers while collider 
	void OnTriggerStay(Collider c){
		if (c.tag == "Zombie") {
			c.SendMessage("TakeDamage", AttackDamage);
		}
	}
}
