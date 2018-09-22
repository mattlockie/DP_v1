using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    private Transform target;
    public Rigidbody2D rb;
    //private readonly float speed = 1.0f;
    private readonly float moveSpeed = 10.0f;

    private Transform startingPosition;
    private Transform endingPosition;

    private Bezier bezier;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("DropPod").transform;
    }

    void Start()
    {
        startingPosition = transform;
        endingPosition = target;
  
        // determine and create a point between our start and end positions
        Vector3 halfway = (startingPosition.up + startingPosition.right).normalized;
        GameObject halfwayPoint = new GameObject();
        halfwayPoint.name = "HalfwayPoint";

        // for a better arc, slightly increase t.y manually
        halfwayPoint.transform.position = new Vector3(halfway.x, halfway.y * 3.0f, halfway.z);

        // create the curve
        bezier = Bezier.Instance;
        bezier.DrawQuadtraticCurve(startingPosition, halfwayPoint.transform, endingPosition);

        StartCoroutine(MoveTowardsTarget());
    }

    void Update()
    {
        transform.Translate();
    }

    IEnumerator MoveTowardsTarget()
    {
        Debug.Log("Bomb Starting!");
        Vector3[] positions = bezier.positions;

        // TODO: decrease time to simulate bomb dropping faster
        
        for (int i = 0; i < bezier.numPoints; i++)
        {
            //float t = i / (float)bezier.numPoints;
            //Debug.Log("T" + t);
            MoveNow(positions[i]);
            yield return new WaitForSeconds(.018f);
        }

        yield break;
    }

    void MoveNow(Vector3 moveToPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, moveToPosition, moveSpeed);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "DropPod")
        {
            // TODO: Destroy DroPod and End Game
            Debug.Log("Bomb has hit: " + hitInfo.name);
            // TODO: remove temporary halfwayPoint and line renderer curve
        }
    }
}