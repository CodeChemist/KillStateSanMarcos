﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoldierAI : MonoBehaviour {

	public float ViewAngle = 180;
	public float maxRange = 50f;
	private float nextDamageStep;
	private bool hasAttacked = false;

	private Animation animate;
	private Transform soldier;
	private List<GameObject> targets;
	private GameObject closest;

	private ParticleEmitter Muzzleflash;
	private Light[] MuzzleLight;
	private AudioSource GunShot;

	public float AttackDamage;
	// Use this for initialization
	void Start () {
		soldier = this.transform;
		animate = gameObject.GetComponent<Animation>();
		Muzzleflash = gameObject.GetComponentInChildren<ParticleEmitter>();
		MuzzleLight = gameObject.GetComponentsInChildren<Light>();
		GunShot = gameObject.GetComponentInChildren <AudioSource>();
		targets = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		MFlash (false);
		GetTargets ();
		FindClosest ();
		
		//Debug.Log (closest);
		if (closest != null) {
			LookAtClosest ();
			AttackClosest ();
		} else if (hasAttacked == true)
			animate.Play ("Idle Aim");
	}


	void MFlash(bool Boo)
	{
		Muzzleflash.emit = Boo;
		MuzzleLight[0].enabled = Boo;
		MuzzleLight [1].enabled = Boo;
		if (Boo) GunShot.Play();
	}

	void GetTargets() {
		targets = new List<GameObject>();
		GameObject[] Zombies= GameObject.FindGameObjectsWithTag("Zombie");
		
		foreach(GameObject Zomb in Zombies)
			targets.Add (Zomb);
		
		targets.Add(GameObject.FindGameObjectWithTag ("Player"));

	}



	void FindClosest()
	{
		closest = null;
		float range = (maxRange * maxRange);//squares distance
		float closestDistance = Mathf.Infinity;

		foreach (GameObject target in targets)
		{
			float dist = (target.transform.position - soldier.position).sqrMagnitude; //returns distance between soldier and target squared for easier computations

			if (dist < range )  //within range
			{
				float angle = Vector3.Angle (target.transform.position - soldier.position, transform.forward);//angle between target and object
				if ((dist < closestDistance) && (angle <= (ViewAngle / 2)))
				{
					closest = target;
					closestDistance = dist;
				}
			}
		}
	}

	void LookAtClosest() {
		Vector3 LookAt = (closest.transform.position - soldier.position).normalized;
		LookAt.y = 0;
		soldier.transform.rotation = Quaternion.Slerp (soldier.rotation, Quaternion.LookRotation (LookAt), Time.deltaTime * 10);
	}

	void AttackClosest(){
		hasAttacked = true;

		if (Time.time >= nextDamageStep)
		{
			
			animate.Play("Idle Firing");
			nextDamageStep = Time.time + Random.Range(.5f, 1.5f);
			MFlash(true);
			closest.SendMessage("TakeDamage", AttackDamage);
		}


	}


}
