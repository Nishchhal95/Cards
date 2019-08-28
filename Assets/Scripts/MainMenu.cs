using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject ShopPanel;
    public GameObject SettingPanel;


   public void PlayGame()
    {
        SceneManager.LoadScene("Player");
        cardsLogic.Instance.AssignCardToXpeople();
    }

    public void Shop()
    {
        ShopPanel.SetActive(true);
    }

    public void SettingPanelshow()
    {
        SettingPanel.SetActive(true);
    }


}