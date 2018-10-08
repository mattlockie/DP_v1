using UnityEngine;

public class TrooperMovement : MonoBehaviour {

    public enum State
    {
        Idle,
        Moving,
        ReachedTarget
    };

    public Animator anim;
    private Transform target;
    private Transform[] targetSteps;
  
    public State state = State.Idle;
    private readonly float moveSpeed = 1.5f;
    public bool move = false;
    public bool reachedTarget = false;

    public GameManager.Side side;

    [Header("Left Side")]
    public Transform targetLeft;
    public Transform[] targetLeftSteps;
    [Header("Right Side")]
    public Transform targetRight;
    public Transform[] targetRightSteps;

    private float offset;

 	void Update() 
	{
        // if we should move and the game has not ended...
        if (move && !GameManager.GameEnded)
        {
            TranslateToTarget();
        }

        // make sure when the game ends, that any anim on this trooper is stopped
        if (GameManager.GameEnded)
        {
            StopAnim();
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
            targetSteps = targetLeftSteps;
        }
        else if (side == GameManager.Side.Right)
        {
            target = targetRight;
            targetSteps = targetRightSteps;
        }
        else
        {
            Debug.LogWarning("Target 'Side' not found!?");
        }

        Vector3 dir = new Vector3(target.transform.position.x, transform.position.y, transform.position.z) - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.Translate(dir.normalized * moveSpeed * Time.deltaTime, Space.World);

        // set the troopers state to Moving when they're heading towards target
        state = State.Moving;

        // if we're not on the first trooper, have them finish a tiny bit earlier for a smooth jump 
        if (DeployedTroops.troopersReachedTarget > 0)
        {
            offset = 0.15f;
            offset = side == GameManager.Side.Right ? offset : offset * -1;
        }

        // if we've reached our destination...
        if (Mathf.Abs(transform.position.x) <= Mathf.Abs(target.transform.position.x + offset))
        {
            // stop movement and anim
            StopAnim();
            move = false;

            // set the troopers state to ReachedTarget so the next trooper can move
            state = State.ReachedTarget;

            // move trooper to next target
            for (int i = 0; i < targetSteps.Length; i++)
            {
                if (DeployedTroops.troopersReachedTarget == i)
                {
                    SetTarget(targetSteps[i]);
                    DeployedTroops.troopersReachedTarget++;
                    break;
                }
            }
        }
    }

    void SetTarget(Transform t)
    {
        transform.position = new Vector3(t.transform.position.x, t.transform.position.y, transform.position.z);
    }
}