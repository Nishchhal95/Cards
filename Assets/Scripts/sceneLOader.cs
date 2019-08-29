using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneLOader : MonoBehaviour
{



    [SerializeField]
    private string nextSceneName = null;

    /// <summary>
    /// Ons the click transition button.
    /// </summary>
	public void OnClickTransitionButton()
    {
        SceneManager.LoadScene(nextSceneName);
    }



}
