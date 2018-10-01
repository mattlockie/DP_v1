using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject splashScreen;
    public GameObject instructions;

    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ShowInstructions()
    {
        splashScreen.SetActive(false);
        instructions.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowInstructions();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayGame(); 
        }
    }
}