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

			if (heldWeapon.transform.root == p.transform) {//if player holds the weapon

				gameObject.transform.parent = heldWeapon.transform.parent.transform;
				heldWeapon.transform.parent = null;
				Vector3 tempPosition = heldWeapon.transform.position;
				heldWeapon.transform.position = gameObject.transform.position;
				gameObject.transform.position = tempPosition;

				break;
			}
		}
		yield return new WaitForSeconds (1f);
	}
	void OnMouseOver()
	{
		if (Input.GetKey ("r")) {
			StartCoroutine(Swap());
		}
	}
}
	