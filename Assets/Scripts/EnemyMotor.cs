using UnityEngine;

using System.Collections;
//using Assets.Scripts; //Making sure the git revert took as well as error resolving.

public class EnemyMotor : MonoBehaviour {

    public float maxSpeed = 2.0f;

	public Transform target;
	public float ViewAngle = 180;
	public float maxRange = 10f;
	public float maxRotation = 1f;
	private Animator animator;
	public float attackDelay = 1;

    // Kinematic
	public Vector3 velocity;
    public float rotation = 0f;

    // Steering

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
		//if(velocity.magnitude > 0)
		//	Debug.Log (" speed " + velocity.magnitude);
		if (animator.GetBool ("Dead") == true) {
			velocity = new Vector3 (0, 0, 0); //stop moving
			rotation = 0; //stop rotating
		} 
		else {
			animator.SetFloat ("Speed", velocity.magnitude); //update animator
			character.Translate (velocity * Time.deltaTime, Space.World);//move character

			if (CheckEyesight ()) // In Range and i can see you!
				BeginHunt();
			else if (seen == true) 
				MoveToLastSeen ();
			 else if (wander == true) 
				KinWander ();

		}
	}

	public bool CheckEyesight() {
		RaycastHit hit;
		float angle;
	
		Vector3 EyeLine = character.position + new Vector3 (0f, (float)(GetComponent<CapsuleCollider> ().height - .2), 0f);
		//Debug.DrawRay (EyeLine, (target.position - EyeLine),Color.blue);
	
		if (Physics.Raycast (EyeLine, (target.position - EyeLine), out hit, maxRange)) { //raycast from eye level to target, true if something in range
			angle = Vector3.Angle (target.position - character.position, transform.forward);//angle between target and object
		
			if ((hit.transform == target) && (angle <= (ViewAngle / 2))) {//if target is in sight and withing view width
				return true;
			}
			return false;
		}
		return false;
	}

	public void BeginHunt() {
		seen = true;
		wander = false;
		lastSeen = target.position;
		KinSeek (target);
	}

	public void MoveToLastSeen() {
		velocity = (lastSeen - character.position).normalized * maxSpeed;
		velocity.y = 0;
		character.rotation = Quaternion.LookRotation (velocity);
		//not seen, move to last location
		//print ("lastSeen "+ lastSeen.x+ "position "+ character.position.x );
		if ((lastSeen - character.position).magnitude < .1) { //arrived at last seen location
			wander = true;
			seen = false;
		}
	}



	public void Attack(){
			animator.SetBool("Attack",true);
	}

	public void Walk(){
		animator.SetBool("Attack",false);
	}
	
	public void Dead() {
		StartCoroutine (KillCoroutine());
	}

	IEnumerator KillCoroutine() {

		animator.SetBool("Dead",true);
		yield return new WaitForSeconds (3.0f);

		Destroy (gameObject);
		Debug.Log (Time.time);
	}

	// Begin various functions for movement, pass in target

    public void Stop()
    {
        velocity = Vector3.zero;
        rotation = 0f;
      //  acceleration = Vector3.zero;
      //  angular = 0f;
    }


    // Kinematic Steering

    // KinSeek
    public void KinSeek(Transform target)
    {
		float dist = Vector3.Distance(transform.position, target.position);
		if (animator.GetBool("Attack") && dist < 0.1)
//			velocity = velocity;
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


	/* 

    // Dynamic Steering, couldnt get working right -Harrison

    private Vector3 acceleration;
    private float angular = 0f;
    public float maxAccel = 0.5f;
    public float maxAngular = 0.8f;


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

        GameObject newTarget = new GameObject();
        newTarget.transform.position = target.position + motor.velocity * prediction;

        Seek(newTarget.transform);

        Destroy(newTarget);
    }

    // Evade
    public void Evade(Transform target)
    {

        float maxPrediction = 5f;
        float prediction = 0f;

        Vector3 direction = target.position - character.position;
        float distance = direction.magnitude;

        float speed = velocity.magnitude;

        if (speed <= distance / maxPrediction)
            prediction = maxPrediction;
        else
            prediction = distance / speed;

        GameObject newTarget = new GameObject();
        newTarget.transform.position = target.position + enemy.velocity * prediction;

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

*/

}
