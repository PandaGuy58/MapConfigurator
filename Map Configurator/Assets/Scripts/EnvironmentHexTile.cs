using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentHexTile : MonoBehaviour
{
    public List<GameObject> rocks = new List<GameObject> ();

    public GameObject targetLocation;

    public GameObject topCentreCheck;
    public GameObject topCentreSpawn;

    public GameObject bottomCentreCheck;
    public GameObject bottomCentreSpawn;

    public GameObject topLeftCheck;
    public GameObject topLeftSpawn;

    public GameObject topRightCheck;
    public GameObject topRightSpawn;

    public GameObject bottomLeftCheck;
    public GameObject bottomLeftSpawn;

    public GameObject bottomRightCheck;
    public GameObject bottomRightSpawn;

    public GameObject environmentTilePrefab;

    float targetScale;
    // Start is called before the first frame update

    private void Start()
    {
        int pickRock = Random.Range(0, rocks.Count);
        GameObject environmentObject = Instantiate(rocks[pickRock], targetLocation.transform.position, Quaternion.identity);

        Vector3 vectorRotation = Vector3.zero;
        vectorRotation.y = Random.Range(0, 360);
        environmentObject.transform.localEulerAngles = vectorRotation;
        environmentObject.transform.parent = transform;

        if (targetScale != 10)
        {
            Vector3 calculateScale = environmentObject.transform.localScale;
            calculateScale *= targetScale;
            environmentObject.transform.localScale = calculateScale;
        }
        else
        {
            Vector3 calculateScale = environmentObject.transform.localScale;
            calculateScale.y *= 7.5f;
            calculateScale.x *= 3;
            calculateScale.z *= 3;
            environmentObject.transform.localScale = calculateScale;
        }


    }
    public void RegularStart()
    {
        targetScale = 1;
    }

    public void ManualStart(float scaleMultiply)
    {
        targetScale = scaleMultiply;
    }



    public List<EnvironmentHexTile> InstantiateEnvironmentTiles(List<EnvironmentHexTile> environmentTiles, float scaleMultiplier, Transform targetParent)
    {
        Collider[] hitColliders;
        bool environmentTileFound;
        bool regularTileFound;
        Vector3 targetLocation;




        environmentTileFound = false;
        regularTileFound = false;
        hitColliders = Physics.OverlapSphere(bottomLeftCheck.transform.position, 0.1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Tile"))
            {
                regularTileFound = true;
            }

            if (hitCollider.gameObject.CompareTag("Environment Tile"))
            {
                environmentTileFound = true;
            }
        }

        if (!environmentTileFound && !regularTileFound)
        {
            targetLocation = bottomLeftSpawn.transform.position;
            GameObject tempObj = Instantiate(environmentTilePrefab, targetLocation, Quaternion.identity);

            EnvironmentHexTile tempEnvHex = tempObj.GetComponent<EnvironmentHexTile>();
            tempEnvHex.ManualStart(scaleMultiplier);


            environmentTiles.Add(tempEnvHex);
            tempObj.transform.parent = targetParent;

        }


        environmentTileFound = false;
        regularTileFound = false;
        hitColliders = Physics.OverlapSphere(topCentreCheck.transform.position, 0.1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Tile"))
            {
                regularTileFound = true;
            }

            if (hitCollider.gameObject.CompareTag("Environment Tile"))
            {
                environmentTileFound = true;
            }
        }

        if (!environmentTileFound && !regularTileFound)
        {
            targetLocation = topCentreSpawn.transform.position;
            GameObject tempObj = Instantiate(environmentTilePrefab, targetLocation, Quaternion.identity);

            EnvironmentHexTile tempEnvHex = tempObj.GetComponent<EnvironmentHexTile>();
            tempEnvHex.ManualStart(scaleMultiplier);

            environmentTiles.Add(tempEnvHex);
            tempObj.transform.parent = targetParent;
        }



        environmentTileFound = false;
        regularTileFound = false;
        hitColliders = Physics.OverlapSphere(topLeftCheck.transform.position, 0.1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Tile"))
            {
                regularTileFound = true;
            }

            if (hitCollider.gameObject.CompareTag("Environment Tile"))
            {
                environmentTileFound = true;
            }
        }

        if (!environmentTileFound && !regularTileFound)
        {
            targetLocation = topLeftSpawn.transform.position;
            GameObject tempObj = Instantiate(environmentTilePrefab, targetLocation, Quaternion.identity);

            EnvironmentHexTile tempEnvHex = tempObj.GetComponent<EnvironmentHexTile>();
            tempEnvHex.ManualStart(scaleMultiplier);

            environmentTiles.Add(tempEnvHex);
            tempObj.transform.parent = targetParent;
        }







        environmentTileFound = false;
        regularTileFound = false;
        hitColliders = Physics.OverlapSphere(topRightCheck.transform.position, 0.1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Tile"))
            {
                regularTileFound = true;
            }

            if (hitCollider.gameObject.CompareTag("Environment Tile"))
            {
                environmentTileFound = true;
            }
        }

        if (!environmentTileFound && !regularTileFound)
        {
            targetLocation = topRightSpawn.transform.position;
            GameObject tempObj = Instantiate(environmentTilePrefab, targetLocation, Quaternion.identity);

            EnvironmentHexTile tempEnvHex = tempObj.GetComponent<EnvironmentHexTile>();
            tempEnvHex.ManualStart(scaleMultiplier);

            environmentTiles.Add(tempEnvHex);
            tempObj.transform.parent = targetParent;
        }





        environmentTileFound = false;
        regularTileFound = false;
        hitColliders = Physics.OverlapSphere(bottomRightCheck.transform.position, 0.1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Tile"))
            {
                regularTileFound = true;
            }

            if (hitCollider.gameObject.CompareTag("Environment Tile"))
            {
                environmentTileFound = true;
            }
        }

        if (!environmentTileFound && !regularTileFound)
        {
            targetLocation = bottomRightSpawn.transform.position;
            GameObject tempObj = Instantiate(environmentTilePrefab, targetLocation, Quaternion.identity);

            EnvironmentHexTile tempEnvHex = tempObj.GetComponent<EnvironmentHexTile>();
            tempEnvHex.ManualStart(scaleMultiplier);

            environmentTiles.Add(tempEnvHex);
            tempObj.transform.parent = targetParent;
        }



        environmentTileFound = false;
        regularTileFound = false;
        hitColliders = Physics.OverlapSphere(bottomCentreCheck.transform.position, 0.1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Tile"))
            {
                regularTileFound = true;
            }

            if (hitCollider.gameObject.CompareTag("Environment Tile"))
            {
                environmentTileFound = true;
            }
        }

        if (!environmentTileFound && !regularTileFound)
        {
            targetLocation = bottomCentreSpawn.transform.position;
            GameObject tempObj = Instantiate(environmentTilePrefab, targetLocation, Quaternion.identity);

            EnvironmentHexTile tempEnvHex = tempObj.GetComponent<EnvironmentHexTile>();
            tempEnvHex.ManualStart(scaleMultiplier);

            environmentTiles.Add(tempEnvHex);
            tempObj.transform.parent = targetParent;
        }



        return environmentTiles;

    }

}
