using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DatabaseControl;
using UnityEngine.SceneManagement;

public class UserAccountManager : MonoBehaviour
{
    public static UserAccountManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    public static string LoggedIn_Username { get; protected set; }
    private static string LoggedIn_Password = "";

    public static string LoggedIn_Data { get; protected set; }

    public static bool IsLoggedIn { get; protected set; }

    public string loggedInSceneName = "MainLevel01";
    public string loggedOutSceneName = "LoginMenu";

    public delegate void OnDataReceivedCallback(string data);

    public void LogOut()
    {
        LoggedIn_Username = "";
        LoggedIn_Password = "";

        IsLoggedIn = false;

        Debug.Log("User logged out");

        SceneManager.LoadScene(loggedOutSceneName);
    }

    public void LogIn(string username, string password)
    {
        LoggedIn_Username = username;
        LoggedIn_Password = password;

        IsLoggedIn = true;

        Debug.Log("Logged in as " + username);

        SceneManager.LoadScene(loggedInSceneName);
    }

    IEnumerator GetData(string playerUsername, string playerPassword, OnDataReceivedCallback onDataReceived)
    {
        string data = "ERROR";

        IEnumerator e = DCF.GetUserData(playerUsername, playerPassword); // << Send request to get the player's data string. Provides the username and password
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Error")
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            playerUsername = "";
            playerPassword = "";
        }
        else
        {
            //The player's data was retrieved. 
            string DataReceived = response;
            data = DataReceived;
        }

        if (onDataReceived != null)
            onDataReceived.Invoke(data);
    }
    IEnumerator SetData(string playerUsername, string playerPassword, string data)
    {
        IEnumerator e = DCF.SetUserData(playerUsername, playerPassword, data); // << Send request to set the player's data string. Provides the username, password and new data string
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Success")
        {

        }
        else
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            playerUsername = "";
            playerPassword = "";
        }
    }
    public void SendData(string data)
    {
        if (IsLoggedIn)
        {
            StartCoroutine(SetData(LoggedIn_Username, LoggedIn_Password, data));
        }
    }
    public void GetData(OnDataReceivedCallback onDataReceived)
    {
        if (IsLoggedIn)
        {
            StartCoroutine(GetData(LoggedIn_Username, LoggedIn_Password, onDataReceived));
        }
    }
}
