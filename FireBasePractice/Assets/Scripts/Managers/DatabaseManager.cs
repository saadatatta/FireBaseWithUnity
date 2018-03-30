using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase;

public class DatabaseManager : MonoBehaviour {

    public static DatabaseManager sharedInstance = null;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
        else if(sharedInstance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://fir-test-b424a.firebaseio.com/");
        Debug.Log(Router.Players());
    }

    public void CreateNewPlayer(Player player,string uid)
    {
        string playerJson = JsonUtility.ToJson(player);
        Router.PlayerWithUID(uid).SetRawJsonValueAsync(playerJson);
    }

    public void GetPlayers(Action<List<Player>> completionBlock)
    {
        List<Player> tempList = new List<Player>();

        Router.Players().GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot players = task.Result;

            foreach (DataSnapshot playerNode in players.Children)
            {
                var playerDictionary = (IDictionary<string,object>)playerNode.Value;
                Player newPlayer = new Player(playerDictionary);
                tempList.Add(newPlayer);
            }

            completionBlock(tempList);

        });
    }
}
