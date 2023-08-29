using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexControlNavigationDemo : MonoBehaviour
{
    Renderer rend;

    float raycastTime = -1;

    public Color defaultColour;
    public Color targetColour;
    public Color nonWalkableColour;
    public Color startColour;
    public Color endColour; 

    public Color currentColour = Color.white;
    public bool walkable = true;


    public Vector2 hexCoordinate;
    public HexControlNavigationDemo parent;
    public int gCost;
    public int hCost;

    public bool start;
    public bool end;

 //   public bool navigationDemo;
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    void Start()
    {
        rend = GetComponent<Renderer>();
        GameObject master = GameObject.Find("Game Master");
        transform.parent = GameObject.Find("HexTiles").transform;
    }

    public void MouseClickDemo()
    {
        walkable = !walkable;
    }

    // Update is called once per frame
    public void UpdateTile()
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


    public void GetRaycast()
    {
        raycastTime = Time.time + 0.05f;
    }



    public void InitialiseTile(int x, int y)
    {
        hexCoordinate.x = x;
        hexCoordinate.y = y;
        gameObject.name = x.ToString() + "," + y.ToString();
    }

    public void InitialiseColours(Color defaultCol, Color targetCol, Color unwalkCol, Color startCol, Color endCol)
    {
        defaultColour = defaultCol;
        targetColour = targetCol;
        nonWalkableColour = unwalkCol;
        startColour = startCol;
        endColour = endCol;
    }
}





    /*
    public void PlaceObjectAtLocation(GameObject targetObject)
    {
        GameObject tempObj = Instantiate(targetObject, new Vector3(0,0,0), Quaternion.identity);
        tempObj.transform.parent = gameObject.transform;
        objectAtLocation = tempObj;
        tempObj.transform.localPosition = new Vector3(0.5f, 1.115f, 0.5f);
    }

    public void RemoveObjectAtLocation()
    {
        objectAtLocation = null;
    }
}

    */



/*



    public void FirstRaycast()
    {

    }

    public void LastRaycast()
    {

    }

*/
//     if(timeHexLastRaycast > Time.time - 0.5f)
//     {
//       float timePassed = Time.time - timeHexFirstRaycast;
//       float percent = timePassed / 0.6f;
//       float curveEvaluate = sCurve.Evaluate(percent);

//      currentVisibility = Mathf.Lerp(0, targetVisibility, curveEvaluate);
//   }
//  else
//   {
//      currentVisibility -= 0.5f * Time.deltaTime;
//  }

//    rend.material.SetFloat("_Visibility", currentVisibility);