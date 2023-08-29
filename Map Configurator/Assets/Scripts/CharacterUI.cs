using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterUI : MonoBehaviour
{
    public int turnsLeft;
    public int health;
    RectTransform rectTransform;


    public float currentTurnPercentage = 0;

    public float currentTurns;

    public Slider turnSlider;
    public Slider healthSlider;

    public Image healthColour;
    public Image turnColour;

    float visibility = 0;

    bool combatEnabled;

    public AnimationCurve curve;

    float recentHealthTarget;
    float currentHealthTarget;
    float healthTransition = 0;

    float recentTurnTarget = 0;
    float currentTurnTarget;
    float turnTransition = 0;


    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
 
        Vector3 currentRotation = rectTransform.eulerAngles;
        currentRotation.y = 0;
        currentRotation.z = 0;
        rectTransform.eulerAngles = currentRotation;


        float curveEvaluate = curve.Evaluate(visibility);
        curveEvaluate = curveEvaluate * 0.3f;
        Color targetColour = healthColour.color;
        targetColour.a = curveEvaluate;
        healthColour.color = targetColour;

        targetColour = turnColour.color;
        targetColour.a = curveEvaluate;
        turnColour.color = targetColour;


        if(healthTransition != 1)
        {
            healthTransition += Time.deltaTime;
            curveEvaluate = curve.Evaluate(healthTransition);
            float healthCalculate = Mathf.Lerp(recentHealthTarget, currentHealthTarget, curveEvaluate);
            healthSlider.value = healthCalculate;

            if(healthTransition > 1)
            {
                healthTransition = 1;
            }
        }

        if(turnTransition != 1)
        {
            turnTransition += Time.deltaTime;
            curveEvaluate = curve.Evaluate(turnTransition);
            float turnCalculate = Mathf.Lerp(recentTurnTarget, currentTurnTarget, curveEvaluate);
            turnSlider.value = turnCalculate;

            if(turnTransition > 1)
            {
                turnTransition = 1;
            }
        }

        if (combatEnabled)
        {
            visibility += Time.deltaTime;
            if(visibility > 1)
            {
                visibility = 1;
            }
        }
        else
        {
            visibility -= Time.deltaTime;
            if(visibility < 0)
            {
                visibility = 0;
            }
        }

        
    }

    public void Initialise(HexGrid hexGrid, AnimationCurve targetCurve)
    {
        curve = targetCurve;
    }

    public void CharacterUpdate(bool combat)
    {
        combatEnabled = combat;
    }

    public void UpdateHealth(float healthTarget, float maxHealth)
    {
        recentHealthTarget = currentHealthTarget;
        currentHealthTarget = healthTarget / maxHealth;
        healthTransition = 0;

    }

    public void UpdateTurns(float turnTarget)
    {
        recentTurnTarget = currentTurnTarget;
        currentTurnTarget = turnTarget / 2;
        turnTransition = 0;
    }
}






        /*
        //   gameObject.transform.rotation = Quaternion.LookRotation(-playerCamera.transform.position + gameObject.transform.position);      // adjust rotation to face player camera
        Vector3 currentRotation = rectTransform.eulerAngles;
        currentRotation.y = 0;
        currentRotation.z = 0;
        rectTransform.eulerAngles = currentRotation;

        currentHealthPercentage = Mathf.Lerp(currentHealthPercentage, targetHealthPercentage, Time.deltaTime);
        currentTurnPercentage = Mathf.Lerp(currentTurnPercentage, targetTurnPercentage, Time.deltaTime);

        turnSlider.value = currentTurnPercentage;
        healthSlider.value = currentHealthPercentage;

        if (combatEnabled)
        {

            Color sliderColour = healthColour.color;
            Color targetSliderColour = sliderColour;
            targetSliderColour.a = 0.4f;
            sliderColour = Color.Lerp(sliderColour, targetSliderColour, Time.deltaTime);
            healthColour.color = sliderColour;


            sliderColour = turnColour.color;
            targetSliderColour = sliderColour;
            targetSliderColour.a = 0.4f;
            sliderColour = Color.Lerp(sliderColour, targetSliderColour, Time.deltaTime);
            turnColour.color = sliderColour;



        }
        else
        {
            Color sliderColour = healthColour.color;
            Color targetSliderColour = sliderColour;
            targetSliderColour.a = 0;
            sliderColour = Color.Lerp(sliderColour, targetSliderColour, Time.deltaTime);
            healthColour.color = sliderColour;

            sliderColour = turnColour.color;
            targetSliderColour = sliderColour;
            targetSliderColour.a = 0;
            sliderColour = Color.Lerp(sliderColour, targetSliderColour, Time.deltaTime);
            turnColour.color = sliderColour;

        }





    }

    public void CharacterUpdate(bool combat)
    {
        combatEnabled = combat;

    }

        
        
      //  float currentTurns, float currentHealth, float maxHealth, bool combat)
  //  {
    //    targetTurnPercentage = currentTurns / 2;
     //   targetHealthPercentage = currentHealth / maxHealth;



    public void Initialise(HexGrid hexGrid)
    {
     //   playerCamera = hexGrid.playerCamera.gameObject;
    }
}
        */