using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    public GameObject soundonbutton;
    public GameObject soundoffbutton;
    public GameObject chatonbutton;
    public GameObject chatoffbutton;
    public GameObject vibrateonbutton;
    public GameObject vibrateoffbutton;

    public void soundon()
    {
        soundonbutton.SetActive(true);
        soundoffbutton.SetActive(false);
    }

    public void soundoff()
    {
        soundonbutton.SetActive(false);
        soundoffbutton.SetActive(true);
    }
    public void chaton()
    {
        chatonbutton.SetActive(true);
        chatoffbutton.SetActive(false);
    }

    public void chatoff()
    {
        chatonbutton.SetActive(false);
        chatoffbutton.SetActive(true);
    }
    public void vibrateon()
    {
        vibrateonbutton.SetActive(true);
        vibrateoffbutton.SetActive(false);
    }

    public void vibrateoff()
    {
        vibrateonbutton.SetActive(false);
        vibrateoffbutton.SetActive(true);
    }


}
