using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleAreas : MonoBehaviour
{
    List<List<HexTile>> preventLocations = new List<List<HexTile>>();
    List<List<HexTile>> trackLocations = new List<List<HexTile>>();

    List<List<Interactable>> interactablesList = new List<List<Interactable>>();    
    List<List<CharacterController>> npcControllers = new List<List<CharacterController>>();   
    List<List<CharacterController>> enemyControllers = new List<List<CharacterController>>();   

    List<List<LightNoise>> lightScripts = new List<List<LightNoise>>();

    // camera detect

    public bool battleTriggered = false;
    public Vector3 battleTargetCameraPosition;


    public CharacterController player;

    List<int> currentIndexes;
    List<HexTile> currentBattleField;


    WorldObject playerWorldObject;
    HexGrid hexGrid;

    public List<CharacterController> battleCharacters;
    public List<CharacterController> targetSkeletons;
    public List<CharacterController> targetNPCs;

    public List<GameObject> targetSkeletonObjects;
    public List<GameObject> targetNPCsObjects;

    int currentBattleIndex;
    int characterIndex;
    bool moveInitiated;

    public TextMeshProUGUI onScreenText;
    public AnimationCurve curve;
    float deathTimer = 0;

    int currentTurns;
    float currentCooldown;
    bool takeAwayTurns = false;

    public AudioMaster audioMaster;

    // Start is called before the first frame update
    void Start()
    {
        hexGrid = GetComponent<HexGrid>();
    }

    // Update is called once per frame
    void Update()
    {


        ControlObjectsInScene();
        DetectBattle();
        InitialiseBattle();

        if(player.currentHealth > 0)
        {
            Battle();
        }
        else
        {
            DeathText();
        }
        

        FinaliseBattle(false);

        if (!player.combat)
        {
            List<int> indexes = player.worldObject.recentTile.chamberIndexes;
            if(indexes.Count == 1)
            {
                if (indexes[0] == 0)
                {
                    audioMaster.playerInVillage = Time.time + 0.1f;
                }


                List<Interactable> targetInteracts = interactablesList[indexes[0]];
                for(int i = 0; i < targetInteracts.Count; i++)
                {
                    targetInteracts[i].Activate();
                }
            }

        }

    }

    void DeathText()
    {
        onScreenText.text = "Death";
        deathTimer += Time.deltaTime;
        float curveEvaluate = curve.Evaluate(deathTimer);
        Color startColour = new Color(1, 1, 1, 0);
        Color endColour = new Color(1, 1, 1, 1);
        Color targetColour = Color.Lerp(startColour, endColour, curveEvaluate);
        onScreenText.color = targetColour;
    }

    void DetectBattle()
    {
        if(!battleTriggered)
        {
            currentBattleIndex = -1;
            
            for (int i = 0; i < trackLocations.Count; i++)
            {
                List<HexTile> targetList = trackLocations[i];
                for (int ii = 0; ii < targetList.Count; ii++)
                {
                    if (targetList[ii].CheckPlayerAtLocation())
                    {
                        currentBattleIndex = i;
                    }
                }
            }
        }

    }

    void InitialiseBattle()
    {

        if (currentBattleIndex != -1 && !battleTriggered)
        {
            
            targetSkeletons = enemyControllers[currentBattleIndex];

            if (targetSkeletons.Count != 0)
            {
                
                battleTriggered = true;
                player.Interrupt();
                player.combat = true;
                currentTurns = 2;



                hexGrid.InitialiseChamberTunnelCombat(currentBattleIndex, true);
                hexGrid.DisableAccessToChambers(currentBattleIndex);
                currentBattleField = hexGrid.RequestChamber(currentBattleIndex);

                // calculate camera
                Vector3 calculateCamera = Vector3.zero;
                for (int i = 0; i < currentBattleField.Count; i++)
                {
                    currentBattleField[i].combatInitiated = true;

                    if (i == 0)
                    {
                        calculateCamera = currentBattleField[i].transform.position;
                    }
                    else
                    {
                        calculateCamera += currentBattleField[i].transform.position;
                    }

                    battleTargetCameraPosition = calculateCamera / currentBattleField.Count;
                }


                characterIndex = -1;
                moveInitiated = false;
                takeAwayTurns = false;


                battleCharacters = new List<CharacterController>();
                battleCharacters.Add(player);

                targetNPCs = npcControllers[currentBattleIndex];
                targetNPCs.Add(player);

                targetNPCsObjects = new List<GameObject>();
                targetNPCsObjects.Add(player.gameObject);

                targetSkeletonObjects = new List<GameObject>();




                for (int i = 0; i < targetSkeletons.Count; i++)
                {
                    targetSkeletons[i].combat = true;
                    targetSkeletonObjects.Add(targetSkeletons[i].gameObject);
                    targetSkeletons[i].moveAllowed = false;

                    battleCharacters.Add(targetSkeletons[i]);
                }
    
                for(int i = 0; i < targetNPCs.Count; i++)
                {
                    targetNPCs[i].combat = true;
                    targetNPCs[i].nearbyControllers = targetSkeletons;
                    targetNPCs[i].nearbyControllerObjects = targetSkeletonObjects;
                    targetNPCs[i].moveAllowed = false;

                    battleCharacters.Add(targetNPCs[i]);
                    
                }
                
                for (int i = 0; i < targetSkeletons.Count; i++)
                {
                    
                    targetSkeletons[i].nearbyControllers = targetNPCs;
                }
            }

        }
    }



    void Battle()
    {
        if (battleTriggered)
        {
            audioMaster.playerInBattle = Time.time + 0.1f;


            if (!moveInitiated)
            {
                characterIndex++;
                if (characterIndex == battleCharacters.Count - 1)
                {
                    characterIndex = 0;
                }

                if (battleCharacters[characterIndex].currentHealth > 0)
                {
                    battleCharacters[characterIndex].currentTurns = 2;
                    battleCharacters[characterIndex].moveComplete = false;

                    battleCharacters[characterIndex].interruptCooldown = Time.time + 1;
                    battleCharacters[characterIndex].ui.UpdateTurns(2);

                    moveInitiated = true;
                    currentTurns = 2;
                    currentCooldown = Time.time;
                }
            }
            else
            {

                if(battleCharacters[characterIndex].player)
                {
                    if(battleCharacters[characterIndex].moveAllowed)
                    {
                        for (int i = 0; i < battleCharacters.Count; i++)
                        {
                            battleCharacters[i].shaderTime = Time.time + 0.1f;
                            if (battleCharacters[characterIndex] == battleCharacters[i])
                            {
                                battleCharacters[i].selectedTime = Time.time + 0.1f;

                            }
                        }
                    }
                }
                else
                {
                    if(currentCooldown != -1)
                    {
                        for (int i = 0; i < battleCharacters.Count; i++)
                        {
                            battleCharacters[i].shaderTime = Time.time + 0.1f;
                            if (battleCharacters[characterIndex] == battleCharacters[i])
                            {
                                battleCharacters[i].selectedTime = Time.time + 0.1f;

                            }
                        }
                    }

                }



                if (currentTurns != 0 && currentCooldown != -1)
                {


                    if (Time.time > currentCooldown + 1.3f)
                    {
                        currentCooldown = -1;
                        battleCharacters[characterIndex].moveAllowed = true;
                        battleCharacters[characterIndex].moveComplete = false;
                        takeAwayTurns = true;
                        battleCharacters[characterIndex].recentCombatTile = null;


                    }

                }


                if (currentTurns == 0)
                {
                    moveInitiated = false;
                }






                if (battleCharacters[characterIndex].moveComplete)
                {

                    if (takeAwayTurns)
                    {
                        takeAwayTurns = false;
                        currentTurns--;
                        currentCooldown = Time.time;


                    }

                }
            }



            if(targetSkeletons.Count == 0)
            {
                FinaliseBattle(true);
            }
        }
        
    }

    void FinaliseBattle(bool battleOver)
    {
        if((battleTriggered && player.worldObject.recentTile.chamberIndexes.Count == 2) || battleOver)
        {
            battleTriggered = false;
            player.currentTurns = 0;
            player.ui.UpdateTurns(0);
            hexGrid.InitialiseChamberTunnelCombat(currentBattleIndex, false);
            hexGrid.EnableAccessToChambers();

            for (int i = 0; i < currentBattleField.Count; i++)
            {
                currentBattleField[i].combatInitiated = false;
            }

            for (int i = 0; i < battleCharacters.Count; i++)
            {
                battleCharacters[i].combat = false;
            }

        }
    }

    void ControlObjectsInScene()
    {
        if (playerWorldObject == null)                                                   // control lights in the scene
        {
            playerWorldObject = player.worldObject;
        }
        else
        {
            currentIndexes = playerWorldObject.recentTile.chamberIndexes;

            for (int i = 0; i < currentIndexes.Count; i++)
            {
                List<LightNoise> lights = lightScripts[currentIndexes[i]];
                for (int ii = 0; ii < lights.Count; ii++)
                {
                    lights[ii].Activate();
                }

                List<CharacterController> enemies = enemyControllers[currentIndexes[i]];
                for (int ii = 0; ii < enemies.Count; ii++)
                {
                    enemies[ii].Activate();
                }

                List<CharacterController> npcs = npcControllers[currentIndexes[i]];
                for (int ii = 0; ii < npcs.Count; ii++)
                {
                    npcs[ii].Activate();
                }

            }
        }
    }

    public void PrepareBattleEssentials(List<CharacterController> enemyControl, List<CharacterController> npcControl, List<HexTile> preventLocationsList, List<HexTile> trackLocationsList, List<LightNoise> lightNoiseList, List<Interactable> interactList)
    {
        npcControllers.Add(npcControl);
        enemyControllers.Add(enemyControl);
        preventLocations.Add(preventLocationsList);
        trackLocations.Add(trackLocationsList);
        lightScripts.Add(lightNoiseList);
        interactablesList.Add(interactList);
    }

}


