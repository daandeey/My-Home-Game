using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    private int score;

    public Text scoreText;

    public void PlayAgain ()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void GoToMainMenu ()
    {
        SceneManager.LoadScene("MainMenu");
    }
    void SetScoreText () {
        scoreText.text = "SCORE: " + score.ToString();
    }

    void Start () {
        score = PlayerPrefs.GetInt("Player Score");
        SetScoreText();
    }
}
