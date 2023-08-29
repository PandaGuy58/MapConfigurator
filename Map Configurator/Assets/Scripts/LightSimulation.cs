using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSimulation : MonoBehaviour
{
    public float perlinNoiseValue;

    public float intensityBase = 0.5f;
    public float intensityJumpScale = 0.1f;
    public float intensityScrollSpeed = 1;

    Light compLight;

    public float positionScrollSpeed = 1;
    public float positionJumpScale = 0.25f;

    Vector3 initialPositionValue;

    float timeLightEnabled = -1;
    Camera cam;

    float curveEvaluate = 0;
    public AnimationCurve curve;

    private void Start()
    {
        compLight = GameObject.Find("Point Light").GetComponent<Light>();
        initialPositionValue = compLight.transform.position;
        compLight.intensity = 0;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    private void Update()
    {
        RaycastLight();

        if (timeLightEnabled != -1)
        {
            curveEvaluate = curve.Evaluate((Time.time - timeLightEnabled) / 5);

            CalculateIntensity();
            CalculatePosition();
        }



        //   perlinNoiseValue = Mathf.PerlinNoise(Time.time)
    }

    void CalculateIntensity()
    {
        float calculate = (intensityJumpScale * Mathf.PerlinNoise(Time.time * intensityScrollSpeed, 1f + Time.time * intensityScrollSpeed));
        if(calculate < 0)
        {
            calculate = -calculate;
        }
        calculate = intensityBase + calculate;
        compLight.intensity = calculate * curveEvaluate;
    }

    void CalculatePosition()
    {
        float x = Mathf.PerlinNoise(Time.time * positionScrollSpeed, 1 + Time.time * positionScrollSpeed) - 0.5f;
        float z = Mathf.PerlinNoise(4 + Time.time * positionScrollSpeed, 5 + Time.time * positionScrollSpeed) - 0.5f;

        Vector3 calculatePostion = initialPositionValue;
        calculatePostion.x += x * positionJumpScale;
        calculatePostion.z += z * positionJumpScale;


        compLight.transform.position = calculatePostion;
    }

    public void RaycastLight()
    {
        if(timeLightEnabled == -1)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 100))
            {
                if(hit.transform.gameObject.CompareTag("Candle"))
                {
                    timeLightEnabled = Time.time;
                }
            }
        }
    }
}
    /*
    Light light;
    public AnimationCurve curve;
    public float currentValue = 0;

    float lightValue = 2.5f;

    float flickerTargetTime = -1;
    float flickerTimeDuration;
   // bool flicker = false;

    float targetLightValue;
    float flickerValue = 0;


    // coordinates 


    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if(flickerTargetTime == -1)
        {
            flickerTargetTime = Time.time + Random.Range(0.75f,1.75f);
            flickerTimeDuration = Random.Range(0.3f, 0.5f);
            flickerValue = -1;
        }
        
        if(Time.time >= flickerTargetTime)
        {
            FlickerEffect();
        }
        else
        {
            currentValue += (Random.Range(0, 0.75f)) * Time.deltaTime;
            int wholeNumber = (int)currentValue;
            float number = currentValue - wholeNumber;

            float evalulate = curve.Evaluate(number);
            evalulate += 0.5f;
            targetLightValue = evalulate * lightValue;

            light.intensity = targetLightValue;
        }
    }

    void FlickerEffect()
    {
        if(flickerValue == -1)
        {
            flickerValue = targetLightValue + Random.Range(0.3f, 0.7f);
        }
        light.intensity = flickerValue;
        flickerValue -= 0.02f * Time.deltaTime;
        if (Time.time > flickerTargetTime + flickerTimeDuration)
        {
            flickerTargetTime = -1;
            flickerValue = -1;
        }
    }
}
*/