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

    // TODO: Cargo Manager! 
    // see if I can have a bool that determines if we drop based on collider or by a time counter
    // so that it can be played with in the inspector.

    void Start() 
	{
        dropTime = Random.Range(dropTimeMin, dropTimeMax);
    }
	
	void Update() 
	{
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
        Instantiate(cargo, transform.position + cargoOffset, Quaternion.identity);
    }

    //private void OnTriggerEnter2D(Collider2D hitInfo)
    //{
    //    if (useTrigger && !cargoDropped)
    //    {
    //        for (int i = 0; i < triggers.Length + 1; i++)
    //        {
    //            if (hitInfo == triggers[i])
    //            {
    //                DropCargo();
    //            }
    //        }
    //    }
    //}
}