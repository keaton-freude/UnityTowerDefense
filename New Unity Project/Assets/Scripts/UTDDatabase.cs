using System.Security.Cryptography;
using System.Collections.Generic;
using System;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using UnityEngine;

public class UTDDatabase
{

    

    //Constructor
    public UTDDatabase()
    {
    }

    public string GetSwcSH1(string value)
    {
        SHA1 algorithm = SHA1.Create();
        byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
        string sh1 = "";
        for (int i = 0; i < data.Length; i++)
        {
            sh1 += data[i].ToString("x2").ToUpperInvariant();
        }
        return sh1;
    }

    public void CreateAccount(string name, string password)
    {
        
        string Salt = CreateSalt(12);
        string Hash = CreatePasswordHash(password, Salt);
        string answer = "";

        WWWForm form = new WWWForm();
        form.AddField("Command", "CreateAccount");
        form.AddField("acctName", name);
        form.AddField("acctHash", Hash);
        form.AddField("acctSalt", Salt);

        WWWLoader.instance.myDelegate += OnReceiveCreateAccountResponse;
        WWWLoader.Load("http://unitytowerdefense.com/WebService.php", form);


    }

    private string CreateSalt(int size)
    {
        //Generate a cryptographic random number.
        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        byte[] buff = new byte[size];
        rng.GetBytes(buff);

        // Return a Base64 string representation of the random number.
        return Convert.ToBase64String(buff);
    }

    private string CreatePasswordHash(string pwd, string salt)
    {
        string saltAndPwd = String.Concat(pwd, salt);
        string hashedPwd = GetSwcSH1(saltAndPwd);
        return hashedPwd;
    }

    string password = "";

    public void AccountLoginCorrect(string username, string pwd)
    {
        string salt = "";
        string CorrectHashedPwd = "";
        string answer = "";
        password = pwd;

        UnityEngine.WWWForm form = new UnityEngine.WWWForm();
        form.AddField("Command", "GetHashSalt");
        form.AddField("acctName", username);
        WWWLoader.instance.myDelegate += OnGetHashAndSalt;
        WWWLoader.Load("http://unitytowerdefense.com/WebService.php", form);
    }

    public bool Authenticated = false;

    public void OnGetHashAndSalt(string answer)
    {
        WWWLoader.instance.myDelegate -= OnGetHashAndSalt;

        if (!answer.Contains("Success"))
            return;
        
        string[] result = answer.Split(',');

        string CorrectHashedPwd = result[1];
        string salt = result[2];

        string otherHash = CreatePasswordHash(password, salt);

        Debug.Log("Correct: " + CorrectHashedPwd);
        Debug.Log("Otherhash: " + otherHash);

        if (otherHash == CorrectHashedPwd)
        {
            Authenticated = true;
        }
        else
        {
            ((LogInGameState)GameObject.Find("__NetworkManager").GetComponent<NetworkManager>().StateStack.Peek()).status = "Login Failed";
        }
    }

    public void OnReceiveCreateAccountResponse(string answer)
    {
        
        if (answer.Contains("Success"))
        {
            if (GameObject.Find("__NetworkManager").GetComponent<NetworkManager>().StateStack.Peek() is LogInGameState)
            {
				//Debug.Log (((LogInGameState)GameObject.Find("__NetworkManager").GetComponent<NetworkManager>().StateStack.Peek()).username);
				//GameObject.Find ("__NetworkManager").GetComponent<NetworkManager>().accountName = ((LogInGameState)GameObject.Find("__NetworkManager").GetComponent<NetworkManager>().StateStack.Peek()).username;
                LogInGameState state = ((LogInGameState)GameObject.Find("__NetworkManager").GetComponent<NetworkManager>().StateStack.Peek());
                state.status = "Account Creation Success!";
            }
            

        }
        else
        {
            if (GameObject.Find("__NetworkManager").GetComponent<NetworkManager>().StateStack.Peek() is LogInGameState)
            {
                LogInGameState state = ((LogInGameState)GameObject.Find("__NetworkManager").GetComponent<NetworkManager>().StateStack.Peek());
                state.status = "Acount Creation Failed. Username Duplicate!";
            }
        }
    }
}