using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public enum Side
    {
        Left,
        Right
    };

    private int score = 0;
    public Text scoreTextValue;
    private int highScore;
    public Text highScoreTextValue;

    public static bool GameEnded { get; set; }

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

        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        InitialiseGameManager();

        // setup high score
        GetHighScore();
        highScoreTextValue.text = highScore.ToString();
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
        SaveHighScore();
        Debug.Log("GAME OVER!");
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