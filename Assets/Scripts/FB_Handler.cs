using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using UnityEngine.UI;

public class FB_Handler : MonoBehaviour
{
    public string SavedUsername;
    public string SavedEmail;
    public Sprite SavedProfile;

    public Text FB_UserName;
    public Text FB_UserName2;
    public Text FB_Email;
    public Image FB_Profile;
    public Image FB_Profile2;
    public GameObject MainMenuScreen;
    public GameObject welcomescreen;
    public Button LoginButton;

    /* While using this prefab, You will need Username Text, Profile Image, and Login Button to Call FBLogin() Function inside Canvas. */

    public static FB_Handler instance = null;
    

    public bool CheckIfLoggedIn()
    {
        if (FB.IsLoggedIn)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

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
        permissions.Add("email");
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

            if (PlayerPrefs.GetInt("login", 0) == 0)
            {
                welcomescreen.SetActive(true);
                PlayerPrefs.SetInt("login", 1);
            }

            MainMenuScreen.SetActive(true);
            LoginButton.gameObject.SetActive(false);

            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
            FB.API("/me?fields=email", HttpMethod.GET, DisplayEmail);
         
          
        }
        else
        {
            //Things to do if FB login failed.
            welcomescreen.SetActive(false);
            MainMenuScreen.SetActive(false);
            LoginButton.gameObject.SetActive(true);

        }
    }

    void DisplayUsername(IResult result)    //Displays Username
    {
        if (result.Error == null)
        {
            SavedUsername = "" + result.ResultDictionary["first_name"];
            FB_UserName.text = SavedUsername;
            FB_UserName2.text = SavedUsername;
            Debug.Log("" + SavedUsername);
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
            SavedProfile = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
            FB_Profile.sprite = SavedProfile;
            FB_Profile2.sprite = SavedProfile;
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    
    void DisplayEmail(IResult result)    //Displays Email
    {
        if (result.Error == null)
        {
            SavedEmail = "" + result.ResultDictionary["email"];
            FB_Email.text = SavedEmail;
            Debug.Log("" + SavedEmail);
        }
        else
        {
            Debug.Log(result.Error);
        }
    }
}
