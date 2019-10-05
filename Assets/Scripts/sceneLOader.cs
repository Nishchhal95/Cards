using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneLOader : MonoBehaviour
{



  [SerializeField]
    private string nextSceneName = null;


  int currentSceneIndex;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    /// <summary>
    /// Ons the click transition button.
    /// </summary>
	public void OnClickTransitionButton()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void reloadSameScene()
    {
        SceneManager.LoadScene(currentSceneIndex );
    }

    public void onbackclick()
    {
        SceneManager.LoadScene("MainPage");

        FB_Handler.instance.ResetMainMenu();
    }

}
