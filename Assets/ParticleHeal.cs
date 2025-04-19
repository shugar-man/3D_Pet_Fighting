using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHeal : MonoBehaviour
{
    public int heal = 20;
    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag.Equals("Player")) // Check if it's the player
        {
        Debug.Log("Particle heal: " + other.name);

        // Example: damage the enemy
        Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.HealHP(heal);
            }
        }
    }
}
