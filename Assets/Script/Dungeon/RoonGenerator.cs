using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator
{
    private int maxIterations;
    private int roomLengthMin;
    private int roomWidthMin;
    private int roomLengthMax;
    private int roomWidthMax;

    public RoomGenerator(int maxIterations, int roomLengthMin, int roomWidthMin, int roomLengthMax, int roomWidthMax)
    {
        this.maxIterations = maxIterations;
        this.roomLengthMin = roomLengthMin;
        this.roomWidthMin = roomWidthMin;
        this.roomLengthMax = roomLengthMax;
        this.roomWidthMax = roomWidthMax;
    }

    public List<RoomNode> GenerateRoomsInGivenSpaces(List<Node> roomSpaces, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset)
    {
        List<RoomNode> listToReturn = new List<RoomNode>();

        foreach (var space in roomSpaces)
        {
            // Generate random length and width within the given bounds
            int randomLength = UnityEngine.Random.Range(roomLengthMin, roomLengthMax);
            int randomWidth = UnityEngine.Random.Range(roomWidthMin, roomWidthMax);

            // Randomly offset the room within the given space
            Vector2Int newBottomLeftPoint = StructureHelper.GenerateBottomLeftCornerBetween(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner, roomBottomCornerModifier, roomOffset);

            // Randomize the top-right point and apply small offsets to create a random shape
            Vector2Int newTopRightPoint = new Vector2Int(newBottomLeftPoint.x + randomWidth, newBottomLeftPoint.y + randomLength);

            // Apply random distortion to create more irregular shapes
            newBottomLeftPoint.x += UnityEngine.Random.Range(-roomOffset, roomOffset);
            newBottomLeftPoint.y += UnityEngine.Random.Range(-roomOffset, roomOffset);
            newTopRightPoint.x += UnityEngine.Random.Range(-roomOffset, roomOffset);
            newTopRightPoint.y += UnityEngine.Random.Range(-roomOffset, roomOffset);

            // Create additional corners for irregularity (e.g., trapezoidal or polygonal rooms)
            Vector2Int bottomRightPoint = new Vector2Int(newTopRightPoint.x + UnityEngine.Random.Range(-roomOffset, roomOffset), newBottomLeftPoint.y);
            Vector2Int topLeftPoint = new Vector2Int(newBottomLeftPoint.x + UnityEngine.Random.Range(-roomOffset, roomOffset), newTopRightPoint.y);

            // Update the room corners
            space.BottomLeftAreaCorner = newBottomLeftPoint;
            space.TopRightAreaCorner = newTopRightPoint;
            space.BottomRightAreaCorner = bottomRightPoint;
            space.TopLeftAreaCorner = topLeftPoint;

            // Add the room node to the list
            listToReturn.Add((RoomNode)space);
        }

        return listToReturn;
    }
}