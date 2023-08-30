using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationMaster : MonoBehaviour
{
    [SerializeField]
    private GameObjectPool tilesPool;

    [SerializeField]
    private GameObjectPool chestPool;

    [SerializeField]
    private GameObjectPool greenAssetPool;

    [SerializeField]
    private GameObjectPool yellowAssetPool;

    private List<GameObject> allTilesList;
    private List<HexInstance> allHexList;

    private int chestTotalNumber;
    private int waterTotalNumber;
    private int alternativeBiomeTotalNumber;

    private int randomSeedVal;

    public HexInstance[,] hexInstances;
    public Vector2 testVector;

   // public int assetDensity = 5;

    public List<GameObject> GetTilesList()
    {
        return allTilesList;
    }

    public int GetChest()
    {
        return chestTotalNumber;
    }

    public int GetWater()
    {
        return waterTotalNumber;
    }

    public int GetAlternativeBiome()
    {
        return alternativeBiomeTotalNumber;
    }

    public void Generate(int width, int height, int noise, int seedX, int seedY, int chest, int biome, int randSeed, int water, int waterHeight, int assetSize, int assetDensity)            
    {
        randomSeedVal = randSeed;

        ResetLists();
        tilesPool.ResetPool();
        chestPool.ResetPool();
        greenAssetPool.ResetPool();
        yellowAssetPool.ResetPool();    

        float noiseFloat = noise * 0.01f;

        float seedXFloat = seedX;
        float seedYFloat = seedY;
        float chestFloat = chest;

        float biomeFloat = biome;

        float[,] noiseMap = Noise.GenerateNoiseMap(width, height, noiseFloat, seedXFloat, seedYFloat);

        InstantiateTiles(width, height, noiseMap);

        chestFloat = chestFloat * 0.1f;                   
        chestFloat = chestFloat * allTilesList.Count;

        InstantiateChests((int)chestFloat);

        biomeFloat = biomeFloat * 0.1f;
        biomeFloat = biomeFloat * allTilesList.Count;

        ConfigureBiome((int)biomeFloat);
        ConfigureWater(water, waterHeight);
        ConfigureTileAssets(assetDensity, assetSize);

        



    //    for(int i = 0; i < assetDensity; i++)
      //  {
      //      ConfigureTileAssets();
       // }
        
    }


    void ResetLists()
    {
        allTilesList = new List<GameObject>();
        allHexList = new List<HexInstance>();

        chestTotalNumber = 0;
        waterTotalNumber = 0;
        alternativeBiomeTotalNumber = 0;
    }


    void InstantiateTiles(int xLength, int yLength, float[,] noiseMap)
    {
        hexInstances = new HexInstance[xLength, yLength];

        Random.seed = randomSeedVal;


        float calculateXpos;
        float calculateYpos;
        int arrayX;
        int arrayY;

        for (int i = 0; i < xLength / 2; i++)
        {
            arrayX = i * 2;
            calculateXpos = i * 1.5f;
            for (int j = 0; j < yLength; j++)
            {
                arrayY = j;
                calculateYpos = j * 0.866f;
                Vector3 target = new Vector3(calculateXpos, noiseMap[arrayX, arrayY], calculateYpos);

                GameObject tile = tilesPool.RequestObject();

                HexInstance hexInstance = tile.GetComponent<HexInstance>();
                hexInstances[arrayX, arrayY] = hexInstance;
                hexInstance.indexLocation = new Vector2(arrayX, arrayY);

                allTilesList.Add(tile);
                allHexList.Add(hexInstance);

                Vector2 calculateCoordinate = new Vector2(i * 2, j - i);                               //(j * -1 ,  i * 2  );
                hexInstance.coordinates = calculateCoordinate;

                tile.transform.position = target;
                tile.name = arrayX.ToString() + " " + arrayY.ToString();

            }
        }



        for (int i = 0; i < xLength / 2; i++)
        {
            arrayX = 1 + i * 2;
            calculateXpos = 0.75f + i * 1.5f;
            for (int j = 0; j < yLength; j++)
            {
                arrayY = j;
                calculateYpos = 0.433f + j * 0.866f;
                Vector3 target = new Vector3(calculateXpos, noiseMap[arrayX, arrayY], calculateYpos);

                GameObject tile = tilesPool.RequestObject();
                tile.transform.position = target;
                tile.name = arrayX.ToString() + " " + arrayY.ToString();

                HexInstance hexInstance = tile.GetComponent<HexInstance>();
                hexInstances[arrayX, arrayY] = hexInstance;
                hexInstance.indexLocation = new Vector2(arrayX, arrayY);

                allTilesList.Add(tile);
                allHexList.Add(hexInstance);

                Vector2 calculateCoordinate = new Vector2(i * 2 +1, j - i);
                hexInstance.coordinates = calculateCoordinate;
            }
        }


    }

    void InstantiateChests(int chestsNumber)
    {
        Random.seed = randomSeedVal;

        List<GameObject> allTilesListInstance = new(allTilesList);
        Vector3 calculate = new Vector3(0.5f, 5, 0.5f);
        for(int i = 0; i < chestsNumber;i++)
        {
         //   Debug.Log(Time.time + " " + i);
            int randomChoice = Random.Range(0, allTilesListInstance.Count);
            // Debug.Log(allTilesListInstance.Count + " " + Time.time);
        //    Debug.Log(allTilesListInstance.Count + " " + randomChoice);

            GameObject tile = allTilesListInstance[randomChoice];
            Vector3 targetLocation = tile.transform.position;
            targetLocation += calculate;

            GameObject chest = chestPool.RequestObject();
            chest.transform.position = targetLocation;

            allTilesListInstance.RemoveAt(randomChoice);
            chestTotalNumber++;
        }
    }

    void ConfigureBiome(int biomeNumber)
    {
        Random.seed = randomSeedVal;

        List<HexInstance> hexListInstance = new(allHexList);

        for (int i = 0; i < hexListInstance.Count; i++)
        {
            hexListInstance[i].waterBiome = false;
            hexListInstance[i].alternativeBiome = false;

        }

        for (int i = 0; i < biomeNumber; i++)
        {
            Debug.Log(Time.time +" " + i + " " + biomeNumber); ;
            int randomChoice = Random.Range(0, hexListInstance.Count);
            HexInstance targetHex = hexListInstance[randomChoice];
            targetHex.alternativeBiome = true;

            hexListInstance.RemoveAt(randomChoice);
            alternativeBiomeTotalNumber++;

        }

     //   Debug.Log(hexListInstance.Count + " " + Time.time);

    //    for (int i = 0; i < hexListInstance.Count; i++)
     //   {
        //    hexListInstance[i].alternativeBiome = false;
         //   hexListInstance[i].waterBiome = false;
      //  }

    }

    void ConfigureWater(int waterNumber, int waterHeight)
    {
        Random.seed = randomSeedVal;

        if (waterNumber != 0)
        {
            
            HexInstance startHex = allHexList[Random.Range(0, allHexList.Count)];
            List<HexInstance> selectedHexes = new List<HexInstance>();
            selectedHexes.Add(startHex);

            startHex.InitialiseWater(waterHeight);
            waterNumber--;
            waterTotalNumber++;

            if (startHex.alternativeBiome)
            {
                alternativeBiomeTotalNumber--;
            }

            while (waterNumber > 0 && selectedHexes.Count > 0)
            {
                HexInstance randomInstance = selectedHexes[Random.Range(0, selectedHexes.Count)];
                List<HexInstance> neighbours = GetNeighboursAvailableWater(randomInstance);

                if (neighbours.Count > 0)
                {
                    HexInstance nextRandomInstance = neighbours[Random.Range(0, neighbours.Count)];
                    nextRandomInstance.waterBiome = true;
                    selectedHexes.Add(nextRandomInstance);
                    waterNumber--;

                    nextRandomInstance.InitialiseWater((float)waterHeight);
                    waterTotalNumber++;

                    if (nextRandomInstance.alternativeBiome)
                    {
                        alternativeBiomeTotalNumber--;
                    }
                }
                else
                {
                    selectedHexes.Remove(randomInstance);
                }
            }
        }
    }


    void ConfigureTileAssets(int assetDensity, int assetSize)
    {
        for (int i = 0; i < allHexList.Count; i++)
        {
            HexInstance targetHex = allHexList[i];
            Color targetColour = targetHex.InitialiseColour();

            for (int ii = 0; ii < assetDensity; ii++)
            {
                GameObject targetAsset = null;

                if (targetHex.alternativeBiome && !targetHex.waterBiome)
                {
                    targetAsset = yellowAssetPool.RequestObject();
                }
                else if (!targetHex.waterBiome)
                {
                    targetAsset = greenAssetPool.RequestObject();
                }

                if (targetAsset != null)
                {
                    targetHex.SpawnAssets(targetAsset);
                    MeshRenderer rend = targetAsset.GetComponent<MeshRenderer>();
                    rend.material.color = targetColour;

                 //   float floatAssetSize  = (float)assetSize;
                 //   floatAssetSize *= 0.05f;

                    Vector3 size = new Vector3(0.05f, 0.05f, 0.05f);
                    size = size + (size * 0.5f * assetSize);

                    size.x = size.x * Random.Range(1.0f, 1.5f);
                    size.y = size.y * Random.Range(1.0f, 1.5f);
                    size.z = size.z * Random.Range(1.0f, 1.5f);

                    targetAsset.transform.localScale = size;

                }
            }
        }
    }
        /*
    //    for(int i = 0; i < allHexList.Count; i++)
     //   {
            HexInstance hexInstance = allHexList[i];
            GameObject targetAsset = null;            

            if (hexInstance.alternativeBiome && !hexInstance.waterBiome)
            {
                targetAsset = yellowAssetPool.RequestObject();
            }
            else if(!hexInstance.waterBiome)
            {
                targetAsset = greenAssetPool.RequestObject();
            }

            if(targetAsset != null)
            {
               // allHexList[i].SpawnAssets(targetAsset);
            }
       // }
    }
        */

    HexInstance FindHexInArrayByIndex(Vector2 index)                    // search by array index
    {
        return hexInstances[(int)index.x, (int)index.y];
    }

    HexInstance FindHexInArrayByCoordinate(Vector2 coordinates)                     //search by hex coordinate
    {
        int targetX = (int)coordinates.x;
        int targetY = (int)coordinates.y - (int)coordinates.x / -2;

        bool fail = false;

        if (targetX < 0 || targetX > hexInstances.GetLength(0) -1)
        {
            fail = true;
        }
        else if (targetY < 0 || targetY > hexInstances.GetLength(1) -1)
        {
            fail = true;
        }

        if (!fail)
        {
            return hexInstances[targetX, targetY];
        }

        return null;
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

    public List<HexInstance> GetNeighboursAvailableWater(HexInstance hexTile)
    {
        Vector2 centreCoordinate = hexTile.coordinates;
        List<HexInstance> neighbours = new List<HexInstance>();

        Vector2 targetVector = centreCoordinate;
        targetVector.x += 1;
        HexInstance target = FindHexInArrayByCoordinate(targetVector);
        if(target != null)
        {
            if (target.waterBiome == false)
            {
                neighbours.Add(target);
            }
        }

        

        targetVector = centreCoordinate;
        targetVector.x -= 1;
        target = FindHexInArrayByCoordinate(targetVector);
        if (target != null)
        {
            if (target.waterBiome == false)
            {
                neighbours.Add(target);
            }
        }

        targetVector = centreCoordinate;
        targetVector.y += 1;
        target = FindHexInArrayByCoordinate(targetVector);
        if (target != null)
        {
            if (target.waterBiome == false)
            {
                neighbours.Add(target);
            }
        }

        targetVector = centreCoordinate;
        targetVector.y -= 1;
        target = FindHexInArrayByCoordinate(targetVector);
        if (target != null)
        {
            if (target.waterBiome == false)
            {
                neighbours.Add(target);
            }
        }

        targetVector = centreCoordinate;
        targetVector.x += 1;
        targetVector.y -= 1;
        target = FindHexInArrayByCoordinate(targetVector);
        if (target != null)
        {
            if (target.waterBiome == false)
            {
                neighbours.Add(target);
            }
        }

        targetVector = centreCoordinate;
        targetVector.x -= 1;
        targetVector.y += 1;
        target = FindHexInArrayByCoordinate(targetVector);
        if (target != null)
        {
            if (target.waterBiome == false)
            {
                neighbours.Add(target);
            }
        }

        return neighbours;
    }

    public UIValues RequestIUValues()
    {
        UIValues values = new UIValues(waterTotalNumber, chestTotalNumber, alternativeBiomeTotalNumber);
        return values;
    }
}

public class UIValues
{
    public int water;
    public int chest;
    public int biome;
    public UIValues(int water, int chest, int biome)
    {
        this.water = water;
        this.chest = chest;
        this.biome = biome;
    }
}







/*
 *        
        if(waterNumber > 0)
        {
            Random.seed = randomSeedVal;

            HexInstance startHex = allHexList[Random.Range(0, allHexList.Count)];
            List<HexInstance> selectedHexes = new List<HexInstance>();
            selectedHexes.Add(startHex);

            startHex.InitialiseWater(waterHeight);
            waterNumber--;

            while (waterNumber > 0 && !(selectedHexes.Count == 0))
            {

                HexInstance randomInstance = selectedHexes[Random.Range(0, selectedHexes.Count)];
                List<HexInstance> neighbours = GetNeighboursAvailableWater(randomInstance);
                if (neighbours.Count > 0)
                {
                    HexInstance nextRandomInstance = neighbours[Random.Range(0, neighbours.Count)];
                    nextRandomInstance.waterBiome = true;
                    selectedHexes.Add(nextRandomInstance);
                    waterNumber--;

                    nextRandomInstance.InitialiseWater((float)waterHeight);
                    waterNumber++;

                    if(nextRandomInstance.alternativeBiome)
                    {
                        alternativeBiomeTotalNumber--;
                    }
                }
                else
                {
                    selectedHexes.Remove(randomInstance);
                }

            }
        }
    }
 


targetHex.waterBiome = true;

//     HexInstance neighbour = targetHex.GetNeighbour();
Vector2 target = targetHex.indexLocation;
target.x += 1;

HexInstance hex = FindHexInArrayByIndex(target);
if (hex != null)
{
    hex.waterBiome = true;
}


//   if(neighbour != null)
//   {
//  Debug.Log(Time.time);
//      neighbour.waterBiome = true;
//  }

//   waterNumber -= 1;
//   while(waterNumber != 0)
//  {

// }
//   HexInstance neighbour = targetHex.GetNeighbour();
//   neighbour.waterBiome = true;

*/