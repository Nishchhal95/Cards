using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionTimer : MonoBehaviour
{

    private Image SelectionUI;

    private float TimeTaken = 30f;
    private float t = 0;


    void Start()
    {
        SelectionUI = this.GetComponent<Image>();
        SelectionUI.fillAmount = 0;
    }


    void OnEnable()
    {
        t = 0;
       
            //SelectionUI.fillAmount = 0;
        
    }


    void Update()
    {
        if(t <= TimeTaken)
        {
            t += Time.deltaTime;
            SelectionUI.fillAmount += Time.deltaTime/TimeTaken;
        }
       
    }
}
