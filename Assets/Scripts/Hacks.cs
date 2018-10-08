using UnityEngine;

public class Hacks : MonoBehaviour
{
    public bool hacksEnabled = false;
    public GameObject hacksEnabledText;

    public GameObject trooper;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            hacksEnabled = !hacksEnabled;
            hacksEnabledText.SetActive(hacksEnabled);
            //Debug.Log("HACKS " + (hacksEnabled ? "ENABLED" : "DISABLED"));
        }

        if (hacksEnabled)
        {
            // deal mortal damage to object that has a collider
            if (Input.GetMouseButtonDown(0))
            {
                ClickSelect();
            }

            // spawn a trooper at mouse position
            if (Input.GetMouseButtonDown(1))
            {
                SpawnTrooper();
            }

            // reset high score to zero 
            if (Input.GetKeyDown(KeyCode.R))
            {
                //Debug.Log("HACK: Setting score to Zero");
                PlayerPrefs.SetInt("HighScore", 0);
                GameManager.Instance.highScoreTextValue.text = PlayerPrefs.GetInt("HighScore").ToString();
            }
        }
    }

    void SpawnTrooper()
    {
        Vector3 rayPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

        Instantiate(trooper, rayPos, Quaternion.identity);
    }

    void ClickSelect()
    {
        Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

        if (hit)
        {
            // get sound FX
            Effects effects = hit.collider.GetComponent<Effects>();
            if (effects != null)
            {
                string soundToPlay = effects.GetSound("Death");
                AudioManager.Instance.Play(soundToPlay);
            }
            LifeManager target = hit.collider.gameObject.GetComponent<LifeManager>();
            if (target != null)
            {
                target.TakeMortalDamage();
            }
        }

        // if we just click anywhere on the screen while there's a bomb, destroy it
        GameObject bomb = GameObject.FindGameObjectWithTag("Bomb");
        if (bomb != null)
        {
            Destroy(bomb);
        }
    }
}