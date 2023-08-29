using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPathNew : MonoBehaviour
{
    List<HexInstance> currentHeldPath;
    public HexMaster hexMaster;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<HexInstance> RequestPath(HexInstance start, HexInstance end)
    {
        CalculatePath(start, end);
        RetracePath(start, end);

        return currentHeldPath;
    }

    void CalculatePath(HexInstance start, HexInstance end)
    {
        List<HexInstance> openSet = new List<HexInstance>();
        HashSet<HexInstance> closedSet = new HashSet<HexInstance>();

        openSet.Add(start);
        while(openSet.Count > 0)
        {
            // Debug.Log(openSet.Count);
            HexInstance currentHex = openSet[0];
            for(int i = 1; i < openSet.Count; i ++)
            {
                if (openSet[i].fCost < currentHex.fCost || openSet[i].fCost == currentHex.fCost && openSet[i].hCost < currentHex.hCost)
                {
                    currentHex = openSet[i];
                }
            }

            openSet.Remove(currentHex);
            closedSet.Add(currentHex);

            if (currentHex == end)
            {
                return;
            }

            foreach (HexInstance neighbour in hexMaster.GetNeighbours(currentHex))
            {
                if (neighbour != null)
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentHex.gCost + CalculateLength(currentHex, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = CalculateLength(neighbour, end);
                        neighbour.parent = currentHex;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
        }
    }

    int CalculateLength(HexInstance startHex, HexInstance endHex)
    {
        List<Vector2> path = new List<Vector2>();
        Vector2 currentCoordinate = startHex.coordinates;
        Vector2 endCoordinate = endHex.coordinates;
        bool complete = false;
        while (!complete)
        {
            if (currentCoordinate == endCoordinate)
            {
                complete = true;
            }
            else
            {
                if (currentCoordinate.x < endCoordinate.x && currentCoordinate.y == endCoordinate.y)
                {
                    currentCoordinate.x++;
                }
                else if (currentCoordinate.x > endCoordinate.x && currentCoordinate.y == endCoordinate.y)
                {
                    currentCoordinate.x--;
                }
                else if (currentCoordinate.x == endCoordinate.x && currentCoordinate.y > endCoordinate.y)
                {
                    currentCoordinate.y--;
                }
                else if (currentCoordinate.x == endCoordinate.x && currentCoordinate.y < endCoordinate.y)
                {
                    currentCoordinate.y++;
                }
                else if (currentCoordinate.x > endCoordinate.x && currentCoordinate.y < endCoordinate.y)
                {
                    currentCoordinate.x--;
                    currentCoordinate.y++;
                }
                else if (currentCoordinate.x < endCoordinate.x && currentCoordinate.y > endCoordinate.y)
                {
                    currentCoordinate.x++;
                    currentCoordinate.y--;
                }
                else if (currentCoordinate.x > endCoordinate.x)
                {
                    currentCoordinate.x--;
                }
                else if (currentCoordinate.y > endCoordinate.y)
                {
                    currentCoordinate.y--;
                }
                else if (currentCoordinate.x < endCoordinate.x)
                {
                    currentCoordinate.x++;
                }
                else if (currentCoordinate.x > endCoordinate.x)
                {
                    currentCoordinate.x--;
                }

                path.Add(currentCoordinate);
            }
        }

        return path.Count;
    }

    /*
     *     void RetracePath(HexTile startHex, HexTile endHex)
    {
        List<HexTile> path = new List<HexTile>();
        HexTile currentHex = endHex;

        while (currentHex != startHex)
        {
            if(currentHex.parent != null)
            {
                path.Add(currentHex);
                currentHex = currentHex.parent;
            }
            else
            {
                currentHex = startHex;              // path impossible
                path.Clear();
            }
        }
        path.Reverse();
        currentHeldPath = path;
    }
    */

    void RetracePath(HexInstance start, HexInstance end)
    {
      //  Debug.Log(Time.time);
        List<HexInstance> path = new List<HexInstance>();
        HexInstance current = end;

        while(current != start)
        {
            if (current.parent != null)
            {
                path.Add(current);
                current = current.parent;
            }
            else
            {
                current = start;
                path.Clear();
            }
        }

        path.Reverse();
        currentHeldPath = path;

    }

}

/*
 *     int CalculateLength(HexTile startHex, HexTile endHex)
{
    List<Vector2> path = new List<Vector2>();
    Vector2 current = startHex.hexCoordinate;
    Vector2 end = endHex.hexCoordinate;
    bool complete = false;
    while (!complete)
    {
        if (current == end)
        {
            complete = true;
        }
        else
        {
            if (current.x < end.x && current.y == end.y)
            {
                current.x++;
            }
            else if (current.x > end.x && current.y == end.y)
            {
                current.x--;
            }
            else if (current.x == end.x && current.y > end.y)
            {
                current.y--;
            }
            else if (current.x == end.x && current.y < end.y)
            {
                current.y++;
            }
            else if (current.x > end.x && current.y < end.y)
            {
                current.x--;
                current.y++;
            }
            else if (current.x < end.x && current.y > end.y)
            {
                current.x++;
                current.y--;
            }
            else if (current.x > end.x)
            {
                current.x--;
            }
            else if (current.y > end.y)
            {
                current.y--;
            }
            else if (current.x < end.x)
            {
                current.x++;
            }
            else if (current.x > end.x)
            {
                current.x--;
            }
            path.Add(current);
        }
    }
    return path.Count;
}
*/
