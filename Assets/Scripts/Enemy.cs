using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public GameObject deathEffect;
    public int health = 1;
    public float speed = 20.0f;
    private int points = 10;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        PlayDeathEffect();
        Destroy(gameObject);
    }

    private void PlayDeathEffect()
    {
        // create the death effect at the enemy position
        GameObject deathEffectInstance = (GameObject)Instantiate(deathEffect, transform.position, transform.rotation);
        // after a delay, destroy the object :: needs sufficient time to play the fade-out effect
        Destroy(deathEffectInstance, 4.0f);
    }
}