using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    public void PlayAgain ()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void GoToMainMenu ()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
