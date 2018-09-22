using UnityEngine;

public class Weapon : MonoBehaviour {

    public Transform projectileSpawnPoint;
    public GameObject projectilePrefab;

    public float fireRate = 0.2f;
    private float fireCooldown = 0f;

    void Update() 
	{
        // shoot projectile
        if (Input.GetButton("Fire1"))
        {
            if (fireCooldown >= fireRate)
            {
                // fire!
                Shoot();
                // reset
                fireCooldown = 0f;
            }
        }

        // only adjust the cooldown to it's max value so we don't count up every frame if we haven't fired in a while
        if (fireCooldown <= fireRate)
        {
            fireCooldown += Time.deltaTime;
        }
        
    }

    void Shoot()
    {
        Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
    }
}