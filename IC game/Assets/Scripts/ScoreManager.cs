using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public Text scoreText;
    public Text highScoreText;

    int score = 0;
    int highscore = 0;
    float timeElapsed = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize highscore from PlayerPrefs
        highscore = PlayerPrefs.GetInt("HighScore", 0);
        scoreText.text = "Score: " + score.ToString();
        highScoreText.text = "High Score: " + highscore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= 1f)
        {
            AddScore(1);
            timeElapsed = 0f;
        }
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score.ToString();
        if (score > highscore)
        {
            highscore = score;
            highScoreText.text = "High Score: " + highscore.ToString();
            PlayerPrefs.SetInt("HighScore", highscore);
        }
    }
}
