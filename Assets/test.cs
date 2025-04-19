using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
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
    }

}
