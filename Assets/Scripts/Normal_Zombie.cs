using UnityEngine;
using System.Collections;

public class Normal_Zombie : MonoBehaviour {
	public float ViewAngle = 60;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Vector3 left = (Quaternion.AngleAxis(-ViewAngle/2, Vector3.up )*fwd );
		Vector3 right = (Quaternion.AngleAxis(ViewAngle/2, Vector3.up )*fwd );
		Debug.DrawRay (transform.position + new Vector3(0,1,0), fwd*10, Color.green);
		Debug.DrawRay (transform.position + new Vector3(0,1,0), left*10, Color.red);
		Debug.DrawRay (transform.position + new Vector3(0,1,0), right*10, Color.red);

	}
}
