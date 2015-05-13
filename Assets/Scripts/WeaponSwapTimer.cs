using UnityEngine;
using System.Collections;

public class WeaponSwapTimer : MonoBehaviour {


	public bool swapped = false;
	public float swapDelay = 1f;
	private float swapTime;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame

	void Update () {
		if (Time.time >= swapTime)
			swapped = false;
	}

	public void hasSwapped() {
		swapped = true;
		swapTime = Time.time + swapDelay;
	}

}
