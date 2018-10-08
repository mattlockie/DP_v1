using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject splashScreen;
    public GameObject instructions;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ShowInstructions()
    {
        splashScreen.SetActive(false);
        instructions.SetActive(true);
    }

    private void Quit()
    {
        Application.Quit();
    }
}