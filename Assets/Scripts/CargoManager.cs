using UnityEngine;

public class CargoManager : MonoBehaviour {

    [Header("Cargo Settings")]
    public GameObject cargo;
    public bool cargoDropped = false;
    public Vector3 cargoOffset;

    [Header("Triggered Cargo")]
    public bool useTrigger;
    public Collider2D trigger;

    [Header("Timed Cargo")]
    public float dropTimeMin = 1.0f;
    public float dropTimeMax = 8.0f;
    private float dropTimeCounter = 0.0f;
    private float dropTime;

    void Start() 
	{
        // find a random drop time
        dropTime = Random.Range(dropTimeMin, dropTimeMax);
    }
	
	void Update() 
	{
        // can either drop cargo based on a trigger (collider that we hit) or based on a timer.
        if (useTrigger)
        {
            return;
        }

        if (!cargoDropped)
        {
            dropTimeCounter += Time.deltaTime;
            if (dropTimeCounter >= dropTime)
            {
                DropCargo();
            }
        }
    }

    public void DropCargo()
    {
        cargoDropped = true;
        // we don't actually want to drop cargo if the game has ended
        if (!GameManager.GameEnded)
        {
            Instantiate(cargo, transform.position + cargoOffset, Quaternion.identity);
        }
    }
}