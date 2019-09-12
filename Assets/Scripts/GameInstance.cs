using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInstance : MonoBehaviour
{
    public static GameInstance new_instance = null;

    public int MinimumBettingValue;
    public GameObject InternetPanel;

    private void Awake()
    {
        if (new_instance == null)
        {
            new_instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        CheckConnection();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckConnection();
    }

    public void CheckConnection()
    {
        StartCoroutine(checkInternetConnection((isConnected) => {
            if (isConnected == false)
            {
                //Check Internet
                InternetPanel.SetActive(true);
            }
            else
            {
                //Okay
                InternetPanel.SetActive(false);
            }
        }));
    }

    IEnumerator checkInternetConnection(System.Action<bool> action)
    {
        WWW www = new WWW("http://google.com");
        yield return www;
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }



}
