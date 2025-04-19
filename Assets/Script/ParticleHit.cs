using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHit : MonoBehaviour
{
    public int damage = 20;

    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag.Equals("Player")) // Check if it's the player
        {
        Debug.Log("Particle hit: " + other.name);

        // Example: damage the enemy
        Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
