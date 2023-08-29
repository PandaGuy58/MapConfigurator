using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    RectTransform rectTransform;
    GameObject playerCamera;
    float healthPercentage;
    public Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gameObject.name);
        Debug.Log(playerCamera.name);
        gameObject.transform.rotation = Quaternion.LookRotation(-playerCamera.transform.position + gameObject.transform.position);      // adjust rotation to face player camera
        Vector3 currentRotation = rectTransform.eulerAngles;
        currentRotation.y = 0;
        currentRotation.z = 0;
        rectTransform.eulerAngles = currentRotation;
    }

    public void InitialisePlayerCamera(GameObject targetCamera)
    {
        playerCamera = targetCamera;
    }

    public void UpdateUI(float current, float max)
    {
        //  Debug.Log(Time.time);
        //  Debug.Log(current + " " + max + " ");
     //   Debug.Log(current + " " + max);
      //  Debug.Log(current / max);
        healthPercentage = current / max;
    //    Debug.Log(healthPercentage);
        healthSlider.value = healthPercentage;
    }
}
