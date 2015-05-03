/*
 * 
 * ------------------------------- Motor(chase).cs -------------------------------
 * 
 * Purpose:     To manage the enemy attack events. Note: THIS IS FOR TESTING PURPOSES ONLY. 
 *      This is a pre-main game integration script that is very likely to not play well 
 *      with the rest. Do not include when building.
 * 
 * Created By:  Harrison Cole ('manually branched' from his origional on 5/2/15)
 * Modified By: Chase Perdue
 * Date:        5/2/15  (not sure when his origional create date is)
 * Class:       CS485 - Game Programming
 * 
 * Changelog:
 *      +  5/2/15 ------ Playing around with the Unity Tutorial script integration (merging Motor with EnemyAttack(chase)).
 *                                                                          - Chase Perdue

using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Motor : MonoBehaviour {

    public float maxSpeed = 2.0f;
    public float maxAccel = 0.5f;
    public float maxRotation = 1f;
    public float maxAngular = 0.8f;
	public Transform target;
	public float ViewAngle = 180;
	public float maxRange = 10f;

	private PlayerHealth playerHealth;
	private Animator animator;
	private float nextDamageStep;
	public float attackDelay = 1;

    GameObject player;                  //  <-- (added by Chase. 5/2/15)
    public int attackDamage = 10;       //  <-- (added by Chase. 5/2/15)
    float timer;                        //  <-- (added by Chase. 5/2/15)


    // Kinematic
    public Vector3 velocity;
    public float rotation = 0f;

    // Steering
    private Vector3 acceleration;
    private float angular = 0f;
	private Vector3 lastSeen;
	private bool seen = false;
    private Transform character;
	private bool wander = false;

	// Use this for initialization
	void Start () {
        character = this.transform;
		animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");    //  <-- (added by Chase. 5/2/15)
        //playerHealth = player.GetComponent<PlayerHealth>();     //  <-- (added by Chase. 5/2/15)
        playerHealth = target.GetComponent<PlayerHealth>();  
	}
	
	// Update is called once per frame
	void Update () 
    {
		animator.SetFloat ("Speed", velocity.magnitude);
		RaycastHit hit;
		float angle;

		if (acceleration.magnitude > maxAccel)
			acceleration = acceleration.normalized * maxAccel;
		if (angular > maxAngular)
			angular = maxAngular;
		else if (angular < -maxAngular)
			angular = -maxAngular;

		velocity += acceleration;
		rotation += angular;

		if (velocity.magnitude > maxSpeed)
			velocity = velocity.normalized * maxSpeed;
		if (rotation > maxRotation)
			rotation = maxRotation;
		else if (rotation < -maxRotation)
			rotation = -maxRotation;

		character.Translate(velocity * Time.deltaTime, Space.World);
		character.Rotate(character.up, rotation);

		Debug.DrawRay (transform.position+ new Vector3 (0f, 0.8f, 0f), (target.position - transform.position),Color.blue);

		if (Physics.Raycast (transform.position+ new Vector3 (0f, 0.8f, 0f), (target.position - transform.position), out hit, maxRange)) { //raycast to target
			angle = Vector3.Angle (target.position - character.position, transform.forward);//angle between target and object

			if ((hit.transform == target) && (angle <= (ViewAngle / 2))) {//if target is in sight and withing view width

				seen = true;
				wander = false;
				lastSeen = target.position;
				KinSeek (target);

				// In Range and i can see you!
			} else if (seen == true) {

				velocity = (lastSeen - character.position).normalized * maxSpeed;
				character.rotation = Quaternion.LookRotation (velocity);
				//not seen, move to last location
				//print ("lastSeen "+ lastSeen.x+ "position "+ character.position.x );
				if ((lastSeen - character.position).magnitude < .1) { //arrived at last seen location

					wander = true;
					seen = false;
				}
			} else if (wander == true) {
				KinWander ();
			}
		}
	}



	public void OnTriggerEnter (Collider other) {
		if (other.transform == target.transform) {
			animator.SetBool("Attack",true);
			if (Time.time >= nextDamageStep)
			{
				nextDamageStep = Time.time + attackDelay;
				playerHealth.currentHealth -= 10;
			}
		}
	}

	public void OnTriggerExit(Collider other) {
		if (other.transform == target.transform) {
			animator.SetBool("Attack",false);
			nextDamageStep = Time.time + attackDelay;
		}
	}

	// Begin various functions for movement, pass in target

    public void Stop()
    {
        velocity = Vector3.zero;
        rotation = 0f;
        acceleration = Vector3.zero;
        angular = 0f;
    }

    // Kinematic Steering

    // KinSeek
    public void KinSeek(Transform target)
    {
		if (animator.GetBool("Attack") == true)
			velocity = new Vector3 (0,0,0);
		else
		velocity = new Vector3 (target.position.x - character.position.x , 0f ,target.position.z - character.position.z).normalized * maxSpeed;
		Vector3 lookAt = new Vector3(target.position.x - character.position.x, 0 ,target.position.z - character.position.z);
		character.rotation = Quaternion.LookRotation(lookAt);
    }



    // KinFlee
    public void KinFlee(Transform target)
    {
        velocity = character.position - target.position;
        velocity = velocity.normalized * maxSpeed;
        character.rotation = Quaternion.LookRotation(velocity);
    }

    //KinArrive
    public void KinArrive(Transform target, float radius, float timeToTarget)
    {
        velocity = target.position - character.position;

        velocity /= timeToTarget;

        if (velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
        }

        character.rotation = Quaternion.LookRotation(velocity);
    }

    // KinWander
    public void KinWander()
    {
        velocity = maxSpeed/2 * character.forward;
        rotation = Random.Range(-10f, 10f) * maxRotation;
    }

    // Dynamic Steering

    // Seek
    public void Seek(Transform target)
    {
        acceleration = target.position - character.position;
    }

    // Flee
    public void Flee(Transform target)
    {
        acceleration = character.position - target.position;
    }

    // Arrive
    public void Arrive(Transform target, float clearRadius, float slowRadius, float timeToTarget)
    {
        float targetSpeed;

        Vector3 direction = target.position - character.position;
        float distance = direction.magnitude;

        if (distance < clearRadius)
        {
            Stop();
        }
        else
        {

            if (distance > slowRadius)
                targetSpeed = maxSpeed;
            else
                targetSpeed = maxSpeed * distance / slowRadius;

            Vector3 targetVelocity = direction;
            targetVelocity.Normalize();
            targetVelocity *= targetSpeed;

            acceleration = targetVelocity - velocity;
            acceleration /= timeToTarget;
        }
    }

    // Pursue
    public void Pursue(Transform target)
    {
        Motor motor = target.GetComponent<Motor>();

        float maxPrediction = 5f;
        float prediction = 0f;

        Vector3 direction = target.position - character.position;
        float distance = direction.magnitude;

        float speed = velocity.magnitude;

        if (speed <= distance / maxPrediction)
            prediction = maxPrediction;
        else
            prediction = distance / speed;

        // Currently makes a game object and uses that transform then destroys it,
        // but that seems like a lot of processing. Maybe change this?

        GameObject newTarget = new GameObject();
        newTarget.transform.position = target.position + motor.velocity * prediction;

        Seek(newTarget.transform);

        Destroy(newTarget);
    }

    // Evade
    public void Evade(Transform target)
    {
        Motor motor = target.GetComponent<Motor>();

        float maxPrediction = 5f;
        float prediction = 0f;

        Vector3 direction = target.position - character.position;
        float distance = direction.magnitude;

        float speed = velocity.magnitude;

        if (speed <= distance / maxPrediction)
            prediction = maxPrediction;
        else
            prediction = distance / speed;

        // Currently makes a game object and uses that transform then destroys it,
        // but that seems like a lot of processing. Maybe change this?

        GameObject newTarget = new GameObject();
        newTarget.transform.position = target.position + motor.velocity * prediction;

        Flee(newTarget.transform);

        Destroy(newTarget);
    }


    
    // Obstacle Avoidance
    public void ObstacleAvoidance()
    {
        RaycastHit hit;

        Debug.DrawRay(character.position, character.forward * 5f, Color.green);

        if (Physics.Raycast(character.position, character.forward, out hit, 5f))
        {
            GameObject newTarget = new GameObject();
            newTarget.transform.position = hit.point + hit.normal * 2.5f;
            Seek(newTarget.transform);
        }
    }
}


        //COMMENTED EVERYTHING BECAUSE MY PLAN FAILED. COPYING SCRIPT FILES IS A FOOLISH WAY TO MAKE CHANGES.
 */