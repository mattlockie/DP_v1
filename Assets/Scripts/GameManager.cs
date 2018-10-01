using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public enum Side
    {
        Left,
        Right
    };

    public GameObject gameOverText;

    private int score = 0;
    public Text scoreTextValue;
    private int highScore;
    public Text highScoreTextValue;

    public static bool GameEnded { get; set; }
    private float gameOverCountdown = 2.0f;
    private bool gameOverCountdownReached;

    void InitialiseGameManager()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        // ensure that we start with game not ended for scene reloads
        GameEnded = false;
        
        // temporarily commented this out as we could probably have this loaded on MainMenu and stay alive the whole time
        //DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        InitialiseGameManager();

        // setup high score
        GetHighScore();
        highScoreTextValue.text = highScore.ToString();
    }

    private void Update()
    {
        if (GameEnded)
        {
            if (gameOverCountdown <= 0.0f)
            {
                //Debug.Log("Can now play again!");
                gameOverCountdownReached = true;
                if (Input.GetKeyDown(KeyCode.I))
                {
                    ShowInstructions();
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    PlayGame();
                }
            }
            if (!gameOverCountdownReached)
            {
                gameOverCountdown -= Time.deltaTime;
            }
        }
    }

    private void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
        GameEnded = false;
    }

    private void ShowInstructions()
    {
        //gameOverText.SetActive(false);
        //GameEnded = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void UpdateScore(int value)
    {
        if (score + value <= 0)
        {
            score = 0;
        }
        else
        {
            score += value;
        }
        scoreTextValue.text = score.ToString();
    }

    public void EndGame()
    {
        GameEnded = true;
        gameOverText.SetActive(true);

        SaveHighScore();
    }

    public void SaveHighScore()
    {
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScoreTextValue.text = score.ToString();
        }
    }

    public void GetHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }
}

// TODO: Sound effects
// TODO: Music
// TODO: Adjust spawner to feel like it's a bit more "continuous"