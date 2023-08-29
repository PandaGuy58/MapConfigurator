using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightParent : MonoBehaviour
{
    public GameObject lightObject;
    private void Start()
    {
        lightObject.SetActive(false);
    }

    public void EnableLight(bool action)
    {
        lightObject.SetActive(action);
    }



    

}
