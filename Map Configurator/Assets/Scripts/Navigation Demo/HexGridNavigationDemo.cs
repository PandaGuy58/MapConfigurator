using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridNavigationDemo : MonoBehaviour
{
    public GameObject tile;
    Camera cam;

    public List<HexControlNavigationDemo> hexTiles = new List<HexControlNavigationDemo>();

    public LayerMask targetLayer;

  //  GameObject recentRaycast;
 //   HexControlNavigationDemo recentHexControl;

 //   public AnimationCurve sCurve;
    // navigation demo
    public Color defaultColour;
    public Color targetColour;
    public Color nonWalkableColour;
    public Color startColour;
    public Color endColour;

    public GameObject playerObject;

  //  public bool navigationDemo;

    void Start()
    {
        cam = Camera.main;
        InstantiateTiles();
        InitialiseTilesInList();
    }



    // Update is called once per frame
    void Update()
    {
        UpdateHexTiles();
        RaycastTiles();
    }


    public void UpdateHexTiles()
    {
        for (int i = 0; i < hexTiles.Count; i++)
        {
            hexTiles[i].UpdateTile();
        }
    }


    public void RaycastTiles()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, targetLayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                HexControlNavigationDemo hexTile = hit.transform.gameObject.GetComponent<HexControlNavigationDemo>();
                hexTile.MouseClickDemo();
            }
        }
    }


    public void InitialiseTilesInList()
    {
        for (int i = 0; i < hexTiles.Count; i++)
        {
            hexTiles[i].InitialiseColours(defaultColour, targetColour, nonWalkableColour,startColour, endColour);
        }
    }

    void InstantiateTiles()
    {
        int xCoordinate;
        int yCoordinate;
        for (int x = 0; x < 20; x++)
        {
            xCoordinate = x * 2;
            yCoordinate = -x - 1;
            for (int y = 0; y < 30; y++)
            {
                yCoordinate++;
                GameObject tempTile = Instantiate(tile, new Vector3(x * 1.4995408f, -0.99f, y * 0.8660469f), Quaternion.identity);
                HexControlNavigationDemo tempHexControl = tempTile.GetComponent<HexControlNavigationDemo>();
                tempHexControl.InitialiseTile(xCoordinate, yCoordinate);
                hexTiles.Add(tempHexControl);
            }
        }

        for (int x = 0; x < 20; x++)
        {
            xCoordinate = 1 + x * 2;
            yCoordinate = -x - 1;
            for (int y = 0; y < 30; y++)
            {
                yCoordinate++;

                GameObject tempTile = Instantiate(tile, new Vector3(0.7497704f + x * 1.4995408f, -0.99f, y * 0.8660469f + 0.43302345f), Quaternion.identity);
                HexControlNavigationDemo tempHexControl = tempTile.GetComponent<HexControlNavigationDemo>();
                tempHexControl.InitialiseTile(xCoordinate, yCoordinate);
                hexTiles.Add(tempHexControl);
            }
        }
    }

    public HexControlNavigationDemo FindHexTile(Vector2 targetXexCoordinate)
    {
        for (int i = 0; i < hexTiles.Count; i++)
        {
            if (targetXexCoordinate == hexTiles[i].hexCoordinate)
            {
                return hexTiles[i];
            }

        }
        HexControlNavigationDemo hexTemp = null;
        return hexTemp;
    }

    public List<HexControlNavigationDemo> GetNeighbours(HexControlNavigationDemo hexTile)
    {
        Vector2 centreCoordinate = hexTile.hexCoordinate;
        List<HexControlNavigationDemo> neighbours = new List<HexControlNavigationDemo>();

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
}








    /*
    void PlaceObjectIntoWorld()
    {
        HexControlNavigationDemo targetHex = FindHexTile(new Vector2(5, 5));
        targetHex.PlaceObjectAtLocation(playerObject);
    }

}



    */
/*
GameObject target = hit.transform.gameObject;
if (target != recentRaycast)
{
    recentRaycast = target;
    timeFirstRaycast = Time.time;
    recentHexControl = target.GetComponent<HexControl>();
}
else
{
    if (Time.time > timeFirstRaycast + 0.25f)
    {
        recentHexControl.GetRaycast();
    }
}
}
else
{
recentRaycast = null;
}
}
*/


//      GameObject gameObject = Instantiate(targetObject, new Vector3(0, 0, 0), Quaternion.identity);
/*
 * You can cut the amount of code down further:
HexControl tempTile = Instantiate(tile, new Vector3(x * "What ever number you put", -0.99f,  z * "What ever number you put"), Quaternion.identity).GetComponent<HexControl>();
*/


//      HexControl temp = FindHexTile(x, y);
//     temp.PlaceObjectAtLocation(gameObject);








/*
for (int x = 0; x < 10; x++)
{
    float xVal = x * 1.4995408f;
    for (int z = 1; z < 10; z++)
    {
        float zVal = (z -1) * 0.8660469f;

        GameObject tempTile = Instantiate(tile, new Vector3(xVal, -0.99f, zVal), Quaternion.identity);
        HexControl tempHexControl = tempTile.GetComponent<HexControl>();
        tempHexControl.InitialiseTile(x * 2, z -1);
        hexTiles.Add(tempHexControl);

        //   HexControl hexControl = tile.GetComponent<HexControl>();
        //   hexControl.InitialiseTile(x,z);
        //    tile.name = x.ToString() + z.ToString();
        //   Instantiate(tile, new Vector3(xVal, -0.99f, zVal), Quaternion.identity);
    }
}

for (int x = 0; x < 10; x++)
{
    float xVal = 0.7497704f + x * 1.4995408f;
    for (int z = 1; z < 10; z++)
    {
        float zVal = 0.43302345f + (z -1) * 0.8660469f;

        GameObject tempTile = Instantiate(tile, new Vector3(xVal, -0.99f, zVal), Quaternion.identity);
        HexControl tempHexControl = tempTile.GetComponent<HexControl>();
        tempHexControl.InitialiseTile(x * 2 + 1, z -1);
        hexTiles.Add(tempHexControl);
    }
}
}
}
*/