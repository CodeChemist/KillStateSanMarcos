/*
 * ------------------------------- PlayerHealth.cs -------------------------------
 * 
 * Purpose:     To manage the player health UI events.
 * 
 * Created By:  Unity Technologies (free Survival Shooter tutorial as cited elsewhere)
 * Modified By: Chase Perdue
 * Date:        4/26/15
 * Class:       CS485 - Game Programming
 * 
 * Changelog:
 *      + 4/26/15 ------ Initial version. - Chase Perdue
 * 		+ 5/6/15  ------ Adding message passing to take damage from enemies.
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float startingHealth = 100f;
    public float currentHealth;
    //public Slider healthSlider;
    public Image damageImage;
    //public AudioClip deathClip;           //I don't have a clip for that.
    public float flashSpeed = 5f;           //used in the damageImage control. 
	public float deathFlashSpeed = 0.1f;	//used in player death HOPEFULLY a slower flash/fade to respawn.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     //red, mostly transparent full screen image
	private GameObject gm;		//ref to GM object
	private GameObject hb;		//ref to GUI health box....IS RELEVANT??
	private GameObject pMov;	//ref to player movement. Here goes!

	//CharacterMotorMovement playerMovement;

    //Animator anim;      
    //AudioSource playerAudio;

    //PlayerMovement playerMovement;        //reference to previously made script in the tutorial. 
                                            //not used in this project. does Harrison have something already?

    //PlayerShooting playerShooting;        //this line was commented out when I get here...
    bool damaged;


    void Awake ()
    {
        //anim = GetComponent <Animator> ();                    
        //playerAudio = GetComponent <AudioSource> ();
		//playerMovement = GetComponent <CharacterMotorMovement> ();		//attempting to call so movement can be halted upon death.
		pMov = GameObject.Find ("CharacterMotorMovement");		//I just made this one up. would it work??
        //playerShooting = GetComponentInChildren <PlayerShooting> ();        //this line was commented out when I get here...
        currentHealth = startingHealth;		//initializes starting health

		gm = GameObject.Find ("GameManager");
		hb = GameObject.Find("HealthBar");
		//hb.SendMessage ("UpdateHealth", currentHealth);
    }

	/// <summary>
	/// Update this instance with damage flash as health goes down.
	/// </summary>
    void Update ()
    {
		return;
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }


    public void TakeDamage (float amount){
		StartCoroutine (TouchedCoroutine (amount));

	}

	// Handles attacking player, prevents multiple successive collisions
	IEnumerator TouchedCoroutine(float amount){
		// check if the player has already been attacked
		if (!damaged) {
			// set flag to true to prevent additional collisions
			damaged = true;
		
			// Send message to player to deduct health
			Debug.Log ("Took Damage: " + amount);

			// attacks with anywhere from 0.5x to 1.5x base damage amount ~chris
			currentHealth -= (amount / 2 + Random.Range (0f, amount));
			hb.SendMessage ("UpdateHealth", currentHealth);
			//healthSlider.value = currentHealth;
			
			//playerAudio.Play ();
			
			if (currentHealth <= 0) {
				Death ();
			}
		
			// Pause for a second before allowing additional collisions
			yield return new WaitForSeconds (1.0f);
		
			// turn flag off to reallow collisions
			damaged = false;
		}

	}


	/// <summary>
	/// Death this instance. Player has died and sees slow(ish) fade to respawn.
	/// </summary>
    void Death ()
    {

		Application.LoadLevel(3);
        //isDead = true;

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


    public void RestartLevel ()
    {
        Application.LoadLevel (Application.loadedLevel);

		//Chris's solution:

		// play the death sound effect
		//AudioSource.PlayClipAtPoint(dyingSound, transform.position);
		
		// remove the player from the level
		//Destroy(gameObject);
		
		// call the Game Manager to respawn a new player object
		//gm.SendMessage("SpawnPlayer");
    }
}
