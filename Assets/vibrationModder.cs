using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vibrationModder : MonoBehaviour
{
    // Start is called before the first frame update

    bool VibOn = false;

   void Start()
    {
        VibOn = true;
    }

    public void vibrateIt()
    {
        if(VibOn==true)

        {
            Handheld.Vibrate();
            Debug.Log("vibrated itttttttttttttttttttttttttt");
        }
      
    }




    public void TogglerVibration()
    {
        if (VibOn == true)
        {
          VibOn= false;
        }

        else
        {
            VibOn = true;
        }
    }
}
