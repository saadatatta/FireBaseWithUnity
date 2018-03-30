
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Router : MonoBehaviour {

    private static DatabaseReference baseRef = FirebaseDatabase.DefaultInstance.RootReference;

    public static DatabaseReference Players()
    {
        return baseRef.Child("players");
    }

    public static DatabaseReference PlayerWithUID(string uid)
    {
        return baseRef.Child("players").Child(uid);
    }
}
