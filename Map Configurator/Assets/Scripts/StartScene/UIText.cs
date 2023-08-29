using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIText : MonoBehaviour
{
    public Color startColour;
    TextMeshProUGUI textMeshPro;

    public TextMeshProUGUI containSymbolOne;
    public TextMeshProUGUI containSymbolTwo;

    float raycastTime;
    float raycastValue = 0;

    float glowIntensityTarget = 0.10f;

    public AnimationCurve curve;

    public float glowNoiseScrollSpeed;
    public float glowNoiseJumpScale;

    public float colourChangeOffset;
    public float colourTransitionScrollSpeed;
    public float colourTransitionJumpScale;

    public int action;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (Time.time < raycastTime + 0.1f)
        {
            float noise = Mathf.PerlinNoise(Time.time, 1 + Time.time) - 0.5f;
            if (noise < 0)
            {
                noise = -noise;
            }

            raycastValue += Time.deltaTime * 0.7f;
            raycastValue -= noise * Time.deltaTime;
            if (raycastValue > 1)
            {
                raycastValue = 1;
            }
        }
        else if (Time.time > raycastTime + 0.15f)
        {
            float noise = Mathf.PerlinNoise(Time.time, 1 + Time.time) - 0.5f;
            if (noise < 0)
            {
                noise = -noise;
            }

            raycastValue -= Time.deltaTime * 1f;
            raycastValue += noise * Time.deltaTime;
            if (raycastValue < 0)
            {
                raycastValue = 0;
            }
        }

        CalculateIntensity();
        CalculateColour();
    }

    void CalculateIntensity()
    {
        float noise = Mathf.PerlinNoise(Time.time * glowNoiseScrollSpeed, 1 + Time.time * glowNoiseScrollSpeed) - 0.5f;
        noise = noise * glowNoiseJumpScale;
        if(noise < 0)
        {
            noise = -noise;
        }
        float currentGlowIntensity = curve.Evaluate(raycastValue) * (glowIntensityTarget + noise);
        textMeshPro.fontMaterial.SetFloat("_GlowPower", currentGlowIntensity);

        containSymbolOne.fontMaterial.SetFloat("_GlowPower", currentGlowIntensity * 0.75f);
        containSymbolTwo.fontMaterial.SetFloat("_GlowPower", currentGlowIntensity * 0.75f);
    }

    void CalculateColour()
    {
        float r = Mathf.PerlinNoise(colourChangeOffset + Time.time * colourTransitionScrollSpeed, colourChangeOffset + 1 + Time.time * colourTransitionScrollSpeed) - 0.5f;
        float g = Mathf.PerlinNoise(colourChangeOffset + 2 + Time.time * colourTransitionScrollSpeed, colourChangeOffset + 3 + Time.time * colourTransitionScrollSpeed) - 0.5f;
        float b = Mathf.PerlinNoise(colourChangeOffset + 4 + Time.time * colourTransitionScrollSpeed, colourChangeOffset + 5 + Time.time * colourTransitionScrollSpeed) - 0.5f;

        Color targetColour = startColour;
        targetColour.r += r * colourTransitionJumpScale;
        targetColour.g += g * colourTransitionJumpScale;
        targetColour.b += b * colourTransitionJumpScale;

        textMeshPro.fontMaterial.SetColor("_GlowColor", targetColour);

        containSymbolOne.fontMaterial.SetColor("_GlowColor", targetColour);
        containSymbolTwo.fontMaterial.SetColor("_GlowColor", targetColour);
    }

    public int RaycastText()
    {
        raycastTime = Time.time;
        return action;
    }
}
/*

public AnimationCurve curve;
TMP_Text text;
Color textColour = Color.black;
float raycastValue;

float raycastTime = 0;

public Color targetTextColour;
//   Color currentTargetColour;

public float colourScrollSpeed = 0.25f;
public float colourChangeScale = 0.25f;

Color currentTargetColour;

public float colourNoiseOffset;

public float noiseBase = 0.05f;
public float noiseJumpScale = 0.2f;
public float noiseScrollSpeed = 0.2f;


// Start is called before the first frame update
void Start()
{
    text = GetComponent<TMP_Text>();
}

// Update is called once per frame

*/

//        float calculateNoise = noiseBase + (noiseJumpScale * Mathf.PerlinNoise(Time.time * noiseScrollSpeed, 1f + Time.time * noiseScrollSpeed));

//    CalculateTargetColour();
//     float evaluateVal = curve.Evaluate(raycastValue);
//   Color currentColour = Color.Lerp(textColour, currentTargetColour, evaluateVal - calculateNoise);
//   text.color = currentColour;


/*
public void CalculateTargetColour()
{
    float r = Mathf.PerlinNoise(colourNoiseOffset + Time.time * colourScrollSpeed, colourNoiseOffset + 1 + Time.time * colourScrollSpeed) - 0.5f;
    float g = Mathf.PerlinNoise(colourNoiseOffset + 2 + Time.time * colourScrollSpeed, colourNoiseOffset + 3 + Time.time * colourScrollSpeed) - 0.5f;
    float b = Mathf.PerlinNoise(colourNoiseOffset + 4 + Time.time * colourScrollSpeed, colourNoiseOffset + 5 + Time.time * colourScrollSpeed) - 0.5f;

    currentTargetColour = targetTextColour;
    currentTargetColour.r += r * colourChangeScale;
    currentTargetColour.g += g * colourChangeScale;
    currentTargetColour.b += b * colourChangeScale;
}

public void TEst()
{
//     text.
}
}
*/

/*
 *             public float positionScrollSpeed = 1;
    public float positionJumpScale = 0.25f;

    Vector3 initialPositionValue;


                float x = Mathf.PerlinNoise(Time.time * positionScrollSpeed, 1 + Time.time * positionScrollSpeed) - 0.5f;
     //   float y = Mathf.PerlinNoise(2 + Time.time * positionScrollSpeed, 3 + Time.time * positionScrollSpeed) - 0.5f;
        float z = Mathf.PerlinNoise(4 + Time.time * positionScrollSpeed, 5 + Time.time * positionScrollSpeed) - 0.5f;

        Vector3 calculatePostion = initialPositionValue;
        calculatePostion.x += x * positionJumpScale;
     //   calculatePostion.y += y * positionJumpScale;
        calculatePostion.z += z * positionJumpScale;


        transform.position = calculatePostion;
*/