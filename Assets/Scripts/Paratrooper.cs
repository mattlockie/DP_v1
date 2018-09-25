using UnityEngine;

public class Paratrooper : MonoBehaviour {

    public Rigidbody2D chuteRigidBody;
    public Rigidbody2D trooperRigidBody;

    public GameObject[] chuteDeathEffects;
    public GameObject[] trooperDeathEffects;

    private int chuteHealth = 1;
    //private int trooperHealth = 1;
    private int points = 5;

    void Start() 
	{
        chuteRigidBody.drag = 3.0f;
        trooperRigidBody.drag = 3.0f;
    }
	
	void Update() 
	{
		
	}

    public void TakeDamage(int damage, string tag, string damageSource)
    {
        if (tag == "Chute")
        {
            chuteHealth -= damage;
            if (chuteHealth <= 0)
            {
                DestroyChute();
            }
        }
        else if (tag == "Trooper")
        {

        }
        else
        {
            // do nothing
        }
        
    }

    void DestroyChute()
    {
        GameManager.Instance.UpdateScore(points);
        PlayDeathEffects();
        Destroy(chuteRigidBody.gameObject);
        // make trooper fall at normal speed as they aren't being held by a chute!
        trooperRigidBody.drag = 0.0f;
    }

    void Die()
    {
        // update score
        GameManager.Instance.UpdateScore(points);

        // die!
        PlayDeathEffects();
        Destroy(gameObject);
    }

    void Despawn()
    {
        // just despawn
        Destroy(gameObject);
    }

    void PlayDeathEffects()
    {
        // create the death effect at the enemy position
        //foreach (GameObject effect in deathEffects)
        //{
        //    GameObject deathEffectInstance = (GameObject)Instantiate(effect, transform.position, transform.rotation);
        //    // after a delay, destroy the object :: needs sufficient time to play the fade-out effect
        //    Destroy(deathEffectInstance, 4.0f);
        //}
    }

    // TODO: Allow the chute to take 1 hit damage, get destroyed, then set gravity scale back to 1 for the Trooper
    // TODO: Allow the Trooper to take 1 hit damage, then destory entire Paratrooper gameObject.
    // TODO: Trooper detecting ground and chute being destroyed.
}