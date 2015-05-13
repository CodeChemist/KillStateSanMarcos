using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}


	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		if (Input.GetKey ("escape")) {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

	}

}
