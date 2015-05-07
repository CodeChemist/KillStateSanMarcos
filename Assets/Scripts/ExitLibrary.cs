using UnityEngine;
using System.Collections;

public class ExitLibrary : MonoBehaviour {

	void OnTriggerEnter(){

		
		// load next scene
		Application.LoadLevel (Application.loadedLevel + 1);
	}
}
