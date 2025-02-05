using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseLights : MonoBehaviour
{
    [SerializeField] private GameObject Lights;

    private void Update()
    {
        if(SelectionManager.instance.FuseFound && SelectionManager.instance.GeneratorFound == true)
        {
            if(Lights.CompareTag("HouseLights"))
            {
                Lights.SetActive(true);
            }
        }
    }
}
