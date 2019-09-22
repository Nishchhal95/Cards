using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{

    public Slider LoadingSlider;
    public GameObject LoadingPanel;
    private bool WaitForAPI = false;
    
    void Update()
    {
        if(WaitForAPI==false)
        {
            //LoadingSlider.value += Time.deltaTime / 4;
            LoadingSlider.value = Mathf.Lerp(LoadingSlider.value, 1, Time.deltaTime);
            if (LoadingSlider.value >= 0.98f)
            {
                WaitForAPI = true;
            }
        }
    }

    public void SetDisable()
    {
        LoadingPanel.SetActive(false);
    }

}
