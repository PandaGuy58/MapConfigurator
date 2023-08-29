using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRandomise : MonoBehaviour
{

    public List<GameObject> arms;
    public List<GameObject> body;
    public List<GameObject> head;
    public List<GameObject> legs;
    public List<GameObject> shoulders;
    public List<GameObject> weapon;
    public List<GameObject> shield;

    List<Renderer> rendersList = new List<Renderer>();

    Color selected = Color.white;
    Color ally = Color.green;
    Color enemy = Color.red;
    public Color player = Color.yellow;

    Color defaultColour;


    float currentShaderVisibility = 0;
    public Color currentShaderColour = Color.black;
    public Color targetShaderColour = Color.black;
    public float currentVisibility;
    public AnimationCurve curve;

    public float shaderTime;

    public float selectedTime;
    public float forceInvisibleTime;

    public void Initialise(int colours)         // 0 player / 1 ally / 2 enemy
    {
        RandomiseCharacter(arms);
        RandomiseCharacter(body);
        RandomiseCharacter(head);
        RandomiseCharacter(legs);
        RandomiseCharacter(shoulders);
        RandomiseCharacter(weapon);
        RandomiseCharacter(shield);

        if (colours == 0)
        {
            defaultColour = player;
        }
        else if (colours == 1)
        {
            defaultColour = ally;
        }
        else
        {
            defaultColour = enemy;
        }
    }


    void RandomiseCharacter(List<GameObject> bodyPart)
    {
        if (bodyPart.Count > 0)
        {
            for (int i = 0; i < bodyPart.Count; i++)
            {
                bodyPart[i].SetActive(false);
            }

            GameObject targetPart = bodyPart[Random.Range(0, bodyPart.Count)];
            targetPart.SetActive(true);
            rendersList.Add(targetPart.GetComponent<Renderer>());
        }
    }

    public void UpdateShader()
    {

        if(selectedTime > Time.time)
        {
            currentShaderColour = Color.Lerp(currentShaderColour, selected, 2 * Time.deltaTime);
        }
        else
        {
            currentShaderColour = Color.Lerp(currentShaderColour, defaultColour, 2 * Time.deltaTime);
        }

        if(forceInvisibleTime > Time.time)
        {
            currentShaderVisibility -= Time.deltaTime;
            {
                if (currentShaderVisibility < 0)
                {
                    currentShaderVisibility = 0;
                }
            }
        }
        else if(shaderTime > Time.time)
        {
            currentShaderVisibility += Time.deltaTime;
            if(currentShaderVisibility > 1)
            {
                currentShaderVisibility = 1;
            }
        }
        else
        {
            currentShaderVisibility -= Time.deltaTime;
            {
                if(currentShaderVisibility < 0)
                {
                    currentShaderVisibility = 0;
                }
            }
        }


        float visibilityEvaluate = curve.Evaluate(currentVisibility);
        float shaderEvaluate = curve.Evaluate(currentShaderVisibility);
        for (int i = 0; i < rendersList.Count; i++)
        {

            rendersList[i].material.SetFloat("_Float", shaderEvaluate * 0.05f);
            rendersList[i].material.SetColor("_Color", currentShaderColour);
            rendersList[i].material.SetFloat("_Visibility", visibilityEvaluate);
        }
    }
}











    /*
    public void ForceVisible()
    {
        for (int i = 0; i < rendersList.Count; i++)
        {
           // rendersList[i].material.SetFloat("_Float", currentShaderVisibility);
            rendersList[i].material.SetColor("_Color", currentShaderColour);
            rendersList[i].material.SetFloat("_Visibility", 0);
        }
    }

    */
    /*
public void UpdateShader(float target, Color col,  float currentVisibility)
{
  currentShaderVisibility = Mathf.Lerp(currentShaderVisibility, target, 2 * Time.deltaTime);
  currentShaderColour = Color.Lerp(currentShaderColour, col, 2 * Time.deltaTime);


  for (int i = 0; i < rendersList.Count; i++)
  {
      rendersList[i].material.SetFloat("_Float", currentShaderVisibility);
      rendersList[i].material.SetColor("_Color", currentShaderColour);
      rendersList[i].material.SetFloat("_Visibility", currentVisibility);
  }

  if (target == 2)
  {
      currentShaderVisibility = Mathf.Lerp(currentShaderVisibility, 0.05f, 2 * Time.deltaTime);
      currentShaderColour = Color.Lerp(currentShaderColour, selected, 2 * Time.deltaTime);
  }
  else if (target == 1)
  {
      currentShaderVisibility = Mathf.Lerp(currentShaderVisibility, 0.1f, 2 * Time.deltaTime);
      currentShaderColour = Color.Lerp(currentShaderColour, selected, 2 * Time.deltaTime);
  }
  else
  {

      currentShaderVisibility = Mathf.Lerp(currentShaderVisibility, 0, 2 * Time.deltaTime);
      currentShaderColour = Color.Lerp(currentShaderColour, selected, 2 * Time.deltaTime);
  }
  */


