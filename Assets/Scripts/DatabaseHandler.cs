using UnityEngine;
using Mono.Data.Sqlite; 
using System.Data; 
using System;

public class DatabaseHandler : MonoBehaviour {

    private IDbConnection dbconn;

    private IDbCommand dbcmd;

    private IDataReader reader;
    
    void Start () {
        string location = "URI=file:" + Application.dataPath + "/Database/unity.sqlite3";
        //Debug.Log(location);
        string conn = location;

        dbconn = (IDbConnection) new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.

        dbcmd = dbconn.CreateCommand();

        // string sqlQuery = "SELECT username , score " + "FROM leaderboard";
        // dbcmd.CommandText = sqlQuery;
        
        // reader = dbcmd.ExecuteReader();

        // while (reader.Read()) {
        //     string username = reader.GetString(0);
        //     int score = reader.GetInt32(1);
            
        //     Debug.Log( "username= "+ username +"  score="+ score );
        // }
    }

    void OnDestroy () {

        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    public void insertScore (string username, int score) {
        Debug.Log( "username= "+ username +"  score="+ score );
        string sqlQuery = "INSERT INTO leaderboard (username, score) VALUES ( '"+username+"', "+score+")";
        dbcmd.CommandText = sqlQuery;
        
        reader = dbcmd.ExecuteReader();

        reader.Close();
        reader = null;
    }
}