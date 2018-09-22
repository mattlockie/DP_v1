using UnityEngine;

public class Enemy : MonoBehaviour {

    public GameObject deathEffect;
    public Rigidbody2D rb;

    public GameObject cargo;
    public float cargoDropDelay;
    private Vector3 cargoDropOffset;

    public int health = 1;
    public float speed = 5.0f;
    private readonly int points = 10;

    void Start()
    {
        // move enemy across the screen based on speed
        MoveAcrossScreen();
    }

    void MoveAcrossScreen()
    {
        Transform enemyTransform = rb.transform;
        
        if (enemyTransform.position.x < 0)
        {   
            // if spawned on left side of screen, move right
            transform.rotation = Quaternion.Euler(enemyTransform.rotation.x, 0, enemyTransform.rotation.z);
        }
        else if (enemyTransform.position.x > 0)
        {
            // if spawned on right side of screen, move left
            transform.rotation = Quaternion.Euler(enemyTransform.rotation.x, -180, enemyTransform.rotation.z);
        }
        rb.velocity = transform.right * speed;
    }

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
        // update score
        GameManager.Instance.UpdateScore(points);

        // die!
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