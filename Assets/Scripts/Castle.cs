using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Castle : MonoBehaviour {

    GameObject dbCursor;
    GameObject player;

    DatabaseHandler dbHandler;

    Player playerObject;

    private string leaderBoardString;
    public Text leaderBoardText;

    void Start () {

        dbCursor = GameObject.FindWithTag("DatabaseHandler");
        player = GameObject.FindWithTag("Player");
        dbHandler = dbCursor.GetComponent<DatabaseHandler>();
        leaderBoardString = dbHandler.GetHighScores();
        SetLeaderBoard();
    }
    
    void OnTriggerEnter2D (Collider2D other) {

        if (other.tag == "Player") {

            playerObject = player.GetComponent<Player>();
            SceneManager.LoadScene("Win");

            if (dbHandler != null && playerObject != null) {
                int score = playerObject.GetScore();
                long elapsed = playerObject.GetElapsedTime();
                string username = PlayerPrefs.GetString("Username", "rwk");
                dbHandler.InsertScore(username, score, elapsed);
                SceneManager.LoadScene("Win");
            }
        }
    }

    void SetLeaderBoard() {
        leaderBoardText.text = leaderBoardString;
    }
}
