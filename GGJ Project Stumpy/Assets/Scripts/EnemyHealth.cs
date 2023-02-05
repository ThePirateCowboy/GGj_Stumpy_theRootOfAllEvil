using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    /* This script is meant to be on anything that can be hit by the melee attack. 
     * It decides how much health the object has, whether it can be damageable at all, the time after a 
     * hit that the object is invulnerable in order to avoid double damage on a single hit. 
     * 
     * References: 
     * 
     * https://github.com/I-Am-Err00r/Melee-Attack/blob/main/EnemyHealth.cs
     */

    [Header("Object Stats")]
    public GameObject Loot;
    [Range(0,100)]public float chanceTodrop = 50;
    public Animator anim;
    public GameObject DeathEffect;
    public int enemyValue;
    [Tooltip("Enabling this means the object can not be damaged.")]
    [SerializeField] private bool damageable = true;
    [Tooltip("The  total health of the object should the object be damagable.")]
    [SerializeField] private int healthAmount = 100;
    [Tooltip("The amount of time that has to have elapsed since the last damage taken, in order for the object to be damagable again.")]
    [SerializeField] private float invulnerabilityTime = .2f;
    [Tooltip("If enabled a successfull downward attack on the object will propell the player upward.")]
    public bool giveUpwardForce = true;
    //*****************************************
    private bool hit;
    private int currentHealth;

    private void Start()
    {
        currentHealth = healthAmount;
    }

    /// <summary>
    /// Damages the object by the amount, stores the new health value. If the object has no more health left this will destroy the object.
    /// </summary>
    /// <param name="amount"></param>
    public void Damage(int amount)
    {
        if (damageable && !hit && currentHealth > 0)
        {
            hit = true;
            currentHealth -= amount;
            anim.SetTrigger("isHurt");
            if (currentHealth <= 0)
            {
                float dropselect = Random.Range(0, 100);
                if (dropselect <= chanceTodrop)
                {
                    Instantiate(Loot, transform.position, transform.rotation);
                }

                currentHealth = 0;
                gameObject.SetActive(false);
                Instantiate(DeathEffect, transform.position, transform.rotation);
            }
            else
            {
                StartCoroutine(TurnOffHit());
            }
        }
    }
    
    /// <summary>
    /// Turns off the capacity for this object to be damaged for the invulnerabilityTime(seconds).
    /// </summary>
    /// <returns></returns>
    private IEnumerator TurnOffHit()
    {
        yield return new WaitForSeconds(invulnerabilityTime);
        hit = false;
    }
}
