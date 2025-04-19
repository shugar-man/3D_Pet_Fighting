using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPetHit : MonoBehaviour
{
    public int damage = 5;

    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag.Equals("Enemy")) // Check if it's the player
        {
        Debug.Log("Particle hit enemy: " + other.name);

        // Example: damage the enemy
        HealthEnemy enemyHealth = other.GetComponent<HealthEnemy>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }
}
