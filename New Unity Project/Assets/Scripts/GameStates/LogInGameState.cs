using System;
using UnityEngine;
public class LogInGameState: GameState
{
    public string username = "";
    private string password = "";
    private string registerUsername = "";
    private string registerPassword = "";
    private string registerPasswordAgain = "";
    public string status = "";

    public GUITexture backgroundTexture;

    public LogInGameState(NetworkManager manager)
    {
        this.networkManager = manager;
        database = manager.database;
    }

    public override void OnGUI()
    {
        GUI.Label(new Rect(Screen.width * .5f - 75, Screen.height * .1f, 150, 45), "Login or Register Account");

        GUI.Label(new Rect(Screen.width * .365f, Screen.height * .13f, 100, 45), "Username");

        username = GUI.TextField(new Rect(Screen.width * .365f, Screen.height * .14f + 15, 150, 25), username);

        GUI.Label(new Rect(Screen.width * .365f, Screen.height * .14f + 40, 100, 45), "Password");

        password = GUI.PasswordField(new Rect(Screen.width * .365f, Screen.height * .14f + 65, 150, 25), password, '*');

        if (GUI.Button(new Rect(Screen.width * .365f + 25, Screen.height * .14f + 95, 75, 25), "Submit"))
        {
            database.AccountLoginCorrect(username, password);
            status = "Logging in ... ";
        }

        if (database.Authenticated)
        {
			networkManager.accountName = username;
            networkManager.StateStack.Push(new MainMenuGameState(this.networkManager));
            backgroundTexture.enabled = false;
        }

        GUI.Label(new Rect(Screen.width * .52f, Screen.height * .13f, 100, 45), "Username");
        registerUsername = GUI.TextField(new Rect(Screen.width * .52f, Screen.height * .14f + 15, 150, 25), registerUsername);

        GUI.Label(new Rect(Screen.width * .52f, Screen.height * .14f + 40, 100, 45), "Password");

        registerPassword = GUI.PasswordField(new Rect(Screen.width * .52f, Screen.height * .14f + 65, 150, 25), registerPassword, '*');

        GUI.Label(new Rect(Screen.width * .52f, Screen.height * .14f + 90, 150, 45), "Repeat Password");

        registerPasswordAgain = GUI.PasswordField(new Rect(Screen.width * .52f, Screen.height * .14f + 115, 150, 25), registerPasswordAgain, '*');

        if (GUI.Button(new Rect(Screen.width * .52f, Screen.height * .14f + 145, 75, 25), "Submit"))
        {
            database.CreateAccount(registerUsername, registerPassword);

            status = "Creating Account ...";
        }

        GUI.Label(new Rect(Screen.width * .35f, Screen.height * .335f, 300, 25), status);
    }

	public override void Cleanup ()
	{
		/* Deauthenticate */
		database.Authenticated = false;
		username = "";
		password = "";
		registerPassword = "";
		registerPasswordAgain = "";
		registerUsername = "";
		status = "";
	}
	
	public override void Update ()
	{
	}
}

