using UnityEngine;
using System.Collections.Generic;

public class Trooper : MonoBehaviour {

    public enum State
    {
        Falling,
        Grounded
    };

    public Rigidbody2D rb;
    public GameObject chute;
    private readonly float defaultDrag = 1.0f;
    public float chuteDrag = 4.0f;
    private Vector3 chuteOffset;
    private GameObject troopersChute;
    public bool chuteDestroyed = false;
    public float chuteCountdown = 0.5f;
    private bool chuteReleased = false;

    private readonly int fallDamage = 1;

    public List<GameObject> alternateDeathEffects;

    private LifeManager lifeManager;

    private State state = State.Falling;
    public GameManager.Side side;

    void Awake()
    {
        // offset for chute to line up with trooper correctly
        chuteOffset = new Vector3(0, 0.655f, 0);
        state = State.Falling;

        lifeManager = GetComponent<LifeManager>();
    }
	
	void Update() 
	{
        if (!chuteDestroyed)
        {
            if (troopersChute == null && chuteReleased)
            {
                chuteDestroyed = true;
                rb.drag = defaultDrag;
            }
        }

        if (!chuteReleased)
        {
            chuteCountdown -= Time.deltaTime;
            if (chuteCountdown <= 0.0f)
            {
                chuteReleased = true;
                troopersChute = (GameObject)Instantiate(chute, transform.position + chuteOffset, Quaternion.identity, transform);
                rb.drag = chuteDrag;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D colliderInfo)
    {
        // kill the trooper if he falls without a chute (i.e. chute has been shot)
        if (colliderInfo.collider.tag == "Ground" && chuteDestroyed)
        {
            // get sound FX
            Effects effects = GetComponent<Effects>();
            if (effects != null)
            {
                string soundToPlay = effects.GetSound("GroundHit");
                AudioManager.Instance.Play(soundToPlay);
            }
            lifeManager.SwitchDeathEffects(alternateDeathEffects);
            lifeManager.TakeDamage(fallDamage);

            return;
        }

        // if the trooper lands on the ground without chute being destroyed, remove the chute
        if (colliderInfo.collider.tag == "Ground" && !chuteDestroyed)
        {
            // swap out our death effects for if/when we're landed on
            lifeManager.SwitchDeathEffects(alternateDeathEffects);

            rb.drag = defaultDrag;

            state = State.Grounded;

            rb.isKinematic = true;

            Destroy(troopersChute);

            return;
        }

        // if the trooper hits another trooper, destroy them both
        if (colliderInfo.collider.tag == "Trooper" && state != State.Falling)
        {
            // get sound FX
            Effects effects = GetComponent<Effects>();
            if (effects != null)
            {
                string soundToPlay = effects.GetSound("GroundHit");
                AudioManager.Instance.Play(soundToPlay);
            }

            lifeManager.SwitchDeathEffects(alternateDeathEffects);
            // get the LifeManager of the other Trooper we've hit and damge it
            LifeManager hitTrooper = colliderInfo.gameObject.GetComponent<LifeManager>();
            hitTrooper.SwitchDeathEffects(alternateDeathEffects);
            hitTrooper.TakeDamage(fallDamage);
            DeployedTroops.Instance.RemoveTrooper(hitTrooper.gameObject); // remove trooper from our deployed trooper list
            

            // take damage ourselves
            lifeManager.TakeDamage(fallDamage);

            return;
        }

        // if the trooper lands on another troopers chute, destroy that chute
        if (colliderInfo.collider.tag == "Chute" && state == State.Falling)
        {
            // get sound FX
            Effects effects = colliderInfo.collider.GetComponent<Effects>();
            if (effects != null)
            {
                string soundToPlay = effects.GetSound("Death");
                AudioManager.Instance.Play(soundToPlay);
            }
            LifeManager troopersChute = colliderInfo.collider.gameObject.GetComponent<LifeManager>();
            troopersChute.TakeDamage(fallDamage);

        }
    }

    private void OnTriggerEnter2D(Collider2D colliderInfo)
    {
        // if we land on the DropPod...
        if ((colliderInfo.tag == "Mount" || colliderInfo.tag == "Base") && state == State.Falling)
        {
            // if the game isn't already over...
            if (!GameManager.GameEnded) {
                // get sound FX
                Effects effects = colliderInfo.GetComponent<Effects>();
                if (effects != null)
                {
                    string soundToPlay = effects.GetSound("Death");
                    AudioManager.Instance.Play(soundToPlay);
                }
                // get the LifeManager of the other Mount and damge it
                LifeManager target = GameObject.Find("Mount").GetComponent<LifeManager>();
                target.TakeMortalDamage();
            }

            // reduce points to 0 for this trooper and take damage ourselves
            lifeManager.points = 0;
            lifeManager.TakeMortalDamage();

            return;
        }
    }
}