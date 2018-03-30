using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Firebase.Auth;

public class FormManager : MonoBehaviour {

    public InputField emailInput;
    public InputField passwordInput;

    public Button SignUpButton;
    public Button LogInButton;

    public Text StatusText;

    public AuthManager authManager;

    private void Awake()
    {
        ToggleButtonStates(false);
        authManager.authCallback += HandleCallback;
    }

    private IEnumerator HandleCallback(Task<FirebaseUser> task, string operation)
    {
        if (task.IsCanceled || task.IsFaulted)
            UpdateStatus("Sorry there was an error creating your account.");
        else if (task.IsCompleted)
        {
            if(operation == "sign_up")
            {
                FirebaseUser newPlayer = task.Result;
                Debug.LogFormat("Welcome to app {0}", newPlayer.Email);

                Player player = new Player(newPlayer.Email, 0, 1);
                DatabaseManager.sharedInstance.CreateNewPlayer(player, newPlayer.UserId);

            }

            
            UpdateStatus("Loading the game scene.");
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("Successful Login");
        }
        yield return null;
    }

    public void ValidateEmail()
    {
        string email = emailInput.text;

        var regexPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
     + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
     + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";

        if (email != "" && Regex.IsMatch(email,regexPattern))
            ToggleButtonStates(true);
        else
            ToggleButtonStates(false);
    }

    public void OnSignUp()
    {
        authManager.SignUpNewUser(emailInput.text, passwordInput.text);
        Debug.Log("Sign Up");
    }

    public void OnSignIn()
    {
        authManager.LoginExistingUser(emailInput.text, passwordInput.text);
        Debug.Log("Sign In");
    }

    private void UpdateStatus(string message)
    {
        StatusText.text = message;
    }

    private void ToggleButtonStates(bool value)
    {
        SignUpButton.interactable = value;
        LogInButton.interactable = value;
    }

    private void OnDestroy()
    {
        authManager.authCallback -= HandleCallback;
    }
}
