using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Title : MonoBehaviour
{
    public Color startColour;
  //  TextMeshProUGUI textMeshPro;

    public float glowValueBase = -0.25f;
    public float glowValueScrollSpeed = 0.5f;
    public float glowValueJumpScale = 0.75f;
    public float glowValueOffSet = 2;

  //  public float colourTransitionBase;
    public float colourTransitionScrollSpeed;
    public float colourTransitionJumpScale;
    public float colourTransitionOffSet;

  //  public List<TextMeshProUGUI> allTMPFeattures;
    public TextMeshProUGUI titleGui;
    public List<TextMeshProUGUI> cornersGui;
    public List<TextMeshProUGUI> bordersGui;
    /*
    //  public Color startColour;
    // Start is called before the first frame update
    public AnimationCurve curve;
    //  TMP_Text text;

    float currentValue = 0.5f;

    //    float blackToColourTransitionValue = 0;
    public float transitionBase = -0.5f;
    public float transitionJumpScale = 0.2f;
    public float transitionScrollSpeed = 0.5f;

    public float colourNoiseOffset;
    public float colourScrollSpeed = 0.25f;
    public float colourChangeScale = 0.25f;

    Color currentTargetColour;
    public Color targetTextColour;

    float timeActivated = -1;

    TextMeshProUGUI textElement;
    */

    float calculateTime;
    float timeActivated = -1;

    private void Start()
    {
        titleGui = GetComponent<TextMeshProUGUI>();

        titleGui.fontMaterial.SetFloat("_GlowPower", 0);


    }
    // Update is called once per frame
    void Update()
    {
        if(timeActivated != -1 && Time.time > timeActivated)
        {
            calculateTime = Time.time - timeActivated;
            CalculateColour();
            CalculateGlow();
        }

    }

    void CalculateColour()
    {
        float r = Mathf.PerlinNoise(colourTransitionOffSet + + calculateTime * colourTransitionScrollSpeed, colourTransitionOffSet + 1 + calculateTime * colourTransitionScrollSpeed) - 0.5f;
        float g = Mathf.PerlinNoise(colourTransitionOffSet +  2 + calculateTime * colourTransitionScrollSpeed, colourTransitionOffSet + 3 + calculateTime * colourTransitionScrollSpeed) - 0.5f;
        float b = Mathf.PerlinNoise(colourTransitionOffSet +  4 + calculateTime * colourTransitionScrollSpeed, colourTransitionOffSet + 5 + calculateTime * colourTransitionScrollSpeed) - 0.5f;

        Color targetColour = startColour;
        targetColour.r += r * colourTransitionJumpScale;
        targetColour.g += g * colourTransitionJumpScale;
        targetColour.b += b * colourTransitionJumpScale;

        titleGui.fontMaterial.SetColor("_GlowColor", targetColour);

        for (int i = 0; i < cornersGui.Count; i++)
        {
            cornersGui[i].fontMaterial.SetColor("_GlowColor", targetColour);
        }

        for (int i = 0; i < bordersGui.Count; i++)
        {
            bordersGui[i].fontMaterial.SetColor("_GlowColor", targetColour);
        }
    }

    void CalculateGlow()
    {
        float calculate = glowValueJumpScale * Mathf.PerlinNoise(glowValueOffSet + calculateTime * glowValueScrollSpeed, glowValueOffSet + 1 + calculateTime * glowValueScrollSpeed);
        calculate = glowValueBase + calculate;

        titleGui.fontMaterial.SetFloat("_GlowPower", calculate);

        for(int i = 0; i < cornersGui.Count; i++)
        {
            cornersGui[i].fontMaterial.SetFloat("_GlowPower", calculate * 0.4f);
        }

        for(int i = 0; i < bordersGui.Count; i++)
        {
            bordersGui[i].fontMaterial.SetFloat("_GlowPower", calculate * 0.35f);
        }
    }

    public void ScriptActivate()
    {
        if (timeActivated == -1)
        {
            timeActivated = Time.time + 0.5f;
        }
    }
}



/*
float calculate = transitionBase + (transitionJumpScale * Mathf.PerlinNoise(calculateTime * transitionScrollSpeed, 1 + calculateTime * transitionScrollSpeed));
calculate = curve.Evaluate(calculate);


float r = Mathf.PerlinNoise(colourNoiseOffset + calculateTime * colourScrollSpeed, colourNoiseOffset + 1 + calculateTime * colourScrollSpeed) - 0.5f;
float g = Mathf.PerlinNoise(colourNoiseOffset + 2 + calculateTime * colourScrollSpeed, colourNoiseOffset + 3 + calculateTime * colourScrollSpeed) - 0.5f;
float b = Mathf.PerlinNoise(colourNoiseOffset + 4 + calculateTime * colourScrollSpeed, colourNoiseOffset + 5 + calculateTime * colourScrollSpeed) - 0.5f;

currentTargetColour = targetTextColour;
currentTargetColour.r += r * colourChangeScale;
currentTargetColour.g += g * colourChangeScale;
currentTargetColour.b += b * colourChangeScale;


//     text.color = Color.Lerp(Color.black, currentTargetColour, calculate);
*/




/*




    float random = Random.Range(-100, 100);
    random = random / 50;

  //  Debug.Log(random); 
    currentValue += random * Time.deltaTime;
    if(currentValue > 1)
    {
        currentValue = 1;
    }
    else if(currentValue < 0)
    {
        currentValue = 0;
    }

    Color color = Color.Lerp(startColour, endColour, curve.Evaluate(currentValue));
    text.color = color;

    float defaultValue = 0.75f;
    float sine = Mathf.Sin(Time.time / 0.75f);
    sine = sine / 4;

    sine += defaultValue;
    Color currentColour = Color.Lerp(startColour, endColour, sine);
    text.color = currentColour;

    float value = Time.time / 2;
    float sine = Mathf.Sin(value);
    Debug.Log(sine);

    if(sine < 0)
    {
        sine = -sine;
    }

    Color currentColour = Color.Lerp(startColour, endColour, sine);
  //  Debug.Log(currentColour);
    text.color = currentColour;







        public float positionScrollSpeed = 1;
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