using UnityEngine;
using Mono.Data.Sqlite; 
using System.Data; 
using System;

public class DatabaseHandler : MonoBehaviour {

    public IDbConnection dbconn;

    public IDbCommand dbcmd;

    public IDataReader reader;

    public void InsertScore (string username, int score, long elapsed) {

        string location = "URI=file:" + Application.dataPath + "/Database/unity.sqlite3";
        
        string conn = location;

        dbconn = (IDbConnection) new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.

        dbcmd = dbconn.CreateCommand();
        
        string sqlQuery = "INSERT INTO leaderboard (username, score, elapsed) VALUES ( '" + username + "', " + score + ", " + elapsed + ")";
        dbcmd.CommandText = sqlQuery;
        
        reader = dbcmd.ExecuteReader();

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    public string GetHighScores () {

        int i = 0;
        string leaderboard = "Current Leaderboard\n";

        string location = "URI=file:" + Application.dataPath + "/Database/unity.sqlite3";
        
        string conn = location;

        dbconn = (IDbConnection) new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.

        dbcmd = dbconn.CreateCommand();

        string sqlQuery = "SELECT username, score, elapsed FROM leaderboard ORDER BY score DESC, elapsed ASC LIMIT 10;";
        dbcmd.CommandText = sqlQuery;
        
        reader = dbcmd.ExecuteReader();

        while (reader.Read()) {
            i += 1;
            string username = reader.GetString(0);
            int score = reader.GetInt32(1);
            var elapsed = reader.GetFloat(2);

            leaderboard += i.ToString().PadRight(4) + username.PadRight(12) + "\t" + score.ToString().PadRight(7) + elapsed.ToString() + '\n';
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

        Debug.Log(leaderboard);
        return leaderboard;
    }
}