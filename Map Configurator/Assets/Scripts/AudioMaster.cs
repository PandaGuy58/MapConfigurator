using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour
{
    public List<AudioClip> ambient = new List<AudioClip>();     //0
    public List<AudioClip> battle = new List<AudioClip>();      //1
    public List<AudioClip> village = new List<AudioClip>();     //2
    public List<AudioClip> victory = new List<AudioClip>();     //3


    AudioSource audioSource;


    int currentState = -1;
    int targetState = 0;

    float calculateVolume = 0;

    public AnimationCurve curve;

    public bool gameComplete = false;
    public float playerInVillage;
    public float playerInBattle;

    float maxTarget;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update()
    {
        if(gameComplete)
        {
            targetState = 3;
        }
        else if(playerInVillage > Time.time)
        {
            targetState = 2;
        }
        else if(playerInBattle > Time.time)
        {
            targetState = 1;
        }
        else
        {
            targetState = 0;
        }


        float evaluateCurve = curve.Evaluate(calculateVolume);
        audioSource.volume = evaluateCurve * maxTarget;



        if (currentState != targetState)
        {
            calculateVolume -= Time.deltaTime * 0.4f;
            if(calculateVolume < 0)
            {
                calculateVolume = 0;
            }

            if(calculateVolume == 0)
            {
                currentState = targetState;


                if(currentState == 0)
                {
                    audioSource.clip = ambient[Random.Range(0, ambient.Count)];
                    maxTarget = 1;
                }
                else if(currentState == 1)
                {
                    audioSource.clip = battle[Random.Range(0, battle.Count)];
                    maxTarget = 0.3f;
                }
                else if(currentState ==2)
                {
                    audioSource.clip = village[Random.Range(0, village.Count)];
                    maxTarget = 1;
                }
                else
                {
                    audioSource.clip = victory[Random.Range(0, victory.Count)];
                    maxTarget = 1;
                }


                audioSource.Play();
            }

        }
        else
        {
            calculateVolume += Time.deltaTime * 0.4f;
            if (calculateVolume > 1)
            {
                calculateVolume = 1;
            }
        }
    }
}
