using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeEffetor : MonoBehaviour
{

   public  Canvas cv;

    SpriteRenderer sp;
    //RENDER>MATARIAL WORK FLOW CHANGED IN UNITY 
    public Color c1, c2;

    float t;
    float duration = 3f;

    void Start()
    {

        // sp = GetComponent<SpriteRenderer>();
        cv = GetComponent<Canvas>();

      

    }

    void Update()
    {

        colorFaderOne(c1, c2);




    }



    public void colorFaderOne(Color a, Color b)
    {



        // sp.color = Color.Lerp(a, b, t);
             t = Mathf.PingPong(Time.time, duration) / duration;
    }
}
