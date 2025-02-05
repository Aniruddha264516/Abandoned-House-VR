using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanRotate : MonoBehaviour
{
   
    [SerializeField] private float fanspeed;
    private bool Isfanrotate = false;


    void Update()
    {
        if(SelectionManager.instance.FuseFound && SelectionManager.instance.GeneratorFound == true)
        {
            transform.Rotate(0, 0, fanspeed * Time.deltaTime);
            Isfanrotate = true;
        }
       
    }
}
