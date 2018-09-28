using UnityEngine;

public class Hacks : MonoBehaviour
{
    public bool hacksEnabled = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            hacksEnabled = !hacksEnabled;
            Debug.Log("HACKS " + (hacksEnabled ? "ENABLED" : "DISABLED"));
        }

        if (hacksEnabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ClickSelect();
            }
        }
    }

    void ClickSelect()
    {
        //Converting Mouse Pos to 2D (vector2) World Pos
        Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

        if (hit)
        {
            //Debug.Log(hit.collider.gameObject);
            LifeManager target = hit.collider.gameObject.GetComponent<LifeManager>();
            if (target != null)
            {
                target.TakeMortalDamage();
            }
        }

        GameObject bomb = GameObject.FindGameObjectWithTag("Bomb");
        if (bomb != null)
        {
            Destroy(bomb);
        }
    }
}