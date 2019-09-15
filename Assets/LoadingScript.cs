using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{

    public Slider LoadingSlider;
    public GameObject LoadingPanel;

    
    void Update()
    {
        if (LoadingPanel.activeSelf == true)
        {
            LoadingSlider.value += Time.deltaTime / 4;
            if (LoadingSlider.value >= 0.98f)
            {
                LoadingPanel.SetActive(false);
            }
        }
    }
}
