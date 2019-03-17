using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Castle : MonoBehaviour {

    GameObject dbCursor;
    GameObject player;

    DatabaseHandler dbHandler;

    Player playerObject;

    void Start () {

        dbCursor = GameObject.FindWithTag("DatabaseHandler");
        player = GameObject.FindWithTag("Player");
    }
    
    void OnTriggerEnter2D (Collider2D other) {

        if (other.tag == "Player") {

            dbHandler = dbCursor.GetComponent<DatabaseHandler>();
            playerObject = player.GetComponent<Player>();

            if (dbHandler != null && playerObject != null) {
                int score = playerObject.GetScore();
                dbHandler.insertScore("budiman", score);
                SceneManager.LoadScene("Win");
            }
        }
    }
}
