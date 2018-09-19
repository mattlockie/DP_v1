using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb;

    public float speed = 20.0f;
    public int attackDamage = 1;
    private float projectileLifespan = 1.25f;
    private readonly int points = -1;

    void Start() 
	{
        // add velocity to projectile once it has been spawned
        rb.velocity = transform.up * speed;

        // reduce score by each projectile fired
         GameManager.Instance.UpdateScore(points);
    }

    void Update()
    {
        // despawn projectiles
        if (projectileLifespan <= 0f)
        {
            Destroy(gameObject);
        }
        projectileLifespan -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // remove projectile
        Destroy(gameObject);

        // damage the enemy
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(attackDamage);
        }
    }
}