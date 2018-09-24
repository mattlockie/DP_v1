using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    private Transform target;
    public Rigidbody2D rb;
    private float speed = 9.0f;
    private float distanceCheck = 0.175f;
    private float speedTweak = 1.005f;
    private readonly float moveSpeed = 10.0f;

    private Transform startingPosition;
    private Transform endingPosition;

    private Bezier bezier;
    Vector3 halfway;
    private Vector3 currentPoint;
    private Vector3 nextPoint;
    private Vector3[] positions;
    private bool shouldMove = true;

    private int pointIndex = 0;

    public GameObject deathEffect;
    public int health = 1;
    public int points = 30;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("DropPod").transform;
    }

    void Start()
    {
        startingPosition = transform;
        endingPosition = target;

        // determine and create a point between our start and end positions
        // by controlling the halfway point more specifically, we can make the curve nice
        if (startingPosition.position.x < 0)
        {
            // if spawned on left side of screen
            halfway = (startingPosition.up + -startingPosition.right).normalized;
        }
        else if (startingPosition.position.x > 0)
        {
            // if spawned on right side of screen
            halfway = (startingPosition.up + startingPosition.right).normalized;
        }
        GameObject halfwayPoint = new GameObject();
        halfwayPoint.name = "HalfwayPoint";

        // for a better arc, slightly increase t.y manually
        halfwayPoint.transform.position = new Vector3(halfway.x, halfway.y, halfway.z);
        //halfwayPoint.transform.position = new Vector3(halfway.x, halfway.y * 3.0f, halfway.z);

        // create the curve
        bezier = Bezier.Instance;
        bezier.DrawQuadtraticCurve(startingPosition, halfwayPoint.transform, endingPosition);

        // remove halfway point as it's no longer needed (until next bomb)
        Destroy(halfwayPoint);

        positions = bezier.positions;
        nextPoint = positions[1];
        transform.position = positions[0];
        //StartCoroutine(MoveTowardsTarget());
    }

    void Update()
    {
        if (shouldMove)
        {
            Vector3 dir = nextPoint - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
            
            if (Vector3.Distance(transform.position, nextPoint) <= distanceCheck)
            {
                speed *= speedTweak;
                GetNextPoint();
            }
        }
    }

    void GetNextPoint()
    {
        if (pointIndex >= positions.Length)
        {
            Debug.Log("Should move is now FALSE");
            shouldMove = false;
            return;
        }
        pointIndex++;
        nextPoint = positions[pointIndex];
    }

    IEnumerator MoveTowardsTarget()
    {
        Vector3[] positions = bezier.positions;

        for (int i = 0; i < bezier.numPoints; i++)
        {
            MoveNow(positions[i]);
            yield return new WaitForSeconds(.018f);
        }

        yield break;
    }

    void MoveNow(Vector3 moveToPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, moveToPosition, moveSpeed);
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

    void PlayDeathEffect()
    {
        // create the death effect at the enemy position
        GameObject deathEffectInstance = (GameObject)Instantiate(deathEffect, transform.position, transform.rotation);
        // after a delay, destroy the object :: needs sufficient time to play the fade-out effect
        Destroy(deathEffectInstance, 4.0f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "DropPod")
        {
            // TODO: Destroy DroPod and End Game

            Destroy(gameObject);
            
            
            //GameObject hpGo = GameObject.Find("HalfwayPoint");
            //DestroyObject(hpGo);



            // TODO: remove temporary halfwayPoint and line renderer curve
        }
    }
}