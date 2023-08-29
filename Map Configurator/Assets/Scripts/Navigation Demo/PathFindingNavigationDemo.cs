using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingNavigationDemo : MonoBehaviour
{
    HexGridNavigationDemo hexGrid;
    public List<HexControlNavigationDemo> currentHeldPath;

    public HexControlNavigationDemo start;
    public HexControlNavigationDemo end;

  //  public bool navigationDemo;

    private void Update()
    {
        FindPath(start, end);
        RetracePath(start, end);
        for (int i = 0; i < currentHeldPath.Count; i++)
        {
            currentHeldPath[i].GetRaycast();
        }
    }

    private void Start()
    {
        hexGrid = GetComponent<HexGridNavigationDemo>();

        start = hexGrid.FindHexTile(new Vector2(10, 10));
        end = hexGrid.FindHexTile(new Vector2(20, 0));

        start.start = true;
        end.end = true;


    }
    void FindPath(HexControlNavigationDemo startHex, HexControlNavigationDemo endHex)
    {
        List<HexControlNavigationDemo> openSet = new List<HexControlNavigationDemo>();
        HashSet<HexControlNavigationDemo> closedSet = new HashSet<HexControlNavigationDemo>();

        openSet.Add(startHex);
        while (openSet.Count > 0)
        {
            HexControlNavigationDemo currentHex = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentHex.fCost || openSet[i].fCost == currentHex.fCost && openSet[i].hCost < currentHex.hCost)
                {
                    currentHex = openSet[i];
                }
            }

            openSet.Remove(currentHex);
            closedSet.Add(currentHex);

            if (currentHex == endHex)
            {
                return;
            }

            foreach (HexControlNavigationDemo neighbour in hexGrid.GetNeighbours(currentHex))
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
                        neighbour.hCost = CalculateLength(neighbour, endHex);
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

    int CalculateLength(HexControlNavigationDemo startHex, HexControlNavigationDemo endHex)
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
                else if(current.y > end.y)
                {
                    current.y--;
                }
                else if(current.x < end.x)
                {
                    current.x++;
                }
                else if(current.x > end.x)
                {
                    current.x--;
                }
                path.Add(current);
            }
        }
        return path.Count;
    }

    void RetracePath(HexControlNavigationDemo startHex, HexControlNavigationDemo endHex)
    {
        List<HexControlNavigationDemo> path = new List<HexControlNavigationDemo>();
        HexControlNavigationDemo currentHex = endHex;

        while (currentHex != startHex)
        {
            path.Add(currentHex);
            currentHex = currentHex.parent;
        }
        path.Reverse();
        currentHeldPath = path;
    }
}






    /*
    int DetermineShortestPath(Vector2 start, Vector2 end)
    {
        int counter = 0;
        Vector2 currentLocation = start;

        int targetX = (int)end.x;
        int targetY = (int)end.y;

        while (currentLocation != end)
        {
            int currentX = (int)currentLocation.x;
            int currentY = (int)currentLocation.y;

            if(currentX < targetX && currentY == targetY)
            {
                currentX++;
            }
            else if(currentY < targetY && currentX == targetX)
            {
                currentY++;
            }
            else if(currentX > targetX && currentY < targetY)
            {
                currentX--;
                currentY++;
            }
            else if(currentX > targetX && currentY == targetY)
            {
                currentX--;
            }
            else if(currentX == targetX && currentY < targetY)
            {
                currentY--;
            }
            else if(currentX < targetX && currentY > targetY)
            {
                currentY--;
                targetX++;
            }

            currentLocation.x = currentX;
            currentLocation.y = currentY;
        }

        return counter;
    }
}
    */
/*
HexControl startHexTile = hexGrid.FindHexTile((int)start.x, (int)start.y);
HexControl endHexTile = hexGrid.FindHexTile((int)end.x, (int)end.y);

List<HexControl> openSet = new List<HexControl>();
HashSet<HexControl> closeSet = new HashSet<HexControl>();
openSet.Add(startHexTile);

while (openSet.Count > 0)
{
    HexControl currentHex = openSet[0];
    for (int i = 0; i < openSet.Count; i++)
    {
        Debug.Log(i);
        if (openSet[i].fCost < currentHex.fCost || openSet[i].fCost == currentHex.fCost && openSet[i].hCost < currentHex.hCost)
        {
            currentHex = openSet[i];
        }
    }

    openSet.Remove(currentHex);
    closeSet.Add(currentHex);

    if (currentHex == endHexTile)
    {
        RetracePath(startHexTile, endHexTile);
    }

    foreach (HexControl neighbour in hexGrid.GetNeighbours(currentHex))
    {
        if (!neighbour.walkable || closeSet.Contains(neighbour))
        {
            continue;
        }


        Vector2 currentVector = new Vector2(currentHex.xVal, currentHex.yVal);
        Vector2 neighbourVector = new Vector2(neighbour.xVal, neighbour.yVal);
        int newMovementCostToNeighbour = currentHex.gCost + ReturnShortestPath(currentVector, neighbourVector).Count;
        if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
        {
            neighbour.gCost = newMovementCostToNeighbour;

            Vector2 endVector = new Vector2(endHexTile.xVal, endHexTile.yVal);
            neighbour.hCost = ReturnShortestPath(neighbourVector, endVector).Count;

            neighbour.parent = currentHex;
            if(!openSet.Contains(neighbour))
            {
                openSet.Add(neighbour);
            }
        }
    }
}
}
*/




/*
int currentX = (int)current.x;
int currentY = (int)current.y;

if (currentX == endX && currentY == endY)
{
complete = true;
return path;
}
else
{
if (currentX < endX && currentY == endY)
{
    currentX++;
}
else if (currentX == endX && currentY < endY)
{
    currentY++;
}
else if (currentX > endX && currentY < endY)
{
    currentX--;
    currentY++;
}
else if (currentX > endX && currentY == endY)
{
    currentX--;
}
else if (currentX == endX && currentY > endY)
{
    currentY--;
}
else if (currentX < endX && currentY > endY)
{
    currentX++;
    currentY--;
}
current.x = currentX;
current.y = currentY;
path.Add(current);
}
}
return path;
}
*/
/*
else
{
    if (currentX < endX && currentY == endY)
    {
        currentX++;
    }
    else if (currentX == endX && currentY < endY)
    {
        currentY++;
    }
    else if (currentX > endX && currentY < endY)
    {
        currentX--;
        currentY++;
    }
    else if (currentX > endX && currentY == endY)
    {
        currentX--;
    }
    else if (currentX == endX && currentY > endY)
    {
        currentY--;
    }
    else if (currentX < endX && currentY > endY)
    {
        currentX++;
        currentY--;
    }

 //   current.x = currentX;
//    current.y = currentY;

    path.Add(current);
}
}

return path;
}
*/


/*
int currentX = (int)start.x;
int currentY = (int)start.y;

int targetX = (int)end.x;
int targetY = (int)end.y;

List<Vector2> path = new List<Vector2>();
Vector2 current = start;
bool complete = false;

while(!complete)
{
    currentX = (int)current.x;
    currentY = (int)current.y;

    if(currentX == targetX && currentY == targetY)
    {
        complete = true;
    }
    else
    {
        if (currentX < targetX && currentY == targetY)
        {
            currentX++;
        }
        else if (currentX == targetX && currentY < targetY)
        {
            currentY++;
        }
        else if (currentX > targetX && currentY < targetY)
        {
            currentX--;
            currentY++;
        }
        else if (currentX > targetX && currentY == targetY)
        {
            currentX--;
        }
        else if (currentX == targetX && currentY > targetY)
        {
            currentY--;
        }
        else if (currentX < targetX && currentY > targetY)
        {
            currentX++;
            currentY--;
        }

        current.x = currentX;
        current.y = currentY;

        path.Add(current);
    }
}

return path;
}
*/
/*

Vector2 current = new Vector2(currentHex.xVal, currentHex.yVal);
Vector2 neighbour = new Vector2(neighbour.xva)
int startX = ;
int startY = ;
int endX = neighbour.xVal;
int endY = neighbour.yVal;

int newMovementCostToNeighbour = currentHex.gCost + ReturnShortestPath()
}
}
}
*/
/*
while(openSet.Count > 0)
{
    Node currentNode = openSet[0];
    for(int i = 1; i < openSet.Count; i++)
    {
        if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost)
        {
            currentNode = openSet[i];
        }
    }
}
}
*/
/*
public List<Vector2> currentPath;
private void Update()
{
    if (Input.GetMouseButtonDown(0))
    {
 //       currentPath = ReturnShortestPath(Vector2 start, Vector2 end);
    }
}



*/


/*
if (currentX != targetX && currentY != targetY)
{

    if (currentX < targetX && currentY == targetY)
    {
        Debug.Log(Time.time);
        currentX++;
    }
    else if (currentY < targetY && currentX == targetX)
    {
        currentY++;
    }
    else if (currentX > targetX && currentY < targetY)
    {
        currentX--;
        currentY++;
    }
    else if (currentX > targetX && currentY == targetY)
    {
        currentX--;
    }
    else if (currentX == targetX && currentY < targetY)
    {
        currentY--;
    }
    else if (currentX < targetX && currentY > targetY)
    {
        currentY--;
        targetX++;
    }

    current.x = currentX;
    current.y = currentY;
}
}
}

*/


/*
while (currentLocation != end)
{
    Debug.Log(currentLocation);
    counter++;
    if (currentLocation.x < end.x && currentLocation.y < end.y)
    {
        currentLocation.y++;
        currentLocation.x++;
    }
    else if(currentLocation.x < end.x && currentLocation.y == end.y)
    {
        currentLocation.x++;
    }
    else if(currentLocation.x == end.x && currentLocation.y < end.y)
    {
        currentLocation.y--;
    }
    else if(currentLocation.x > end.x && currentLocation.y == end.y)
    {
        currentLocation.x--;
    }
    else if(currentLocation.x > end.x && currentLocation.y < end.y)
    {
        currentLocation.x--;
        currentLocation.y++;
    }
    else if(currentLocation.x == end.x && currentLocation.y < end.y)
    {
        currentLocation.y++;
    }
}

return counter;
}
}
*/
//
/*
 * https://medium.com/@nicholas.w.swift/easy-a-star-pathfinding-7e6689c7f7b2
if (currentLocation.y < end.y)
{
    currentLocation.y += 1;
}
else if(currentLocation.y > end.y)
{
    currentLocation.y -= 1;
}

if (currentLocation.x < end.x)
{
    if (currentLocation.y > end.y)
    {
        currentLocation.x += 1;
        currentLocation.y -= 1;
    }
    else
    {
        currentLocation.x += 1;
        currentLocation.y += 1;
    }

}
else if (currentLocation.x > end.x)
{
    if (currentLocation.y > end.y)
    {
        currentLocation.x -= 1;
        currentLocation.y -= 1;
    }
    else
    {
        currentLocation.x -= 1;
        currentLocation.y += 1;
    }
}


Debug.Log(currentLocation);
}
return counter;
}
}
      */
//     int counter = 0;
//   bool complete = false;
//   Vector2 currentLocation = start;
/*
while (currentLocation != end)
{
    counter++;
    if (currentLocation.x != end.x && currentLocation.y != end.y)
    {
        if (currentLocation.x > end.x)
        {
            currentLocation.x -= 1;
        }
        else if (currentLocation.x < end.x)
        {
            currentLocation.x += 1;
        }
        if (currentLocation.y > end.y)
        {
            currentLocation.y -= 1;
        }
        else
        {
            currentLocation.y += 1;
        }
    }
    else if (currentLocation.x > end.x)
    {
        currentLocation.x -= 1;
    }
    else if (currentLocation.x < end.x)
    {
        currentLocation.x += 1;
    }
    else if (currentLocation.y > end.y)
    {
        currentLocation.y -= 1;
    }
    else
    {
        currentLocation.y += 1;
    }

    Debug.Log(currentLocation);
}

return counter;
}
*/


/*
if (start.x != end.x && start.y != end.y)
{
    if (start.x > end.x)
    {
        currentLocation.x -= 1;
    }
    else
    {
        currentLocation.x += 1;
    }

    if(start.y > end.y)
    {
        currentLocation.y -= 1;
    }
    else
    {
        currentLocation.y += 1;
    }
}
else if (start.x > end.x)
{
    currentLocation.x -= 1;
}
else if(start.x < end.x)
{
    currentLocation.x += 1;
}
else if(start.y > end.y)
{
    currentLocation.y -= 1;
}
else if(start.y < end.y)
{
    currentLocation.y += 1;
}

Debug.Log(currentLocation);
}

return counter;
}

}
*/