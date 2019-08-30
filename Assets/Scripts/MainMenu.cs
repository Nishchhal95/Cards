using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject ShopPanel;
    public GameObject SettingPanel;
    public GameObject mainmenu;
    public GameObject howtoplaypanel;


   public void PlayGame()
    {
        SceneManager.LoadScene("Player");
    }

    public void Shop()
    {
        ShopPanel.SetActive(true);
        mainmenu.SetActive(false);
    }

    public void Shophide()
    {
        ShopPanel.SetActive(false);
        mainmenu.SetActive(true);
    }

    public void SettingPanelshow()
    {
        SettingPanel.SetActive(true);
    }

    public void howtoplayshow()
    {
        howtoplaypanel.SetActive(true);
        mainmenu.SetActive(false);
    }

    public void howtoplayhide()
    {
        howtoplaypanel.SetActive(false);
        mainmenu.SetActive(true);
    }

}