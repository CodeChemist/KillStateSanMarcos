/*
 * 
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
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    //public AudioClip deathClip;           //I don't have a clip for that.
    public float flashSpeed = 5f;           //used in the damageImage control
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     //red, mostly transparent full screen image


    //Animator anim;      
    //AudioSource playerAudio;

    //PlayerMovement playerMovement;        //reference to previously made script in the tutorial. 
                                            //not used in this project. does Harrison have something already?

    //PlayerShooting playerShooting;        //this line was commented out when I get here...
    bool isDead;
    bool damaged;


    void Awake ()
    {
        //anim = GetComponent <Animator> ();                    
        //playerAudio = GetComponent <AudioSource> ();
        //playerMovement = GetComponent <PlayerMovement> ();
        //playerShooting = GetComponentInChildren <PlayerShooting> ();        //this line was commented out when I get here...
        currentHealth = startingHealth;
    }


    void Update ()
    {
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


    public void TakeDamage (int amount)
    {
        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;

        //playerAudio.Play ();

        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        //playerShooting.DisableEffects ();        //this line was commented out when I get here...

        anim.SetTrigger ("Die");

        //playerAudio.clip = deathClip;
        //playerAudio.Play ();

        //playerMovement.enabled = false;
        //playerShooting.enabled = false;        //this line was commented out when I get here...
    }


    public void RestartLevel ()
    {
        Application.LoadLevel (Application.loadedLevel);
    }
}
