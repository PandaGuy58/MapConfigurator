using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
  //  RectTransform rectTransform;
    public GameObject interactionMark;
//    public GameObject exclamationMark;
    public TextMeshProUGUI textmeshPro;
    Renderer rend;
    float timeActivate;
    float visibilityValue;

    public AnimationCurve curve;

    public GameObject playerCamera;
   // CharacterController playerController;
    QuestManager questManager;

    public bool npc;
    CharacterController npcController;

    public bool readyToInteract = true;
    public int interactID;

    public bool health;


    private void Start()
    {

        if(interactionMark != null)
        {
            rend = interactionMark.GetComponent<Renderer>();
        }

        if(npc)
        {
            npcController = GetComponent<CharacterController>();
        }
        
        
    }

    private void Update()
    {

        if(timeActivate > Time.time && readyToInteract)
        {
            if (readyToInteract)
            {
                gameObject.tag = "Interact";
            }

            visibilityValue += Time.deltaTime;
            if(visibilityValue > 1)
            {
                visibilityValue = 1;
            }
        }
        else
        {
            if (!readyToInteract)
            {
                gameObject.tag = "Undefined";
            }

            visibilityValue -= Time.deltaTime;
            if(visibilityValue < 0)
            {
                visibilityValue = 0;
            }
        }

        if(textmeshPro != null)
        {

            float curveEvalate = curve.Evaluate(visibilityValue);

            Color targetColour = textmeshPro.color;
            targetColour.a = curveEvalate;
            textmeshPro.color = targetColour;


            rend.material.SetFloat("_alpha", curveEvalate * 0.3f);

            textmeshPro.transform.rotation = Quaternion.LookRotation(-playerCamera.transform.position + gameObject.transform.position);


            Vector3 currentRotation = textmeshPro.transform.eulerAngles;
           // currentRotation.y = 0;
            currentRotation.z = 0;
            textmeshPro.transform.eulerAngles = currentRotation;

        }





    }

    public void InitialiseInteractable(HexGrid hexGrid, int targetID)
    {
       // Debug.Log(Time.time);
        curve = hexGrid.curve;
        playerCamera = hexGrid.playerCamera.gameObject;
       // playerController = hexGrid.playerCharacter;
        questManager = hexGrid.questManager;
        interactID = targetID;
    }

    public void Interact(CharacterController target)
    {
        if(health)
        {
            target.currentHealth = target.maxHealth;
            target.forceUI = Time.time + 1;
            // restore player health
        }
        else
        {
            readyToInteract = false;
            questManager.SubmitInteract(this);
        }
        

        if (npc)
        {
            npcController.ReceiveLookTarget(target.gameObject);            //target.transform);
        }

    }

    public void Activate()
    {
            timeActivate = Time.time + 0.1f;
    }
}











    //  public bool health;
    //  bool readyToInteract;
    //   CharacterController characterController;
    //   QuestManager questManager;
    //    GameObject playerCamera;

    //    public GameObject interactionMark;
    //   RectTransform rectTransform;
    /*

    public GameObject interactionMark;
    public GameObject targetPlayerCamera;
    public CharacterController player;
    public QuestManager targetQuestManager;

    public bool readyToInteract = true;
    public bool midCombat = false;

    public bool health;
    float timeActivate;

    public CharacterController npcController;
    public int interactID;

    private void Start()
    {
        //   rectTransform = interactionMark.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Time.time > timeActivate)
        {
            if (readyToInteract)
            {
                gameObject.tag = "Interact";

                if (interactionMark != null)
                {

                    interactionMark.SetActive(true);

                    interactionMark.transform.rotation = Quaternion.LookRotation(-targetPlayerCamera.transform.position + gameObject.transform.position);      // adjust rotation to face player camera
                    Vector3 currentRotation = interactionMark.transform.eulerAngles;
                    currentRotation.y = 0;
                    currentRotation.z = 0;
                    interactionMark.transform.eulerAngles = currentRotation;
                }
            }
            else
            {
                gameObject.tag = "Undefined";

                if (interactionMark != null)
                {

                    interactionMark.SetActive(false);

                    interactionMark.transform.rotation = Quaternion.LookRotation(-targetPlayerCamera.transform.position + gameObject.transform.position);      // adjust rotation to face player camera
                    Vector3 currentRotation = interactionMark.transform.eulerAngles;
                    currentRotation.y = 0;
                    currentRotation.z = 0;
                    interactionMark.transform.eulerAngles = currentRotation;
                }
            }
        }
    }
        /*
        else
        {
            if (interactionMark != null)
            {
                
                if (readyToInteract)
                {
                    interactionMark.SetActive(true);

                    interactionMark.transform.rotation = Quaternion.LookRotation(-playerCamera.transform.position + gameObject.transform.position);      // adjust rotation to face player camera
                    Vector3 currentRotation = interactionMark.transform.eulerAngles;
                    currentRotation.y = 0;
                    currentRotation.z = 0;
                    interactionMark.transform.eulerAngles = currentRotation;
                }
                else
                {
                    interactionMark.SetActive(false);
                    interactionMark.transform.rotation = Quaternion.LookRotation(-playerCamera.transform.position + gameObject.transform.position);      // adjust rotation to face player camera
                    Vector3 currentRotation = interactionMark.transform.eulerAngles;
                    currentRotation.y = 0;
                    currentRotation.z = 0;
                    interactionMark.transform.eulerAngles = currentRotation;
                }
            }

        }
    }
   


    public void InitialiseInteractable(HexGrid hexGrid)
    {
        // playerCamera = hexGrid.playerCamera.gameObject;
        //  playerController = hexGrid.playerCharacter;
        //   questManager = hexGrid.questManager;
    }

    public void Interact(CharacterController target)
    {
        if (npcController != null)
        {
            npcController.Interact(target);
        }

        if (health)
        {
            player.RestoreHealth();
        }
        else
        {
            targetQuestManager.SubmitInteraction(interactID);
        }
        readyToInteract = false;
    }

    public void Activate(int active)
    {
        if (active == 1)
        {
            //   Debug.Log(Time.time + " :" + active);
            timeActivate = Time.time + 0.1f;
        }
        else
        {
            //    Debug.Log(Time.time + " :" + active);
            timeActivate = Time.time - 0.1f;
        }


    }


    
    // 0 = health


    // Start is called before the first frame update
    //   void Start()
    //  {

    //  }

    // Update is called once per frame
    //   void Update()
    // {

    //  }

    //   public void InitialiseInteractable(CharacterController cController, QuestManager qManager, GameObject playCam)
    //  {
    //   characterController = cController;
    //  questManager = qManager;
    //   playerCamera = playCam;
    //  }

    /*

    public void PlayerInteract()
    {
        if(health)
        {
          //  characterController.RestoreHealth();
        }
        else
        {

        }
    }

}
    */