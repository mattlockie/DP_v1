using UnityEngine;

public class TrooperTarget : MonoBehaviour {

    public bool targetOccupied = false;

    private void Awake()
    {
        targetOccupied = false;
    }
}