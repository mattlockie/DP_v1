using UnityEngine;
using System.Collections.Generic;

public class Trooper : MonoBehaviour {

    public Rigidbody2D rb;
    public GameObject chute;
    private readonly float defaultDrag = 1.0f;
    public float chuteDrag = 4.0f;
    private Vector3 chuteOffset;
    private GameObject troopersChute;
    private bool chuteDestroyed = false;

    public float chuteCountdown = 0.5f;
    private bool chuteReleased = false;

    private readonly int fallDamage = 1;

    public List<GameObject> alternateDeathEffects;

    private HealthManager healthManager;

    public enum State { Falling, Grounded };
    private State state = State.Falling;

    void Awake()
    {
        // offset for chute to line up with trooper correctly
        chuteOffset = new Vector3(0, 0.655f, 0);
        healthManager = GetComponent<HealthManager>();
        state = State.Falling;
    }
	
	void Update() 
	{
        if (!chuteDestroyed)
        {
            if (troopersChute == null && chuteReleased)
            {
                chuteDestroyed = true;
                rb.drag = defaultDrag;
                //ResetTrooper();
            }
        }

        if (!chuteReleased)
        {
            chuteCountdown -= Time.deltaTime;
            if (chuteCountdown <= 0.0f)
            {
                chuteReleased = true;
                //Debug.Log("Chute released!");
                troopersChute = (GameObject)Instantiate(chute, transform.position + chuteOffset, Quaternion.identity, transform);
                rb.drag = chuteDrag;
            }
        }


        //TODO: different trooper deathEffects depending on state.
        // If you have a chute on, deatheffect will be default. Otherwise, set to the "splatter on ground" death effect.
    }

    void OnCollisionEnter2D(Collision2D colliderInfo)
    {
        // kill the trooper if he falls without a chute (i.e. chute has been shot)
        if (colliderInfo.collider.tag == "Ground" && chuteDestroyed)
        {
            healthManager.SwitchDeathEffects(alternateDeathEffects);
            healthManager.TakeDamage(fallDamage);

            return;
        }

        // if the trooper lands on the ground without chute being destroyed, remove the chute
        if (colliderInfo.collider.tag == "Ground" && !chuteDestroyed)
        {
            // swap out our death effects for if/when we're landed on
            healthManager.SwitchDeathEffects(alternateDeathEffects);
            //ResetTrooper();
            rb.drag = defaultDrag;

            state = State.Grounded;

            Destroy(troopersChute);

            return;
        }

        // if the trooper hits another trooper, destroy them both
        if (colliderInfo.collider.tag == "Trooper" && state != State.Falling)
        {
            healthManager.SwitchDeathEffects(alternateDeathEffects);
            // get the HM of the other Trooper we've hit and damge it
            HealthManager hitTrooper = colliderInfo.gameObject.GetComponent<HealthManager>();
            hitTrooper.SwitchDeathEffects(alternateDeathEffects);
            hitTrooper.TakeDamage(fallDamage);

            // take damage ourselves
            healthManager.TakeDamage(fallDamage);

            return;
        }
    }
 }