using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{

    public List<Interactable> interactablesQuestOne = new List<Interactable>();
    public List<Interactable> interactablesQuestTwo = new List<Interactable>();
    public List<Interactable> interactablesQuestThree = new List<Interactable>();

    public Interactable firstInteract;
    public Interactable campInteract;

    public int stage = 0;

    public List<CharacterController> questOneGuards = new List<CharacterController>();
    public List<CharacterController> questTwoGuards = new List<CharacterController>();
    public List<CharacterController> questThreeGuards = new List<CharacterController>();

    public List<HexTile> questOneTiles = new List<HexTile>();
    public List<HexTile> questTwoTiles = new List<HexTile>();
    public List<HexTile> questThreeTiles = new List<HexTile>();

    public string targetTitle = "";
    public string targetDescription = "";
    Image targetImage;

    public TextMeshProUGUI description;
    public TextMeshProUGUI title;
    public TextMeshProUGUI space;

    public AnimationCurve curve;

    public Image background;
    float backgroundValue = 0;

    public Image village;
    public Image water;
    public Image chest;
    public Image swords;
    public Image clap;



    float contentsValue = 0;

    public bool panelActive = false;

    public BattleAreas battleAreas;
    public CameraController cameraControl;

    public AudioMaster audioMaster;


    void UpdateImage(Image targetImage, float value)
    {
        float curveEvaluate = curve.Evaluate(value);
        Color targetColour = targetImage.color;
        targetColour.a = curveEvaluate;
        targetImage.color = targetColour;
    }

    void UpdateText(TextMeshProUGUI targetText, float value)
    {
        float curveEvaluate = curve.Evaluate(value);
        Color targetColour = targetText.color;
        targetColour.a = curveEvaluate;
        targetText.color = targetColour;

    }

    private void Update()
    {
        if(battleAreas.battleTriggered)
        {
            panelActive = false;
        }



        UpdateImage(background, backgroundValue);

        if (targetImage != null)
        {
            UpdateImage(targetImage, contentsValue);
        }

        description.text = targetDescription;
        title.text = targetTitle;

        UpdateText(description, contentsValue);
        UpdateText(title, contentsValue);
        UpdateText(space, contentsValue);



        cameraControl.questPanel = panelActive;



        if (Input.GetKeyDown(KeyCode.Space))
        {
            panelActive = !panelActive;
        }

        if(panelActive)
        {
            backgroundValue += Time.deltaTime * 2.5f;
            if(backgroundValue > 1)
            {
                backgroundValue = 1;
            }

            if(backgroundValue == 1)
            {
                contentsValue += Time.deltaTime * 2.5f;
                if (contentsValue > 1)
                {
                    contentsValue = 1;
                }
            }

        }
        else
        {
            if(contentsValue == 0)
            {
                backgroundValue -= Time.deltaTime * 2.5f;
                if (backgroundValue < 0)
                {
                    backgroundValue = 0;
                }
            }

            if(contentsValue > 0)
            {
                contentsValue -= Time.deltaTime * 2.5f;
                if (contentsValue < 0)
                {
                    contentsValue = 0;
                }
            }

        }











        if(stage == 0)                          // start game
        {
            ActivateGuards(questOneGuards, questOneTiles, true);
            ActivateGuards(questTwoGuards, questTwoTiles, true);
            ActivateGuards(questThreeGuards, questThreeTiles, true);

            
        }
        else if(stage == 1)                     // visited start + save npcs
        {
            targetTitle = "Save Mercenaries";
            targetDescription = "Remaining: " + interactablesQuestOne.Count;


            ActivateGuards(questOneGuards, questOneTiles, true);
            ActivateGuards(questTwoGuards, questTwoTiles, true);
            ActivateGuards(questThreeGuards, questThreeTiles, true);

            targetImage = swords;

        }
        else if(stage == 2)                     // visit village
        {
            targetTitle = "Village";
            targetDescription = "Speak to Village Leader";

            ActivateGuards(questOneGuards, questOneTiles, false);
            ActivateGuards(questTwoGuards, questTwoTiles, true);
            ActivateGuards(questThreeGuards, questThreeTiles, true);

            targetImage = village;
        }
        else if(stage == 3)                     // visit chests
        {
            targetTitle = "Plunder Treasures";
            targetDescription = "Remaining: " + interactablesQuestTwo.Count;

            ActivateGuards(questOneGuards, questOneTiles, true);
            ActivateGuards(questTwoGuards, questTwoTiles, false);
            ActivateGuards(questThreeGuards, questThreeTiles, true);

            targetImage = chest;
        }
        else if (stage == 4)                    // visit village
        {
            targetTitle = "Village";
            targetDescription = "Speak to Village Leader";

            ActivateGuards(questOneGuards, questOneTiles, true);
            ActivateGuards(questTwoGuards, questTwoTiles, false);
            ActivateGuards(questThreeGuards, questThreeTiles, true);

            targetImage = village;
        }
        else if (stage == 5)                    // visit wells
        {
            targetTitle = "Revitilizing Springs";
            targetDescription = "Remaining: " + interactablesQuestThree.Count;

            ActivateGuards(questOneGuards, questOneTiles, true);
            ActivateGuards(questTwoGuards, questTwoTiles, true);
            ActivateGuards(questThreeGuards, questThreeTiles, false);

            targetImage = water;
        }
        else if (stage == 6)                    // visit village
        {
            targetTitle = "Village";
            targetDescription = "Speak to Village Leader";

            ActivateGuards(questOneGuards, questOneTiles, true);
            ActivateGuards(questTwoGuards, questTwoTiles ,true);
            ActivateGuards(questThreeGuards, questThreeTiles, false);

            targetImage = village;
        }
        else                                     // gg
        {
            targetTitle = "Demo Complete";
            targetDescription = "I hope you enjoyed yourself!";

            ActivateGuards(questOneGuards, questOneTiles, false);
            ActivateGuards(questTwoGuards, questTwoTiles, false);
            ActivateGuards(questThreeGuards, questThreeTiles, false);

            targetImage = clap;

            audioMaster.gameComplete = true;
        }
    }

    void ActivateGuards(List<CharacterController> targetGuards, List<HexTile> targetTiles, bool active)
    {
    
        for(int i = 0; i < targetGuards.Count; i++)
        {
            targetGuards[i].UpdateGuard(active);

            if(active)                  // here
            {
                targetTiles[i].objectAtLocation = targetGuards[i].gameObject;
                targetTiles[i].walkable = false;
            }
            else
            {
                targetTiles[i].objectAtLocation = null;
                targetTiles[i].walkable = true;
            }
        }


    }


    public void InitialiseInteract(Interactable targetInteract)
    {
        if(targetInteract.interactID == 2 || targetInteract.interactID == 3|| targetInteract.interactID == 4)
        {
            interactablesQuestOne.Add(targetInteract);
        }

        else if(targetInteract.interactID == 1)
        {
            firstInteract = targetInteract;
        }

        else if (targetInteract.interactID == 0)
        {
            campInteract = targetInteract;
        }

        else if (targetInteract.interactID == 5 || targetInteract.interactID == 6 || targetInteract.interactID == 7)
        {
            interactablesQuestTwo.Add(targetInteract);
        }

        else
        {
            interactablesQuestThree.Add(targetInteract);
        }
    }

    public void SubmitInteract(Interactable targetInteract)
    {
        if(targetInteract.interactID == 1)
        {
            stage = 1;

            panelActive = true;
        }

        else if(targetInteract.interactID == 2 || targetInteract.interactID == 3 || targetInteract.interactID == 4)
        {
            if(stage == 0)
            {
                panelActive = true;
            }

            stage = 1;
            interactablesQuestOne.Remove(targetInteract);

            if(interactablesQuestOne.Count == 0)
            {
                stage = 2;
                panelActive = true;
                // remove guards to area


            }
        }

        else if(targetInteract.interactID == 0)
        {
            if(stage == 2)
            {
                stage = 3;
                panelActive = true;
                // remove guards to area

                // enable guards to area
            }

            else if(stage == 4)
            {
                stage = 5;
                panelActive = true;
                // remove guards to area

                // enable guards to area
            }
            else if(stage == 6)
            {
                stage = 7;
                panelActive = true;
                // gg 

                // enable all areas
                Debug.Log(Time.time + "gg");
            }
        }

        else if(targetInteract.interactID == 5|| targetInteract.interactID == 6|| targetInteract.interactID == 7)
        {
            interactablesQuestTwo.Remove(targetInteract);

            if(interactablesQuestTwo.Count == 0)
            {
                // enable camp interact
                stage = 4;

                campInteract.readyToInteract = true;

                panelActive = true;
            }
        }

        else if(targetInteract.interactID == 8 || targetInteract.interactID == 9 || targetInteract.interactID == 10)
        {
            interactablesQuestThree.Remove(targetInteract);

            if(interactablesQuestThree.Count == 0)
            {
                stage = 6;
                campInteract.readyToInteract = true;

                panelActive = true;

            }
        }
    }

}




    //  int targetActiveQuest = -1;
    //  int currentActiveQuest = -1;
    //  int remainingInteractions;
    /*

    //  int interactionCounter = 0;
    List<int> interactionIDs = new List<int>();

    int stage = -1;
    bool panelActivated = false;

    public Image background;
    float backgroundValue = 0;
    float imageValue = 0;


    public List<Image> questImages = new List<Image>();
    // village - water - chest - swords - clap

    Image targetImg;

    Color targetColour = new Color(1, 1, 1, 0.8f);
    Color startColour = new Color(1, 1, 1, 0);
    public AnimationCurve curve;

    private void Update()
    {
        if (stage == 0)
        {

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(panelActivated);
            panelActivated = !panelActivated;

            /*
            if (stage == 0)
            {
                targetImg = swords;
            }
            else if (stage == 1)
            {
                targetImg = village;
            }
            else if (stage == 2)
            {
                targetImg = chest;
            }
            else if (stage == 3)
            {
                targetImg = village;
            }
            else if (stage == 4)
            {
                targetImg = clap;
            }
        }
         
}

if (panelActivated)
        {
            ControlImage(true);
        }
        else
        {
            ControlImage(false);
        }
    }

    public void SubmitInteraction(int interactionID)
    {
        interactionIDs.Add(interactionID);

        if (stage == -1)
        {
            if (interactionIDs.Count > 0)
            {
                stage = 0;
            }
        }
    }

    void ControlImage(bool active)
    {
        Image targetImg = null;
        if(stage == 0)
        {
            targetImg = questImages[3];
        }


        Color calcColour;
        if (targetImg != null)
        {
            calcColour = Color.Lerp(startColour, targetColour, curve.Evaluate(imageValue));
            targetImg.color = calcColour;
        }

        calcColour = Color.Lerp(startColour, targetColour, curve.Evaluate(imageValue));
        background.color = calcColour;

        if (active)
        {
            backgroundValue += Time.deltaTime * 2;
            if (backgroundValue > 1)
            {
                backgroundValue = 1;
            }

            if(backgroundValue > 0.5f)
            {
                imageValue += Time.deltaTime * 2;
                if (imageValue > 1)
                {
                    imageValue = 1;
                }
            }
        }
        else
        {
            imageValue -= Time.deltaTime * 2;
            if (imageValue < 0)
            {
                imageValue = 0;
            }

            if (imageValue < 0.5f)
            {
                backgroundValue -= Time.deltaTime * 2;
                if (backgroundValue < 0)
                {
                    backgroundValue = 0;
                }
            }
        }
    }
}
        targetImg
        if (active)
        {
            
        }
        else
        {
            backgroundValue -= Time.deltaTime * 2;
        }

        else if (backgroundValue < 0)
        {
            backgroundValue = 0;
        }

        if (active && 



        if (stage == 0)
        {
            targetImg = questImages[3];
        }

        targetImg.color = calcColour;

    }
}
        */