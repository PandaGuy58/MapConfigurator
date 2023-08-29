using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPath : MonoBehaviour
{
    public HexGrid hexGrid;
    public List<HexTile> currentHeldPath;

    public HexTile start;
    public HexTile end;

    private void Start()
    {
        hexGrid = GetComponent<HexGrid>();
    }

    public List<HexTile> RequestPath(HexTile start, HexTile end)
    {
        CalculatePath(start, end);
        RetracePath(start, end);

        return currentHeldPath;
    }

    void CalculatePath(HexTile startHex, HexTile endHex)
    {
        List<HexTile> openSet = new List<HexTile>();
        HashSet<HexTile> closedSet = new HashSet<HexTile>();

        openSet.Add(startHex);
        while (openSet.Count > 0)
        {
            HexTile currentHex = openSet[0];
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

            foreach (HexTile neighbour in hexGrid.GetNeighbours(currentHex))
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

    int CalculateLength(HexTile startHex, HexTile endHex)
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

    void RetracePath(HexTile startHex, HexTile endHex)
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

    public List<HexTile> RequestShortestPathToInteractable(HexTile start, HexTile interactable)
    {
        List<HexTile> path = new List<HexTile>();
        List<HexTile> availableTiles = interactable.RequestNeighbours(true, false);
        List<List<HexTile>> possiblePaths = new List<List<HexTile>>();
        
        for (int i = 0; i < availableTiles.Count; i++)
        {
            possiblePaths.Add(RequestPath(start, availableTiles[i]));
        }

        int shortestIndex = -1;
        int shortestListCount = 0;
        List<HexTile> currentPath;
        for (int i = 0; i < possiblePaths.Count; i++)
        {
            currentPath = possiblePaths[i];
            if (i == 0)
            {
                shortestIndex = 0;
                shortestListCount = currentPath.Count;
            }
            else
            {
                if(currentPath.Count < shortestListCount)
                {
                    shortestListCount = currentPath.Count;
                    shortestIndex = i;
                }
            }
        }

        if(shortestIndex != -1)
        {
            path = possiblePaths[shortestIndex];
        }

        return path;
    }
}