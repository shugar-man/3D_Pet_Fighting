using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Animator anim;
    public Slider healthBar;
    public PlayerLocomotion player;

    public float maxMana = 100f;
    public float currentMana;
    public Slider manaBar;
    public GameOverScreen gameOverScreen;
    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        anim = GetComponent<Animator>();
        player = GetComponent<PlayerLocomotion>();
    }

    void Update() {
        if (currentHealth >= maxHealth) {
            currentHealth = maxHealth;
        }
        if (healthBar.value != currentHealth) {
            healthBar.value = currentHealth;

        }

        if (currentMana >= maxMana) {
            currentMana = maxMana;
        }
        if (manaBar.value != currentMana) {
            manaBar.value = currentMana;

        }
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage! HP: " + currentHealth);
        anim.SetTrigger("GetHit");
        player.isHitting = true;
        anim.SetBool("isHitting" , true);
        //animatorManager.PlayerTargetAnimation("attack" , false);
        StartCoroutine(ResetIsHitting());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator ResetIsHitting()
    {
        yield return new WaitForSeconds(1f);
        player.isHitting = false;
        anim.SetBool("isHitting" , false);
    }

    void Die()
    {
        gameOverScreen.Setup();
        player.died = true;
        Debug.Log(gameObject.name + " has died!");
        anim.SetTrigger("Die");
        anim.Play("Die");
        GetComponent<BossAI>().enabled = false; // Stop AI
        GetComponent<Collider>().enabled = false; // Disable collisions
        Destroy(gameObject, 3f); // Destroy after animation
        
    }

    public void HealHP(float hp)
    {
        currentHealth += hp;
        Debug.Log(gameObject.name + " get " + hp + "  HP!: " + currentHealth);
        //animatorManager.PlayerTargetAnimation("attack" , false);
    }
    public void HealMana() {
        currentMana += 0.1f;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        Debug.Log("aaa");
        if (other.tag == "Enemy")
        {
            Health playerHealth = other.GetComponent<Health>();
            Debug.Log("a");
            TakeDamage(10f);
            if (playerHealth != null)
            {
                //playerHealth.TakeDamage(damage);
            }
        }
    }*/

}
