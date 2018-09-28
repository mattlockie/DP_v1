using UnityEngine;

public class Bomb : MonoBehaviour {

    public Transform target;
    public Rigidbody2D rb;
    private float speed = 9.0f;
    private readonly float distanceCheck = 0.175f;
    private readonly float speedTweak = 1.005f;

    public int attackDamage = 1;

    private Transform startingPosition;
    private Transform endingPosition;

    private Bezier bezier;
    Vector3 halfway;
    private Vector3 nextPoint;
    private Vector3[] positions;
    private bool shouldMove = true;

    private int pointIndex = 0;

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

        // set our positions and our start point
        positions = bezier.positions;
        nextPoint = positions[1];
        transform.position = positions[0];
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
                if (!GameManager.GameEnded)
                {
                    GetNextPoint();
                }
            }
        }
    }

    void GetNextPoint()
    {
        if (pointIndex >= positions.Length)
        {
            //Debug.Log("Should move is now FALSE");
            shouldMove = false;
            return;
        }
        pointIndex++;
        nextPoint = positions[pointIndex];
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "Mount")
        {
            // destory the bomb
            Destroy(gameObject);
        }

        if (!GameManager.GameEnded)
        {
            LifeManager mount = GameObject.Find("Mount").GetComponent<LifeManager>();
            if (mount != null && hitInfo.tag == "Mount")
            {
                mount.TakeDamage(attackDamage);
            }
        }
    }
}