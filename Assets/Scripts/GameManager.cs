using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private int score = 0;
    public Text scoreText;

    public static GameManager Instance;

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
        scoreText.text = "SCORE: " + score;
    }
}