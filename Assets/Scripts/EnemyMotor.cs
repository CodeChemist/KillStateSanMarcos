using UnityEngine;

using System.Collections;
//using Assets.Scripts; //Making sure the git revert took as well as error resolving.

public class EnemyMotor : MonoBehaviour {

    public float maxSpeed = 2.0f;
    public float maxAccel = 0.5f;
    public float maxRotation = 1f;
    public float maxAngular = 0.8f;
	public Transform target;
	public float ViewAngle = 180;
	public float maxRange = 10f;

	private Animator animator;
	private float nextDamageStep;
	public float attackDelay = 1;

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
		//character.Rotate(character.up, rotation);
		Vector3 EyeLine = character.position + new Vector3 (0f, (float)(GetComponent<CapsuleCollider> ().height - .2), 0f);
		Debug.DrawRay (EyeLine, (target.position - EyeLine),Color.blue);

		if (Physics.Raycast (EyeLine, (target.position - EyeLine), out hit, maxRange)) { //raycast to target
			angle = Vector3.Angle (target.position - character.position, transform.forward);//angle between target and object

			if ((hit.transform == target) && (angle <= (ViewAngle / 2))) {//if target is in sight and withing view width

				seen = true;
				wander = false;
				lastSeen = target.position;
				KinSeek (target);

				// In Range and i can see you!
			} else if (seen == true) {
				velocity = (lastSeen - character.position).normalized * maxSpeed;
				velocity.y = 0;
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

	public void Attack(){
	
	//public void OnTriggerEnter (Collider other) {
		//if (other.transform == target.transform) {
			animator.SetBool("Attack",true);
			if (Time.time >= nextDamageStep)
			{
				nextDamageStep = Time.time + attackDelay;
				//other.SendMessage("attack", 10);
			}
		//}
	}

	public void Walk(){
	//public void OnTriggerExit(Collider other) {
		//if (other.transform == target.transform) {
			nextDamageStep = Time.time + attackDelay;
			animator.SetBool("Attack",false);

		//}
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
		if (animator.GetBool ("Attack") == true)
			velocity = new Vector3 (0, 0, 0);
		else {
			velocity = (target.position - character.position).normalized * maxSpeed;
			velocity.y = 0;
		}
			Vector3 LookAt = (target.position - character.position).normalized * maxSpeed;
			LookAt.y = 0;
			character.rotation = Quaternion.Slerp (character.rotation, Quaternion.LookRotation (LookAt), Time.deltaTime * 5);
		
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
        EnemyMotor motor = target.GetComponent<EnemyMotor>();

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
		EnemyMotor motor = target.GetComponent<EnemyMotor>();

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
