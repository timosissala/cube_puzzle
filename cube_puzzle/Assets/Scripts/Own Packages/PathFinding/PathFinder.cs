using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder
{
    private Node[,] nodes;

    private List<Node> openList;
    private List<Node> closedList;

    private int yMin;
    private int xMin;

    private List<Vector2Int> pathPositions;
    public List<Vector2Int> PathPositions { get { return pathPositions; } }

    public PathFinder(Tilemap groundTiles, Tilemap terrainTiles)
    {
        InitialiseNodesFromTilemaps(groundTiles, terrainTiles);
    }

    public PathFinder(GridController gridController, List<Collider2D> colliders)
    {
        InitialiseNodesFromGrid(gridController, colliders);
    }

    public Vector2Int GetTargetDirection(Vector2Int position, Vector2Int target)
    {
        openList = new List<Node>();
        closedList = new List<Node>();

        Vector2Int direction = new Vector2Int(0, 0);
        bool pathFound = false;

        Node startNode = IsLegalPosition(position) ?
            nodes[position.y - yMin, position.x - xMin] :
            NearestNode(position);

        Node targetNode = IsLegalPosition(target) ?
            nodes[target.y - yMin, target.x - xMin] :
            NearestNode(target);
        
        openList.Add(startNode);

        while (openList.Count > 0 && !pathFound)
        {
            Node currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].GetTotalCost() < currentNode.GetTotalCost() ||
                    openList[i].GetTotalCost() == currentNode.GetTotalCost() &&
                    openList[i].distanceToTarget < currentNode.distanceToTarget)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                pathFound = true;
                direction = GetPathDirection(startNode, currentNode);
            }

            foreach (Node neighbourNode in GetNeighboringNodes(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }

                int moveCost = currentNode.moveCost + GetDistance(currentNode, neighbourNode);

                if (moveCost < neighbourNode.moveCost || !openList.Contains(neighbourNode))
                {
                    neighbourNode.moveCost = moveCost;
                    neighbourNode.distanceToTarget = GetDistance(neighbourNode, targetNode);
                    neighbourNode.parentNode = currentNode;

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        return direction;
    }

    public List<Vector2Int> GetPath()
    {
        return PathPositions;
    }

    private Vector2Int GetPathDirection(Node startNode, Node endNode)
    {
        Vector2Int result;

        List<Node> nodes = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            nodes.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        pathPositions = new List<Vector2Int>();
        for (int i = nodes.Count - 1; i >= 0; i--)
        {
            Node node = nodes[i];
            pathPositions.Add(new Vector2Int(node.x, node.y));
        }

        if (nodes.Count > 1)
        {
            result = GetDirectionToNode(nodes[nodes.Count - 1], nodes[nodes.Count - 2]);
        }
        else
        {
            result = GetDirectionToNode(startNode, endNode);
        }

        return result;
    }

    private Vector2Int GetDirectionToNode(Node startNode, Node endNode)
    {
        return new Vector2Int(endNode.x - startNode.x, endNode.y - startNode.y);
    }

    private List<Node> GetNeighboringNodes(Node currentNode)
    {
        List<Node> neighbourNodes = new List<Node>();

        if (currentNode != null)
        {
            int x = currentNode.x - xMin;
            int y = currentNode.y - yMin;

            for (int yy = y - 1; yy <= y + 1; yy++)
            {
                for (int xx = x - 1; xx <= x + 1; xx++)
                {
                    if (xx >= 0 && xx < nodes.GetLength(1) && yy >= 0 && yy < nodes.GetLength(0))
                    {
                        Node neighbourNode = nodes[yy, xx];

                        if (neighbourNode != null)
                        {
                            neighbourNodes.Add(neighbourNode);
                        }
                    }
                }
            }
        }

        return neighbourNodes;
    }

    public void InitialiseNodesFromTilemaps(Tilemap tileMap, Tilemap terrain)
    {
        yMin = tileMap.cellBounds.yMin;
        int yMax = tileMap.cellBounds.yMax;

        xMin = tileMap.cellBounds.xMin;
        int xMax = tileMap.cellBounds.xMax;

        nodes = new Node[yMax - yMin, xMax - xMin];

        for (int n = xMin; n < xMax; n++)
        {
            for (int p = yMin; p < yMax; p++)
            {
                Vector3Int localPlace = new Vector3Int(n, p, (int)tileMap.transform.position.y);

                if (tileMap.HasTile(localPlace) && !terrain.HasTile(localPlace))
                {
                    // Tile at "place"
                    nodes[p - yMin, n - xMin] = new Node(n, p);
                }
                else
                {
                    // No tile at "place"
                }
            }
        }
    }

    public void InitialiseNodesFromGrid(GridController gridController, List<Collider2D> colliders)
    {
        yMin = gridController.YMin;
        int yMax = gridController.YMax;

        xMin = gridController.XMin;
        int xMax = gridController.XMax;

        nodes = new Node[yMax - yMin, xMax - xMin];

        for (int n = xMin; n < xMax; n++)
        {
            for (int p = yMin; p < yMax; p++)
            {
                Vector2Int localPlace = new Vector2Int(n, p);

                Vector2 worldPosition = gridController.TileToWorldPosition(localPlace);

                bool overLaps = false;

                foreach (Collider2D collider in colliders)
                {
                    if (collider.OverlapPoint(worldPosition))
                    {
                        overLaps = true;
                        break;
                    }
                }

                if (!overLaps)
                {
                    nodes[p - yMin, n - xMin] = new Node(n, p);

#if UNITY_EDITOR
                    gridController.InstantiateTestObject(worldPosition);
#endif
                }
            }
        }
    }

    private bool IsLegalPosition(Vector2Int position)
    {
        return position.x - xMin >= 0 && position.x - xMin < nodes.GetLength(1)
            && position.y - yMin >= 0 && position.y - yMin < nodes.GetLength(0)
            && nodes[position.y - yMin, position.x - xMin] != null;
    }

    private Node NearestNode(Vector2Int position)
    {
        Node nearestNode = null;
        int nearestDistance = -1;
        for (int y = 0; y < nodes.GetLength(0); y++)
        {
            for (int x = 0; x < nodes.GetLength(1); x++)
            {
                Node node = nodes[y, x];
                if (node != null)
                {
                    int distance = Mathf.Abs(node.x - position.x) + Mathf.Abs(node.y - position.y);

                    if (nearestDistance == -1 || distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestNode = node;
                    }
                }
            }
        }

        return nearestNode;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.x - nodeB.x);
        int distY = Mathf.Abs(nodeA.y - nodeB.y);

        return distX + distY;
    }
}