using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	bool esc;
	// Update is called once per frame
	void Update () {
		if (esc == false)
		Screen.lockCursor = true;

		if (Input.GetKey ("escape")) {
			esc = true;
		}
	}
}
