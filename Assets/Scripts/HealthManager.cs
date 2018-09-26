using UnityEngine;
using System.Collections.Generic;

public class HealthManager : MonoBehaviour {

    public int health;
    public int points;
    public List<GameObject> deathEffects;
    public Transform effectsPositionOffset;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void PlayDeathEffects()
    {
        // create the death effect at the enemy position
        foreach (GameObject effect in deathEffects)
        {
            if (effectsPositionOffset != null)
            {
                GameObject deathEffectInstance = (GameObject)Instantiate(effect, effectsPositionOffset.position, effect.transform.rotation);
                // after a delay, destroy the object :: needs sufficient time to play the fade-out effect
                Destroy(deathEffectInstance, 4.0f);
            }
            else
            {
                GameObject deathEffectInstance = (GameObject)Instantiate(effect, transform.position, effect.transform.rotation);
                // after a delay, destroy the object :: needs sufficient time to play the fade-out effect
                Destroy(deathEffectInstance, 4.0f);
            }
        }
    }

    public void SwitchDeathEffects(List<GameObject> alternateDeathEffects)
    {
        // let a gameobject that has different death effects override the default
        deathEffects.Clear();
        deathEffects.AddRange(alternateDeathEffects);
    }

    void Die()
    {
        // update score
        GameManager.Instance.UpdateScore(points);

        // die!
        PlayDeathEffects();
        Destroy(gameObject);
    }
}