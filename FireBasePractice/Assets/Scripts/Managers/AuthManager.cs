
using UnityEngine;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;
using System.Collections;

public class AuthManager : MonoBehaviour {

    FirebaseAuth auth;

    public delegate IEnumerator AuthCallBack(Task<FirebaseUser> task, string operation);
    public event AuthCallBack authCallback;

    private void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void SignUpNewUser(string email,string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            StartCoroutine(authCallback(task, "sign_up"));
        });
    }

    public void LoginExistingUser(string email,string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            StartCoroutine(authCallback(task, "login"));
        });
    }
}
