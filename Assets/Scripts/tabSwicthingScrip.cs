using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tabSwicthingScrip : MonoBehaviour
{
    // Start is called before the first frame update

        public GameObject Basic;
    public GameObject RAnking;

    void Start()
    {
        Basic.SetActive(true);
        RAnking.SetActive(false);
    }
    


     public  void rankingClick()
    {
        Basic.SetActive(false);
        RAnking.SetActive(true);
    }


    public void BasicClick()
    {
        RAnking.SetActive(false); // sequence of these two matters
        Basic.SetActive(true);
       
    }




}
