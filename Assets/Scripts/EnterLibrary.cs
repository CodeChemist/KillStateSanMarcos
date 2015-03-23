using UnityEngine;
using System.Collections;

public class EnterLibrary : MonoBehaviour {

	void OnTriggerEnter(){

		
		// load next scene
		Application.LoadLevel (1);
	}
}
