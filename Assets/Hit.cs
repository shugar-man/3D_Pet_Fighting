using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public int damage = 90;
    [SerializeField] private Collider weapon;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit! " + gameObject.name);
        Debug.Log("hit!" + other.name + other.tag);
        if (other.gameObject.tag.Equals("Enemy")) // Check if it's the player
        {
            Debug.Log("Play hit!");
            HealthEnemy EnemyHealth = other.GetComponent<HealthEnemy>();
            if (EnemyHealth != null)
            {
                EnemyHealth.TakeDamage(damage);
            }
        }
        else if (other.gameObject.tag.Equals("Player") && !gameObject.tag.Equals("Pet")) // Check if it's the player
        {
            Debug.Log(gameObject.tag);
            Debug.Log("Player Get hit!");
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }



    public void EnableWeapon() {
     weapon.enabled = true;  
    }
    public void DisableWeapon() {
        weapon.enabled = false;
    }
    //OnTriggerEnter(Collider
    //void OnParticleCollision(GameObject other)
}
