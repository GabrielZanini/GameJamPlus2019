using UnityEngine;
using UnityEngine.UI;


public class ScoreScript : MonoBehaviour
{
    public Text scoreText;
    public Text highScore;

    public int scoreValue = 0;


    void Start()
    {
        highScore.text =  PlayerPrefs.GetInt("HighScore",0 ).ToString();
    }


    public void AddScore(int points)
    {
        //texto recebe o valor do score que é adicionado no enemy script
        scoreValue += points;

        scoreText.text = scoreValue.ToString();

        if (scoreValue > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", scoreValue);
            highScore.text = scoreValue.ToString();
        }
    }

}
