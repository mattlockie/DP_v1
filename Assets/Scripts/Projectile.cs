using UnityEngine;

public class Projectile : MonoBehaviour {

    public Rigidbody2D rb;

    public float speed = 20.0f;
    public int attackDamage = 1;
    private float projectileLifespan = 1.5f;
    private readonly int points = -1;
    public string fireSound;

    void Start() 
	{
        // add velocity to projectile once it has been spawned
        rb.velocity = transform.up * speed;

        // reduce score by each projectile fired
        GameManager.Instance.UpdateScore(points);

        // play sound effect
        AudioManager.Instance.Play(fireSound);
    }

    void Update()
    {
        // despawn projectiles
        if (projectileLifespan <= 0f)
        {
            Destroy(gameObject);
        }
        projectileLifespan -= Time.deltaTime;

        // we don't want to be able to shoot gameObjects that are off-screen
        if (!GetComponent<Renderer>().isVisible)
        {
            //Debug.Log("INVISIBLE!");
            Destroy(gameObject);
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // damage the enemy
        LifeManager target = hitInfo.GetComponent<LifeManager>();
        if (target != null)
        {
            // get sound FX
            Effects effects = hitInfo.GetComponent<Effects>();
            if (effects != null)
            {
                string soundToPlay = effects.GetSound("Death");
                AudioManager.Instance.Play(soundToPlay);
            }
            Destroy(gameObject);
            target.TakeDamage(attackDamage);
        }
    }
}