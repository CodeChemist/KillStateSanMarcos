/*
 * 
 * ------------------------------- EnemyAttack(chase).cs -------------------------------
 * 
 * Purpose:     To manage the enemy attack events. Note: THIS IS FOR TESTING PURPOSES ONLY. 
 *      This is a pre-main game integration script that is very likely to not play well 
 *      with the rest. Do not include when building.
 * 
 * Created By:  Unity Technologies (free Survival Shooter tutorial as cited elsewhere)
 * Modified By: Chase Perdue
 * Date:        5/2/15
 * Class:       CS485 - Game Programming
 * 
 * Changelog:
 *      +  5/2/15 ------ Initial version. - Chase Perdue
 */
using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;


    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    //EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();
        //enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }


    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && playerInRange/* && enemyHealth.currentHealth > 0*/)
        {
            Attack ();
        }

        if(playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger ("PlayerDead");
        }
    }


    void Attack ()
    {
        timer = 0f;

        if(playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }
    }
}
