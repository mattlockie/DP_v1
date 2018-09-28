using UnityEngine;

public class TrooperMovement : MonoBehaviour {

    public Animator anim;
    public Transform targetLeft;
    public Transform targetRight;
    private Transform target;

    public enum State
    {
        Idle,
        Moving,
        ReachedTarget
    };

    public State state = State.Idle;
    private readonly float moveSpeed = 1.5f;
    public bool move = false;
    public bool reachedTarget = false;

    public GameManager.Side side;

 	void Update() 
	{
        if (move)
        {
            TranslateToTarget();
        }
    }

    public void StartAnim()
    {
        anim.SetBool("Walk", true);
    }

    public void StopAnim()
    {
        anim.SetBool("Walk", false);
    }


    public void TranslateToTarget()
    {
        if (side == GameManager.Side.Left)
        {
            target = targetLeft;
        }
        else if (side == GameManager.Side.Right)
        {
            target = targetRight;
        }
        else
        {
            Debug.Log("Target 'Side' not found!?");
        }

        Vector3 dir = new Vector3(target.transform.position.x, transform.position.y, transform.position.z) - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.Translate(dir.normalized * moveSpeed * Time.deltaTime, Space.World);

        // set the troopers state to Moving when they're heading towards target
        state = State.Moving;

        // if we've reached our destination...
        if (Mathf.Abs(transform.position.x) <= Mathf.Abs(target.transform.position.x))
        {
            // make sure we're on the exact point
            transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);

            // stop movement and anim
            move = false;
            StopAnim();

            // set the troopers state to ReachedTarget so the next trooper can move
            state = State.ReachedTarget;
        }
    }
}