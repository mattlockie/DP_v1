using UnityEngine;
using System.Collections.Generic;

public class DeployedTroops : MonoBehaviour {

    public static DeployedTroops Instance;

    public List<GameObject> troopersLeft;
    public List<GameObject> troopersRight;
    private List<GameObject> troopersToMove;

    private bool moveNow = false;
    private bool canAddTroops = true;
    private bool allTroopsMoved = false;

    public static int troopersReachedTarget = 0;

    private void Awake()
    {
        InitialiseDeployedTroops();
    }

    void Update()
    {
        // this is probably over-complicated, but in short we're iterating
        // through our troopers that are to be moved, and kicking off their
        // movement once the previous trooper has finished.
        if (moveNow)
        {
            if (!allTroopsMoved)
            {
                TrooperMovement prevTroopersMovement = null;
                for (int i = 0; i < troopersToMove.Count; i++)
                {
                    TrooperMovement thisTroopersMovement = troopersToMove[i].GetComponent<TrooperMovement>();
                    if (prevTroopersMovement == null && thisTroopersMovement.state == TrooperMovement.State.Idle) // move first trooper
                    {
                        thisTroopersMovement.StartAnim();
                        thisTroopersMovement.side = thisTroopersMovement.gameObject.GetComponent<Trooper>().side;
                        thisTroopersMovement.move = true;
                    }
                    else if (
                        prevTroopersMovement != null &&
                          prevTroopersMovement.state == TrooperMovement.State.ReachedTarget && thisTroopersMovement.state == TrooperMovement.State.Idle
                        )
                    {
                        thisTroopersMovement.StartAnim();
                        thisTroopersMovement.side = thisTroopersMovement.gameObject.GetComponent<Trooper>().side;
                        thisTroopersMovement.move = true;
                    }

                    // if every trooper has reached the target, set allTroops to moved which triggers the game ending
                    if (troopersToMove.Count == i + 1 && thisTroopersMovement.state == TrooperMovement.State.ReachedTarget)
                    {
                        allTroopsMoved = true;

                        return;
                    }
                    prevTroopersMovement = troopersToMove[i].GetComponent<TrooperMovement>();
                }
            }
        }

        // blow up the Mount and end the game
        if (allTroopsMoved)
        {
            if (!GameManager.GameEnded)
            {
                // get sound FX
                GameObject mount = GameObject.Find("Mount");
                Effects effects = mount.GetComponent<Effects>();
                if (effects != null)
                {
                    string soundToPlay = effects.GetSound("Death");
                    AudioManager.Instance.Play(soundToPlay);
                }
                // destroy the mount
                LifeManager lifeManager = mount.GetComponent<LifeManager>();
                if (lifeManager != null)
                {
                    lifeManager.TakeMortalDamage();
                }
                GameManager.Instance.EndGame();
            }
        }
    }

    void InitialiseDeployedTroops()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddTrooper(GameObject trooper)
    {
        // add a Trooper to either the left or right list of troopers
        //      NB: this is the only place where we set whether the trooper is on the left or right!
        if (trooper.transform.position.x < 0) // left sude
        {
            trooper.GetComponent<Trooper>().side = GameManager.Side.Left;

            troopersLeft = ResetTroopers(troopersLeft);
            troopersLeft.Add(trooper);

            if (troopersLeft.Count == 4)
            {
                // "clone" a new team of troopers that we want to move
                troopersToMove = new List<GameObject>(troopersLeft);
                MakeInvincible(troopersToMove);

                moveNow = true;
                canAddTroops = false;
            }
        }
        else if (trooper.transform.position.x > 0) // right sude
        {
            trooper.GetComponent<Trooper>().side = GameManager.Side.Right;

            troopersRight = ResetTroopers(troopersRight);
            troopersRight.Add(trooper);
            if (troopersRight.Count == 4)
            {
                troopersToMove = new List<GameObject>(troopersRight);
                MakeInvincible(troopersToMove);

                moveNow = true;
                canAddTroops = false;
            }
        }
        else
        {
            Debug.LogWarning("Could not find a 'Side' to add this trooper to!");
        }
    }

    void MakeInvincible(List<GameObject> deployedTroopers)
    {
        foreach (GameObject trooper  in deployedTroopers)
        {
            trooper.GetComponent<LifeManager>().invincible = true;
        }
    }

    public void RemoveTrooper(GameObject trooperToRemove)
    {
        GameManager.Side side = trooperToRemove.GetComponent<Trooper>().side;
        if (side == GameManager.Side.Left)
        {
            troopersLeft.Remove(trooperToRemove);
        }
        else if (side == GameManager.Side.Right)
        {
            troopersRight.Remove(trooperToRemove);
        }
        else
        {
            Debug.LogWarning("Could not find a 'Side' to remove this trooper from!");
        }
    }

    private List<GameObject> ResetTroopers(List<GameObject> _troopers)
    {
        // troopers that land are added to a list of troopers that might get together
        // and destroy the turret. 
        // these ground troops can be destroyed if antoher trooper lands on them so 
        // we need to be able to reset this list when we add new troopers.
        List<GameObject> troopersCopy = new List<GameObject>(_troopers);

        _troopers.Clear();

        for (int i = 0; i < troopersCopy.Count; i++)
        {
            if (troopersCopy[i] != null)
            {
                _troopers.Add(troopersCopy[i]);
            }
        }
        return _troopers;
    }

    private void OnTriggerEnter2D(Collider2D colliderInfo)
    {
        Trooper trooper = colliderInfo.GetComponent<Trooper>();
        if (trooper.tag == "Trooper" && !trooper.chuteDestroyed)
        {
             if (canAddTroops)
            {
                AddTrooper(colliderInfo.gameObject);
            }
        }
    }
}