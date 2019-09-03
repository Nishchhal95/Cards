using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject soundcontroller;
    void Start()
    {
        
    }

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
