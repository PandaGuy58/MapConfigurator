using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 currentCoordinate = transform.position;
        Vector3 forwardCoordinate = transform.forward;
        currentCoordinate += forwardCoordinate * mouseScroll;

        transform.position = currentCoordinate;
        
    }
}
