using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetDIamonds : MonoBehaviour
{
    public Text diamondsText;

    // Start is called before the first frame update
    void Start()
    {
        diamondsText.text = PlayerPrefs.GetInt("diamond", 0).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
