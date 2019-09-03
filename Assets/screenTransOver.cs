using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenTransOver : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject blackCanvas;

    void Start()
    {
        blackCanvas.SetActive(true);
    }


    private void Update()
    {
        Destroy(blackCanvas, 2f);
    }


}
