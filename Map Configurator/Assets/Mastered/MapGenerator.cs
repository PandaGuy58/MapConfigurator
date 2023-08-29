using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public HexMaster hexMaster;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public float seedX;
    public float seedY;

    float[,] noiseMap;

    public MeshGenerator meshGenerator;
    int chestValue;



    public void GenerateMap(int mapWidth, int mapHeight, float scale, float seedX, float seedY, float requiredNumberOfChests)
    {

        chestValue = (int)requiredNumberOfChests;

        if (mapWidth < 1)
        {
            mapWidth = 1;
        }

        if (mapHeight < 1)
        {
            mapHeight = 1;
        }


        noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, scale, seedX, seedY);

        hexMaster.ResetGeneration();
        hexMaster.InstantiateTiles(mapWidth, mapHeight, noiseMap);

        meshGenerator.CalculateMesh();



        List<Vector2> chosenChestLocations = new List<Vector2>();
        List<Vector2> availableChestlocations = new List<Vector2>();

        for(int i = 0; i < mapWidth -1; i++)
        {
            for(int ii = 0; ii < mapHeight -1; ii++)
            {
                availableChestlocations.Add(new Vector2(i, ii));    
            }
        }

        while (chosenChestLocations.Count < chestValue)
        {
            int randomNumber = Random.Range(0, availableChestlocations.Count);
            Debug.Log(Time.time + " " + availableChestlocations.Count + "  " + randomNumber);
            Vector2 chosenLocation = availableChestlocations[randomNumber];
            availableChestlocations.RemoveAt(randomNumber);
            chosenChestLocations.Add(chosenLocation);
        }


        hexMaster.InstantiateChests(chosenChestLocations);    



    }


    /*
    public void InstiateChests(List<Vector2> arrayLocations)
    {
        HexInstance[,] hexInstancesPointer = hexMaster.hexInstances;
        Vector2 target = arrayLocations[0];
        hexInstancesPointer[(int)target.x, (int)target.y].SpawnChest();

    }
    */

    public void SelectChestLocations(int chestFrequency)
    {
        List<Vector2> selectedArrayLocations = new List<Vector2>();
        Debug.Log("width: " + mapWidth + "map height: " + mapHeight);


    }
}










/*
public class MeshData
{
    
}
*/