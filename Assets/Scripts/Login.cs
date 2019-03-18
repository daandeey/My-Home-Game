using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public InputField UsernameInput;
    public InputField PasswordInput;
    public Button login;

    // Start is called before the first frame update
    void Start()
    {
        if (login != null)
        {
            login.onClick.AddListener(TaskOnClick);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (UsernameInput.text != "" && PasswordInput.text != "")
            {
                Debug.Log("Sending Request");
                StartCoroutine(request(UsernameInput.text, PasswordInput.text));
            }
        }
    }

    void TaskOnClick()
    {
        Debug.Log("Sending request");
        if (UsernameInput.text != "" && PasswordInput.text != "")
        {
            StartCoroutine(request(UsernameInput.text, PasswordInput.text));
        }
    }

    IEnumerator request(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        string url = "https://myhome-be.herokuapp.com/login/";

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
                    PlayerPrefs.SetInt("Login", 1);
                    SceneManager.LoadScene("SampleScene");
                }
                else
                {
                    Debug.Log("Password or Username invalid");
                }
            }
        }
    }
}

[System.Serializable]
public class Response
{
    public bool success;
}