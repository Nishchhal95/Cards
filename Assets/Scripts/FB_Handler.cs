﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using UnityEngine.UI;

public class FB_Handler : MonoBehaviour
{
    public Text FB_UserName;
    public Image FB_Profile;
    public GameObject MainMenuScreen;
    public Button LoginButton;

    /* While using this prefab, You will need Username Text, Profile Image, and Login Button to Call FBLogin() Function inside Canvas. */

    public static FB_Handler instance = null;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        FB.Init(SetInit, onHidenUnity);  //Trigger on Game start.
    }

    void SetInit()
    {
        CheckLoginState(FB.IsLoggedIn);
    }

    void onHidenUnity(bool isGameShown)   //Stop Game while FB is logging in.
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void FBLogin()
    {
        List<string> permissions = new List<string>();
        permissions.Add("public_profile");
        FB.LogInWithReadPermissions(permissions, AuthCallBack);
    }
 
    void AuthCallBack(IResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
        }
        else
        {
            CheckLoginState(FB.IsLoggedIn);
        }
    }

    void CheckLoginState(bool isLoggedIn)  //Check current FB Login status.
    {
        if (isLoggedIn)
        {
            //Things to do if FB login completed.

            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);

            MainMenuScreen.SetActive(true);
            LoginButton.gameObject.SetActive(false);
        }
        else
        {
            //Things to do if FB login failed.

            MainMenuScreen.SetActive(false);
            LoginButton.gameObject.SetActive(true);

        }
    }

    void DisplayUsername(IResult result)    //Displays Username
    {
        if (result.Error == null)
        {
            string name = "" + result.ResultDictionary["first_name"];
            FB_UserName.text = name;
            Debug.Log("" + name);
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    void DisplayProfilePic(IGraphResult result)   //Displays Profile Picture
    {
        if (result.Texture != null)
        {
            Debug.Log("Profile Pic");
            FB_Profile.sprite = Sprite.Create(result.Texture,new Rect(0,0,128,128),new Vector2());
        }
        else
        {
            Debug.Log(result.Error);
        }
    }
}
