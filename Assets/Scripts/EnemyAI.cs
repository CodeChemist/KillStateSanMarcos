/*
	Chris Finley
	CS485 Assignment 1
	2D Platformer
	2015.02.17

	This script controls the enemy behaviors
 */

using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	public int walkSpeed = 6;			// rate of enemy walking speed 
	public int patrolRange = 5;			// distance enemy will walk from starting point
	public int damage = 20;				// damage inflicted to player

	public AudioClip attackSound;		// sound to play when enemy attacks
	public AudioClip deathSound;		// sound to play when enemy dies

	private Vector3 startPos;			// initial enemy position
	private Vector3 previousPos;		// position in previous frame
	private int direction = 1;		// direction enemy is walking
	private bool playerIsNear = false;	// if player is within sensing radius
	private bool wasTouched = false;	// if player was already counted as "attacked"
	private bool wasChasing = false;	// if enemy was chasing, potentially outside patrol zone

	private GameObject player;			// reference to player object

	// store starting position on initialization
	void Awake() {
		startPos = transform.position;
	}

	void Start () {
		// store start position for patrol calculations
		previousPos = transform.position;

		// loop walking animation
		GetComponent<Animation>().wrapMode = WrapMode.Loop;

		// store reference to instance of player
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

		// find new reference to player if died and respawned
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
		}

		// if the player is near by, chase them
		if (playerIsNear) {
			Chase ();
		}

		// return to start position after chase ends
		else if (wasChasing) {
			ReturnToPost ();
		}

		// default walking behavior
		else {
			Patrol ();
		}
	}

	// Function to handle the movement of the enemy in the left-right direction
	// speedFactor is a multiple of the set walk rate
	void Move(int speedFactor){
		
		// calculate new position for this frame
		Vector3 newPos = previousPos - new Vector3((direction * walkSpeed * speedFactor *  Time.deltaTime), 0f, 0f);
		
		// smooth the animation between positions
		transform.position = Vector3.Lerp (previousPos, newPos, Time.deltaTime);

		// set previous position for the next frame
		previousPos = transform.position;
	}

	// Function to handle actions when enemy had died
	void EnemyDied(){
		// play enemy death sound effect
		AudioSource.PlayClipAtPoint(deathSound, transform.position);
		
		// remove enemy from level
		Destroy (gameObject);
	}
	bool OutOfBounds(){
		return Mathf.Abs (startPos.x - previousPos.x) > patrolRange;
	}

	// function to control actions for default enemy walking action
	void Patrol(){
		// determine if enemy should turn around to walk back towards starting position
		// the negative multiplication will cause the direction to flip without needing to
		// know which way its currently facing
		if (OutOfBounds()){
			direction *= -1;
			Turn (startPos);
		}

		// move the enemy character
		Move (10);
	}


	// function to control actions when enemy calls off chasing the player
	void ReturnToPost(){

		// if enemy has returned to starting position, resume normal patrol
		// use approximation of start position due to float decimals to avoid
		// enemy getting stuck
		if (Mathf.Abs(transform.position.x - startPos.x) <= 0.1f) {
			wasChasing = false;
			GetComponent<Animation>()["walk"].speed = 1.0f;
			return;
		}

		// determine which direction the enemy needs to head
		direction = (previousPos.x > startPos.x) ? 1 : -1;

		// move the enemy character
		Move (15);
	}

	// function to control actions when enemy chases the player
	void Chase(){

		// determine which direction to chase the player 
		direction = (previousPos.x > player.transform.position.x) ? 1 : -1;

		// turn to face the player if needed
		if (transform.forward.x != direction && Mathf.Abs(previousPos.x - player.transform.position.x) > 0.25f) {
			Turn (player.transform.position);
		}

		// move the enemy character
		Move (50);
	}


	// Received message from collider, player is within biting distance
	void Bite(Collider c){

		// turn to face the player
		Turn (c.transform.position);

		// play attack sound
		AudioSource.PlayClipAtPoint(attackSound, transform.position);

		// play attack animation
		GetComponent<Animation>().Play("byte",PlayMode.StopAll);	
	}

	// Received message from collider, player has left biting distance
	void Walk(Collider c){
		
		// play attack animation
		GetComponent<Animation>().Play("walk",PlayMode.StopAll);	
	}

	// Function to turn enemy character
	void Turn(Vector3 t){

		// Get position of coordinate to look at
		transform.LookAt( new Vector3 (t.x, 0f, 0f));

		// assign euler angles to variable
		Vector3 angles = transform.eulerAngles;

		// setting x and z to 0 ensures left-right rotation ONLY
		// around the Y axis and eliminates glitchy movement problems
		angles.x = 0f;
		angles.z = 0f;

		// set new rotation values
		transform.eulerAngles = angles;

	}

	// Received message from collider, player touched the enemy
	void Touched(Collider c){
		// call coroutine for wait
		StartCoroutine (TouchedCoroutine (c));
	}

	// Handles attacking player, prevents multiple successive collisions
	IEnumerator TouchedCoroutine(Collider c){
		// check if the player has already been attacked
		if (!wasTouched) {
			// set flag to true to prevent additional collisions
			wasTouched = true;

			// Send message to player to deduct health
			c.SendMessage ("Attacked", damage);

			// Pause for a second before allowing additional collisions
			yield return new WaitForSeconds(1.0f);

			// return to walk animation
			GetComponent<Animation>().Play("walk",PlayMode.StopAll);

			// turn flag off to reallow collisions
			wasTouched = false;
		}
	}

	// Received message from collider, player jumped on enemy killing it
	void Smashed(){
		// Check for false positive of both colliders being triggered on contact
		if (!wasTouched) {
			// set flag to prevent player from taking damage
			wasTouched = true;

			// Call death function
			EnemyDied();
		}
	}

	// Received message from collider, player is within chasing distance
	void PlayerSensed(Collider c){

		// set flag to true so enemy will chase player in next update
		playerIsNear = true;
		Turn (c.transform.position);
		GetComponent<Animation>()["walk"].speed = 2.5f;
	}

	// Received message from collider, player has escaped chasing distance
	void PlayerLost(Collider c){

		// turn off chasing flag
		playerIsNear = false;

		if (OutOfBounds ()) {
			// set flag to send enemy back to starting position
			wasChasing = true;

			// face the direction of the starting position
			Turn (startPos);
			
			GetComponent<Animation>() ["walk"].speed = 1.2f;
		} 
		else {
			GetComponent<Animation>()["walk"].speed = 1.0f;
		}
	}
}
