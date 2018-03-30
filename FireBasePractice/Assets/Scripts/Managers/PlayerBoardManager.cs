using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

public class PlayerBoardManager : MonoBehaviour {

    public List<Player> playerList = new List<Player>();

    public GameObject rowPrefab;
    public GameObject scrollContainer;

    private void Awake()
    {
        playerList.Clear();

        DatabaseManager.sharedInstance.GetPlayers(result =>
        {
            playerList = result;
            Debug.Log(playerList[0].email);
            InitializeUI();
        });

        //Router.Players().ValueChanged += NewPlayerAdded;

        // Order the data returned from database.
        //Router.Players().OrderByChild("level");
    }

    private void NewPlayerAdded(object sender, ValueChangedEventArgs e)
    {
        if (e.Snapshot.Value == null)
            Debug.Log("Sorry, there was no data at that node.");
        else
            Debug.Log("New player has joined the game");
    }

    private void InitializeUI()
    {
        foreach (Player player in playerList)
        {
            CreateRow(player);
        }
    }

    void CreateRow(Player player)
    {
        GameObject newRow = Instantiate(rowPrefab);
        newRow.transform.SetParent(scrollContainer.transform, false);
    }
}
