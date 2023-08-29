using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public AnimationCurve curve;

    List<HexTile> preventLocations = new List<HexTile>();
    List<HexTile> trackLocations = new List<HexTile>();


    List<int> coreLocations = new List<int>() { 1,3,4,5,6,7,8,9,10,11,13};
    List<List<Vector2>> chamberCoordinates = new List<List<Vector2>>();

    List<Vector2> zeroZero = new List<Vector2>()
    {
        new Vector2(0,0), new Vector2(0,1), new Vector2(0,2),
        new Vector2(1,0), new Vector2(1,1), new Vector2(1,2),
        new Vector2(2,-1), new Vector2(2,0), new Vector2(2,1)
    };

    List<Vector2> oneZero = new List<Vector2>
    {
        new Vector2(3,-1), new Vector2(3,0), new Vector2(3,1),
        new Vector2(4,-2), new Vector2(4,-1), new Vector2(4,0)
    };

    List<Vector2> twoZero = new List<Vector2>
    {
        new Vector2(5,-2), new Vector2(5,-1), new Vector2(5,0),
        new Vector2(6,-3), new Vector2(6,-2), new Vector2(6,-1),
        new Vector2(7,-3), new Vector2(7,-2), new Vector2(7,-1)
    };

    List<Vector2> threeZero = new List<Vector2>
    {
        new Vector2(8,-4), new Vector2(8, -3), new Vector2(8,-2),
        new Vector2(9,-4), new Vector2(9,-3), new Vector2(9,-2)
    };


    List<Vector2> fourZero = new List<Vector2>
    {
        new Vector2(10,-5), new Vector2(10,-4), new Vector2(10,-3),
        new Vector2(11,-5), new Vector2(11,-4), new Vector2(11,-3),
        new Vector2(12,-6), new Vector2(12,-5), new Vector2(12,-4)
    };

                                                                                   // procedural generation essentials
                                                                                   // small
    public List<GameObject> rocks = new List<GameObject>();                        // 0
    public List<GameObject> caveScatter = new List<GameObject>();                  // 1
    public List<GameObject> smallFires = new List<GameObject>();                   // 2
    public List<GameObject> smallRedCrystals = new List<GameObject>();             // 3
    public List<GameObject> smallBlueCrystals = new List<GameObject>();            // 4
    public List<GameObject> smallIrons = new List<GameObject>();                   // 5
    public List<GameObject> smallGolds = new List<GameObject>();                   // 6


                                                                                   // big
    public List<GameObject> bigRocks = new List<GameObject>();                     // 0
    public List<GameObject> bigFires = new List<GameObject>();                     // 1
    public List<GameObject> tents = new List<GameObject>();                        // 2
    public List<GameObject> bigRedCrystals = new List<GameObject>();               // 3
    public List<GameObject> bigGolds = new List<GameObject>();                     // 4
    public List<GameObject> bigIrons = new List<GameObject>();                     // 5
    public List<GameObject> bigBlueCrystals = new List<GameObject>();              // 6

    public GameObject playerObject;
    public GameObject skeleton;
    public GameObject npc;

    public List<GameObject> interactableWater = new List<GameObject>();
    public GameObject interactableChest;


    public Camera playerCamera;
    public GameObject playerCameraObject;
    public CameraController camController;

    public FindPath findPath;
    public HexTile playerCurrentTile;

    List<HexTile> hexTiles = new List<HexTile>();                                       // surrounding environment
    List<EnvironmentHexTile> envTilesOne = new List<EnvironmentHexTile>();
    List<EnvironmentHexTile> envTilesTwo = new List<EnvironmentHexTile>();
    List<EnvironmentHexTile> envTilesThree = new List<EnvironmentHexTile>();
    List<EnvironmentHexTile> envTilesFour = new List<EnvironmentHexTile>();
    List<EnvironmentHexTile> envTilesFive = new List<EnvironmentHexTile>();
    List<EnvironmentHexTile> envTilesSix = new List<EnvironmentHexTile>();
    List<EnvironmentHexTile> envTilesSeven = new List<EnvironmentHexTile>();

    public List<List<HexTile>> chambers = new List<List<HexTile>>();
    HexTile startTile;

    List<HexTile> tunnelTiles = new List<HexTile>();

    List<Vector2> availableForceCoordinates = new List<Vector2> { new Vector2(-1, 1), new Vector2(1, -1), new Vector2(1, 1), new Vector2(-1, -1) };
    List<int> availableForcePick = new List<int> { 0, 1, 2, 3 };

    List<int> chamberType = new List<int>() { 0 };

    // 0 - camp / 1 - start location
    // 2 - area one regular / 3 - area one special
    // 4 - area one regular / 5 - area two special
    // 6 - area three regular / 7 - area four special

    public GameObject tile;
    public GameObject targetLocation;


    public Transform hexParent;
    public Transform environmentParent; 
    
    public BattleAreas battleAreas;
    

    // generation 

    Vector2 calculateCoordinate;
    Vector2 startCoordinate;

    HexTile targetTileOne;
    HexTile targetTileTwo;
    HexTile targetTileThree;
    HexTile targetTileFour;

    List<Vector2> selectedChamberCoordinates;
    List<Vector2> theoreticalChamberCoordinates;

    List<HexTile> listHexTiles;
    GameObject temp;

    int selectedChamberType;
    int random;


    public GameObject sceneLight;
    public GameObject emptyObject;
    GameObject chamberParent;
    public GameObject healthInteractable;
    public QuestManager questManager;
    public Transform chambersParentTransform;

    public CharacterController playerCharacter;


    List<CharacterController> npcControl = new List<CharacterController>();
    List<CharacterController> enemyControl = new List<CharacterController>();
    List<Interactable> interactables = new List<Interactable>();
    List<LightNoise> lightNoiseList = new List<LightNoise>();

  //  List<Interactable> allIngameInteractables = new List<Interactable>();

    public GameObject interactableNPC;
    int interactIDCounter = 0;

    List<HexTile> questOneGuardTiles = new List<HexTile>();
    List<HexTile> questTwoGuardTiles = new List<HexTile>();
    List<HexTile> questThreeGuardTiles = new List<HexTile>();

    public Transform guards;


    void Start()
    {
        InitialiseGame();
    }


    void InitialiseGame()
    {

        sceneLight.SetActive(false);

        findPath = GetComponent<FindPath>();
        battleAreas = GetComponent<BattleAreas>();

        chamberCoordinates.Add(zeroZero);
        chamberCoordinates.Add(InitialiseChamberLocations(chamberCoordinates[chamberCoordinates.Count - 1]));
        chamberCoordinates.Add(InitialiseChamberLocations(chamberCoordinates[chamberCoordinates.Count - 1]));

        chamberCoordinates.Add(oneZero);
        chamberCoordinates.Add(InitialiseChamberLocations(chamberCoordinates[chamberCoordinates.Count - 1]));
        chamberCoordinates.Add(InitialiseChamberLocations(chamberCoordinates[chamberCoordinates.Count - 1]));

        chamberCoordinates.Add(twoZero);
        chamberCoordinates.Add(InitialiseChamberLocations(chamberCoordinates[chamberCoordinates.Count - 1]));
        chamberCoordinates.Add(InitialiseChamberLocations(chamberCoordinates[chamberCoordinates.Count - 1]));

        chamberCoordinates.Add(threeZero);
        chamberCoordinates.Add(InitialiseChamberLocations(chamberCoordinates[chamberCoordinates.Count - 1]));
        chamberCoordinates.Add(InitialiseChamberLocations(chamberCoordinates[chamberCoordinates.Count - 1]));

        chamberCoordinates.Add(fourZero);
        chamberCoordinates.Add(InitialiseChamberLocations(chamberCoordinates[chamberCoordinates.Count - 1]));
        chamberCoordinates.Add(InitialiseChamberLocations(chamberCoordinates[chamberCoordinates.Count - 1]));

        SpawnCave(0);
        SpawnCave(1);
        SpawnCave(2);

        startTile.InitialiseCoordinates(new Vector2(0, 0));

 


     //   InitialiseEnvironmentTiles();
     //   envTilesTwo = InitialiseFurtherEnvironmentTiles(envTilesOne, 1.5f);
     //   envTilesThree = InitialiseFurtherEnvironmentTiles(envTilesTwo, 2);

      //  envTilesFour = InitialiseFurtherEnvironmentTiles(envTilesThree, 2.5f);
       // envTilesFive = InitialiseFurtherEnvironmentTiles(envTilesFour, 3);
      //  envTilesSix = InitialiseFurtherEnvironmentTiles(envTilesFive, 3.5f);
     //   envTilesSeven = InitialiseFurtherEnvironmentTiles(envTilesSix, 5);
     //   InitialiseFurtherEnvironmentTiles(envTilesSeven, 10);

        CompleteCaveChambers();

      //  InitialiseIndexes();
      //  InitialiseGuards();
     


    }

    void Update()
    {
        for (int i = 0; i < hexTiles.Count; i++)
        {
            hexTiles[i].UpdateTile();
        }

    }

    void InitialiseGuards()
    {
        for (int i = 0; i < hexTiles.Count; i++)
        {
            if (hexTiles[i].chamberIndexes.Count == 2)
            {
                List<int> chamberIndexes = hexTiles[i].chamberIndexes;
                if (chamberIndexes.Contains(0) && chamberIndexes.Contains(1))
                {
                    if (hexTiles[i].guardPosition)
                    {
                        questOneGuardTiles.Add(hexTiles[i]);
                    }
                }
                else if (chamberIndexes.Contains(0) && chamberIndexes.Contains(7))
                {
                    if (hexTiles[i].guardPosition)
                    {
                        questTwoGuardTiles.Add(hexTiles[i]);
                    }
                }
                else if (chamberIndexes.Contains(0) && chamberIndexes.Contains(13))
                {
                    if (hexTiles[i].guardPosition)
                    {
                        questThreeGuardTiles.Add(hexTiles[i]);
                    }
                }

            }
        }

        for (int i = 0; i < questOneGuardTiles.Count; i++)
        {
            temp = Instantiate(npc, questOneGuardTiles[i].targetLocation.transform.position, Quaternion.identity);
            Vector3 vectorRotation = Vector3.zero;
            vectorRotation.y = Random.Range(0, 360);
            temp.transform.localEulerAngles = vectorRotation;

            temp.GetComponent<WorldObject>().recentTile = questOneGuardTiles[i];
            CharacterController cControl = temp.GetComponent<CharacterController>();
            cControl.InitialiseCharacter(this);

            questManager.questOneGuards.Add(cControl);
            questManager.questOneTiles.Add(questOneGuardTiles[i]);
            temp.transform.SetParent(guards.transform);

            questOneGuardTiles[i].objectAtLocation = temp;

            if (questTwoGuardTiles[i].alternativeFormation)
            {
                if (questTwoGuardTiles[i].topTile)
                {
                    temp.transform.eulerAngles = new Vector3(0, 260, 0);
                }
                else
                {
                    temp.transform.eulerAngles = new Vector3(0, 78, 0);
                }
            }
            else
            {
                if (questTwoGuardTiles[i].topTile)
                {
                    temp.transform.eulerAngles = new Vector3(0, 60, 0);
                }
                else
                {
                    temp.transform.eulerAngles = new Vector3(0, 250, 0);
                }
            }
        }

        for (int i = 0; i < questTwoGuardTiles.Count; i++)
        {
            temp = Instantiate(npc, questTwoGuardTiles[i].targetLocation.transform.position, Quaternion.identity);
            Vector3 vectorRotation = Vector3.zero;
            vectorRotation.y = Random.Range(0, 360);
            temp.transform.localEulerAngles = vectorRotation;

            temp.GetComponent<WorldObject>().recentTile = questOneGuardTiles[i];
            CharacterController cControl = temp.GetComponent<CharacterController>();
            cControl.InitialiseCharacter(this);

            questManager.questTwoGuards.Add(cControl);
            questManager.questTwoTiles.Add(questTwoGuardTiles[i]);
            temp.transform.SetParent(guards.transform);

            questTwoGuardTiles[i].objectAtLocation = temp;

            if (questTwoGuardTiles[i].alternativeFormation)
            {
                if (i == 0)
                {
                    temp.transform.eulerAngles = new Vector3(0, 250, 0);
                }
                else
                {
                    temp.transform.eulerAngles = new Vector3(0, 76, 0);
                }
            }
            else
            {
                if (i == 0)
                {
                    temp.transform.eulerAngles = new Vector3(0, 60, 0);
                }
                else
                {
                    temp.transform.eulerAngles = new Vector3(0, 250, 0);
                }
            }
        }


        for (int i = 0; i < questThreeGuardTiles.Count; i++)
        {
            temp = Instantiate(npc, questThreeGuardTiles[i].targetLocation.transform.position, Quaternion.identity);
            Vector3 vectorRotation = Vector3.zero;
            vectorRotation.y = Random.Range(0, 360);
            temp.transform.localEulerAngles = vectorRotation;

            temp.GetComponent<WorldObject>().recentTile = questOneGuardTiles[i];
            CharacterController cControl = temp.GetComponent<CharacterController>();
            cControl.InitialiseCharacter(this);

            questManager.questThreeGuards.Add(cControl);
            questManager.questThreeTiles.Add(questThreeGuardTiles[i]);
            temp.transform.SetParent(guards.transform);

            questThreeGuardTiles[i].objectAtLocation = temp;

            if (questTwoGuardTiles[i].alternativeFormation)
            {
                if (i == 0)
                {
                    temp.transform.eulerAngles = new Vector3(0, 30, 0);
                }
                else
                {
                    temp.transform.eulerAngles = new Vector3(0, 0, 0);
                }
            }
            else
            {
                if (i == 0)
                {
                    temp.transform.eulerAngles = new Vector3(0, 60, 0);
                }
                else
                {
                    temp.transform.eulerAngles = new Vector3(0, 250, 0);
                }

            }
        }
    }

    List<Vector2> InitialiseChamberLocations(List<Vector2> locations)
    {
        List<Vector2> newLocationsList = new List<Vector2>();
        for (int i = 0; i < locations.Count; i++)
        {
            Vector2 location = locations[i];
            location.y += 3;
            newLocationsList.Add(location);
        }

        return newLocationsList;
    }

    void InitialiseEnvironmentTiles()
    {
        for (int i = 0; i < hexTiles.Count; i++)
        {
            envTilesOne = hexTiles[i].InstantiateEnvironmentTiles(envTilesOne, environmentParent);
        }
    }



    List<EnvironmentHexTile> InitialiseFurtherEnvironmentTiles(List<EnvironmentHexTile> startTiles, float scaleMultiplier)
    {
        List<EnvironmentHexTile> cachedTiles = new List<EnvironmentHexTile> { };
        for (int i = 0; i < startTiles.Count; i++)
        {
            cachedTiles = startTiles[i].InstantiateEnvironmentTiles(cachedTiles, scaleMultiplier, environmentParent);
        }

        return cachedTiles;
    }

    void SpawnCave(int questNumber)
    {
        Vector2 currentPosition = Vector2.zero;
        List<Vector2> occupiedLocations = new List<Vector2> { new Vector2(0, 0) };
        List<Vector2> startCoordinates = new List<Vector2>() { new Vector2(0, 0) };

        List<Vector2> forceCoordinates = new List<Vector2>();
        int chambersNumber = -1;
        int sideChambers = -1;
        int forcePick = -1;

        int regularMarker = -1;
        int specialMarker = -1;


        int forceCoordinateRandom = Random.Range(0, availableForceCoordinates.Count);

        forceCoordinates.Add(availableForceCoordinates[forceCoordinateRandom]);
        forceCoordinates.Add(availableForceCoordinates[forceCoordinateRandom]);
        availableForceCoordinates.RemoveAt(forceCoordinateRandom);

        forcePick = availableForcePick[forceCoordinateRandom];
        availableForcePick.RemoveAt(forceCoordinateRandom);

        if (questNumber == 0)
        {
            chambersNumber = 3;
            sideChambers = 3;

            regularMarker = 2;
            specialMarker = 3;
        }
        else if (questNumber == 1)
        {
            chambersNumber = 3;
            sideChambers = 3;

            regularMarker = 4;
            specialMarker = 5;
        }
        else if (questNumber == 2)
        {
            chambersNumber = 3;
            sideChambers = 3;

            regularMarker = 6;
            specialMarker = 7;
        }


        if (questNumber == 0)
        {
            chambers.Add(InstantiateTiles(currentPosition, new Vector2(6, 8)));
        }

        List<Vector2> previousDirectionChoices = new List<Vector2>();


        while (chambersNumber != 0)                                                                     // main chambers
        {
            chambersNumber--;
            int listChoice = occupiedLocations.Count;
            bool taskComplete = false;

            while (!taskComplete)
            {
                listChoice--;

                List<Vector2> possibleDirections = new List<Vector2>()
                {
                    new Vector2(-1,1), new Vector2(1,-1), new Vector2(1,1), new Vector2(-1,-1)
                };

                bool complete = false;
                while (!complete)
                {
                    if (possibleDirections.Count == 0)
                    {
                        complete = true;
                        previousDirectionChoices.Clear();
                    }
                    else
                    {
                        if (previousDirectionChoices.Count == 2)
                        {
                            if (previousDirectionChoices[0] == previousDirectionChoices[1])
                            {
                                possibleDirections.Remove(previousDirectionChoices[0]);
                            }
                        }

                        Vector2 current = occupiedLocations[listChoice];
                        int pick = -1;
                        Vector2 target = Vector2.zero;
                        if (forceCoordinates.Count != 0)
                        {
                            target = current + forceCoordinates[0];
                            forceCoordinates.RemoveAt(0);
                            pick = forcePick;
                        }
                        else
                        {
                            pick = Random.Range(0, possibleDirections.Count);
                            target = current + possibleDirections[pick];
                        }


                        if (occupiedLocations.Contains(target) || target.y == 0 || target.x == 0)
                        {
                            possibleDirections.RemoveAt(pick);
                        }
                        else
                        {
                            occupiedLocations.Add(target);
                            complete = true;
                            taskComplete = true;
                            currentPosition = startCoordinates[listChoice];

                            if (possibleDirections[pick].y == -1 & possibleDirections[pick].x == 1)
                            {
                                currentPosition.x += possibleDirections[pick].x * 6;
                                currentPosition.x += 1;
                                currentPosition.y -= 1;
                                currentPosition.y += possibleDirections[pick].y * 8;
                                currentPosition.x += 1;
                                currentPosition.y -= 1;
                                currentPosition.x += 1;
                            }
                            else if (possibleDirections[pick].y == 1 & possibleDirections[pick].x == -1)
                            {
                                currentPosition.y += possibleDirections[pick].y * 8;
                                currentPosition.x -= 1;
                                currentPosition.x -= 1;
                                currentPosition.y += 1;
                                currentPosition.x += possibleDirections[pick].x * 6;

                                currentPosition.y += 1;
                                currentPosition.x -= 1;
                            }
                            else if (possibleDirections[pick].y == 1 & possibleDirections[pick].x == 1)
                            {
                                currentPosition.x += possibleDirections[pick].x * 6;
                                currentPosition.y += possibleDirections[pick].y * 7;
                                currentPosition.x += 1;
                                currentPosition.y += 1;
                                currentPosition.x += 1;
                                currentPosition.y += 1;

                                currentPosition.x += 1;
                                currentPosition.y += 1;
                            }
                            else                                                         // -1, -1
                            {
                                currentPosition.x -= 1;
                                currentPosition.y -= 1;
                                currentPosition.x -= 1;
                                currentPosition.y -= 1;
                                currentPosition.x += possibleDirections[pick].x * 6;
                                currentPosition.y += possibleDirections[pick].y * 7;

                                currentPosition.x -= 1;
                                currentPosition.y -= 1;
                            }

                            chambers.Add(InstantiateTiles(currentPosition, new Vector2(6, 8)));
                            startCoordinates.Add(currentPosition);

                            if (questNumber == 0 && chambersNumber == 0)
                            {
                                chamberType.Add(1);
                            }
                            else
                            {
                                chamberType.Add(regularMarker);
                            }

                            if (previousDirectionChoices.Count == 2)
                            {
                                previousDirectionChoices.RemoveAt(0);
                            }

                            previousDirectionChoices.Add(possibleDirections[pick]);
                        }
                    }
                }
            }
        }

        // side chambers
        List<Vector2> occupiedLocationsTemp = new List<Vector2>(occupiedLocations);
        List<Vector2> startCoordinatesTemp = new List<Vector2>(startCoordinates);

        while (sideChambers != 0)
        {
            sideChambers--;
            bool sideChamberSpawned = false;
            int spawned = 0;
            while (!sideChamberSpawned)
            {
                int random = Random.Range(2, occupiedLocationsTemp.Count - spawned);
                List<Vector2> possibleDirections = new List<Vector2>()
                {
                   new Vector2(-1,1), new Vector2(1,-1), new Vector2(1,1), new Vector2(-1,-1)
                };

                bool complete = false;
                while (!complete)
                {

                    if (possibleDirections.Count == 0)
                    {
                        complete = true;
                        occupiedLocationsTemp.RemoveAt(random);
                        startCoordinatesTemp.RemoveAt(random);
                    }
                    else
                    {
                        Vector2 current = occupiedLocationsTemp[random];
                        int pick = Random.Range(0, possibleDirections.Count);
                        Vector2 target = current + possibleDirections[pick];

                        if (occupiedLocations.Contains(target) || target.y == 0 || target.x == 0)
                        {
                            possibleDirections.RemoveAt(pick);
                        }
                        else
                        {
                            spawned += 1;
                            occupiedLocations.Add(target);
                            complete = true;
                            sideChamberSpawned = true;
                            currentPosition = startCoordinatesTemp[random];

                            if (possibleDirections[pick].y == -1 & possibleDirections[pick].x == 1)
                            {
                                currentPosition.x += possibleDirections[pick].x * 6;
                                currentPosition.x += 1;
                                currentPosition.y -= 1;
                                currentPosition.y += possibleDirections[pick].y * 8;
                                currentPosition.x += 1;
                                currentPosition.y -= 1;
                                currentPosition.x += 1;
                            }
                            else if (possibleDirections[pick].y == 1 & possibleDirections[pick].x == -1)
                            {
                                currentPosition.y += possibleDirections[pick].y * 8;
                                currentPosition.x -= 1;
                                currentPosition.x -= 1;
                                currentPosition.y += 1;
                                currentPosition.x += possibleDirections[pick].x * 6;

                                currentPosition.y += 1;
                                currentPosition.x -= 1;
                            }
                            else if (possibleDirections[pick].y == 1 & possibleDirections[pick].x == 1)
                            {
                                currentPosition.x += possibleDirections[pick].x * 6;
                                currentPosition.y += possibleDirections[pick].y * 7;
                                currentPosition.x += 1;
                                currentPosition.y += 1;
                                currentPosition.x += 1;
                                currentPosition.y += 1;

                                currentPosition.x += 1;
                                currentPosition.y += 1;
                            }
                            else                                                         // -1, -1
                            {
                                currentPosition.x -= 1;
                                currentPosition.y -= 1;
                                currentPosition.x -= 1;
                                currentPosition.y -= 1;
                                currentPosition.x += possibleDirections[pick].x * 6;
                                currentPosition.y += possibleDirections[pick].y * 7;

                                currentPosition.x -= 1;
                                currentPosition.y -= 1;
                            }

                            chambers.Add(InstantiateTiles(currentPosition, new Vector2(6, 8)));
                            startCoordinates.Add(currentPosition);
                            chamberType.Add(specialMarker);
                        }
                    }
                }
            }
        }



        for (int i = 0; i < occupiedLocations.Count; i++)                                       // tunnels
        {
            Vector2 currentLocation = occupiedLocations[i];
            Vector2 targetLocation = currentLocation;
            targetLocation.x += 1;
            targetLocation.y += 1;

            Vector2 calculateTarget;
            if (occupiedLocations.Contains(targetLocation))
            {
                calculateTarget = startCoordinates[i];
                calculateTarget.x += 7;
                calculateTarget.y += 7;
                InstantiateTunnel(calculateTarget, false);
            }

            targetLocation.y -= 2;
            if (occupiedLocations.Contains(targetLocation))
            {
                calculateTarget = startCoordinates[i];
                calculateTarget.x += 7;
                InstantiateTunnel(calculateTarget, true);
            }
        }
    }


    void CompleteCaveChambers()
    {
        for (int i = 0; i < chambers.Count; i++)
        {
            chamberParent = Instantiate(emptyObject, Vector3.zero, Quaternion.identity);
            chamberParent.name = "Chamber: " + i;
            chamberParent.transform.parent = chambersParentTransform;

            selectedChamberType = chamberType[i];
            listHexTiles = chambers[i];
            startCoordinate = listHexTiles[0].hexCoordinate;

            lightNoiseList = new List<LightNoise>();
            npcControl = new List<CharacterController>();
            enemyControl = new List<CharacterController>();
            interactables = new List<Interactable>();

            preventLocations = new List<HexTile>();
            trackLocations = new List<HexTile>();


            trackLocations.Add(listHexTiles[0]);
            trackLocations.Add(listHexTiles[1]);
            trackLocations.Add(listHexTiles[7]);
            trackLocations.Add(listHexTiles[8]);
            trackLocations.Add(listHexTiles[54]);
            trackLocations.Add(listHexTiles[55]);
            trackLocations.Add(listHexTiles[61]);
            trackLocations.Add(listHexTiles[62]);


            if (selectedChamberType == 0)
            {
                Camp();
            }
            else if (selectedChamberType == 1)
            {
                StartLocation();
            }
            else if (selectedChamberType == 2)
            {
                AreaOneRegular();
            }
            else if (selectedChamberType == 3)
            {
                AreaOneSpecial();
            }
            else if (selectedChamberType == 4)
            {
                AreaTwoRegular();
            }
            else if (selectedChamberType == 5)
            {
                AreaTwoSpecial();
            }
            else if (selectedChamberType == 6)
            {
                AreaThreeRegular();
            }
            else if (selectedChamberType == 7)
            {
                AreaThreeSpecial();
            }

            battleAreas.PrepareBattleEssentials(enemyControl, npcControl, preventLocations, trackLocations, lightNoiseList, interactables);
        }
    }

    GameObject SelectSmallObject(int listSelector)
    {
        List<GameObject> targetList = new List<GameObject>();

        if (listSelector == 0)
        {
            targetList = rocks;
        }
        else if (listSelector == 1)
        {
            targetList = caveScatter;
        }
        else if (listSelector == 2)
        {
            targetList = smallFires;
        }
        else if (listSelector == 3)
        {
            targetList = smallRedCrystals;
        }
        else if (listSelector == 4)
        {
            targetList = smallBlueCrystals;
        }
        else if (listSelector == 5)
        {
            targetList = smallIrons;
        }
        else                                //if 6
        {
            targetList = smallGolds;
        }

        random = Random.Range(0, targetList.Count);
        return targetList[random];

    }

    GameObject SelectBigObject(int listSelector)
    {
        List<GameObject> targetList = new List<GameObject>();

        if (listSelector == 0)
        {
            targetList = bigRocks;
        }
        else if (listSelector == 1)
        {
            targetList = bigFires;
        }
        else if (listSelector == 2)
        {
            targetList = tents;
        }
        else if (listSelector == 3)
        {
            targetList = bigRedCrystals;
        }
        else if (listSelector == 4)
        {
            targetList = bigGolds;
        }
        else if (listSelector == 5)
        {
            targetList = bigIrons;
        }
        else                         // if 6
        {
            targetList = bigBlueCrystals;
        }


        random = Random.Range(0, targetList.Count);
        return targetList[random];
    }

    void SpawnRegularArea(List<GameObject> additionals, int bigObject, List<int> smallObjects)
    {
        List<int> availableCoreLocations = new(coreLocations);
        List<int> availableBigLocations = new List<int> { 1, 6, 7, 8, 13 };

        List<int> chosenLightLocations = new List<int>();
        List<int> chosenBigLocations = new List<int>();
        List<int> additionalLocations = new List<int>();

        chosenBigLocations.Add(availableBigLocations[Random.Range(0, availableBigLocations.Count)]);
        availableBigLocations.Remove(chosenBigLocations[0]);
        availableCoreLocations.Remove(chosenBigLocations[0]);

        chosenBigLocations.Add(availableBigLocations[Random.Range(0, availableBigLocations.Count)]);
        availableBigLocations.Remove(chosenBigLocations[1]);
        availableCoreLocations.Remove(chosenBigLocations[1]);

        int healthPosition = availableCoreLocations[Random.Range(0, availableCoreLocations.Count)];
        availableCoreLocations.Remove(healthPosition);
        availableBigLocations.Remove(healthPosition);

        while (additionalLocations.Count != additionals.Count)
        {
            additionalLocations.Add(availableCoreLocations[Random.Range(0, availableCoreLocations.Count)]);
            availableCoreLocations.Remove(additionalLocations[additionalLocations.Count - 1]);
            availableBigLocations.Remove(additionalLocations[additionalLocations.Count - 1]);
        }

        while(chosenLightLocations.Count != 2)
        {
            chosenLightLocations.Add(availableCoreLocations[Random.Range(0, availableCoreLocations.Count)]);
            availableCoreLocations.Remove(chosenLightLocations[chosenLightLocations.Count - 1]);
            availableBigLocations.Remove(chosenLightLocations[chosenLightLocations.Count - 1]);
        }


        //  if(availableBigLocations.Contains(healthPosition))
        //   {
        //  availableBigLocations.Remove(healthPosition);
        //   }

        //   chosenLightLocations.Add(availableCoreLocations[Random.Range(0, availableCoreLocations.Count)]);
        //   availableCoreLocations.Remove(chosenLightLocations[0]);
        //    if (availableBigLocations.Contains(chosenLightLocations[0]))
        //  {
        //   availableBigLocations.Remove(chosenLightLocations[0]);


        //    chosenLightLocations.Add(availableCoreLocations[Random.Range(0, availableCoreLocations.Count)]);
        //    availableCoreLocations.Remove(chosenLightLocations[1]);
        //    availableBigLocations.Remove(chosenLightLocations[1]);



        for (int i = 0; i < chamberCoordinates.Count; i++)
        {
            selectedChamberCoordinates = chamberCoordinates[i];
            theoreticalChamberCoordinates = new(chamberCoordinates[i]);
            List<GameObject> targetObjects = new List<GameObject>();
            if(healthPosition == i)
            {
                targetObjects.Add(healthInteractable);
                GenerateSmallLocation(targetObjects);
            }
            else if (availableBigLocations.Contains(i))
            {
                targetObjects.Add(SelectSmallObject(smallObjects[Random.Range(0, smallObjects.Count)]));
                targetObjects.Add(SelectSmallObject(smallObjects[Random.Range(0, smallObjects.Count)]));
                GenerateSmallLocation(targetObjects);
            }
            else if(chosenBigLocations.Contains(i))
            {
                GenerateBigLocation(i, SelectBigObject(bigObject), targetObjects);
            }
            else if (additionalLocations.Contains(i))
            {
                targetObjects.Add(additionals[0]);
                additionals.RemoveAt(0);
                GenerateSmallLocation(targetObjects);
            }
            else if(chosenLightLocations.Contains(i))
            {
                targetObjects.Add(SelectSmallObject(2));
                GenerateSmallLocation(targetObjects);
            }
            else if(availableCoreLocations.Contains(i))
            {
                targetObjects.Add(SelectSmallObject(smallObjects[Random.Range(0, smallObjects.Count)]));
                GenerateSmallLocation(targetObjects);
            }
        }
    }
    /*
    else if (chosenBigLocations.Contains(i))
    {
        GenerateBigLocation(i, SelectBigObject(0), targetObjects);
    }
    else if (additionalLocations.Contains(i))
    {
        targetObjects.Add(additionals[0]);
        additionals.RemoveAt(0);
    }
    else if(chosenLightLocations.Contains(i))
    {
        targetObjects.Add(SelectSmallObject(2));
        GenerateSmallLocation(targetObjects);
    }
    else                //if (availableCoreLocations.Contains(i))
    {
        targetObjects.Add(SelectSmallObject(smallObjects[Random.Range(0, smallObjects.Count)]));
        GenerateSmallLocation(targetObjects);
    }
}
}
    */

    /*
    List<int> availableLightLocations = new(coreLocations);
    List<int> availableBigLocations = new List<int> { 1, 6, 7, 8, 13 };

    List<int> chosenLightLocations = new List<int>();
    List<int> chosenBigLocations = new List<int>();

    int healthPosition = coreLocations[Random.Range(0, coreLocations.Count)];




    List<int> availableAdditionalLocations = new(coreLocations);
    List<int> chosenAdditionalLocations = new List<int>();

    int additionalCounter = additionals.Count;
    while(additionalCounter != 0)
    {
        additionalCounter--;
        random = Random.Range(0, availableAdditionalLocations.Count);
        chosenAdditionalLocations.Add(availableAdditionalLocations[random]);
        availableAdditionalLocations.RemoveAt(random);
    }

    while (chosenLightLocations.Count != 2)
    {
        random = Random.Range(0, availableLightLocations.Count);
        chosenLightLocations.Add(availableLightLocations[random]);
        if (availableBigLocations.Contains(availableLightLocations[random]))
        {
            availableBigLocations.Remove(availableLightLocations[random]);
        }
        availableLightLocations.RemoveAt(random);
    }

    while (chosenBigLocations.Count != 2)
    {
        random = Random.Range(0, availableBigLocations.Count);
        if (availableBigLocations[random] == healthPosition)
        {
            availableBigLocations.RemoveAt(random);
        }
        else
        {
            chosenBigLocations.Add(availableBigLocations[random]);
            availableBigLocations.RemoveAt(random);
        }
    }


    for (int i = 0; i < chamberCoordinates.Count; i++)
    {
        selectedChamberCoordinates = chamberCoordinates[i];
        theoreticalChamberCoordinates = new(chamberCoordinates[i]);
        List<GameObject> targetObjects = new List<GameObject>();

        if(healthPosition == i)
        {
            targetObjects.Add(healthInteractable);
        }
        if(chosenAdditionalLocations.Contains(i))
        {
            targetObjects.Add(additionals[additionalCounter]);
            additionalCounter++;
        }

        if (chosenLightLocations.Contains(i))
        {
            targetObjects.Add(SelectSmallObject(2));
            targetObjects.Add(SelectSmallObject(smallObjects[Random.Range(0, smallObjects.Count)]));
            GenerateSmallLocation(targetObjects);
        }
        else if (chosenBigLocations.Contains(i))
        {
            GenerateBigLocation(i, SelectBigObject(0), targetObjects);
        }
        else if (availableLightLocations.Contains(bigObject))
        {
            targetObjects.Add(SelectSmallObject(smallObjects[Random.Range(0, smallObjects.Count)]));
            targetObjects.Add(SelectSmallObject(smallObjects[Random.Range(0, smallObjects.Count)]));
            GenerateSmallLocation(targetObjects);
        }
        else
        {
       //     Debug.Log(targetObjects.Count + " : " + i);
            targetObjects.Add(SelectSmallObject(smallObjects[Random.Range(0, smallObjects.Count)]));
            GenerateSmallLocation(targetObjects);
        }
    }
}

    */

    void SpawnSpecialArea(List<GameObject> additionals, int bigObject, List<int> smallObjects, GameObject additional)
    {
        List<int> availableBigLocations = new List<int> { 1, 6, 7, 8, 13 };
        List<int> availableCoreLocations = new(coreLocations);
        List<int> availableCentralAreas = new List<int> { 4, 7, 10 };

        List<int> chosenBigLocations = new List<int>();
        List<int> additionalLocations = new List<int>();
        int chosenCentralArea;

        chosenBigLocations.Add(availableBigLocations[Random.Range(0, availableBigLocations.Count)]);
        availableBigLocations.Remove(chosenBigLocations[0]);
        availableCoreLocations.Remove(chosenBigLocations[0]);
        availableCentralAreas.Remove(chosenBigLocations[0]);

        chosenBigLocations.Add(availableBigLocations[Random.Range(0, availableBigLocations.Count)]);
        availableBigLocations.Remove(chosenBigLocations[1]);
        availableCoreLocations.Remove(chosenBigLocations[1]);
        availableCentralAreas.Remove(chosenBigLocations[1]);

        chosenCentralArea = availableCentralAreas[Random.Range(0, availableCentralAreas.Count)];
        availableBigLocations.Remove(chosenCentralArea);
        availableCoreLocations.Remove(chosenCentralArea);

        while (additionalLocations.Count != additionals.Count)
        {
            additionalLocations.Add(availableCoreLocations[Random.Range(0, availableCoreLocations.Count)]);
            availableCoreLocations.Remove(additionalLocations[additionalLocations.Count - 1]);
            availableBigLocations.Remove(additionalLocations[additionalLocations.Count - 1]);
        }

        for (int i = 0; i < chamberCoordinates.Count; i++)
        {
            selectedChamberCoordinates = chamberCoordinates[i];
            theoreticalChamberCoordinates = new(chamberCoordinates[i]);
            List<GameObject> targetObjects = new List<GameObject>();

            if (availableBigLocations.Contains(i))
            {
                targetObjects.Add(SelectSmallObject(smallObjects[Random.Range(0, smallObjects.Count)]));
                GenerateSmallLocation(targetObjects);
            }
            else if (chosenBigLocations.Contains(i))
            {
                GenerateBigLocation(i, SelectBigObject(bigObject), targetObjects);
            }
            else if (additionalLocations.Contains(i))
            {
                targetObjects.Add(additionals[0]);
                additionals.RemoveAt(0);
                GenerateSmallLocation(targetObjects);
            }
            else if (availableCoreLocations.Contains(i))
            {
                targetObjects.Add(SelectSmallObject(smallObjects[Random.Range(0, smallObjects.Count)]));
                GenerateSmallLocation(targetObjects);
            }
            else if(chosenCentralArea == i)
            {
                targetObjects.Add(additional);
                GenerateSmallLocation(targetObjects);
            }
        }
    }

        /*
        List<int> availableBigLocations = new List<int> { 1, 6, 7, 8, 13 };
        List<int> chosenBigLocations = new List<int>();

        
        int chosenCentralArea = -1;

        List<int> availableAdditionalLocations = new(coreLocations);
        List<int> chosenAdditionalLocations = new List<int>();
        int additionalCounter = additionals.Count;
        while (additionalCounter != 0)
        {
            additionalCounter--;
            random = Random.Range(0, availableAdditionalLocations.Count);
            chosenAdditionalLocations.Add(availableAdditionalLocations[random]);
            availableAdditionalLocations.RemoveAt(random);
        }

        while (chosenBigLocations.Count != 2)
        {
            random = Random.Range(0, availableBigLocations.Count);
            chosenBigLocations.Add(availableBigLocations[random]);

            if(availableCentralAreas.Contains(availableBigLocations[random]) && additional != null)
            {
                availableCentralAreas.Remove(availableBigLocations[random]);
            }
            availableBigLocations.RemoveAt(random);
        }

        if(additional != null)
        {
            chosenCentralArea = availableCentralAreas[Random.Range(0, availableCentralAreas.Count)];
        }

        for (int i = 0; i < chamberCoordinates.Count; i++)
        {
            selectedChamberCoordinates = chamberCoordinates[i];
            theoreticalChamberCoordinates = new(chamberCoordinates[i]);
            List<GameObject> targetObjects = new List<GameObject>();

            if(chosenCentralArea == i)
            {
                targetObjects.Add(additional);
            }

            if (chosenAdditionalLocations.Contains(i))
            {
                targetObjects.Add(additionals[additionalCounter]);
                additionalCounter++;

            }

            if (chosenBigLocations.Contains(i))
            {
                GenerateBigLocation(i, SelectBigObject(bigObject), targetObjects);
            }
            else if (availableBigLocations.Contains(i))
            {
                targetObjects.Add(SelectSmallObject(smallObjects[Random.Range(0, smallObjects.Count)]));
                targetObjects.Add(SelectSmallObject(smallObjects[Random.Range(0, smallObjects.Count)]));
                targetObjects.Add(SelectSmallObject(smallObjects[Random.Range(0, smallObjects.Count)]));
                GenerateSmallLocation(targetObjects);
            }
            else if (coreLocations.Contains(i))
            {
                targetObjects.Add(SelectSmallObject(smallObjects[Random.Range(0, smallObjects.Count)]));
                targetObjects.Add(SelectSmallObject(smallObjects[Random.Range(0, smallObjects.Count)]));
                GenerateSmallLocation(targetObjects);
            }
            else
            {
                targetObjects.Add(SelectSmallObject(smallObjects[Random.Range(0, smallObjects.Count)]));
                GenerateSmallLocation(targetObjects);
            }
        }


    }
        */


    void StartLocation()
    {
        List<int> availableLightLocations = new List<int> { 4, 7, 10 };
        List<int> availableBigLocations = new List<int>() { 1, 6, 7, 8, 13 };
        List<int> chosenLightLocations = new List<int>();
        List<int> chosenBigLocations = new List<int>();

        random = Random.Range(0, availableLightLocations.Count);
        chosenLightLocations.Add(availableLightLocations[random]);
        if (availableLightLocations[random] == 7)
        {
            availableBigLocations.RemoveAt(2);
        }
        availableLightLocations.RemoveAt(random);

        random = Random.Range(0, availableLightLocations.Count);
        chosenLightLocations.Add(availableLightLocations[random]);
        if (availableLightLocations[random] == 7)
        {
            availableBigLocations.RemoveAt(2);
        }
        availableLightLocations.RemoveAt(random);


        random = Random.Range(0, availableBigLocations.Count);
        chosenBigLocations.Add(availableBigLocations[random]);
        availableBigLocations.RemoveAt(random);

        random = Random.Range(0, availableBigLocations.Count);
        chosenBigLocations.Add(availableBigLocations[random]);
        availableBigLocations.RemoveAt(random);

        bool npcComplete = false;

        for (int i = 0; i < chamberCoordinates.Count; i++)
        {
            selectedChamberCoordinates = chamberCoordinates[i];
            theoreticalChamberCoordinates = new(chamberCoordinates[i]);
            List<GameObject> targetObjects = new List<GameObject>();

            if (chosenLightLocations.Contains(i))
            {
                targetObjects.Add(SelectSmallObject(2));
                if (!npcComplete)
                {
                    npcComplete = true;
                    targetObjects.Add(playerObject);
                    targetObjects.Add(interactableNPC);
                }
                else
                {
                    targetObjects.Add(SelectSmallObject(1));
                    targetObjects.Add(SelectSmallObject(0));
                }

                GenerateSmallLocation(targetObjects);
            }
            else if(chosenBigLocations.Contains(i))
            {
                GenerateBigLocation(i, SelectBigObject(0), targetObjects);
            }
            else if(availableBigLocations.Contains(i))
            {
                targetObjects.Add(SelectSmallObject(1));
                targetObjects.Add(SelectSmallObject(0));
                GenerateSmallLocation(targetObjects);
            }
            else
            {
                targetObjects.Add(SelectSmallObject(Random.Range(0, 2)));
                GenerateSmallLocation(targetObjects);
            }
        }
    }                                       // rocks

    void Camp()
    {
        List<int> availableBigLocations = new List<int> { 1, 6, 7, 8, 13 };
        List<int> fires = new List<int>();
        List<int> tents = new List<int>();

        List<GameObject> objects = new List<GameObject> { npc, interactableNPC };

        while (fires.Count != 2)
        {
            random = Random.Range(0, availableBigLocations.Count);
            fires.Add(availableBigLocations[random]);
            availableBigLocations.RemoveAt(random);
        }

        while (tents.Count != 2)
        {
            random = Random.Range(0, availableBigLocations.Count);
            tents.Add(availableBigLocations[random]);
            availableBigLocations.RemoveAt(random);
        }


        for (int i = 0; i < chamberCoordinates.Count; i++)
        {
            selectedChamberCoordinates = chamberCoordinates[i];
            theoreticalChamberCoordinates = new(chamberCoordinates[i]);
            List<GameObject> targetObjects = new List<GameObject>();

            if (availableBigLocations.Contains(i))
            {
                targetObjects.Add(SelectSmallObject(Random.Range(0, 2)));
              //  targetObjects.Add(SelectSmallObject(Random.Range(0, 2)));
              //  targetObjects.Add(SelectSmallObject(Random.Range(0, 2)));
                GenerateSmallLocation(targetObjects);
            }
            else if (fires.Contains(i))
            {
                targetObjects.Add(objects[0]);
                objects.RemoveAt(0);
                GenerateBigLocation(i, SelectBigObject(1), targetObjects);
            }
            else if (tents.Contains(i))
            {
                GenerateBigLocation(i, SelectBigObject(2), targetObjects);
            }
            else if (coreLocations.Contains(i))
            {
                targetObjects.Add(SelectSmallObject(Random.Range(0, 2)));
            //    targetObjects.Add(SelectSmallObject(Random.Range(0, 2)));
                GenerateSmallLocation(targetObjects);
            }
            else
            {
                targetObjects.Add(SelectSmallObject(Random.Range(0, 2)));
                GenerateSmallLocation(targetObjects);
            }
        }
    }

    void AreaOneRegular()
    {
        SpawnRegularArea(new List<GameObject>() { }, 0, new List<int> { 0, 1, 5 });
    }                         // rocks + iron scatter

    void AreaOneSpecial()
    {
        List<GameObject> objects = new List<GameObject>() {  SelectSmallObject(2), SelectSmallObject(2), skeleton };
        SpawnSpecialArea(objects, 0, new List<int> { 0, 1, 5 }, interactableNPC);
    }                         // rocks + gold scatter                       

    void AreaTwoRegular()                   // iron area                   
    {
        SpawnRegularArea(new List<GameObject>(),5, new List<int> { 0, 1, 5 });
    }

    void AreaTwoSpecial()                   // red crystal area
    {
        List<GameObject> objects = new List<GameObject>() { skeleton };
        SpawnSpecialArea(objects, 3, new List<int> { 0, 1, 3 }, interactableChest);

    }

    void AreaThreeRegular()                 // gold area
    {
        SpawnRegularArea(new List<GameObject>(), 4, new List<int> { 0, 1, 6 });
    }
    void AreaThreeSpecial()                 // blue crystal area           
    {
        List<GameObject> objects = new List<GameObject> { skeleton, skeleton};
        SpawnSpecialArea(objects, 6, new List<int> { 0, 1, 4 }, interactableWater[0]);
        interactableWater.RemoveAt(0);
    }

    void GenerateBigLocation(int chamberArea, GameObject bigObject, List<GameObject> additionalObjects)
    {
        Vector3 calculateVector;
        if (chamberArea == 1 || chamberArea == 13)
        {
            int randomFormation = Random.Range(0, 3);

            if (randomFormation == 2)
            {
                int randomLocation = Random.Range(0, 2);


                calculateCoordinate = startCoordinate + selectedChamberCoordinates[1 + randomLocation];
                targetTileOne = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[3 + randomLocation];
                targetTileTwo = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[4 + randomLocation];
                targetTileThree = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[7 + randomLocation];
                targetTileFour = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateVector = targetTileOne.targetLocation.transform.position
                                           + targetTileTwo.targetLocation.transform.position
                                           + targetTileThree.targetLocation.transform.position
                                           + targetTileFour.targetLocation.transform.position;

                calculateVector = calculateVector / 4;
                calculateVector.y += 10;


                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[1 + randomLocation]);
                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[3 + randomLocation]);
                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[4 + randomLocation]);
                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[7 + randomLocation]);

            }
            else if (randomFormation == 1)
            {
                int randomLocation = Random.Range(0, 3);
                int baseNumber = -1;
                int additionalNumber = 0;

                if (randomLocation == 0)
                {
                    baseNumber = 1;
                }
                else if (randomLocation == 1)
                {
                    baseNumber = 3;
                    additionalNumber = 1;
                }
                else if (randomLocation == 2)
                {
                    baseNumber = 4;
                    additionalNumber = 1;
                }

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[baseNumber];
                targetTileOne = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[baseNumber + 1];
                targetTileTwo = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[baseNumber + 2 + additionalNumber];           //  + 3 + additionalNumber];
                targetTileThree = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[baseNumber + 3 + additionalNumber];         // + 4];
                targetTileFour = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateVector = targetTileOne.targetLocation.transform.position
                        + targetTileTwo.targetLocation.transform.position
                        + targetTileThree.targetLocation.transform.position
                        + targetTileFour.targetLocation.transform.position;

                calculateVector = calculateVector / 4;
                calculateVector.y += 10;


                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[baseNumber]);
                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[baseNumber + 1]);
                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[baseNumber + 2 + additionalNumber]);
                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[baseNumber + 3 + additionalNumber]);

            }
            else
            {
                int randomLocation = Random.Range(0, 3);
                int baseNumber = -1;
                int additionalNumber = 0;

                if (randomLocation == 0)
                {
                    baseNumber = 0;
                }
                else if (randomLocation == 1)
                {
                    baseNumber = 1;
                }
                else
                {
                    baseNumber = 3;
                    additionalNumber = 1;
                }


                calculateCoordinate = startCoordinate + selectedChamberCoordinates[baseNumber];
                targetTileOne = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[baseNumber + 1];
                targetTileTwo = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[baseNumber + 3 + additionalNumber];
                targetTileThree = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[baseNumber + 4 + additionalNumber];
                targetTileFour = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateVector = targetTileOne.targetLocation.transform.position
                        + targetTileTwo.targetLocation.transform.position
                        + targetTileThree.targetLocation.transform.position
                        + targetTileFour.targetLocation.transform.position;

                calculateVector = calculateVector / 4;
                calculateVector.y += 10;



                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[baseNumber]);
                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[baseNumber + 1]);
                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[baseNumber + 3 + additionalNumber]);
                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[baseNumber + 4 + additionalNumber]);
            }

            temp = FinaliseInstantiate(bigObject, calculateVector);

        }
        else
        {

            int randomFormation = Random.Range(0, 3);
            if (randomFormation == 2)
            {
                int randomLocation = Random.Range(0, 2);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[randomLocation];
                targetTileOne = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[3 + randomLocation];
                targetTileTwo = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[4 + randomLocation];
                targetTileThree = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[6 + randomLocation];
                targetTileFour = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateVector = targetTileOne.targetLocation.transform.position
                                           + targetTileTwo.targetLocation.transform.position
                                           + targetTileThree.targetLocation.transform.position
                                           + targetTileFour.targetLocation.transform.position;

                calculateVector = calculateVector / 4;
                calculateVector.y += 10;



                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[randomLocation]);
                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[3 + randomLocation]);
                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[4 + randomLocation]);
                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[6 + randomLocation]);

            }
            else if (randomFormation == 1)
            {
                int randomLocation = Random.Range(0, 3);
                int baseNumber = -1;
                int additionalNumber = 0;

                if (randomLocation == 0)
                {
                    baseNumber = 0;
                }
                else if (randomLocation == 1)
                {
                    baseNumber = 1;
                }
                else if (randomLocation == 2)
                {
                    baseNumber = 4;
                    additionalNumber = -1;
                }

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[baseNumber];
                targetTileOne = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[baseNumber + 1];
                targetTileTwo = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[baseNumber + 3 + additionalNumber];
                targetTileThree = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[baseNumber + 4 + additionalNumber];
                targetTileFour = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateVector = targetTileOne.targetLocation.transform.position
                        + targetTileTwo.targetLocation.transform.position
                        + targetTileThree.targetLocation.transform.position
                        + targetTileFour.targetLocation.transform.position;

                calculateVector = calculateVector / 4;
                calculateVector.y += 10;



                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[baseNumber]);
                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[baseNumber + 1]);
                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[baseNumber + 3 + additionalNumber]);
                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[baseNumber + 4 + additionalNumber]);

            }
            else
            {
                int randomLocation = Random.Range(0, 3);
                int additionalNumber = 0;
                int baseNumber = -1;

                if (randomLocation == 0)
                {
                    baseNumber = 0;
                }
                else if (randomLocation == 1)
                {
                    baseNumber = 3;
                    additionalNumber = -1;
                }
                else
                {
                    baseNumber = 4;
                    additionalNumber = -1;
                }


                calculateCoordinate = startCoordinate + selectedChamberCoordinates[baseNumber];
                targetTileOne = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[baseNumber + 1];
                targetTileTwo = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[baseNumber + 4 + additionalNumber];  // + additionalNumber];
                targetTileThree = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateCoordinate = startCoordinate + selectedChamberCoordinates[baseNumber + 5 + additionalNumber];     //+ additionalNumber];
                targetTileFour = FindHexTileInTargetList(calculateCoordinate, listHexTiles);

                calculateVector = targetTileOne.targetLocation.transform.position
                        + targetTileTwo.targetLocation.transform.position
                        + targetTileThree.targetLocation.transform.position
                        + targetTileFour.targetLocation.transform.position;

                calculateVector = calculateVector / 4;
                calculateVector.y += 10;



                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[baseNumber]);
                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[baseNumber + 1]);
                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[baseNumber + 4 + additionalNumber]);
                theoreticalChamberCoordinates.Remove(selectedChamberCoordinates[baseNumber + 5 + additionalNumber]);

            }

            temp = FinaliseInstantiate(bigObject, calculateVector);



        }

        targetTileOne.objectAtLocation = temp;
        targetTileTwo.objectAtLocation = temp;
        targetTileThree.objectAtLocation = temp;
        targetTileFour.objectAtLocation = temp;


        for (int i = 0; i < additionalObjects.Count; i++)
        {
            random = Random.Range(0, theoreticalChamberCoordinates.Count);
            calculateCoordinate = startCoordinate + theoreticalChamberCoordinates[random];
            theoreticalChamberCoordinates.RemoveAt(random);
            targetTileOne = FindHexTileInTargetList(calculateCoordinate, listHexTiles);
            temp = FinaliseInstantiate(additionalObjects[i], targetTileOne.targetLocation.transform.position);
            targetTileOne.objectAtLocation = temp;
        }
    }


    void GenerateSmallLocation(List<GameObject> objects)         
    {
        for(int i = 0; i < objects.Count; i++)
        {
            random = Random.Range(0, theoreticalChamberCoordinates.Count);
            calculateCoordinate = startCoordinate + theoreticalChamberCoordinates[random];
            theoreticalChamberCoordinates.RemoveAt(random);
            targetTileOne = FindHexTileInTargetList(calculateCoordinate, listHexTiles);
            temp = FinaliseInstantiate(objects[i], targetTileOne.targetLocation.transform.position);
            targetTileOne.objectAtLocation = temp;
        }

    }

    GameObject FinaliseInstantiate(GameObject targetObject, Vector3 coordinate)
    {
        temp = Instantiate(targetObject, coordinate, Quaternion.identity);
        Vector3 vectorRotation = Vector3.zero;
        vectorRotation.y = Random.Range(0, 360);
        temp.transform.localEulerAngles = vectorRotation;

        Interactable interactScript = null;

        if (temp.CompareTag("Player"))
        {
            temp.GetComponent<WorldObject>().recentTile = targetTileOne;
            CharacterController character = temp.GetComponent<CharacterController>();
            playerCharacter = character;
            playerCharacter.InitialiseCharacter(this);
            battleAreas.player = character;
            camController.player = character;
        }
        else if (temp.CompareTag("Light"))
        {
            lightNoiseList.Add(temp.GetComponent<LightNoise>());
            temp.transform.parent = chamberParent.transform;
        }
        else if(temp.CompareTag("Enemy"))
        {
            temp.GetComponent<WorldObject>().recentTile = targetTileOne;
            CharacterController cControl = temp.GetComponent<CharacterController>();
            cControl.InitialiseCharacter(this);
            temp.transform.parent = chamberParent.transform;

            enemyControl.Add(cControl);
 
        }
        else if (temp.CompareTag("NPC"))
        {
            temp.GetComponent<WorldObject>().recentTile = targetTileOne;
            CharacterController cControl = temp.GetComponent<CharacterController>();
            cControl.InitialiseCharacter(this);
            temp.transform.parent = chamberParent.transform;

            npcControl.Add(cControl);

            if(cControl.interact)
            {
                interactScript = cControl.interactable;
            }
        }
        else if(temp.CompareTag("Interact"))
        {
            interactScript = temp.GetComponent<Interactable>();
            temp.transform.parent = chamberParent.transform;
        }
        else
        {
            temp.transform.parent = chamberParent.transform;
        }

        if (interactScript != null)
        {
            if(!interactScript.health)
            {
                interactScript.InitialiseInteractable(this, interactIDCounter);
                interactables.Add(interactScript);

                questManager.InitialiseInteract(interactScript);

                interactIDCounter++;
            }
        }

        return temp;
    }

    List<HexTile> InstantiateTiles(Vector2 starPosition, Vector2 size)
    {
        List<HexTile> returnTiles = new List<HexTile>();

        int xStart = (int)starPosition.x;
        int xEnd = (int)starPosition.x + (int)size.x;
        int yStart = (int)starPosition.y;
        int yEnd = (int)starPosition.y + (int)size.y;

        for (int x = xStart; x < xEnd + 1; x++)
        {
            for (int y = yStart; y < yEnd + 1; y++)
            {
                GameObject tempTile = Instantiate(tile, new Vector3(x * 1.4995408f, -0.9f, y * 0.8660469f), Quaternion.identity);
                HexTile tempHexControl = tempTile.GetComponent<HexTile>();
                tempHexControl.InitialiseTile(targetLocation, this, hexParent);
                hexTiles.Add(tempHexControl);
                returnTiles.Add(tempHexControl);

                if (x == 0 && y == 0)
                {
                    startTile = tempHexControl;
                }
            }
        }

        for (int x = xStart; x < xEnd; x++)
        {
            for (int y = yStart; y < yEnd + 1; y++)
            {
                GameObject tempTile = Instantiate(tile, new Vector3(0.7497704f + x * 1.4995408f, -0.9f, y * 0.8660469f + 0.43302345f), Quaternion.identity);
                HexTile tempHexControl = tempTile.GetComponent<HexTile>();
                tempHexControl.InitialiseTile(targetLocation, this, hexParent);
                hexTiles.Add(tempHexControl);
                returnTiles.Add(tempHexControl);
            }
        }

        return returnTiles;
    }


    List<HexTile> InstantiateTunnel(Vector2 starPosition, bool altarnative)
    {
        List<HexTile> returnTiles = new List<HexTile>();
        int xStart = (int)starPosition.x;
        int yStart = (int)starPosition.y;


        if (!altarnative)
        {
            yStart += 1;

            GameObject tempTile = Instantiate(tile, new Vector3(xStart * 1.4995408f, -0.9f, yStart * 0.8660469f), Quaternion.identity);
            HexTile tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);

            yStart += 1;
            tempTile = Instantiate(tile, new Vector3(xStart * 1.4995408f, -0.9f, yStart * 0.8660469f), Quaternion.identity);
            tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);

            xStart += 1;
            tempTile = Instantiate(tile, new Vector3(xStart * 1.4995408f, -0.9f, yStart * 0.8660469f), Quaternion.identity);
            tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);

            yStart += 1;
            tempTile = Instantiate(tile, new Vector3(xStart * 1.4995408f, -0.9f, yStart * 0.8660469f), Quaternion.identity);
            tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);

            // ------------------------------------------------------------------

            xStart = (int)starPosition.x;
            yStart = (int)starPosition.y + 1;

            tempTile = Instantiate(tile, new Vector3(0.7497704f + xStart * 1.4995408f, -0.9f, yStart * 0.8660469f + 0.43302345f), Quaternion.identity);
            tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);

            tempHexControl.guardPosition = true;

            yStart = (int)starPosition.y + 2;

            tempTile = Instantiate(tile, new Vector3(0.7497704f + xStart * 1.4995408f, -0.9f, yStart * 0.8660469f + 0.43302345f), Quaternion.identity);
            tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);

            tempHexControl.guardPosition = true;
            tempHexControl.topTile = true;

            xStart += 1;

            tempTile = Instantiate(tile, new Vector3(0.7497704f + xStart * 1.4995408f, -0.9f, yStart * 0.8660469f + 0.43302345f), Quaternion.identity);
            tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);

            yStart += 1;

            tempTile = Instantiate(tile, new Vector3(0.7497704f + xStart * 1.4995408f, -0.9f, yStart * 0.8660469f + 0.43302345f), Quaternion.identity);
            tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);



            xStart = (int)starPosition.x - 1;
            yStart = (int)starPosition.y;

            tempTile = Instantiate(tile, new Vector3(0.7497704f + xStart * 1.4995408f, -0.9f, yStart * 0.8660469f + 0.43302345f), Quaternion.identity);
            tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);

            yStart += 1;

            tempTile = Instantiate(tile, new Vector3(0.7497704f + xStart * 1.4995408f, -0.9f, yStart * 0.8660469f + 0.43302345f), Quaternion.identity);
            tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);

        }
        else
        {
            GameObject tempTile = Instantiate(tile, new Vector3(xStart * 1.4995408f, -0.9f, yStart * 0.8660469f), Quaternion.identity);
            HexTile tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);
            

            yStart -= 1;
            tempTile = Instantiate(tile, new Vector3(xStart * 1.4995408f, -0.9f, yStart * 0.8660469f), Quaternion.identity);
            tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);
            

            xStart += 1;
            tempTile = Instantiate(tile, new Vector3(xStart * 1.4995408f, -0.9f, yStart * 0.8660469f), Quaternion.identity);
            tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);
            

            yStart -= 1;
            tempTile = Instantiate(tile, new Vector3(xStart * 1.4995408f, -0.9f, yStart * 0.8660469f), Quaternion.identity);
            tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);
            

            //  ---------------------------------------------------------

            xStart = (int)starPosition.x;
            yStart = (int)starPosition.y;

            tempTile = Instantiate(tile, new Vector3(0.7497704f + xStart * 1.4995408f, -0.9f, yStart * 0.8660469f - 0.43302345f), Quaternion.identity);
            tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);

            tempHexControl.guardPosition = true;
            tempHexControl.alternativeFormation = true;
            tempHexControl.topTile = true;

            yStart -= 1;

            tempTile = Instantiate(tile, new Vector3(0.7497704f + xStart * 1.4995408f, -0.9f, yStart * 0.8660469f - 0.43302345f), Quaternion.identity);
            tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);

            tempHexControl.guardPosition = true;
            tempHexControl.alternativeFormation = true;

            xStart += 1;

            tempTile = Instantiate(tile, new Vector3(0.7497704f + xStart * 1.4995408f, -0.9f, yStart * 0.8660469f - 0.43302345f), Quaternion.identity);
            tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);

            yStart -= 1;


            tempTile = Instantiate(tile, new Vector3(0.7497704f + xStart * 1.4995408f, -0.9f, yStart * 0.8660469f - 0.43302345f), Quaternion.identity);
            tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);

            xStart = (int)starPosition.x - 1;
            yStart = (int)starPosition.y;

            tempTile = Instantiate(tile, new Vector3(0.7497704f + xStart * 1.4995408f, -0.9f, yStart * 0.8660469f - 0.43302345f), Quaternion.identity);
            tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);

            yStart = (int)starPosition.y + 1;
            tempTile = Instantiate(tile, new Vector3(0.7497704f + xStart * 1.4995408f, -0.9f, yStart * 0.8660469f - 0.43302345f), Quaternion.identity);
            tempHexControl = tempTile.GetComponent<HexTile>();
            tempHexControl.InitialiseTile(targetLocation, this, hexParent);
            hexTiles.Add(tempHexControl);
            returnTiles.Add(tempHexControl);
            tunnelTiles.Add(tempHexControl);

        }

        return returnTiles;
    }



    public HexTile FindHexTile(Vector2 targetXexCoordinate)
    {
        for (int i = 0; i < hexTiles.Count; i++)
        {
            if (targetXexCoordinate == hexTiles[i].hexCoordinate)
            {
                return hexTiles[i];
            }

        }
        HexTile hexTemp = null;
        return hexTemp;
    }

    HexTile FindHexTileInTargetList(Vector2 targetXexCoordinate, List<HexTile> hexTileList)
    {
        for (int i = 0; i < hexTileList.Count; i++)
        {
            if (targetXexCoordinate == hexTileList[i].hexCoordinate)
            {
                return hexTileList[i];
            }

        }
        HexTile hexTemp = null;
        return hexTemp;
    }

    public List<HexTile> GetNeighbours(HexTile hexTile)
    {
        Vector2 centreCoordinate = hexTile.hexCoordinate;
        List<HexTile> neighbours = new List<HexTile>();

        Vector2 targetVector = centreCoordinate;
        targetVector.x += 1;
        neighbours.Add(FindHexTile(targetVector));

        targetVector = centreCoordinate;
        targetVector.x -= 1;
        neighbours.Add(FindHexTile(targetVector));

        targetVector = centreCoordinate;
        targetVector.y += 1;
        neighbours.Add(FindHexTile(targetVector));

        targetVector = centreCoordinate;
        targetVector.y -= 1;
        neighbours.Add(FindHexTile(targetVector));

        targetVector = centreCoordinate;
        targetVector.x += 1;
        targetVector.y -= 1;
        neighbours.Add(FindHexTile(targetVector));

        targetVector = centreCoordinate;
        targetVector.x -= 1;
        targetVector.y += 1;
        neighbours.Add(FindHexTile(targetVector));

        return neighbours;
    }

    void InitialiseIndexes()
    {   
        // place guard
        for (int i = 0; i < chambers.Count; i++)
        {
            List<HexTile> hexTileList = chambers[i];

            for (int j = 0; j < hexTileList.Count; j++)
            {
                hexTileList[j].chamberIndexes.Add(i);
                hexTileList[j].tunnel = false;
            }
        }

        for (int i = 0; i < chambers.Count; i++)
        {
            List<HexTile> hexTileList = chambers[i];

            for (int j = 0; j < hexTileList.Count; j++)
            {
                if (j == 0 || j == 62 || j == 55 || j == 8)
                {
                    hexTileList[j].InitialiseTunnel(new List<int> { j });
                }
            }

        }
    }

    public List<HexTile> RequestChamber(int chamberIndex)
    {
        return chambers[chamberIndex];
    }

    public void TunnelsWalkable(bool state)
    {
        for (int i = 0; i < tunnelTiles.Count; i++)
        {
            tunnelTiles[i].walkable = state;
        }
    }

    public void InitialiseChamberTunnelCombat(int chamberIndex, bool state)
    {

            List<HexTile> tiles = chambers[chamberIndex];
            tiles[0].InitialiseTunnelForCombat(state);
            tiles[62].InitialiseTunnelForCombat(state);
            tiles[55].InitialiseTunnelForCombat(state);
            tiles[8].InitialiseTunnelForCombat(state);

    }


    public void DisableAccessToChambers(int chamberAcessible)
    {
        List<HexTile> targetTiles;
        for (int i = 0; i < chambers.Count; i++)
        {
            if (i != chamberAcessible)
            {
                targetTiles = chambers[i];
                for (int j = 0; j < targetTiles.Count; j++)
                {
                    targetTiles[j].forceUnwalkable = true;
                }

            }
        }
    }


    public void EnableAccessToChambers()
    {
        List<HexTile> targetTiles;
        for (int i = 0; i < chambers.Count; i++)
        {
            targetTiles = chambers[i];
            for (int j = 0; j < targetTiles.Count; j++)
            {
                targetTiles[j].forceUnwalkable = false;
            }
        }

    }
}
