using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//https://www.youtube.com/watch?v=y3OZXMxsrUI
public class HealthBar : MonoBehaviour {

	Image img;

	// Use this for initialization
	void Start () {
		img = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		//img = GetComponent<Image>();
	}

	void UpdateHealth(float hAmount){
		img.fillAmount = hAmount / 100f;
	}
}
