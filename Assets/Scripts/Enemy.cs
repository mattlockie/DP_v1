using UnityEngine;

public class Enemy : MonoBehaviour {

    public Rigidbody2D rb;
    private CargoManager cargoManager;

    public float speed = 5.0f;

    void Start()
    {
        cargoManager = GetComponent<CargoManager>();
         // move enemy across the screen based on speed
        //cargoDropTime = Random.Range(cargoDropTimeMin, cargoDropTimeMax);
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

    void Despawn()
    {
        // just despawn
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // if this enemy uses a trigger to drop cargo, check if we've hit the respective trigger
        // and that we haven't already dropped cargo
        if (cargoManager.useTrigger && !cargoManager.cargoDropped)
        {
            if (hitInfo.tag == cargoManager.trigger.tag)
            {
                cargoManager.DropCargo();
            }
        }

        // once an enemy is off screen, let's despawn them
        if (hitInfo.tag == "DespawnTrigger")
        {
            Despawn();
        }
    }
}