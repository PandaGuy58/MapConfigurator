using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class HexMaster : MonoBehaviour
{
    public GameObject instantiateTile;
    public HexInstance[,] hexInstances;





   // public int xDistance = 10;
  //  public int yDistance = 12;

  //  public FindPathNew findPath;
//    public List<HexInstance> hexList;

  //  public int xDistanceParameter;
 //   public int yDistanceParameter;

 //   public Transform hexTilesParent;

    bool worldExist = false;

    public Transform hexTiles;
    TilePool tilePool;
    ChestPool chestPool;
    
    public MeshGenerator meshGenerator;
    void Start()
    {
        tilePool = GetComponent<TilePool>();
        chestPool = GetComponent<ChestPool>();
    }

    public void GenerateMap()
    {
   //    InstantiateTiles(xDistanceParameter, yDistanceParameter);

        //   Vector2 targetCoordinates 
   //     HexInstance hexInstance = FindHexInArrayByCoordinate(new Vector2(3, -1));

     //   hexInstance.selected = true;

     //   hexInstance = FindHexInArrayByCoordinate(new Vector2(4, 0));
     //   hexInstance.selected = true;

//
   //     hexInstance = FindHexInArrayByCoordinate(new Vector2(0, 5));
    //    hexInstance.selected = true;
        // Debug.Log(targetCoordinates);

      //  Debug.Log(hexInstances.GetLength(1));
     //   worldExist = true;
      //   worldExist = true;
    }

    private void Update()
    {
        if(worldExist)
        {
        //    HexInstance start = FindHexInArrayByCoordinate(new Vector2(4, -2));
        //    HexInstance end = FindHexInArrayByCoordinate(new Vector2(0, 6));

          //  hexList = findPath.RequestPath(start, end);
            //   Debug.Log(hexList.Count);

        //    for (int i = 0; i < hexList.Count; i++)
         //   {
         //       hexList[i].pointerTime = Time.time + 0.1f;
       //     }
        }

    }

    // Update is called once per frame

    public void ResetGeneration()
    {
        tilePool.ResetPool();
        chestPool.ResetPool();
    }

    public void InstantiateTiles(int xLength, int yLength, float[,] noiseMap)
    {
        hexInstances = new HexInstance[xLength,yLength];
        meshGenerator.ResetVerticesList();

        float calculateXpos;
        float calculateYpos;
        // int x = xLength;
        //int y = yLength;
        int arrayX;
        int arrayY;

        if(xLength < 1)
        {
            xLength = 1;
        }

        if(yLength < 1)
        {
            yLength = 1;
        }

        //     int targetX = 0;
        //    if(xLength%2==0)
        //    {

        //    }


      //  HexInstance tempHexInstance = null;

        for(int i = 0; i < xLength /2; i++)
        {
            arrayX = i * 2;
            calculateXpos = i * 1.5f;
            for (int j = 0; j < yLength; j++)
            {
                arrayY = j;
                calculateYpos = j * 0.866f;
                Vector3 target = new Vector3(calculateXpos, noiseMap[arrayX, arrayY], calculateYpos);

                HexInstance hexInstance = tilePool.requestHex();
                GameObject hexObject = hexInstance.gameObject;

                hexObject.transform.position = target;
                hexObject.name = arrayX.ToString() + " " + arrayY.ToString();


             //   GameObject temp = Instantiate(instantiateTile, target, Quaternion.identity);
             //   temp.name = 

           //     HexInstance hex = temp.GetComponent<HexInstance>();
                hexInstances[arrayX,arrayY] = hexInstance;
                hexInstance.coordinates = new Vector2(arrayX, arrayY - (int)arrayX / 2);

                hexObject.transform.parent = hexTiles;

                //   Vector3 coordinate = temp.transform.position;
                //    coordinate.y = noiseMap[arrayX, arrayY];

                //     temp.transform.position = coordinate;
                // Debug.Log(noiseMap[arrayX, arrayY]);

                hexInstance.height = noiseMap[arrayX, arrayY];

                //  Color calculateColour = Color.Lerp(Color.black, Color.white, noiseMap[arrayX, arrayY]);

                //  hex.Initialise(calculateColour);
                //   hex.meshRend.material.color = calculateColour;



                //    if (tempHexInstance != null)
                //  {
                //     Transform pointA = tempHexInstance.pointA;
                //   Transform pointB = tempHexInstance.pointB;

                //    Transform pointC = hexInstance.pointC;
                //   Transform pointD = hexInstance.pointD;

                //    meshGenerator.AddFourVertices(pointB.position, )
                //    meshGenerator.AddFourVertices(pointA.position, pointB.position, pointC.position, pointD.position);
                //     }

                //  tempHexInstance = hexInstance;

                //    meshGenerator.AddTwoVertices()

             //   meshGenerator.AddFourVertices(hexInstance.pointA.position,
                                      //          hexInstance.pointB.position,
                                        //            hexInstance.pointC.position,
                                       //                 hexInstance.pointD.position);

                
            }
        }

        meshGenerator.CalculateMesh();



        for(int i = 0; i < xLength / 2; i++)
        {
            arrayX = 1 + i * 2;
            calculateXpos = 0.75f + i * 1.5f;
            for(int j = 0; j < yLength; j++)
            {
                arrayY = j;
                calculateYpos = 0.433f + j * 0.866f;
                Vector3 target = new Vector3(calculateXpos, noiseMap[arrayX, arrayY], calculateYpos);



                HexInstance hexInstance = tilePool.requestHex();
                GameObject hexObject = hexInstance.gameObject;

                hexObject.transform.position = target;
                hexObject.name = arrayX.ToString() + " " + arrayY.ToString();



                hexInstances[arrayX, arrayY] = hexInstance;
                hexInstance.coordinates = new Vector2(arrayX, arrayY - (int)arrayX / 2);

                hexObject.transform.parent = hexTiles;

                hexInstance.height = noiseMap[arrayX, arrayY];



                //   temp.transform.parent = hexTiles;


                //   Vector3 coordinate = temp.transform.position;
                // coordinate.y = noiseMap[arrayX, arrayY];

                // temp.transform.position = coordinate;

                //  hex.height = noiseMap[arrayX, arrayY];

                //  Color calculateColour = Color.Lerp(Color.white, Color.black, noiseMap[arrayX, arrayY] * 0.5f);

                //   hex.Initialise(calculateColour);
                //  hex.meshRend.material.color = calculateColour;

                //      temp.transform.parent = hexTilesParent;

            }
        }
    }

    HexInstance FindHexInArrayByCoordinate(Vector2 coordinates)
    {

     //   Debug.Log(Time.timeAsDouble + " " + coordinates.x + " " + coordinates.y);
        int targetX = (int)coordinates.x;
        int targetY = (int)coordinates.y - (int)coordinates.x / -2;

        bool fail = false;

        if(targetX < 0 || targetX > hexInstances.GetLength(0))
        {
            fail = true;
        }
        else if(targetY < 0 || targetY > hexInstances.GetLength(1))
        {
            fail = true;
        }

        if(!fail)
        {
            return hexInstances[targetX, targetY];
        }

        return null;    
    }

    public void InstantiateChests(List<Vector2> chosenLocations)
    {
        for(int i = 0; i < chosenLocations.Count; i++)
        {
            Vector2 targetLocation = chosenLocations[i];
            HexInstance targetHex = hexInstances[(int)targetLocation.x, (int)targetLocation.y];
            GameObject targetChest = chestPool.RequestChest();
            if(targetHex == null)
            {
                Debug.Log(Time.time);
            }
            targetHex.SpawnChest(targetChest);

           //
           //targetHex.SpawnChest(chestPool.RequestChest());
        }
    }

    HexInstance FindHexInArrayByIndex(Vector2 index)
    {
        return hexInstances[(int)index.x,(int)index.y];
    }


    public List<HexInstance> GetNeighbours(HexInstance hexTile)
    {
        Vector2 centreCoordinate = hexTile.coordinates;
        List<HexInstance> neighbours = new List<HexInstance>();

        Vector2 targetVector = centreCoordinate;
        targetVector.x += 1;
        neighbours.Add(FindHexInArrayByCoordinate(targetVector));

        targetVector = centreCoordinate;
        targetVector.x -= 1;
        neighbours.Add(FindHexInArrayByCoordinate(targetVector));

        targetVector = centreCoordinate;
        targetVector.y += 1;
        neighbours.Add(FindHexInArrayByCoordinate(targetVector));

        targetVector = centreCoordinate;
        targetVector.y -= 1;
        neighbours.Add(FindHexInArrayByCoordinate(targetVector));

        targetVector = centreCoordinate;
        targetVector.x += 1;
        targetVector.y -= 1;
        neighbours.Add(FindHexInArrayByCoordinate(targetVector));

        targetVector = centreCoordinate;
        targetVector.x -= 1;
        targetVector.y += 1;
        neighbours.Add(FindHexInArrayByCoordinate(targetVector));

        return neighbours;
    }
}
