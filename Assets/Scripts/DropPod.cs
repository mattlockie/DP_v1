using UnityEngine;

public class DropPod : MonoBehaviour {

    public GameObject defaultWeapon;
    public Transform weaponSpawnPoint;

    private Vector3 currentAngle;
    private float pivotSpeed = 150.0f;
    private int minAngle = 100;
    private int maxAngle = 260;

    void Start() 
	{
        // create default starting weapon
        Instantiate(defaultWeapon, weaponSpawnPoint.position, Quaternion.identity, weaponSpawnPoint.transform);
	}

    void Update()
    {
        RotateWeapon();
    } 

    void RotateWeapon()
    {
        // while holding down left/right, angle the weapon between a min/max value
        float rotateValue = -Input.GetAxis("Horizontal") * pivotSpeed * Time.deltaTime;
        currentAngle = weaponSpawnPoint.transform.eulerAngles;
        weaponSpawnPoint.transform.rotation = Quaternion.Euler(currentAngle.x, currentAngle.y, Mathf.Clamp (currentAngle.z + rotateValue, minAngle, maxAngle));
    }
}