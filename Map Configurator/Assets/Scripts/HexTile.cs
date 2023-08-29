using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public bool guardPosition = false;
    public bool alternativeFormation = false;
    public bool topTile = false;
    public Vector2 hexCoordinate;

    public int gCost;
    public int hCost;


    public bool walkable = true;
    public bool forceUnwalkable = false;
    Renderer rend;
    public HexTile parent;

    public GameObject objectAtLocation;
    public GameObject targetLocation;

    float pointTime;
    float pointValue = 0;

    HexGrid gridMaster;

    bool visited = false;

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

    public List<int> chamberIndexes = new List<int>();
    public bool tunnel = true;

    public bool combatInitiated = false;

    public Color walkColour = Color.blue;
    Color enemyColour = Color.red;
    public Color playerColour = Color.yellow;
    Color allyColour = Color.green;
    Color selectColour = Color.white;

    Color currentColour = Color.black;
    Color targetColour = Color.black;


    float targetVisibility = 0.017f;
    public bool visible = false;
    float visibilityVal = 0;
    public AnimationCurve curve;

    public float visualCombatActivate = 0;
    public float pointerActivate = 0;

    public float selectTime;

  //  public bool forceWalkable;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }


    // Update is called once per frame

    public void PlaceObjectAtLocation(GameObject targetObject)
    {
        objectAtLocation = targetObject;
    }

    public void RemoveObjectAtLocation()
    {
        objectAtLocation = null;
    }

    public void InitialiseTile(GameObject moveTarget, HexGrid gridScript, Transform hexParent)
    {
        GameObject tempObj = Instantiate(moveTarget, new Vector3(0, 0, 0), Quaternion.identity);
        tempObj.transform.parent = transform;
        tempObj.transform.localPosition = new Vector3(0.5f, 10, 0.5f);
        targetLocation = tempObj;

        gridMaster = gridScript;
        transform.parent = hexParent;
    }

    public void InitialiseCoordinates(Vector2 receiveVector)
    {
        if (!visited)
        {
            visited = true;

            hexCoordinate.x = receiveVector.x;
            hexCoordinate.y = receiveVector.y;
            gameObject.name = hexCoordinate.x.ToString() + "," + hexCoordinate.y.ToString();

            Collider[] hitColliders;
            HexTile targetTile;

            hitColliders = Physics.OverlapSphere(topCentreCheck.transform.position, 0.1f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("Tile"))
                {
                    targetTile = hitCollider.gameObject.GetComponent<HexTile>();
                    targetTile.InitialiseCoordinates(new Vector2(hexCoordinate.x, hexCoordinate.y + 1));
                }
            }

            hitColliders = Physics.OverlapSphere(bottomCentreCheck.transform.position, 0.1f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("Tile"))
                {
                    targetTile = hitCollider.gameObject.GetComponent<HexTile>();
                    targetTile.InitialiseCoordinates(new Vector2(hexCoordinate.x, hexCoordinate.y - 1));
                }
            }

            hitColliders = Physics.OverlapSphere(topRightCheck.transform.position, 0.1f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("Tile"))
                {
                    targetTile = hitCollider.gameObject.GetComponent<HexTile>();
                    targetTile.InitialiseCoordinates(new Vector2(hexCoordinate.x + 1, hexCoordinate.y));
                }
            }

            hitColliders = Physics.OverlapSphere(bottomRightCheck.transform.position, 0.1f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("Tile"))
                {
                    targetTile = hitCollider.gameObject.GetComponent<HexTile>();
                    targetTile.InitialiseCoordinates(new Vector2(hexCoordinate.x + 1, hexCoordinate.y - 1));
                }
            }

            hitColliders = Physics.OverlapSphere(topLeftCheck.transform.position, 0.1f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("Tile"))
                {
                    targetTile = hitCollider.gameObject.GetComponent<HexTile>();
                    targetTile.InitialiseCoordinates(new Vector2(hexCoordinate.x - 1, hexCoordinate.y + 1));
                }
            }

            hitColliders = Physics.OverlapSphere(bottomLeftCheck.transform.position, 0.1f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("Tile"))
                {
                    targetTile = hitCollider.gameObject.GetComponent<HexTile>();
                    targetTile.InitialiseCoordinates(new Vector2(hexCoordinate.x - 1, hexCoordinate.y));
                }
            }
        }
    }

    public List<EnvironmentHexTile> InstantiateEnvironmentTiles(List<EnvironmentHexTile> environmentTiles, Transform targetParent)
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
            EnvironmentHexTile environmentTile = tempObj.GetComponent<EnvironmentHexTile>();
            environmentTiles.Add(environmentTile);
            environmentTile.RegularStart();
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
            EnvironmentHexTile environmentTile = tempObj.GetComponent<EnvironmentHexTile>();
            environmentTiles.Add(environmentTile);
            environmentTile.RegularStart();
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
            EnvironmentHexTile environmentTile = tempObj.GetComponent<EnvironmentHexTile>();
            environmentTiles.Add(environmentTile);
            environmentTile.RegularStart();
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
            EnvironmentHexTile environmentTile = tempObj.GetComponent<EnvironmentHexTile>();
            environmentTiles.Add(environmentTile);
            environmentTile.RegularStart();
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
            EnvironmentHexTile environmentTile = tempObj.GetComponent<EnvironmentHexTile>();
            environmentTiles.Add(environmentTile);
            environmentTile.RegularStart();
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
            EnvironmentHexTile environmentTile = tempObj.GetComponent<EnvironmentHexTile>();
            environmentTiles.Add(environmentTile);
            environmentTile.RegularStart();
            tempObj.transform.parent = targetParent;
        }

        return environmentTiles;
    }


    public void PointAtTile()
    {
        pointTime = Time.time + 0.05f;
    }


    public void UpdateTile()
    {

        rend.material.SetFloat("_Visibility", curve.Evaluate(visibilityVal) * targetVisibility);
        rend.material.SetColor("_CurrentColour", currentColour);
        currentColour = Color.Lerp(currentColour, targetColour, 2 * Time.deltaTime);


        
        if (visible)
        {
            visibilityVal += Time.deltaTime * 2;
            if (visibilityVal > 1)
            {
                visibilityVal = 1;
            }
        }
        else
        {
            visibilityVal -= Time.deltaTime * 2;
            if (visibilityVal < 0)
            {
                visibilityVal = 0;
            }
        }


        if (forceUnwalkable)
        {
            walkable = false;
        }
        else if(objectAtLocation == null)
        {
            walkable = true;
        }
        else
        {
            walkable = false;
        }

        if (!combatInitiated)
        {
            visible = false;
        }
        else                                                                    // combat behaviour
        {
            visible = true;
        }


        if (visualCombatActivate >= Time.time)
        {
            targetColour = selectColour;
        }
        else if (tunnel)
        {
            targetColour = allyColour;
            gameObject.tag = "Tile";
        }
        else if (objectAtLocation != null)
        {
            if (pointerActivate > Time.time)
            {
                if (objectAtLocation.CompareTag("Player"))
                {
                    Debug.Log(Time.time);
                    visibilityVal -= Time.deltaTime * 4;
                    if(visibilityVal < 0)
                    {
                        visibilityVal = 0;
                    }
                }
                else
                {
                    targetColour = selectColour;
                }
            }
            else if (selectTime > Time.time)
            {
                targetColour = selectColour;
            }
            else if (objectAtLocation.CompareTag("Player"))
            {
                targetColour = playerColour;
                gameObject.tag = "Player";
            }
            else if (objectAtLocation.CompareTag("Enemy"))
            {
                targetColour = enemyColour;
                gameObject.tag = "Enemy";
            }
            else if (objectAtLocation.CompareTag("NPC"))
            {
                targetColour = allyColour;
                gameObject.tag = "NPC";
            }
            else if (objectAtLocation.CompareTag("Interact"))
            {
                visible = false;
                gameObject.tag = "Interact";
            }
            else
            {
                visible = false;
                targetColour = Color.grey;
            }
        }
        else
        {
            targetColour = walkColour;
            gameObject.tag = "Tile";

            if (pointerActivate > Time.time)
            {
                visibilityVal -= Time.deltaTime * 4;
            }
        }

    }


    public bool CheckPlayerAtLocation()
    {
        if (gameObject.CompareTag("Player"))
        {
            Debug.Log(Time.time + " true");
            return true;
        }

        return false;
    }


    public List<HexTile> RequestNeighbours(bool requireWalkable, bool requireTunnel)
    {
        List<HexTile> hexTiles = new List<HexTile>();

        Collider[] hitColliders;
        HexTile tile;

        hitColliders = Physics.OverlapSphere(bottomLeftCheck.transform.position, 0.1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.layer == 6)
            {
                tile = hitCollider.gameObject.GetComponent<HexTile>();

                if(requireWalkable)
                {
                    if (tile.walkable)
                    {
                        hexTiles.Add(tile);
                    }
                }
                else if(requireTunnel)
                {
                    if (tile.tunnel)
                    {
                        hexTiles.Add(tile);
                    }
                }
                else
                {
                    hexTiles.Add(tile);
                }

            }
        }

        hitColliders = Physics.OverlapSphere(topCentreCheck.transform.position, 0.1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.layer == 6)
            {
                tile = hitCollider.gameObject.GetComponent<HexTile>();

                if (requireWalkable)
                {
                    if (tile.walkable)
                    {
                        hexTiles.Add(tile);
                    }
                }
                else if (requireTunnel)
                {
                    if (tile.tunnel)
                    {
                        hexTiles.Add(tile);
                    }
                }
                else
                {
                    hexTiles.Add(tile);
                }
            }
        }

        hitColliders = Physics.OverlapSphere(topLeftCheck.transform.position, 0.1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.layer == 6)
            {
                tile = hitCollider.gameObject.GetComponent<HexTile>();

                if (requireWalkable)
                {
                    if (tile.walkable)
                    {
                        hexTiles.Add(tile);
                    }
                }
                else if (requireTunnel)
                {
                    if (tile.tunnel)
                    {
                        hexTiles.Add(tile);
                    }
                }
                else
                {
                    hexTiles.Add(tile);
                }
            }
        }

        hitColliders = Physics.OverlapSphere(topRightCheck.transform.position, 0.1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.layer == 6)
            {
                tile = hitCollider.gameObject.GetComponent<HexTile>();

                if (requireWalkable)
                {
                    if (tile.walkable)
                    {
                        hexTiles.Add(tile);
                    }
                }
                else if (requireTunnel)
                {
                    if (tile.tunnel)
                    {
                        hexTiles.Add(tile);
                    }
                }
                else
                {
                    hexTiles.Add(tile);
                }
            }
        }

        hitColliders = Physics.OverlapSphere(bottomCentreCheck.transform.position, 0.1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.layer == 6)
            {
                tile = hitCollider.gameObject.GetComponent<HexTile>();

                if (requireWalkable)
                {
                    if (tile.walkable)
                    {
                        hexTiles.Add(tile);
                    }
                }
                else if (requireTunnel)
                {
                    if (tile.tunnel)
                    {
                        hexTiles.Add(tile);
                    }
                }
                else
                {
                    hexTiles.Add(tile);
                }
            }
        }

        hitColliders = Physics.OverlapSphere(bottomRightCheck.transform.position, 0.1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.layer == 6)
            {
                tile = hitCollider.gameObject.GetComponent<HexTile>();

                if (requireWalkable)
                {
                    if (tile.walkable)
                    {
                        hexTiles.Add(tile);
                    }
                }
                else if (requireTunnel)
                {
                    if (tile.tunnel)
                    {
                        hexTiles.Add(tile);
                    }
                }
                else
                {
                    hexTiles.Add(tile);
                }
            }
        }

        return hexTiles;
    }

    public void InitialiseTunnel(List<int> passChamberIndexes)
    {
        bool toContinue = false;
        if(tunnel)
        {
            for (int i = 0; i < passChamberIndexes.Count; i++)
            {
                if (!chamberIndexes.Contains(passChamberIndexes[i]))
                { 
                    chamberIndexes.Add(passChamberIndexes[i]);
                    toContinue = true;
                }
            }
        }
        else
        {
            toContinue = true;
        }

        if(toContinue)
        {
            List<HexTile> tiles = RequestNeighbours(false, true);
            for(int i = 0; i < tiles.Count; i++)
            {
                tiles[i].InitialiseTunnel(chamberIndexes);
            }
        }
    }

    public bool DetectNeighbouringObject(GameObject targetObject)
    {
        List<HexTile> neighbourTiles = RequestNeighbours(false, false);
 //       Debug.Log(neighbourTiles.Count);
        List<GameObject> neighbourObjects = new List<GameObject>();
        for(int i = 0; i < neighbourTiles.Count; i++)
        {
            if (neighbourTiles[i].objectAtLocation != null)
            {
                neighbourObjects.Add(neighbourTiles[i].objectAtLocation);
           //     Debug.Log(neighbourTiles[i].objectAtLocation.name);
            }
        }
        
        if(neighbourObjects.Contains(targetObject))
        {
            return true;
        }

        return false;
    }

    public void InitialiseTunnelForCombat(bool state)
    {
        List<HexTile> foundTunnelTiles = RequestNeighbours(false, true);
        for(int i = 0; i < foundTunnelTiles.Count; i++)
        {
            if(foundTunnelTiles[i].combatInitiated != state)
            {
                foundTunnelTiles[i].combatInitiated = state;
                foundTunnelTiles[i].InitialiseTunnelForCombat(state);
            }
        }
    }

}



//  void DisableBorderObjects()
//  {
//    for (int i = 0; i < objects.Count; i++)
//   {
//      objects[i].SetActive(false);
//   }
//  }
/*
 public void InitialiseGuard(QuestManager qManager)
 {
     if(guardPosition)
     {
         if (chamberIndexes.Contains(0) && chamberIndexes.Contains(1))
         {
             Debug.Log("quest 0");
         }
         else if (chamberIndexes.Contains(0) && chamberIndexes.Contains(6))
         {
             Debug.Log("quest 1");
         }
         else if (chamberIndexes.Contains(0) && chamberIndexes.Contains(12))
         {
             Debug.Log("quest 2");
         }
     }

 }
*/

//    public void InitialiseTunnel(List<int>)
//   {

//   }

/*
 *        Collider[] hitColliders;
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
        EnvironmentHexTile environmentTile = tempObj.GetComponent<EnvironmentHexTile>();
        environmentTiles.Add(environmentTile);
        environmentTile.RegularStart();
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
        EnvironmentHexTile environmentTile = tempObj.GetComponent<EnvironmentHexTile>();
        environmentTiles.Add(environmentTile);
        environmentTile.RegularStart();
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
        EnvironmentHexTile environmentTile = tempObj.GetComponent<EnvironmentHexTile>();
        environmentTiles.Add(environmentTile);
        environmentTile.RegularStart();
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
        EnvironmentHexTile environmentTile = tempObj.GetComponent<EnvironmentHexTile>();
        environmentTiles.Add(environmentTile);
        environmentTile.RegularStart();
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
        EnvironmentHexTile environmentTile = tempObj.GetComponent<EnvironmentHexTile>();
        environmentTiles.Add(environmentTile);
        environmentTile.RegularStart();
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
        EnvironmentHexTile environmentTile = tempObj.GetComponent<EnvironmentHexTile>();
        environmentTiles.Add(environmentTile);
        environmentTile.RegularStart();
        tempObj.transform.parent = targetParent;
    }
*/





/*
private void OnTriggerEnter(Collider other)
{
    Debug.Log(other.gameObject.name);
    if(other.gameObject.CompareTag("Building"))
    {
        objectAtLocation = other.gameObject;
        Debug.Log("building");
    }
}
}
*/
/*
if(other.gameObject.CompareTag("Environment"))
{
    Debug.Log(other.gameObject.name);
    objectAtLocation = other.gameObject;
}
}
}
*/
//    private void (Collision collision)
//   {

//    }

/*
private void OnCollisionEnter (Collision other)
{
    if(other.gameObject.CompareTag("Environment"))
    {
        Debug.Log("environment");
        objectAtLocation = other.gameObject;
    }
}

}

*/









/*
public void PlaceObjectAtLocation(GameObject targetObject)
{
    GameObject tempObj = Instantiate(targetObject, new Vector3(0, 0, 0), Quaternion.identity);
    tempObj.transform.parent = gameObject.transform;
    objectAtLocation = tempObj;
    tempObj.transform.localPosition = new Vector3(0.5f, 1.115f, 0.5f);
}
}   
*/

/*
 *     public void UpdateTile()
{
    if(start)
    {
        currentColour = Color.Lerp(currentColour, startColour, 2 * Time.deltaTime);
    }
    else if(end)
    {
        currentColour = Color.Lerp(currentColour, endColour, 2 * Time.deltaTime);
    }
    else if (!walkable)
    {
        currentColour = Color.Lerp(currentColour, nonWalkableColour, 2 * Time.deltaTime);
    }
    else if (Time.time < raycastTime)
    {
        currentColour = Color.Lerp(currentColour, targetColour, 2 * Time.deltaTime);
    }
    else
    {
        currentColour = Color.Lerp(currentColour, defaultColour, 2 * Time.deltaTime);
    }

    rend.material.SetColor("_CurrentColour", currentColour);
}
*/
