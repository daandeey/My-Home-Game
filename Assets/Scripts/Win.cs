using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

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
        if(score >= 500)
        {
            StartCoroutine(request());
        }

        SetScoreText();
    }

    IEnumerator request()
    {
        int nominal = score / 500 * 10000;
        score = score % 500;
        
        WWWForm form = new WWWForm();
        form.AddField("tipe", 2);
        form.AddField("nominal", nominal);

        string url = "https://myhome-be.herokuapp.com/notifications/";

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Response res = JsonUtility.FromJson<Response>(www.downloadHandler.text);

                if (res.success)
                {
                    Debug.Log("Notification sent");
                }
                else
                {
                    Debug.Log("Faild to post request");
                }
            }
        }
    }
}