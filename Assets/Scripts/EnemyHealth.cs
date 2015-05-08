using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {
	bool damaged = false;
	public float startingHealth = 100f;
	public float currentHealth;
	bool isDead = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TakeDamage (float amount){
		StartCoroutine (TouchedCoroutine (amount));
		
	}


	IEnumerator TouchedCoroutine(float amount){
		// check if the player has already been attacked
		if (!damaged) {
			// set flag to true to prevent additional collisions
			
			// Send message to zombie to deduct health
			Debug.Log ("Zombie Damage: " + amount);
			damaged = true;
			
			// attacks with anywhere from 0.5x to 1.5x base damage amount ~chris
			currentHealth -= (amount / 2 + Random.Range (0f, amount));

			if (currentHealth <= 0 && !isDead) {
				Death ();
			}
			
			// Pause for a second before allowing additional collisions
			yield return new WaitForSeconds (0.5f);
			
			// turn flag off to reallow collisions
			damaged = false;
		}
		
	}
	
	
	/// <summary>
	/// Death this instance. Player has died and sees slow(ish) fade to respawn.
	/// </summary>
	void Death ()
	{
		isDead = true;
		SendMessage ("Dead");
		//playerShooting.DisableEffects ();        //this line was commented out when I get here...
		
		// invalid reference chase, needs fixing ~chris
		//anim.SetTrigger ("Die");
		
		//playerAudio.clip = deathClip;
		//playerAudio.Play ();
		
		//pMov.SetActive = false;		//WHEN DO I ENABLE THIS??  
		//Unity example has playerMovement.enabled = false; before I changed to pMov, but 
		//pMov.SetControllable = false;
		
		//I WANT THESE LINES TO WORK. Havent tried them though.
		//playerShooting.enabled = false;        
	}
}
