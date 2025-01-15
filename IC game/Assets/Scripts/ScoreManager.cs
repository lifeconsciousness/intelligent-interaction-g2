using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public Text scoreText;
    public Text highScoreText;

    private readonly string HIGH_SCORE_KEY = "HighScore";
    private readonly string scorePrefix = "Score: ";
    private readonly string highScorePrefix = "High Score: ";

    int score = 0;
    int highscore = 0;
    float timeElapsed = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize highscore from PlayerPrefs
        highscore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        scoreText.text = scorePrefix + score.ToString();
        highScoreText.text = highScorePrefix + highscore.ToString();
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
        scoreText.text = scorePrefix + score.ToString();
        if (score > highscore)
        {
            highscore = score;
            highScoreText.text = highScorePrefix + highscore.ToString();
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, highscore);
        }
    }
}
