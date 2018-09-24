using UnityEngine;

public class Enemy : MonoBehaviour {

    public GameObject deathEffect;
    public Rigidbody2D rb;

    public GameObject cargo;
    private bool cargoDropped;
    //private Vector3 cargoDropOffset;

    public int health = 1;
    public float speed = 5.0f;
    private readonly int points = 10;

    //private float despawnCountdown;
    //private float despawnAfter = 5.0f;

    void Start()
    {
        // move enemy across the screen based on speed
        //despawnCountdown = despawnAfter;
        MoveAcrossScreen();
    }

    private void Update()
    {
        //despawnCountdown -= Time.deltaTime;
        //if (despawnCountdown <= 0)
        //{
        //    Despawn();
        //    despawnCountdown = despawnAfter;
        //}
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

    void Despawn()
    {
        // just despawn
        Destroy(gameObject);
    }

    void PlayDeathEffect()
    {
        // create the death effect at the enemy position
        GameObject deathEffectInstance = (GameObject)Instantiate(deathEffect, transform.position, transform.rotation);
        // after a delay, destroy the object :: needs sufficient time to play the fade-out effect
        Destroy(deathEffectInstance, 4.0f);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (cargo.name == "Bomb")
        {
            if (!cargoDropped && hitInfo.tag == "BombTrigger")
            {
                cargoDropped = true;
                Instantiate(cargo, transform.position, Quaternion.identity);
            }
        }

        // once an enemy is off screen, let's despawn them
        if (hitInfo.tag == "DespawnTrigger")
        {
            Despawn();
        }
    }

}