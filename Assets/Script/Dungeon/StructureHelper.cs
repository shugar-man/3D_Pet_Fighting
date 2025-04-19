using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class StructureHelper
{
    public static List<Node> TraverseGraphToExtractLowestLeafes(Node parentNode)
    {
        Queue<Node> nodesToCheck = new Queue<Node>();
        List<Node> listToReturn = new List<Node>();
        if (parentNode.ChildrenNodeList.Count == 0)
        {
            return new List<Node>() { parentNode };
        }
        foreach (var child in parentNode.ChildrenNodeList)
        {
            nodesToCheck.Enqueue(child);
        }
        while (nodesToCheck.Count > 0)
        {
            var currentNode = nodesToCheck.Dequeue();
            if (currentNode.ChildrenNodeList.Count == 0)
            {
                listToReturn.Add(currentNode);
            }
            else
            {
                foreach (var child in currentNode.ChildrenNodeList)
                {
                    nodesToCheck.Enqueue(child);
                }
            }
        }
        return listToReturn;
    }

    public static Vector2Int GenerateBottomLeftCornerBetween(
    Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier, int offset)
{
    int minX = boundaryLeftPoint.x + offset;
    int maxX = boundaryRightPoint.x - offset;
    int minY = boundaryLeftPoint.y + offset;
    int maxY = boundaryRightPoint.y - offset;

    // Randomly select X and Y coordinates within the boundaries, scaled by pointModifier
    int randomX = Random.Range(minX, (int)(minX + (maxX - minX) * pointModifier));
    int randomY = Random.Range(minY, (int)(minY + (maxY - minY) * pointModifier));

    return new Vector2Int(randomX, randomY);
}

    public static Vector2Int GenerateTopRightCornerBetween(
    Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier, int offset)
{
    // Calculate the adjusted boundaries considering the offset
    int minX = boundaryLeftPoint.x + offset;
    int maxX = boundaryRightPoint.x - offset;
    int minY = boundaryLeftPoint.y + offset;
    int maxY = boundaryRightPoint.y - offset;

    // Ensure that minX and minY are not exceeding the max boundaries
    minX = Mathf.Min(minX, maxX);
    minY = Mathf.Min(minY, maxY);

    // Randomly generate the top-right corner within the adjusted boundaries
    int randomX = Random.Range((int)(minX + (maxX - minX) * pointModifier), maxX);
    int randomY = Random.Range((int)(minY + (maxY - minY) * pointModifier), maxY);

    return new Vector2Int(randomX, randomY);
}

    public static Vector2Int CalculateMiddlePoint(Vector2Int v1, Vector2Int v2)
    {
        Vector2 sum = v1 + v2;
        Vector2 tempVector = sum / 2;
        return new Vector2Int((int)tempVector.x, (int)tempVector.y);
    }
}

public enum RelativePosition
{
    Up,
    Down,
    Right,
    Left
}