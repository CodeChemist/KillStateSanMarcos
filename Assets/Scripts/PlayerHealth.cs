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
 *      + 4/26/15 --------- Initial version. - Chase Perdue
 *      + 5/2/15  --------- Re-made via VS2013's auto-create missing reference feature. 
 *                          Filename is missing (chase) for now and it is confusing 
 *                          and frustrating. Will change on restart.
 *      + 5/3/15  --------- Changed filename to resolve conflict. Added comments as needed.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts
{
    /// <summary>
    /// Class PlayerHealth should govern all aspects pertaining to the player's health as 
    /// they play the game. Health is a value that can be lost and gained depending on 
    /// external influences such as health pickups and enemy attacks.
    /// </summary>
    public class PlayerHealth : MonoBehaviour
    {
        public int startingHealth = 100;
        public int currentHealth;
        public Slider healthSlider;
        public Image damageImage;

        //I don't have a clip for this.
        //public AudioClip deathClip; 

        //used in the damageImage control
        public float flashSpeed = 5f;           

        //red, mostly transparent full screen image
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     

        //Harrison already has this.
        //Animator anim;        

        //Don't have this yet.
        //AudioSource playerAudio;

        //reference to previously made script in the tutorial. 
        //PlayerMovement playerMovement;        
        
        //this line was commented out when I get here...
        //PlayerShooting playerShooting;        
        
        bool isDead;
        bool damaged;

        /// <summary>
        /// Awake initializes health variable upon instantiation.
        /// </summary>
        void Awake()
        {
            //These three lines don't have refrences...yet.
            //anim = GetComponent <Animator> ();                    
            //playerAudio = GetComponent <AudioSource> ();
            //playerMovement = GetComponent <PlayerMovement> ();

            //this line was commented out when I get here...
            //playerShooting = GetComponentInChildren <PlayerShooting> ();        
            
            currentHealth = startingHealth;
        }

        /// <summary>
        /// As a player recieves damage, a full screen red image will flash. Not taking damege, 
        /// color fades away.
        /// </summary>
        void Update()
        {
            if (damaged)
            {
                damageImage.color = flashColour;
            }
            else
            {
                damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            }
            damaged = false;
        }

        /// <summary>
        /// TakeDamage takes health away from the player according to sucessful enemy attacks.
        /// Player health decreases as does the inset bar on the health slider. If the player's 
        /// health goes too low, they die.
        /// Called by a script outside of this file.
        /// </summary>
        /// <param name="amount">The value of damage an enemy has inflicted in a single strike.</param>
        public void TakeDamage(int amount)
        {
            damaged = true;

            currentHealth -= amount;

            healthSlider.value = currentHealth;

            //playerAudio.Play ();

            if (currentHealth <= 0 && !isDead)
            {
                Death();
            }
        }

        /// <summary>
        /// Handles player dealth event. When player health reaches 0 or lower (from TakeDamage method),
        /// the isDead plag is set to true and any game over messages will then appear.
        /// 
        /// TODO:
        ///     + player death animations, sounds etc.
        ///     + player death menu/screen
        ///     + How should movement be disabled?
        /// </summary>
        void Death()
        {
            isDead = true;

            //playerShooting.DisableEffects ();        //this line was commented out when I get here...

            // invalid reference chase, needs fixing ~chris
            //anim.SetTrigger ("Die");

            //playerAudio.clip = deathClip;
            //playerAudio.Play ();

            //playerMovement.enabled = false;
            //playerShooting.enabled = false;        //this line was commented out when I get here...
        }


        public void RestartLevel()
        {
            Application.LoadLevel(Application.loadedLevel);    //How is this even called?
        }
    }
}
