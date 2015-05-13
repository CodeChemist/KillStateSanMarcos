using UnityEngine;
using System.Collections;

public class GrabWeapon : MonoBehaviour {
	GameObject p;
	GameObject []weapon;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}


	
	
	IEnumerator Swap(){
		
		p = GameObject.FindGameObjectWithTag ("Player");
		
		weapon = GameObject.FindGameObjectsWithTag ("Weapon");
		
		foreach (GameObject heldWeapon in weapon) {

			if ((heldWeapon.transform.root == p.transform)  && (heldWeapon.transform.GetComponentInParent<WeaponSwapTimer>().swapped == false)) {//if player holds the weapon and hasnt swapped recently

				heldWeapon.transform.parent.SendMessage("hasSwapped"); //send has swapped

				gameObject.transform.parent = heldWeapon.transform.parent; //make world weapon a child of hand
				heldWeapon.transform.parent = null; //remove held wepon from hand

				Quaternion tempRotation = heldWeapon.transform.rotation;//store hand rotation
				Vector3 tempPosition = heldWeapon.transform.position; //store hand position

				heldWeapon.transform.rotation = gameObject.transform.rotation;
				heldWeapon.transform.position = gameObject.transform.position; //move held weapon away from hand
				gameObject.transform.position = tempPosition; //move world weapon to hand
				gameObject.transform.rotation = tempRotation;

				break;
			}

		}
		yield return new WaitForSeconds(0.0f);
	}
	void OnMouseOver()
	{
		if (Input.GetKey ("r")) {
			StartCoroutine(Swap());
		}
	}
}
	