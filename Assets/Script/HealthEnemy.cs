using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthEnemy : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Animator anim;
    public GameObject DamagePopup;
    public PickCoin pickCoin;
    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        pickCoin = GameObject.Find("Player").GetComponent<PickCoin>();
    }


    public void TakeDamage(float damage)
    {

        
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage! HP Enemy: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        ShowDamage(damage);
        
    }

    void ShowDamage(float damage) {
        var go = Instantiate(DamagePopup , transform.position , Quaternion.identity , transform);
        go.GetComponent<TextMesh>().text = damage.ToString();
    }

    void Die()
    {
        
        Debug.Log(gameObject.name + " has died!");
        anim.SetTrigger("Die");
        anim.Play("Die");
        GetComponent<Collider>().enabled = false; // Disable collisions
        Destroy(gameObject, 1f); // Destroy after animation
        pickCoin.DefeatBoss();
        if( gameObject.tag.Equals("Boss")) {
            SceneManager.LoadScene("HomeScene");
        }
    }
}
