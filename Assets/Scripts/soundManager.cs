using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    // Start is called before the first frame update


    private static soundManager instance;

    public GameObject soundcontroller;


    void Start()
    {
        if (soundManager.instance == null)
        {
            soundManager.instance = this;
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    //toggling system
   public void SoundControll()
    {
        if(AudioListener.pause==true)
        {
            AudioListener.pause = false;
        }

        else
        {
            AudioListener.pause = true;
        }
    }
}
