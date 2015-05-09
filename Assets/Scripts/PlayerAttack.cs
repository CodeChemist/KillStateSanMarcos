using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

	public float rotationDegreesPerSecond = 30f;
	public float rotationDegreesAmount = 60f;
	public float totalRotation = 0;
	public float AttackDamage = 25;
	private bool atk = false;
	private bool back = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//if we haven't reached the desired rotation, swing
		if (Input.GetMouseButtonDown (0))
			atk = true;

		if ((totalRotation < rotationDegreesAmount) && (atk == true) && (back == false))
			SwingOpen ();
		else if (totalRotation >= rotationDegreesAmount) {
			back = true;
			atk = false;
		}
		if (back == true) {
			SwingClosed ();

			if (totalRotation <= 0)
				back = false;
		}

	}
	
	void SwingOpen()
	{   
		float currentAngleZ = transform.localRotation.eulerAngles.x;
		transform.localRotation = Quaternion.AngleAxis (currentAngleZ + (Time.deltaTime * rotationDegreesPerSecond), Vector3.right);
		totalRotation += Time.deltaTime * rotationDegreesPerSecond;
	}
	void SwingClosed()
	{
		float currentAngleZ = -transform.localRotation.eulerAngles.x;
		transform.localRotation = Quaternion.AngleAxis (currentAngleZ + (Time.deltaTime * rotationDegreesPerSecond), Vector3.left);
		totalRotation -= Time.deltaTime * rotationDegreesPerSecond;
	}

}
