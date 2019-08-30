using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneLOader : MonoBehaviour
{



    public int currentSceneIndex;

    void Start()
    {

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }



    [SerializeField]
    private string nextSceneName = null;

    /// <summary>
    /// Ons the click transition button.
    /// </summary>
	public void OnClickTransitionButton()
    {
        SceneManager.LoadScene(nextSceneName);
    }




    public void reloadSCene()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

}
